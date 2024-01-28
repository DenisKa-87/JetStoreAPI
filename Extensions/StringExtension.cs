using System.Runtime.CompilerServices;

namespace JetStoreAPI.Extensions
{
    public static class StringExtension
    {
        public static int? ParseInt(this string str)
        {
            int result = -1;
            if(Int32.TryParse(str, out result))
            {
                return result;
            }
            return null;
        }
        public static double? ParseDouble(this string str)
        {
            double k = 0;
            if(Double.TryParse(str, out k))
                return k;
            return null;
        }
    }
}
