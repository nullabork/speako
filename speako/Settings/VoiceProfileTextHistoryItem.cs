using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace speako.Settings
{
  public class VoiceProfileTextHistoryItem
  {
    public VoiceProfileTextHistoryItem()
    {
      
    }

    public string GUID { get; set; } = Guid.NewGuid().ToString();

    public string ProviderGUID { get; set; }

    public string VoiceGUID { get; set; }

    public DateTime DateTime { get; set; }

    public string 

    public string CastType => GetType().ToString();
  }

  
}
