using speako.Services.Auth;
using speako.Services.Speak;
using speako.Services.VoiceProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.PreProcessors
{
  public enum TextHook
  {
    MESSAGE,
    VOICE
  }

  public enum StreamHook
  {

    MODIFY_STREAM,
    CREATE_STREAM
  }

  public interface IPreProcessor
  {
    // Process a string message before sending it to TTS (Text-to-Speech).
    virtual Task<PText>? Process(VoiceProfile vp, PText pText)
    {
      return null;
    }

    virtual Task<MemoryStream>? Process(VoiceProfile vp, MemoryStream? stream)
    {
      return null;
    }

    // Configure the processor with settings.
    Task<bool> Configure(IPreProcessorSettings settings);
  }
}
