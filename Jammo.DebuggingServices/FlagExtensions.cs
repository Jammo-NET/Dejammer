using System;
using System.Linq;

namespace Jammo.DebuggingServices
{
    public static class FlagExtensions
    {
        public static bool QueryFlag<TEnum>(this TEnum e, TEnum flag) where TEnum : Enum
        {
            return e.HasFlag(flag);
        }
        
        public static bool QueryFlags<TEnum>(this TEnum e, params TEnum[] flags) where TEnum : Enum
        {
            return flags.All(f => e.HasFlag(f));
        }
    }
}