using System.IO;
using UnityEngine;
using System.Collections;
using System; 

public class MediaFile : IComparable<MediaFile>
{
    public FileInfo file;
    public string fileType;
    
    public MediaFile( FileInfo newFile )
    {
        file = newFile;
        fileType = Path.GetExtension(file.ToString());

        // string filename = Path.GetFileNameWithoutExtension(file.ToString());

    }

    // Method required by iComparable
    public int CompareTo(MediaFile other)
    {
        return 1;
        // if(other == null)
        // {
        //     return 1;
        // }
        // return power - other.power;
    }
}