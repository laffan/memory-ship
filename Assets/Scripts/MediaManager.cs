using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;           
using SimpleJSON;


[System.Serializable]
public class MediaManager : MonoBehaviour
{   

    List<MediaFile> mediaFiles = new List<MediaFile>();
    public Transform imageJPGPrefab;
    public Transform videoPrefab;

    public Transform mediaContainer;
    private Vector3 currentPosition;
    
    public int mediaOffset;
    public Vector3 mediaSpacing = new Vector3( 20, 0 , 0);


    void Start()
    {
        currentPosition = new Vector3( 1, 1 ,1 );
        mediaSpacing = new Vector3( mediaOffset, 0 , 0);
        LoadData();

        foreach(MediaFile media in mediaFiles)
        {
            StartCoroutine( "SortFileTypes", media );
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

                FileInfo mediaFile = new FileInfo(path);
                mediaFiles.Add( new MediaFile( mediaFile ));
            }

        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }

        Debug.Log("Data Loaded");

    }
    
 
    IEnumerator SortFileTypes( MediaFile media )
    {
        // Look inside object
        var output = JsonUtility.ToJson(media, true);
        Debug.Log(output);
            
        if ( media.fileType == ".meta" || media.fileType == ".DS_Store" )
        {
            yield break;
        }
        else {
            switch (media.fileType)
            {
                case ".jpg":
                    StartCoroutine( "LoadImageJPG", media );
                    break;
                case ".mp4":
                    StartCoroutine("LoadVideo", media);
                    break;
                default:
                    print("Yo! Unknown extension:" + media.fileType );
                    break;
            }
        }

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

    IEnumerator LoadImageJPG( MediaFile media )
    {

        // Load to WWW
        string wwwFilePath = "file://" + media.file.FullName.ToString();
        WWW www = new WWW(wwwFilePath);
        yield return www;

        // Instantiate a prefab to hold image
        Transform jpgImage = (Transform) Instantiate(imageJPGPrefab, currentPosition, transform.rotation);
        // Put inside mediaContainer game object.
        jpgImage.transform.parent = mediaContainer.transform;
       
        currentPosition = currentPosition + mediaSpacing;

        // Locate the gameObjects w/in the prefab
        Transform label = jpgImage.Find("Label");
        Transform surface = jpgImage.Find("Surface");

        // Debug : Update label
        label.GetComponent<TextMesh>().text = media.file.FullName.ToString();

        // Apply image to texture
        surface.GetComponent<SpriteRenderer>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));

    }

    public void PlaySingleVideo( string hitName ){
     
        Debug.Log( "Playing " + hitName );

        foreach(Transform media in mediaContainer.transform )
        {
            Transform surface = media.Find("Surface");
            if ( media.name == hitName ) 
            {
                surface.GetComponent<VideoPlayer>().Play();
            }
            else 
            {
                surface.GetComponent<VideoPlayer>().Pause();
            }

        }
    }
}

