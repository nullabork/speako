using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using speako.Settings;

namespace speako.Services.Providers.AWS
{

  public class AWSSettings : JsonSerializable<AWSSettings>
  { 

    public string ApiKey = "";

    public string Name = "AWS";
  }
}
