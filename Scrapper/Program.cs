using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Scrapper.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            new Scrapping().Run("https://www.kurly.com/shop/goods/goods_list.php?category=029", true);
        }
    }
}
