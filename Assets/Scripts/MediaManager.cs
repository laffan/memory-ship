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

  public Transform mediaContainer;
  private Vector3 currentPosition = new Vector3(1, 1, 1);
  
  public int mediaOffset;
  public Vector3 mediaSpacing = new Vector3( 20, 0 , 0);


  void Start()
  {
    // Spacing between each prefab
    mediaSpacing = new Vector3( mediaOffset, 0 , 0);
    // Load media in to mediaFiles
    LoadData();
    // Create a list of ints that can be shuffled to determine video order.
    videoOrder = Enumerable.Range(0, mediaFiles.Count - 1 ).ToList();
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
      // Debug.Log( i + " - " + videoOrder[i] + " - " +  mediaFiles[videoOrder[i]].file.Name );
      StartCoroutine( "LoadVideo", mediaFiles[videoOrder[i]] );
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
 
  void LoadVideo(MediaFile media)
  {
    // Load to WWW
    string wwwFilePath = "file://" + media.file.FullName.ToString();
    // Instantiate prefab container
    Transform video = (Transform) Instantiate(videoPrefab, currentPosition, transform.rotation);
    // Name the container
    video.transform.name = media.file.Name;
    // Put inside mediaContainer game object.
    video.transform.parent = mediaContainer.transform;
    currentPosition = currentPosition + mediaSpacing;
    // Locate correct GameObjects w/in the prefab
    Transform label = video.Find("Label");
    Transform surface = video.Find("Surface");

    // DEBUG : Update label
    // label.GetComponent<TextMesh>().text = media.file.FullName.ToString();

    // Attach Video to surface and pause it
    VideoClip clip = Resources.Load<VideoClip>(wwwFilePath) as VideoClip;
    surface.GetComponent<VideoPlayer>().url = wwwFilePath;
    surface.GetComponent<VideoPlayer>().Pause();
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
        surface.GetComponent<VideoPlayer>().Pause();
    }
  }
}

