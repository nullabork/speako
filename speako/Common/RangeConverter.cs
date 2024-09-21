namespace speako.Common
{
  public static class RangeConverter
  {
    /// <summary>
    /// Converts a value from one range to another for any numeric type.
    /// </summary>
    /// <typeparam name="T">The numeric type (e.g., int, float, double).</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="fromMin">The minimum value of the original range.</param>
    /// <param name="fromMax">The maximum value of the original range.</param>
    /// <param name="toMin">The minimum value of the target range.</param>
    /// <param name="toMax">The maximum value of the target range.</param>
    /// <returns>The value converted to the target range.</returns>
    public static T ConvertRange<T>(T value, T fromMin, T fromMax, T toMin, T toMax) where T : struct, IConvertible
    {
      // Convert all inputs to double for calculation
      double valueD = Convert.ToDouble(value);
      double fromMinD = Convert.ToDouble(fromMin);
      double fromMaxD = Convert.ToDouble(fromMax);
      double toMinD = Convert.ToDouble(toMin);
      double toMaxD = Convert.ToDouble(toMax);

      // Calculate the proportion of the value within the original range
      double proportion = (valueD - fromMinD) / (fromMaxD - fromMinD);

      // Scale the proportion to the target range
      double result = toMinD + (proportion * (toMaxD - toMinD));

      // Convert the result back to the original type and return
      return (T)Convert.ChangeType(result, typeof(T));
    }


    public static T Curve<T>(T value, T fromMin, T fromMax, Func<T, T> transformer) where T : IConvertible
    {
      // Normalize value to the range [0, 1] within the provided FromMin and FromMax
      double normalizedValue = (Convert.ToDouble(value) - Convert.ToDouble(fromMin)) / (Convert.ToDouble(fromMax) - Convert.ToDouble(fromMin));

      // Apply the transformer function to the normalized value
      double transformedValue = Convert.ToDouble(transformer((T)Convert.ChangeType(normalizedValue, typeof(T))));

      // Return the transformed value in the original range
      return (T)Convert.ChangeType(transformedValue, typeof(T));
    }
  }
}
