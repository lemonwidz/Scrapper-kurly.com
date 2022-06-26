using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper
{
    public static class Utils
    {
        public static int GetPriceFromTest(string value)
        {
            if (value.Contains("원"))
                value = value.Replace("원", string.Empty);
            if (value.Contains(","))
                value = value.Replace(",", string.Empty);
            return int.Parse(value);
        }

        public static IWebElement FindElementByXPath(this IWebElement element, string xpath)
        {
            return element.FindElement(By.XPath(xpath));
        }

        public static ReadOnlyCollection<IWebElement> FindElementsByXPath(this IWebElement element, string xpath)
        {
            return element.FindElements(By.XPath(xpath));
        }

        public static int GetInt(string value, int defaultValue)
        {
            int result = -1;
            if(false == int.TryParse(value, out result))
                result = defaultValue;
            return result;
        }

        public static void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Console.Write(string.Format("[{0}] {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
        }
    }
}
