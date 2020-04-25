using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Utilities
{
    public static class StringHelpers
    {
        public static string EnumToProperString(string enumName)
        {
            return string.Concat(enumName.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
    }
}
