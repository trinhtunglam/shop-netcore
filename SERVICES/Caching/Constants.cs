using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES.Caching
{
    public static class Constants
    {
        public static string AppCacheKey = string.Empty;

        /// <summary>
        /// List Language Code allow query on localization, null default is en-US
        /// </summary>
        public static readonly string[] LanguageList = { "en_US", "ar_SA", "cs_CZ", "da_DK", "de_DE", "el_GR", "es_ES", "es_MX", "et_EE", "fi_FI", "fr_FR", "fr_CA", "hu_HU", "hr_HR", "in_ID", "is_IS", "it_IT", "ja_JP", "ko_KR", "ms_MY", "lv_LV", "lt_LT", "nl_NL", "no_NO", "pl_PL", "pt_BR", "pt_PT", "ru_RU", "sv_SE", "sk_SK", "th_TH", "tr_TR", "uk_UA", "vi_VN", "zh_TW", "zh_CN" };

        /// <summary>
        /// Trust You List Language Code allow query on localization, null default is en
        /// </summary>
        public static readonly string[] TrustYouLanguageList = { "en", "ar", "cs", "da", "de", "el", "es", "fi", "fr", "he", "id", "it", "ja", "ko", "ms", "nl", "no", "pl", "pt", "pt-BR", "ru", "sv", "th", "tr", "vi", "zh", "zht" };

        public static IDictionary<string, string> Facilities;
    }
}
