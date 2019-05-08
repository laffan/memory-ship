using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class CameraController : MonoBehaviour
{
    public Transform serialContainer;
    private SerialController serialController;
    private Transform serialIndicator;
    public Transform camera;

    public float smoothSpeed = 10f;
    public Vector3 cameraOffset;


    Camera cam;


    // Initialization
    void Start()
    {

        serialController = serialContainer.Find("SerialController").GetComponent<SerialController>();
        serialIndicator = serialContainer.Find("Indicator");

        camera.position = serialIndicator.position + cameraOffset;

        cam = camera.GetComponent<Camera>();
	}

    void Update(){

        checkVisibleMedia(  );

    }

    // Executed each frame
    void FixedUpdate()
    {
        
        // Move indicator GO with encoder
        string encoderVal = returnSerialMessage();
        int encoderInt = Int32.Parse(encoderVal);
        serialIndicator.transform.position =  new Vector3(encoderInt , 0 , 0);


        Debug.Log( encoderInt );


    }

    void LateUpdate()
    {
        Vector3 desiredPosition = serialIndicator.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp( camera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime );
        // Smoothly move camera to serialIndicator's position
        camera.transform.position = smoothedPosition;
        
        
    }

    private void checkVisibleMedia(  ) {

        // Raycast out from camera

        RaycastHit hitInfo;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        // Vector3[] rays = new [] { 
        //                             new Vector3(cam.pixelWidth/2, cam.pixelHeight/2, 0), 
        //                             new Vector3(0, cam.pixelHeight/2, 0), 
        //                             new Vector3( cam.pixelWidth -1, cam.pixelHeight/2, 0 ) 
        //                         };
        
        // foreach (Vector3 rayVector in rays){
            
        //     Ray ray = cam.ScreenPointToRay(rayVector);

            bool didRayHit = Physics.Raycast(ray, out hitInfo, 100f);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 100);

            if (didRayHit)
            {
                // Send transform name to MediaManager::PlaySingleVideo so only 1 video plays
                transform.GetComponent<MediaManager>().PlaySingleVideo( hitInfo.transform.name );
            }
                
        // }
    }


    private string returnSerialMessage()
    {

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return "null";
        else 
            return message;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");            
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
            Debug.Log("Message arrived: " + message);


    }
}
