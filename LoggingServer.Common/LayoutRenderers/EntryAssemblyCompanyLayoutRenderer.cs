using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace LoggingServer.Common.LayoutRenderers
{
    [LayoutRenderer("entryassemblycompany")]
    public class EntryAssemblyCompanyLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var assm = Assembly.GetEntryAssembly();
            var customAttribute = assm.GetCustomAttributes(false).OfType<AssemblyCompanyAttribute>().FirstOrDefault();
            var value = customAttribute == null ? string.Empty : customAttribute.Company;

            builder.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
        }
    }
}