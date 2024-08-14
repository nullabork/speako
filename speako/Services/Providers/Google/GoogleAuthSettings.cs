using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using speako.Settings;

namespace speako.Services.Providers.Google
{

  public class GoogleAuthSettings : JsonSerializable<GoogleAuthSettings>
  { 

    [JsonProperty("type")]
    public string? Type { get; set; } = "service_account";

    [JsonProperty("project_id")]
    public string? ProjectId { get; set; }


    private string _privateKey;
    [JsonProperty("private_key_id")]
    public string? PrivateKeyId {
      get { return _privateKey; }
      set { _privateKey = value.Replace("\\n", "\n"); } 
    }

    [JsonProperty("private_key")]
    public string? PrivateKey { get; set; }

    [JsonProperty("client_email")]
    public string? ClientEmail { get; set; }

    [JsonProperty("client_id")]
    public string? ClientId { get; set; }

    [JsonProperty("auth_uri")]
    public string AuthUri { get; set; } = "https://accounts.google.com/o/oauth2/auth";

    [JsonProperty("token_uri")]
    public string TokenUri { get; set; } = "https://oauth2.googleapis.com/token";

    [JsonProperty("auth_provider_x509_cert_url")]
    public string AuthProviderX509CertUrl { get; set; } = "https://www.googleapis.com/oauth2/v1/certs";

    [JsonProperty("client_x509_cert_url")]
    public string ClientX509CertUrl { get; set; } = "https://www.googleapis.com/robot/v1/metadata/x509/server%40talk-bork.iam.gserviceaccount.com";
    public string Name { get; set; } = "Google";

    protected override string BeforeSave(GoogleAuthSettings googleAuthSettings, string json)
    {
      return json.Replace("\\\\n", "\\n");
    }


  }
}
