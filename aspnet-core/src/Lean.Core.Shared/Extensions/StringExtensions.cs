using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string src, string target)
        {
            return string.Equals(src, target, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
