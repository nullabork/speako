using speako.Services.Providers;

namespace speako.Services.Auth
{
  public interface IAuthSettings
  {
    string GUID { get; set; }
    string Name { get; set; }
    string CastType { get; set; }
    ITTSProvider Provider { get; set; }

    IAuthSettings InitProvider();

    public bool IsConfigured();


    public string DisplayName { get; }

    public IAuthSettings Duplicate();

    //public string DisplayName { get => return Name; }

    //      return $"{Name}{(Provider != null ? " - " + Provider.Name : "")}";
    //}

  }
}
