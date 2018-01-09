using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Linq;
using System.Xml.Linq;

namespace Web_Apax.Models
{
    public class ResourceManager
    {
        private ResourceManager() { }
        public static string DefaultLanguage = "vi-VN";
        public static string CurrentLanguageName
        {
            get { return System.Threading.Thread.CurrentThread.CurrentCulture.Name; }
        }

        public static string GetString(string key)
        {
            IList<clsNgonNgu> messages = GetResource();
            if (!messages.Any(m => m.sKey == key))
            {
                return key;
            }
            return messages.FirstOrDefault(m => m.sKey == key).sValue;
        }

        private static List<clsNgonNgu> GetResource()
        {
            string currentLanguage = CurrentLanguageName;
            string defaultLanguage = Web_Apax.Models.BSCFunc.defaultLanguage; ;
            if (currentLanguage == null) currentLanguage = "vi-VN";
            if (defaultLanguage == null) defaultLanguage = "vi-VN";
            string cacheKey = "Localization:" + defaultLanguage + ':' + currentLanguage;
            if (HttpRuntime.Cache[cacheKey] == null)
            {
                LoadResource(defaultLanguage, cacheKey);
                if (defaultLanguage != currentLanguage)
                {
                    try
                    {
                        LoadResource(currentLanguage, cacheKey);
                    }
                    catch (FileNotFoundException) { }
                }
            }
            return (List<clsNgonNgu>)HttpRuntime.Cache[cacheKey];
        }

        private static void LoadResource(string culture, string cacheKey)
        {
            string file = HttpContext.Current.Server.MapPath("~/Language/" + culture + "/eLanguage.xml");
            XDocument xDoc = XDocument.Load(file);
            var items = from s in xDoc.Descendants("item") select new clsNgonNgu() { sKey = s.Element("sKey").Value, sValue = s.Element("sValue").Value };
            HttpRuntime.Cache.Insert(cacheKey, items.ToList<clsNgonNgu>(), new CacheDependency(file), DateTime.MaxValue, TimeSpan.Zero);
        }
    }
}