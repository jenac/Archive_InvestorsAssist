using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Utility.Security
{
    public static class SecurityExtension
    {
        public static SecureString ToSecureString(this string value)
        {
            var securedValue = new SecureString();
            if (value.Length > 0)
            {
                foreach (var c in value.ToCharArray()) securedValue.AppendChar(c);
            }
            return securedValue;
        }

        public static string ToPlainString(this SecureString value)
        {
            IntPtr plainString = IntPtr.Zero;
            try
            {
                plainString = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(plainString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(plainString);
            }
        }
    }
}
