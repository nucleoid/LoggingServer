using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace LoggingServer.Common.LayoutRenderers
{
    [LayoutRenderer("entryassemblydescription")]
    public class EntryAssemblyDescriptionLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var assm = Assembly.GetEntryAssembly();
            var customAttribute = assm.GetCustomAttributes(false).OfType<AssemblyDescriptionAttribute>().FirstOrDefault();
            var value = customAttribute == null ? string.Empty : customAttribute.Description;

            builder.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
        }
    }
}