using NAudio.Wave;

namespace speako.Common
{
  /// <summary>
  /// A WaveProvider wrapper that streams audio data while trimming silence from the beginning and end in real-time.
  /// </summary>
  public class StreamingAudioTrimmerWaveProvider : IWaveProvider
  {
    private readonly IWaveProvider _sourceProvider;
    private readonly byte[] _buffer;
    private bool _trimmingDone;
    private bool _endOfAudioDetected;
    private int _bytesPerSample;

    /// <summary>
    /// The threshold level to consider as silence.
    /// </summary>
    private const float SilenceThreshold = 0.01f; // Adjust as needed

    /// <summary>
    /// Initializes a new instance of StreamingAudioTrimmerWaveProvider with the specified input IWaveProvider.
    /// </summary>
    /// <param name="sourceProvider">The input IWaveProvider providing audio data.</param>
    public StreamingAudioTrimmerWaveProvider(IWaveProvider sourceProvider)
    {
      _sourceProvider = sourceProvider ?? throw new ArgumentNullException(nameof(sourceProvider));
      WaveFormat = sourceProvider.WaveFormat;
      _bytesPerSample = WaveFormat.BitsPerSample / 8 * WaveFormat.Channels;
      _buffer = new byte[WaveFormat.SampleRate * _bytesPerSample]; // Allocate buffer based on sample rate and bytes per sample
      _trimmingDone = false;
      _endOfAudioDetected = false;
    }

    /// <summary>
    /// Gets the WaveFormat of the source provider.
    /// </summary>
    public WaveFormat WaveFormat { get; }

    /// <summary>
    /// Reads the audio data from the source provider while trimming silence dynamically.
    /// </summary>
    public int Read(byte[] buffer, int offset, int count)
    {
      if (_endOfAudioDetected)
      {
        return 0; // No more data to read
      }

      int bytesRead = 0;

      do
      {
        bytesRead = _sourceProvider.Read(_buffer, 0, Math.Min(_buffer.Length, count));

        if (bytesRead == 0)
        {
          _endOfAudioDetected = true;
          return 0; // End of source stream
        }
      }
      while (!ContainsSignificantAudio(_buffer, bytesRead));

      // If trimming has started, copy the data to the output buffer
      Array.Copy(_buffer, 0, buffer, offset, bytesRead);
      return bytesRead;
    }

    /// <summary>
    /// Checks if the buffer contains significant audio.
    /// </summary>
    private bool ContainsSignificantAudio(byte[] buffer, int bytesRead)
    {
      for (int i = 0; i < bytesRead; i += _bytesPerSample)
      {
        // Convert byte segment to a float (assuming 32-bit float samples)
        if (i + sizeof(float) <= bytesRead)
        {
          float sample = BitConverter.ToSingle(buffer, i);
          if (Math.Abs(sample) > SilenceThreshold)
            return true;
        }
      }
      return false;
    }
  }
}

