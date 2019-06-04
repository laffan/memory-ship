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

    // Game objects that will be moved by the serial input
    // The camera will follow them smoothly
    private Transform cameraIndicator;
    private Transform lookIndicator;

    private Vector3 lastLookPosition;
    private Vector3 lastCameraPosition;

    private Vector3 newLookPosition;
    private Vector3 newCameraPosition;

    public static string serialReciever;

    public Transform camera;
    public float smoothSpeed = 20f;
    public Vector3 cameraOffset;
    List<string> visibleVideos = new List<string>();
    private MediaManager mediaManager;
    Camera cam;


    // Initialization
    void Start()
    {
      serialController = serialContainer.Find("SerialController").GetComponent<SerialController>();

      // Attach game object instances to indicators
      cameraIndicator = serialContainer.Find("CameraIndicator");
      lookIndicator = serialContainer.Find("LookAtIndicator");

      // Assign camera to "cam" var for raycasting
      // DEBUG : WHY AREN'T YOU USING "camera" here???
      cam = camera.GetComponent<Camera>();
      mediaManager = transform.GetComponent<MediaManager>();

      positionIndicators("0");

      camera.transform.position = cameraIndicator.position;
      camera.transform.LookAt(lookIndicator.position);



  }

    void Update(){

      checkVisibleMedia(  );

  }

    // Executed each frame
    void FixedUpdate()
    {
      // Get encoder val from serial interface
      string encoderVal = returnSerialMessage();
      // Set reciever val to it can be sent in SerialReciever.cs
      serialReciever = encoderVal;
      positionIndicators(encoderVal);

  }

    void LateUpdate()
    {
    positionCameraSmooth();

  }

    void positionIndicators( string encoderVal ){

      // Convert val to int
      int encoderInt = Int32.Parse(encoderVal);
      float cameraPathRadius = MediaManager.mediaRadius - 14;
      // By sending the LookAtIndicator way out it smooths the motion.
      float lookAtRadius = MediaManager.mediaRadius + 100;

      // Changing 80 for both remainder and theta will change change # of clicks for single video.
      int remainder = encoderInt % (80 * (int)MediaManager.videoCount);
      float theta = ((float)remainder / (80 * MediaManager.videoCount)) * (2 * (float)Math.PI);

      // Debug.Log("videoCount: " + MediaManager.videoCount + " || encoderInt: " + encoderInt + " || cameraPathRadius: " + cameraPathRadius + " || remainder: " + remainder + " || theta: " + theta);
      // Debug.Log("Sin(theta): " + (float)Math.Sin(theta) + " || Cos(theta): " + (float)Math.Cos(theta));
      // Debug.Log("theta * ( 360 / (2 * (float)Math.PI )): " + theta * (360 / (2 * (float)Math.PI)) );
      // Debug.Log("---------------------------------------");

      newCameraPosition = new Vector3(((float)Math.Sin(theta) * cameraPathRadius), 0, ((float)Math.Cos(theta) * cameraPathRadius));
      newLookPosition = new Vector3(((float)Math.Sin(theta) * lookAtRadius), 0, ((float)Math.Cos(theta) * lookAtRadius));

      lastCameraPosition = cameraIndicator.position;
      lastLookPosition = lookIndicator.position;

      cameraIndicator.transform.position = newCameraPosition;
      lookIndicator.transform.position = newLookPosition;

  }

    void positionCameraSmooth(){

      Vector3 smoothedCameraPosition = Vector3.Lerp(camera.position, newCameraPosition, 10 * Time.deltaTime);
      Vector3 smoothedLookPosition = Vector3.Lerp(lastLookPosition, newLookPosition, 2 * Time.deltaTime);

      // Smoothly move camera to serialIndicator's position
      camera.transform.position = smoothedCameraPosition;
      camera.transform.LookAt(smoothedLookPosition);
    
    }

    private void checkVisibleMedia(  ) {

        // Raycast out from camera
        RaycastHit hitInfo;
        var newVisibleVideos = new List<string>();

        // Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        Vector3[] rays = new [] { 
                                  new Vector3(cam.pixelWidth/2, cam.pixelHeight/2, 0), 
                                  new Vector3(0, cam.pixelHeight/2, 0), 
                                  new Vector3(cam.pixelWidth -1, cam.pixelHeight/2, 0 ) 
                                };
        
        foreach (Vector3 rayVector in rays){
            
          Ray ray = cam.ScreenPointToRay(rayVector);
          bool didRayHit = Physics.Raycast(ray, out hitInfo, 100f);
  
          // Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 100);

          // If ray hits something, send transform name to MediaManager::PlaySingleVideo 
          // so only 1 video plays
          if (didRayHit)
          {
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
