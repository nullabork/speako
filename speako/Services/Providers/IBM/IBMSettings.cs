using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using speako.Settings;

namespace speako.Services.Providers.IBM
{

  public class IBMSettings : JsonSerializable<IBMSettings>
  {

    public string ApiKey = "";

    public string Name = "IBM";
  }
}
