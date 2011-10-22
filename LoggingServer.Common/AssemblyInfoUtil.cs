using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace LoggingServer.Common
{
    public static class AssemblyInfoUtil
    {
        public static string Company(Assembly assembly)
        {
            var customAttribute = assembly.GetCustomAttributes(false).OfType<AssemblyCompanyAttribute>().FirstOrDefault();
            var value = customAttribute == null ? string.Empty : customAttribute.Company;
            return value;
        }

        public static string Description(Assembly assembly)
        {
            var customAttribute = assembly.GetCustomAttributes(false).OfType<AssemblyDescriptionAttribute>().FirstOrDefault();
            var value = customAttribute == null ? string.Empty : customAttribute.Description;
            return value;
        }

        public static string Guid(Assembly assembly)
        {
            var customAttribute = assembly.GetCustomAttributes(false).OfType<GuidAttribute>().FirstOrDefault();
            var value = customAttribute == null ? System.Guid.Empty.ToString() : customAttribute.Value;
            return value;
        }

        public static string Product(Assembly assembly)
        {
            var customAttribute = assembly.GetCustomAttributes(false).OfType<AssemblyProductAttribute>().FirstOrDefault();
            var value = customAttribute == null ? string.Empty : customAttribute.Product;
            return value;
        }

        public static string Title(Assembly assembly)
        {
            var customAttribute = assembly.GetCustomAttributes(false).OfType<AssemblyTitleAttribute>().FirstOrDefault();
            var value = customAttribute == null ? string.Empty : customAttribute.Title;
            return value;
        }

        public static string Version(Assembly assembly)
        {
            var version = assembly.GetName().Version.ToString();
            return version;
        }
    }
}
