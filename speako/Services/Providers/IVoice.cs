﻿using Newtonsoft.Json;

namespace speako.Services.Providers
{
  public interface IVoice
  {
    string Name { get;}

    string Language { get;}

    string Id { get; }
  }
}