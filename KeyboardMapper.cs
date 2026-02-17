using System;
using System.Collections.Generic;
using System.Text;

namespace KeyboardLayoutSwitcher
{
    public static class KeyboardMapper
    {

        private static readonly Dictionary<char, char> EnToTh = new Dictionary<char, char>();
        private static readonly Dictionary<char, char> ThToEn = new Dictionary<char, char>();

        static KeyboardMapper()
        {
            // Verified Kedmanee Mapping (Exactly 94 characters)
            // Unshifted (47): `1234567890-= (13) + qwertyuiop[]\ (13) + asdfghjkl;' (11) + zxcvbnm,./ (10)
            // Shifted   (47): ~!@#$%^&*()_+ (13) + QWERTYUIOP{}| (13) + ASDFGHJKL:" (11) + ZXCVBNM<>? (10)
            string en = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?";
            string th = "_ๅ/-ภถุึคตจขชๆไำพะัีรนยบลฃฟหกดเ้่าสวงผปแอิืทมใฝ%+๑๒๓๔ู฿๕๖๗๘๙๐\"ฎฑธํ๊ณฯญฐ,ฅฤฆฏโฌ็๋ษศซ.()ฉฮฺ์?<ฒฬ";

            for (int i = 0; i < en.Length && i < th.Length; i++)
            {
                if (!EnToTh.ContainsKey(en[i])) EnToTh.Add(en[i], th[i]);
                if (!ThToEn.ContainsKey(th[i])) ThToEn.Add(th[i], en[i]);
            }
        }

        public static string Convert(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Detect if the string is likely Thai (contains any unique Thai characters)
            bool isThai = false;
            foreach (char c in input)
            {
                // Unique Thai characters are those that exist in ThToEn but are NOT standard English keys
                if (ThToEn.ContainsKey(c) && !EnToTh.ContainsKey(c))
                {
                    isThai = true;
                    break;
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (isThai)
                {
                    // If Thai is detected, try Thai -> English first
                    if (ThToEn.TryGetValue(c, out char enChar))
                    {
                        sb.Append(enChar);
                    }
                    else if (EnToTh.TryGetValue(c, out char thChar))
                    {
                        sb.Append(thChar);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                else
                {
                    // If English/Mixed, try English -> Thai first
                    if (EnToTh.TryGetValue(c, out char thChar))
                    {
                        sb.Append(thChar);
                    }
                    else if (ThToEn.TryGetValue(c, out char enChar))
                    {
                        sb.Append(enChar);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            return sb.ToString();
        }
    }
}
