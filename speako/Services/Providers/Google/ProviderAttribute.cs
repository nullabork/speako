using speako.Common;


namespace speako.Services.Providers.Google
{
  public class ProviderAttribute
  {
    public double Min { get; set; }
    public double Max { get; set; }
    public double Default { get; set; }

    public double MapMin { get; set; } = 0;
    public double MapMax { get; set; } = 100;

    public double GetValue (int value)
    {
      return RangeConverter.ConvertRange<double>(Convert.ToDouble(value), MapMin, MapMax, Min, Max);
    }

    public int GetMapped (double value)
    {
      return Convert.ToInt16(RangeConverter.ConvertRange<double>(value, Min, Max , MapMin, MapMax));
    }

    public int GetDefault()
    {
      return Convert.ToInt16(RangeConverter.ConvertRange<double>(Default, Min, Max, MapMin, MapMax));
    }
  }
}




