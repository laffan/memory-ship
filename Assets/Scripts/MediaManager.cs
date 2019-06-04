using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;           
using SimpleJSON;
using System.Linq;


[System.Serializable]
public class MediaManager : MonoBehaviour
{   

  List<MediaFile> mediaFiles = new List<MediaFile>();
  List<string> videoList = new List<string>();
  List<int> videoOrder = new List<int>();
  
  public bool shuffleMode = true;

  public Transform imageJPGPrefab;
  public Transform videoPrefab;
  public static float videoCount;

  
  public Transform mediaContainer;
  private Vector3 currentPosition = new Vector3(1, 1, 1);
  
  public int mediaOffset;
  public static float mediaRadius = 400;
  public Vector3 mediaSpacing = new Vector3( 20, 0 , 0);


  void Start()
  {
    // Spacing between each prefab
    mediaSpacing = new Vector3( mediaOffset, 0 , 0);
    // Load media in to mediaFiles
    LoadData();
    // Create a list of ints that can be shuffled to determine video order.
    videoOrder = Enumerable.Range(0, mediaFiles.Count - 1 ).ToList();

    videoCount = videoOrder.Count;

    KickOff();
  }


  private void KickOff( )
  {
   if ( shuffleMode ) 
    {
      videoOrder.Shuffle();
    }

    for (int i = 0; i < videoOrder.Count; i++)
    {
      // Package up parameters
      MediaFile media = mediaFiles[videoOrder[i]];
      int current = i;
      object[] parms = new object[2] { media, current };
      // Start coroutine with packaged parameters
      StartCoroutine( "LoadVideo", parms );
    }
  }

  private void LoadData()
  {
    string filePath = Path.Combine(Application.streamingAssetsPath, "mediaList.json");

    if(File.Exists(filePath))
    {
      string dataAsJson = File.ReadAllText(filePath); 
      var JSONdata = JSON.Parse(dataAsJson);

      for (int i = 0; i < JSONdata["mediaList"].Count; i++)
      {
        string path = Path.Combine(Application.streamingAssetsPath, JSONdata["mediaList"][i]);
        // Create new file from path
        FileInfo newFile = new FileInfo(path);
        // Create new media file from file.
        MediaFile newMediaFile = new MediaFile( newFile );
        // Add filename to string list so you can check against it
        // in PlayVideo() 
        videoList.Add( newMediaFile.file.Name );
        // Add to mediaFile List 
        mediaFiles.Add( newMediaFile );
      }
    }
    else
    {
      Debug.LogError("Cannot load game data!");
    }
    Debug.Log("Data Loaded");
  }
 
  void LoadVideo( object[] parms  )
  {
    // Unpackage parameters
    MediaFile media = (MediaFile)parms[0];
    int current = (int)parms[1];
    int count = videoOrder.Count;

    // Load to WWW
    string wwwFilePath = "file://" + media.file.FullName.ToString();
    
    // Instantiate prefab container around circle
    // Math from https://docs.unity3d.com/Manual/InstantiatingPrefabs.html

    float angle = current * (Mathf.PI * 2) / count;
    float sideLength = 17;
    float x = Mathf.Cos(angle) *  mediaRadius;
    float z = Mathf.Sin(angle) *  mediaRadius;
    Vector3 pos = transform.position + new Vector3(x, 0, z);
    float angleDegrees = -angle * Mathf.Rad2Deg + 90;
    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);

    Transform video = (Transform) Instantiate(videoPrefab, pos, rot);

    // Name the container
    video.transform.name = media.file.Name;
    // Put inside mediaContainer game object.
    video.transform.parent = mediaContainer.transform;

    // Locate correct GameObjects w/in the prefab
    Transform label = video.Find("Label");
    Transform surface = video.Find("Surface");

    // DEBUG : Update label
    // label.GetComponent<TextMesh>().text = media.file.FullName.ToString();

    // Attach Video to surface and pause it
    VideoClip clip = Resources.Load<VideoClip>(wwwFilePath) as VideoClip;
    surface.GetComponent<VideoPlayer>().url = wwwFilePath;
    //Assign the Audio from Video to AudioSource to be played
    surface.GetComponent<VideoPlayer>().EnableAudioTrack(0, true);
    // Helps with syncing ... apparently.
    surface.GetComponent<VideoPlayer>().controlledAudioTrackCount = 1;
    // Using stop instead of pause for lag issues
    surface.GetComponent<VideoPlayer>().Stop();
  }

  public void PlayVideo( List<string> visibleVideos ){

    // Pause these vids
    var invisibleVids = videoList.Except(visibleVideos);
    // Debug.Log( "------------------------------------------");
    // Debug.Log( "Playing " + visibleVideos.Count() + " videos");
    // Debug.Log( "Pausing " + invisibleVids.Count() + " videos");

    foreach (string videoName in visibleVideos)
    {
      Transform media = mediaContainer.transform.Find( videoName);
      Transform surface = media.Find("Surface");

      if ( !surface.GetComponent<VideoPlayer>().isPlaying) {

  

        surface.GetComponent<VideoPlayer>().Play();
      }
    }
    foreach (string videoName in invisibleVids)
    {
      Transform media = mediaContainer.transform.Find( videoName);
      Transform surface = media.Find("Surface");
        surface.GetComponent<VideoPlayer>().Stop();
    }
  }
}

