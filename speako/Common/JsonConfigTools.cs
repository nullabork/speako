using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Common
{
  internal class JsonConfigTools
  {

    public static string GetDataDirectory()
    {
      var saveLocation = AppConfig.Default.DefaultSaveLocation;
      if (!String.IsNullOrWhiteSpace(AppConfig.Default.SaveLocation))
      {
        saveLocation = AppConfig.Default.SaveLocation;
      }
      var path = Path.GetFullPath(saveLocation);
      return path;
    }

    public static bool CopyDataLocation(string from, string to, bool recursive)
    {
      if (String.IsNullOrWhiteSpace(from) || String.IsNullOrWhiteSpace(to)) return false;

      if (!Directory.Exists(from)) return false;

      if (!Directory.Exists(to)) return false;

      //if from is empty we dont care
      if (Directory.GetFileSystemEntries(from).Length == 0) return true;


      var dir = new DirectoryInfo(from);

      // If the destination directory doesn't exist, create it
      DirectoryInfo[] dirs = dir.GetDirectories();

      // Get the files in the source directory and copy them to the destination directory
      foreach (FileInfo file in dir.GetFiles())
      {
        string targetFilePath = Path.Combine(to, file.Name);
        file.CopyTo(targetFilePath, overwrite: true);
      }

      // If recursive and copying subdirectories, recursively call this method
      if (recursive)
      {
        foreach (DirectoryInfo subDir in dirs)
        {
          string newDestinationDir = Path.Combine(to, subDir.Name);
          CopyDataLocation(subDir.FullName, newDestinationDir, true);
        }
      }

      return true;
    }
  }
}
