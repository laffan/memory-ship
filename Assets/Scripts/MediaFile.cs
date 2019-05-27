using System.IO;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System; 

public class MediaFile : IComparable<MediaFile>
{
  public FileInfo file;
  public string fileType;
  
  public MediaFile( FileInfo newFile )
  {
  file = newFile;
  fileType = Path.GetExtension(file.ToString());
  }

  // Method required by iComparable
  public int CompareTo(MediaFile other)
  {
    return 1;
  }
}