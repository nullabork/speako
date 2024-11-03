using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Common
{
  public class Prettier
  {
    public static string FormatSizeUnits(long bytes)
    {
      if (bytes >= 1_073_741_824) // 1 GiB
      {
        return (bytes / 1_073_741_824.0).ToString("0.00") + " GB";
      }
      else if (bytes >= 1_048_576) // 1 MiB
      {
        return (bytes / 1_048_576.0).ToString("0.00") + " MB";
      }
      else if (bytes >= 1024) // 1 KiB
      {
        return (bytes / 1024.0).ToString("0.00") + " KB";
      }
      else if (bytes > 1)
      {
        return bytes + " bytes";
      }
      else if (bytes == 1)
      {
        return bytes + " byte";
      }
      else
      {
        return "0 bytes";
      }
    }
  }
}
