using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace LoggingServer.Common.LayoutRenderers
{
    [LayoutRenderer("entryassemblyproduct")]
    public class EntryAssemblyProductLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var assm = Assembly.GetEntryAssembly();
            var customAttribute = assm.GetCustomAttributes(false).OfType<AssemblyProductAttribute>().FirstOrDefault();
            var value = customAttribute == null ? string.Empty : customAttribute.Product;

            builder.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
        }
    }
}