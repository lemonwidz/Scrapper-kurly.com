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
        /// <summary>
        /// 문자열로 이루어진 상품 가격을 정수로 변환
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetPriceFromText(string value)
        {
            if (value.Contains("원"))
                value = value.Replace("원", string.Empty);
            if (value.Contains(","))
                value = value.Replace(",", string.Empty);
            return int.Parse(value);
        }

        /// <summary>
        /// XPATH 사용을 직관적으로 하기위한 확장 매서드
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static IWebElement FindElementByXPath(this IWebElement element, string xpath)
        {
            return element.FindElement(By.XPath(xpath));
        }

        /// <summary>
        /// XPATH 사용을 직관적으로 하기위한 확장 매서드 (List 버전)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindElementsByXPath(this IWebElement element, string xpath)
        {
            return element.FindElements(By.XPath(xpath));
        }

        /// <summary>
        /// 문자열을 받아서 숫자로 리턴하는 매크로 매서드
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(string value, int defaultValue)
        {
            int result = -1;
            if(false == int.TryParse(value, out result))
                result = defaultValue;
            return result;
        }

        /// <summary>
        /// 로그를 기록
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Console.Write(string.Format("[{0}] {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
        }
    }
}
