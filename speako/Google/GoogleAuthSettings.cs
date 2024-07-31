using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;

namespace speako.Google
{
    public class GoogleAuthSettings : JsonSerializable<GoogleAuthSettings>
    {
        protected override string FilePath => Path.Combine("config", "GoogleAuth.json");

        [JsonProperty("type")]
        public string? Type { get; set; } = "asdasd";

        [JsonProperty("project_id")]
        public string? ProjectId { get; set; }

        [JsonProperty("private_key_id")]
        public string? PrivateKeyId { get; set; }

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
    }
}
