using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{

    Ray RayOrigin;
    RaycastHit HitInfo;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Transform cameraTransform = Camera.main.transform;
        
        if(Physics.Raycast(cameraTransform.position,cameraTransform.forward, out HitInfo, 100.0f))
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100.0f, Color.yellow);
            Debug.Log( HitInfo );

        }   
        
        
    }
}
