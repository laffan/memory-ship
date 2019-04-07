using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class CameraManager : MonoBehaviour
{


    SerialPort sp = new SerialPort("/dev/cu.usbmodem14101", 9600); // port, baud rate

    
    public float RCspeed = 2f;
    public float mouseSpeed = 3.5f;
    public Transform camera;
    private float X;
    private float Y;

    void Start(){
        sp.Open();
        sp.ReadTimeout = 1;
    }

     void FixedUpdate() {

         if (sp.IsOpen){
             try {
                 // Get input from rotary encoder
                float RErotation = -sp.ReadByte();
                Debug.Log( sp.ReadByte() );

                // Apply as transform to camera
                camera.transform.position =  new Vector3(RErotation * RCspeed , 0 , 0);
                // camera.transform.position += transform.forward * moveHorizontal * Time.deltaTime;

             
             } catch ( System.Exception ){

             }
         } else {
            // If no arduino, use arrow keys
            // float moveHorizontal = Input.GetAxis ("Horizontal");
            // camera.transform.position += transform.forward * moveHorizontal * Time.deltaTime;


         }


     }
}
