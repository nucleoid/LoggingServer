using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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

        public static IEnumerable<TEnum> AsEnumerable<TEnum>()
        {
            var t = typeof(TEnum);

            if (t.IsEnum == false)
                throw new NotSupportedException("The type must be of Enum.");

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, bool valueSelected)
        {
            var values = from TEnum e in AsEnumerable<TEnum>()
                         select new { Id = e, Name = e.ToString() };
            var selected = valueSelected ? (object)enumObj : null;
            return new SelectList(values, "Id", "Name", selected);
        }

        public static IList<T> BitFieldAsEnumerable<T>(T bitValue)
        {
            var t = typeof(T);

            if (t.IsEnum == false)
                throw new NotSupportedException("The type must be of Enum.");

            var values = new List<T>();
            foreach (var enumValue in Enum.GetValues(typeof(T)).Cast<T>())
            {
                //hacky version of TestFor, because Enums can't be used in generics
                if (((Convert.ToInt32(bitValue) & Convert.ToInt32(enumValue)) == Convert.ToInt32(enumValue)))
                    values.Add(enumValue);
            }
            return values;
        }
    }
}
