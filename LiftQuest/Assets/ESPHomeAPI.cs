using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ESPHomeAPI : MonoBehaviour
{
    // Define server address and port (use the IP address of your Pico W)
    private string serverAddress = "172.20.10.3"; // Replace with your Pico's IP address
    private int port = 12345;

    private TcpClient tcpClient;
    private NetworkStream networkStream;
    private byte[] buffer;
    private float[] sensorData = new float[6];

    void Start()
    {
        ConnectToServer();
    }

    // Connect to the Raspberry Pi Pico W
    void ConnectToServer()
    {
        try
        {
            tcpClient = new TcpClient(serverAddress, port);
            networkStream = tcpClient.GetStream();
            buffer = new byte[24]; // 6 floats * 4 bytes per float = 24 bytes
            Debug.Log("Connected to server.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    void Update()
{
    // Read data from the server if there's data available
    if (networkStream != null && networkStream.DataAvailable)
    {
        try
        {
            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);

            if (bytesRead > 0)
            {
                // Convert received byte array into floats (accel and gyro data)
                for (int i = 0; i < 6; i++)
                {
                    sensorData[i] = BitConverter.ToSingle(buffer, i * 4); // 4 bytes per float
                }

                // Log the sensor data
                Debug.Log($"Accel: X={sensorData[0]:F2}, Y={sensorData[1]:F2}, Z={sensorData[2]:F2} | " +
                          $"Gyro: X={sensorData[3]:F2}, Y={sensorData[4]:F2}, Z={sensorData[5]:F2}");
                
                float speed = 0.1f; // Adjust speed as needed
                Vector3 movement = new Vector3(sensorData[0], sensorData[1], 0) * speed;
                transform.Translate(movement * Time.deltaTime);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading data: " + e.Message);
        }
    }
}


    void OnApplicationQuit()
    {
        // Close the connection when the application quits
        if (networkStream != null)
        {
            networkStream.Close();
        }

        if (tcpClient != null)
        {
            tcpClient.Close();
        }
    }
}
