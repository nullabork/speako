using speako.Common;

namespace speako.Services.Providers
{
  public class ProviderAttribute
  {
    /// <summary>
    /// The minimum value that the provider can accept.
    /// </summary>
    public double Min { get; set; }

    /// <summary>
    /// The maximum value that the provider can accept.
    /// </summary>
    public double Max { get; set; }

    /// <summary>
    /// The default value that the provider suggests as a starting point.
    /// </summary>
    public double Default { get; set; }

    /// <summary>
    /// The minimum value that should be shown to users in the UI. This is used to map the provider's range to a more user-friendly scale, such as 1-100.
    /// </summary>
    public double MapMin { get; set; } = 0;

    /// <summary>
    /// The maximum value that should be shown to users in the UI. This is used to map the provider's range to a more user-friendly scale, such as 1-100.
    /// </summary>
    public double MapMax { get; set; } = 100;

    /// <summary>
    /// Converts a value from the user-friendly range (MapMin to MapMax) to the provider's expected range (Min to Max).
    /// </summary>
    /// <param name="value">A value from the UI range (MapMin to MapMax) to be converted.</param>
    /// <returns>A value mapped to the provider's expected range (Min to Max).</returns>
    public double GetValue(int value)
    {
      return RangeConverter.ConvertRange<double>(Convert.ToDouble(value), MapMin, MapMax, Min, Max);
    }

    /// <summary>
    /// Converts a value from the provider's range (Min to Max) to the user-friendly range (MapMin to MapMax).
    /// </summary>
    /// <param name="value">A value from the provider's range (Min to Max) to be converted.</param>
    /// <returns>A value mapped to the UI-friendly range (MapMin to MapMax).</returns>
    public int GetMapped(double value)
    {
      return Convert.ToInt16(RangeConverter.ConvertRange<double>(value, Min, Max, MapMin, MapMax));
    }

    /// <summary>
    /// Returns the provider's default value, converted and mapped to the UI-friendly range (MapMin to MapMax).
    /// </summary>
    /// <returns>The provider's default value mapped to the UI-friendly range.</returns>
    public int GetDefault()
    {
      return Convert.ToInt16(RangeConverter.ConvertRange<double>(Default, Min, Max, MapMin, MapMax));
    }
  }
}
