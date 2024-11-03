using Accord.Math;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using speako.Common;
using speako.Services.Auth;
using speako.Services.PostProcessors.Discord;
using System.ComponentModel;

namespace speako.Services.Providers.Google
{

  public class GoogleAuthSettings : IAuthSettings, INotifyPropertyChanged
  {

    public string DisplayName => "Google TTS";


    public event PropertyChangedEventHandler PropertyChanged;

    public IProviderSettingsControl SettingsControl()
    {
      return new GoogleSettingsControl();
    }

    public GoogleAuthSettings()
    {
      GUID = Guid.NewGuid().ToString();
      CastType = this.GetType().ToString();
    }

    [JsonIgnore()]
    public ITTSProvider Provider { get; set; }


    [JsonProperty("type")]
    public string? Type { get; set; } = "service_account";

    [JsonProperty("project_id")]
    public string? ProjectId { get; set; }


    private string _privateKey;
    [JsonProperty("private_key_id")]
    public string? PrivateKeyId {
      get { return _privateKey; }
      set { _privateKey = value?.Replace("\\n", "\n"); } 
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
    public string CastType { get; set; }

    public string GUID { get; set; }

    public async Task Init()
    {
      _provider = new GoogleTTSProvider();
      await _provider.Configure(this);
    }

    //this is like... do the fields at least look filled out
    public bool IsConfigured()
    {
      string[] check = ["Type", "ProjectId", "PrivateKeyId", "PrivateKey", "ClientEmail", "ClientId", "AuthUri", "TokenUri", "AuthProviderX509CertUrl", "ClientX509CertUrl"];
      return ObjectUtils.PropertiesAreNotNull(this, check);
    }

    ITTSProvider _provider { get; set; }

    public ITTSProvider GetProvider()
    {
      if (_provider == null)
      {
        _provider = new GoogleTTSProvider();
      }

      return _provider;
    }
  }
}
