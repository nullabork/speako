using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using speako.Settings;

namespace speako.Services.Providers.ElevenLabs
{

  public class ElevenLabsSettings : JsonSerializable<ElevenLabsSettings>
  {

    public string ApiKey = "";
        
    public string GetSettingsType => typeof(ElevenLabsSettings).ToString();

    public string Name = "Eleven Labs";
  }
}
