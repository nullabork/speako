using System.Reflection;

namespace speako.Common
{
  class Compare
  {
    public static bool AreObjectsEqual<T>(T obj1, T obj2)
    {
      if (obj1 == null || obj2 == null)
        return object.Equals(obj1, obj2);

      if (obj1.GetType() != obj2.GetType())
        return false;

      var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
      foreach (var prop in properties)
      {
        var val1 = prop.GetValue(obj1);
        var val2 = prop.GetValue(obj2);
        if (!object.Equals(val1, val2))
          return false;
      } 

      return true;
    }

  }
}
