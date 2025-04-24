// using UnityEngine;

// public class CubeMotionTracker : MonoBehaviour
// {
//     private Receiver receiver;
//     private Vector3 velocity;
//     private Vector3 position;

//     public float sensitivity = 1.0f;
//     public bool useGravityCompensation = true;

//     void Start()
//     {
//         receiver = FindObjectOfType<Receiver>();
//         if (receiver == null)
//         {
//             Debug.LogError("Receiver not found in the scene!");
//         }

//         position = transform.position;
//     }

//     void Update()
//     {
//         // if (receiver == null || !receiver.IsConnected())
//         //     return;

//         // Vector3 accel = receiver.GetCurrentAcceleration();

//         // Optional: remove gravity
//         if (useGravityCompensation)
//         {
//             accel -= new Vector3(0, -9.81f, 0); // might need to be (0, 9.81f, 0)
//         }


//         // Only move on the Y axis (for bicep curls)
//         position.y += accel.y * Time.deltaTime * sensitivity;

//         transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
//     }

//     public void ResetTracker()
//     {
//         velocity = Vector3.zero;
//         position = transform.position;
//     }
// }
