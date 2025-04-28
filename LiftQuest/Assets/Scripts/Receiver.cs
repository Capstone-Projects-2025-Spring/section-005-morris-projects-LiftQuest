using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

public class Receiver : MonoBehaviour
{
    [Header("Network Settings")]
    public string serverIP = "192.168.1.100"; // Replace with your Pico W's IP address
    public int serverPort = 12345;
    
    [Header("Calibration")]
    public float preparationTime = 5f;
    public float calibrationTime = 5f;
    public float restPositionThreshold = 0.3f;
    public float curlPositionThreshold = 0.3f;

    private ProfileManager profileManager;
    private BicepCurlDetector bicepCurlDetector;
    private TcpClient client;
    private Thread receiveThread;
    private bool isConnected = false;
    private bool threadRunning = false;

    // Sensor data
    private float[] sensorData = new float[6];
    private Vector3 currentAcceleration = Vector3.zero;
    private Vector3 currentGyro = Vector3.zero;
    
    // Calibration data
    private Vector3 restPosition;
    private Vector3 curlPosition;
    private bool dataSaved = false;

    private enum CalibrationState { NotStarted, PrepareRest, RestPosition, PrepareCurl, CurlPosition, Complete }
    private CalibrationState calibrationState = CalibrationState.NotStarted;

    private float calibrationTimer = 0f;
    private int repCount = 0;

    [SerializeField] private Player player;

    [SerializeField] private GameManager _gm;

    [SerializeField] private Sprite[] workoutSprites;
    [SerializeField] private SpriteRenderer workoutSR;

    void Start()
    {
        profileManager = FindObjectOfType<ProfileManager>();
        bicepCurlDetector = FindObjectOfType<BicepCurlDetector>();

        if (profileManager == null)
        {
            Debug.LogError("ProfileManager not found");
            return;
        }

        if (bicepCurlDetector == null)
        {
            Debug.LogError("BicepCurlDetector not found");
            return;
        }

        // Always start with calibration
        calibrationState = CalibrationState.NotStarted;

        // If there's a previously saved profile, optionally delete it to avoid confusion
        if (profileManager.ProfileExists("DefaultProfile"))
        {
            profileManager.DeleteProfile("DefaultProfile");
            Debug.Log("Deleted previously saved DefaultProfile to force fresh calibration.");
        }


        ConnectToServer();
    }

    void ConnectToServer()
    {
        try
        {
            client = new TcpClient();
            client.BeginConnect(serverIP, serverPort, new AsyncCallback(ConnectCallback), client);
            Debug.Log("Attempting to connect to Pico W server at " + serverIP + ":" + serverPort);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to initiate connection to Pico W server: " + e.Message);
        }
    }

    void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            client.EndConnect(ar);
            isConnected = true;
            Debug.Log("Connected to Pico W server at " + serverIP + ":" + serverPort);

            threadRunning = true;
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to connect to Pico W server: " + e.Message);
            isConnected = false;
            
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                StartCoroutine(ReconnectAfterDelay(5f));
            });
        }
    }

    IEnumerator ReconnectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Attempting to reconnect...");
        ConnectToServer();
    }

    void ReceiveData()
    {
        byte[] buffer = new byte[24];
        
        while (threadRunning && isConnected)
        {
            try
            {
                if (client != null && client.Connected)
                {
                    NetworkStream stream = client.GetStream();
                    
                    if (stream.DataAvailable)
                    {
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        
                        if (bytesRead == 24)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                byte[] floatBytes = new byte[4];
                                Array.Copy(buffer, i * 4, floatBytes, 0, 4);
                                
                                if (BitConverter.IsLittleEndian)
                                {
                                    sensorData[i] = BitConverter.ToSingle(floatBytes, 0);
                                }
                                else
                                {
                                    Array.Reverse(floatBytes);
                                    sensorData[i] = BitConverter.ToSingle(floatBytes, 0);
                                }
                            }
                            
                            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                                currentAcceleration = new Vector3(sensorData[0], sensorData[1], sensorData[2]);
                                currentGyro = new Vector3(sensorData[3], sensorData[4], sensorData[5]);
                                
                            });
                        }
                    }
                }
                else
                {
                    isConnected = false;
                    Debug.LogError("Connection to Pico W server lost.");
                    
                    UnityMainThreadDispatcher.Instance().Enqueue(() => {
                        StartCoroutine(ReconnectAfterDelay(5f));
                    });
                    
                    break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving data: " + e.Message);
                isConnected = false;
                
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    StartCoroutine(ReconnectAfterDelay(5f));
                });
                
                break;
            }
            
            Thread.Sleep(10);
        }
    }

    void Update()
    {
        switch (calibrationState)
        {
            case CalibrationState.NotStarted:
                Debug.Log("Calibration not started. Preparing for rest position...");
                calibrationState = CalibrationState.PrepareRest;
                calibrationTimer = preparationTime;
                break;

            case CalibrationState.PrepareRest:
                calibrationTimer -= Time.deltaTime;
                if (calibrationTimer <= 0)
                {
                    Debug.Log("Hold the rest position...");
                    calibrationState = CalibrationState.RestPosition;
                    calibrationTimer = calibrationTime;
                }
                break;

            case CalibrationState.RestPosition:
                calibrationTimer -= Time.deltaTime;
                if (calibrationTimer <= 0)
                {
                    restPosition = currentAcceleration;
                    Debug.Log("Rest position calibrated: " + restPosition);
                    calibrationState = CalibrationState.PrepareCurl;
                    calibrationTimer = preparationTime;
                }
                break;

            case CalibrationState.PrepareCurl:
                calibrationTimer -= Time.deltaTime;
                if (calibrationTimer <= 0)
                {
                    Debug.Log("Hold the curl position...");
                    calibrationState = CalibrationState.CurlPosition;
                    calibrationTimer = calibrationTime;
                }
                break;

            case CalibrationState.CurlPosition:
                calibrationTimer -= Time.deltaTime;
                if (calibrationTimer <= 0)
                {
                    curlPosition = currentAcceleration;
                    Debug.Log("Curl position calibrated: " + curlPosition);
                    calibrationState = CalibrationState.Complete;
                    _gm.levelStarted = true;
                }
                break;

            case CalibrationState.Complete:
                if (!dataSaved)
                {
                    SaveCalibrationData();
                    dataSaved = true;
                }
               
                ProcessSensorData();
                break;
        }

    }

    public void SaveCalibrationData()
    {
        if (profileManager != null)
        {
            CalibrationData profile = new CalibrationData(
                "DefaultProfile",
                restPosition,
                curlPosition
            );
            profileManager.SaveProfile(profile);
            Debug.Log("Calibration data saved to default profile.");
        }
    }

    public void ProcessSensorData()
    {
        bool isInRestPosition = Vector3.Distance(currentAcceleration, restPosition) < restPositionThreshold;
        bool isInCurlPosition = Vector3.Distance(currentAcceleration, curlPosition) < curlPositionThreshold;
        
        if (bicepCurlDetector.UpdatePositions(isInRestPosition, isInCurlPosition))
        {
            player.Attack();
            repCount = bicepCurlDetector.GetRepCount();
        }
    }

    public string GetCalibrationStatus()
    {
        switch (calibrationState)
        {
            case CalibrationState.NotStarted:
                return "Calibration \n not started.";
            case CalibrationState.PrepareRest:
                return $"Prepare for \n rest position...";
            case CalibrationState.RestPosition:
                return $"Hold the \n rest position...";
            case CalibrationState.PrepareCurl:
                workoutSR.sprite = workoutSprites[0];
                return $"Prepare for \n curl position...";
            case CalibrationState.CurlPosition:
                workoutSR.sprite = workoutSprites[1];
                return $"Hold the \n curl position...";
            case CalibrationState.Complete:
                if (_gm != null) _gm.levelStarted = true;
                return "Calibration complete! Start exercising.";
            default:
                return "Unknown state.";
        }
    }

    public int GetRepCount()
    {
        return repCount;
    }
    
    public bool IsConnected()
    {
        return isConnected;
    }

    public void ResetCalibration()
    {
        if (profileManager != null && profileManager.ProfileExists("DefaultProfile"))
        {
            profileManager.DeleteProfile("DefaultProfile");
            Debug.Log("Default profile deleted.");
        }

        calibrationState = CalibrationState.NotStarted;
        dataSaved = false;
        restPosition = Vector3.zero;
        curlPosition = Vector3.zero;
        repCount = 0;
        
        if (bicepCurlDetector != null)
        {
            bicepCurlDetector.ResetRepCount();
        }

        Debug.Log("Calibration reset. Please recalibrate.");
    }

    void OnApplicationQuit()
    {
        threadRunning = false;
        
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join(500);
        }
        
        if (client != null && client.Connected)
        {
            client.Close();
        }
    }
}


