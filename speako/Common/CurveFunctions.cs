using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Common
{
  public static class CurveFunctions
  {
    public static Func<double, double> COSP(double steepness = 2, double position = 0.5)
    {
      return (x) =>
      {
        return Math.Round(Math.Pow(Math.Cos((x - position) * (Math.PI / 2)), steepness) * 10000) / 10000;
      };
    }

    public static Func<double, double> SIGMOID(double steepness = 1, double position = 10)
    {
      return (x) =>
      {
        return 1 / (1 + Math.Exp(-steepness * (x - position)));
      };
    }
  }
}
