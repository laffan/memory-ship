using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleofPrefabs : MonoBehaviour
{
    // https://answers.unity.com/questions/28215/instantiate-prefabs-in-a-circle-or-elipsoid.html

    public Transform targetPrefab;
    public float numPoints = 10;
    public float radius = 10;
        

    // Start is called before the first frame update
    void Start()
    {



        for (var pointNum = 0; pointNum < numPoints; pointNum++)
        {

            // Camera position
            var camPos = Camera.main.gameObject.transform.position;

            // "i" now represents the progress around the circle from 0-1
            // we multiply by 1.0 to ensure we get a fraction as a result.
            var i = (pointNum * 1.0) / numPoints;
           
            // get the angle for this step (in radians, not degrees)
            var angle = (float) i * Mathf.PI * 2;
           
            // the X &amp; Y position for this angle are calculated using Sin &amp; Cos
            var x = Mathf.Sin(angle) * radius;
            var z = Mathf.Cos(angle) * radius;

            // Orient circle around camera
            var pos = new Vector3(x, 0, z) + camPos;
           
            // no need to assign the instance to a variable unless you're using it afterwards:
            var newPrefab = Instantiate (targetPrefab, pos, Quaternion.identity);

            newPrefab.transform.LookAt(camPos);


        }
        
    }

}
