using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggingServer.Common.Extensions
{
    public static class EnumExtensions
    {
        public static bool TestFor(this Enum flags, Enum test)
        {
            if (flags.GetType() != test.GetType())
                throw new InvalidOperationException("Test value was not a member of the bitmask");

            if (!Enum.IsDefined(flags.GetType(), test))
                throw new InvalidOperationException("Tests only valid for single member of enumeration");

            if (flags.GetType().GetCustomAttributes(typeof(FlagsAttribute), true).Count() == 0)
                throw new InvalidOperationException("Enumeration is not a bitmask");

            var bitfield = Convert.ToInt32(flags);
            var actual = Convert.ToInt32(test);

            return ((bitfield & actual) == actual);
        }

        public static IEnumerable<T> AsEnumerable<T>()
        {
            var t = typeof(T);

            if (t.IsEnum == false)
                throw new NotSupportedException("The type must be of Enum.");

            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
