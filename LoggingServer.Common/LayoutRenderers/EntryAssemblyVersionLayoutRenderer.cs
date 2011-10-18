using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace LoggingServer.Common.LayoutRenderers
{
    [LayoutRenderer("entryassemblyversion")]
    public class EntryAssemblyVersionLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var assm = Assembly.GetEntryAssembly();
            var version = assm.GetName().Version.ToString();

            builder.Append(Convert.ToString(version, CultureInfo.InvariantCulture));
        }
    }
}