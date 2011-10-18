using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace LoggingServer.Common.LayoutRenderers
{
    [LayoutRenderer("entryassemblyguid")]
    public class EntryAssemblyGuidLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var assm = Assembly.GetEntryAssembly();
            var customAttribute = assm.GetCustomAttributes(false).OfType<GuidAttribute>().FirstOrDefault();
            var value = customAttribute == null ? Guid.Empty.ToString() : customAttribute.Value;

            builder.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
        }
    }
}