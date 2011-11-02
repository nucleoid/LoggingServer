
using System;
using System.Reflection;

namespace LoggingServer.Tests
{
    public static class ReflectionExtensions
    {
        private const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static void InvokeMethod(this object obj, string methodName, object[] parameters)
        {
            Type type = obj.GetType();

            MethodInfo info = type.GetMethod(methodName, DefaultFlags);
            if (info == null)
                throw new Exception("Method not found");

            info.Invoke(obj, parameters);
        }

        public static void SetProperty<T>(this object obj, string propertyName, object value)
        {
            PropertyInfo info = typeof(T).GetProperty(propertyName, DefaultFlags);

            if (info == null)
                throw new Exception("Property not found");

            info.SetValue(obj, value, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, null, null);
        }

        public static object GetField(this object obj, string fieldName)
        {
            FieldInfo info = obj.GetType().GetField(fieldName, DefaultFlags);

            if (info == null)
                throw new Exception("Field not found");

            return info.GetValue(obj);
        }
    }
}
