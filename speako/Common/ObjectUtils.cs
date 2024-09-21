using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace speako.Common
{
    public static class ObjectUtils
    {


    public static T Clone<T>(this T obj) where T : class
    {
      if (obj == null)
        return null;

      // Retrieve the non-public MemberwiseClone method from the object's type
      MethodInfo memberwiseCloneMethod = obj.GetType().GetMethod(
          "MemberwiseClone",
          BindingFlags.Instance | BindingFlags.NonPublic
      );

      if (memberwiseCloneMethod == null)
        throw new InvalidOperationException("MemberwiseClone method not found.");

      // Invoke MemberwiseClone and cast the result to T
      return (T)memberwiseCloneMethod.Invoke(obj, null);
    }

    public static void CopyProperties<T>(T source, T destination)
    {
      // Iterate the Properties of the destination instance and  
      // populate them from their source counterparts  
      PropertyInfo[] destinationProperties = destination.GetType().GetProperties();
      foreach (PropertyInfo destinationPi in destinationProperties)
      {
        if (destinationPi.CanWrite)
        {
          PropertyInfo sourcePi = source.GetType().GetProperty(destinationPi.Name);
          destinationPi.SetValue(destination, sourcePi.GetValue(source, null), null);
        }
      }
    }


    public static bool PropertiesAreNotNull<T>(T obj, string[] check)
    {
      var properties = obj.GetType().GetProperties();

      return !check.Any(key =>
      {
        var property = properties.FirstOrDefault(p => p.Name == key);
        var isNull = property == null || string.IsNullOrEmpty(property.GetValue(obj)?.ToString());

        return isNull;
      });
    }
  }
}
