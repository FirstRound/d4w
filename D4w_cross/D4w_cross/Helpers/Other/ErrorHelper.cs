using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace D4w_cross.Helpers
{
    class ErrorHelper
    {

        //lutaya di4, potom mb vipilit
        private static String ToCamelCase(String message)
        {
            var arr = message.Split('_');
            var res = "";
            StringBuilder sb = new StringBuilder();
            foreach (var word in arr)
            {
                sb.Append(Char.ToUpper(word[0]) + word.Substring(1).ToLowerInvariant());
            }
            sb[0] = Char.ToLower(sb[0]);
            return sb.ToString();
        }

        public static String Translate(String message)
        {
            return Application.Current.Resources[message].ToString();
        }
    }
}
