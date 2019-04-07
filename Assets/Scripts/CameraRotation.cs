// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.IO.Ports;

// public class CameraRotation : MonoBehaviour
// {

//     SerialPort sp = new SerialPort("/dev/cu.usbmodem14101", 9600); // port, baud rate

    
//     public float RCspeed = 2f;
//     public float mouseSpeed = 3.5f;
//     private float X;
//     private float Y;

//     void Start(){
//         sp.Open();
//         sp.ReadTimeout = 1;
//     }


//      void FixedUpdate() {

//          if (sp.IsOpen){
//              try {
//                  // Get input from rotary encoder
//                 float RErotation = -sp.ReadByte();
//                 Debug.Log( sp.ReadByte() );

//                 // ----------------------------------
//                 // v1 - Original Rotation (jumpy)
//                 transform.Rotate(new Vector3(0, sp.ReadByte() * RCspeed, 0));
//                 Y = transform.rotation.eulerAngles.y;                
//                 transform.rotation = Quaternion.Euler(0, RErotation * RCspeed, 0);

//                 // -----------------------------------
//                 // v2 - Using RotateTowards

//                 // current:	The vector being managed.
//                 // target:	The vector.
//                 // maxRadiansDelta:	The distance between the two vectors in radians.
//                 // maxMagnitudeDelta:	The length of the radian.

//                 // Vector3 currentPos = transform.rotation;
//                 // Vector3 targetDir = currentPos + new Vector3(0, RErotation * RCspeed, 0);
//                 // float step = RCspeed * Time.deltaTime;
//                 // Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
//                 // transform.rotation = Quaternion.LookRotation(newDir);

//                 // -----------------------------------
//                 // NOTE TO SELF
//                 // The rotary encoder should be (jumpily) changing the position of an object that the
//                 // camera (smoothly) follows using RotateTowards.


             
//              } catch ( System.Exception ){

//              }
//          } else {
//             // If no arduino, use mouse
//             if(Input.GetMouseButton(0)) {
//                 transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X") * mouseSpeed, 0));
//                 Y = transform.rotation.eulerAngles.y;
//                 transform.rotation = Quaternion.Euler(0, Y, 0);
            
//             }
//          }


//      }
// }
