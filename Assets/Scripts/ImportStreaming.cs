using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class ImportStreaming : MonoBehaviour
{

    public Transform imageJPGPrefab;
    public Transform videoPrefab;
    private float xPos = 1;
    private float yPos = 1;

    // Create a dictionary that accepts "filename" and "filetype" as 2 strings.

    List<MediaFile> mediaFiles = new List<MediaFile>();


    void Start()
    {

        CheckForMedia( );
        // InvokeRepeating("CheckForMedia", 0f, 3.0f);

    }

    void CheckForMedia( )
    {
        print("Streaming Assets Path: " + Application.streamingAssetsPath);

        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] directoryScan = directoryInfo.GetFiles("*.*");


        // Only add new files from directory scan
        foreach (FileInfo file in directoryScan ) {
            mediaFiles.Add( new MediaFile( file ));
        }


         foreach(MediaFile media in mediaFiles)
        {
            StartCoroutine( "SortFileTypes", media );
        }

    }

    IEnumerator SortFileTypes( MediaFile media )
    {

        // Look inside object
        //var output = JsonUtility.ToJson(media, true);
        //Debug.Log(output);


        if ( media.fileType == ".meta" || media.fileType == ".DS_Store" )
        {
            yield break;
        }

        else {

            // Create Prefab, add to scene, hidden.

            switch (media.fileType)
            {
                case ".jpg":
                    // StartCoroutine( "LoadImageJPG", media );
                    print("is JPG");

                    break;
                case ".mp4":
                    StartCoroutine("LoadVideo", media);
                    break;
                case ".mp3":
                    print("is audio");
                    break;
                case ".png":
                    print("is transparent img");
                    break;
                default:
                    print("Unknown extension:" + media.fileType );
                    break;
            }

            // Place Prefab and reveal
            // Modes : Wall, Passage, Map



        }
    }

    IEnumerator LoadImageJPG( MediaFile media )
    {

        // Load the image
        string wwwFilePath = "file://" + media.file.FullName.ToString();
        WWW www = new WWW(wwwFilePath);
        yield return www;

        // Instantiate a prefab to hold image
        Transform jpgImage = (Transform) Instantiate(imageJPGPrefab, new Vector3(xPos, yPos, transform.position.z), transform.rotation);
        xPos = xPos + 15;

        if (xPos > 50 ) {
            yPos = yPos + 15;
            xPos = 0;
        }

        // Locate the gameObjects w/in the prefab
        Transform label = jpgImage.Find("Label");
        Transform surface = jpgImage.Find("Surface");

        // Debug : Update label
        label.GetComponent<TextMesh>().text = media.file.FullName.ToString();

        // Create sprite from texture
        // Texture2D newTexture = Resources.Load <Texture2D>( www );
        // sprite = Sprite.Create (newTexture, new Rect (0, 0, newTexture.width, newTexture.height), new Vector2 (0.5f,0.5f));
        surface.GetComponent<SpriteRenderer>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));

    }

    void LoadVideo(MediaFile media)
    {

        // Load the image
        string wwwFilePath = "file://" + media.file.FullName.ToString();
        // WWW www = new WWW(wwwFilePath);
        // yield return www;

        // Instantiate a prefab to hold image
        Transform video = (Transform) Instantiate(videoPrefab, new Vector3(xPos, yPos, transform.position.z), transform.rotation);
        xPos = xPos + 15;

        if (xPos > 50 ) {
            yPos = yPos + 15;
            xPos = 0;
        }

        // Locate the gameObjects w/in the prefab
        Transform label = video.Find("Label");
        Transform surface = video.Find("Surface");

        // Debug : Update label
        label.GetComponent<TextMesh>().text = media.file.FullName.ToString();

        // Create sprite from texture
        // Texture2D newTexture = Resources.Load <Texture2D>( www );
        // sprite = Sprite.Create (newTexture, new Rect (0, 0, newTexture.width, newTexture.height), new Vector2 (0.5f,0.5f));
        // surface.GetComponent<SpriteRenderer>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));

        
        VideoClip clip = Resources.Load<VideoClip>(wwwFilePath) as VideoClip;

         surface.GetComponent<VideoPlayer>().url = wwwFilePath;
         surface.GetComponent<VideoPlayer>().Pause();



    }


    // Update is called once per frame
        void Update()
    {
        
    }
}

