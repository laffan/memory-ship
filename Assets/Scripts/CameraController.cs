using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.Linq;

public class CameraController : MonoBehaviour
{
    public Transform serialContainer;
    private SerialController serialController;
    private Transform serialIndicator;

    public static string serialReciever;

    public Transform camera;
    public float smoothSpeed = 2f;
    public Vector3 cameraOffset;
    List<string> visibleVideos = new List<string>();
    private MediaManager mediaManager;
    Camera cam;


    // Initialization
    void Start()
    {
      serialController = serialContainer.Find("SerialController").GetComponent<SerialController>();
      serialIndicator = serialContainer.Find("Indicator");

      camera.position = serialIndicator.position + cameraOffset;
      cam = camera.GetComponent<Camera>();
      mediaManager = transform.GetComponent<MediaManager>();
	}

    void Update(){

      checkVisibleMedia(  );

    }

    // Executed each frame
    void LateUpdate()
    {
      // Get encoder val from serial interface
      string encoderVal = returnSerialMessage();
      // Set reciever val to it can be sent in SerialReciever.cs
      serialReciever = encoderVal;
      // Convert val to int
      int encoderInt = Int32.Parse(encoderVal);
      float cameraPathRadius = MediaManager.mediaRadius - 14;
      float lookAtRadius = MediaManager.mediaRadius - 6;

      // Changing 80 for both remainder and theta will change change # of clicks for single video.
      int remainder = encoderInt % (80 * (int)MediaManager.videoCount);
      float theta = ((float)remainder / (80 * MediaManager.videoCount)) * (2 * (float)Math.PI );

      // Debug.Log("videoCount: " + MediaManager.videoCount + " || encoderInt: " + encoderInt + " || cameraPathRadius: " + cameraPathRadius + " || remainder: " + remainder + " || theta: " + theta);
      // Debug.Log("Sin(theta): " + (float)Math.Sin(theta) + " || Cos(theta): " + (float)Math.Cos(theta));
      // Debug.Log("theta * ( 360 / (2 * (float)Math.PI )): " + theta * (360 / (2 * (float)Math.PI)) );
      // Debug.Log("---------------------------------------");

      Vector3 newCameraPosition = new Vector3(((float)Math.Sin(theta) * cameraPathRadius), 0, ((float)Math.Cos(theta) * cameraPathRadius));
      Vector3 newCameraView = new Vector3(((float)Math.Sin(theta) * lookAtRadius), 0, ((float)Math.Cos(theta) * lookAtRadius));

      camera.transform.position = newCameraPosition;
      camera.transform.LookAt( newCameraView );


    }


    // void LateUpdate()
    // {

    //     // Vector3 desiredPosition = serialIndicator.position + cameraOffset;

    //     // // Vector3 smoothedPosition = Vector3.Lerp( camera.transform.position, desiredPosition, smoothSpeed / Time.deltaTime );
        
    //     // Vector3 smoothedPosition = Vector3.Lerp( camera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime );
        
    //     // // Smoothly move camera to serialIndicator's position

    //     // camera.transform.position = smoothedPosition;
        
        
        
    // }

    private void checkVisibleMedia(  ) {

        // Raycast out from camera

        RaycastHit hitInfo;
        var newVisibleVideos = new List<string>();

        // Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        Vector3[] rays = new [] { 
                                    new Vector3(cam.pixelWidth/2, cam.pixelHeight/2, 0), 
                                    new Vector3(0, cam.pixelHeight/2, 0), 
                                    new Vector3( cam.pixelWidth -1, cam.pixelHeight/2, 0 ) 
                                };
        
        foreach (Vector3 rayVector in rays){
            
            Ray ray = cam.ScreenPointToRay(rayVector);

            bool didRayHit = Physics.Raycast(ray, out hitInfo, 100f);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 100);

            if (didRayHit)
            {
                // Send transform name to MediaManager::PlaySingleVideo so only 1 video plays
                newVisibleVideos.Add( hitInfo.transform.name );

            }
                
        }
        if ( !visibleVideos.SequenceEqual(newVisibleVideos) ) {
            visibleVideos = newVisibleVideos;
            mediaManager.PlayVideo( visibleVideos );
        }



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
