using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using speako.Settings;

namespace speako.Services.Providers.Azure
{

  public class AzureSettings : JsonSerializable<AzureSettings>
  {
    public string ApiKey = "";

    public string Name = "Azure";
  }
}
