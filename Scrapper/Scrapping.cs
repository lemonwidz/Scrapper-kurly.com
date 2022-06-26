using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Scrapper.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrapper
{
    public class Scrapping
    {
        private readonly string xpathGoodsList = "//div[@id='goodsList']//div[@class='inner_listgoods']//ul[@class='list']/li";
        private readonly string xpathInfoElement = "./div[@class='item']";
        private readonly string xpathGoodsName = "./a[@class='info']/span[@class='name']";
        private readonly string xpathGoodsPrice = "./a[@class='info']//span[@class='price']";
        private readonly string xpathGoodsDesc = "./a[@class='info']/span[@class='desc']";
        private readonly string xpathGoodsThumbnailURL = "./div[@class='thumb']/a[@class='img']/img";
        private readonly string xpathGoodsDetailDesc = "//div[@id='goods-description']";
        private readonly string xpathPagerElement = "//div[@class='pagediv']/span/a | //div[@class='pagediv']/span/strong";
        private readonly string attributeOuterHTML = "outerHTML";
        private readonly string attributeSrc = "src";

        public List<GoodsInfo> Run(string listUrl, bool isHeadlessMode)
        {
            // 옵션 세팅
            ChromeOptions options = null;
            options = new ChromeOptions();
            if (isHeadlessMode)
            {
                options.AddArgument("headless");
            }

            // 브라우저 로딩
            var driver = new ChromeDriver(options);
            driver.Url = listUrl;

            var result = new List<GoodsInfo>();
            int currentPage = 1;

            // 최초 로딩 대기
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driverUntil => driverUntil.FindElement(By.XPath(xpathGoodsList)));

            while (true)
            {
                try
                {
                    // 상품 List 가져오기
                    var goodsList = driver.FindElements(By.XPath(xpathGoodsList));
                    for (int i = 0; i < goodsList.Count; i++)
                    {
                        var goods = new GoodsInfo();

                        try
                        {
                            var infoElement = goodsList[i].FindElementByXPath(xpathInfoElement);
                            goods.ScrappingStatus = SCRAPPING_STATUS.DOING;
                            // 상품정보 가져오기
                            SetGoodsBasicInfo(infoElement, goods);

                            // 새탭으로 클릭 후 상세 정보 가져오기
                            OpenDetailAndGetItemDetailInfo(driver, infoElement, goods);
                        }
                        catch (Exception ex)
                        {
                            goods.ScrappingStatus = SCRAPPING_STATUS.ERROR;
                            Utils.Log("[ERR] GoodsInfo : {0}", ex.Message);
                        }

                        // 파싱 완료된 정보 추가
                        goods.ScrappingStatus = SCRAPPING_STATUS.COMPLETE;
                        result.Add(goods);
                        Utils.Log("[INF] Page : {0}, List : {1}/{2}, Goods : {3}", currentPage, i + 1, goodsList.Count, goods.Name);
                    }

                    // 다음 페이지가 있는지 확인
                    var firstGoodsHTML = goodsList[0].GetAttribute(attributeOuterHTML);
                    int nextPage = CheckNextPageAndGo(driver, firstGoodsHTML, currentPage);

                    // 다음페이지가 없으면 루프 종료
                    if (currentPage == nextPage)
                        break;

                    currentPage = nextPage;
                }
                catch (Exception ex)
                {
                    Utils.Log("[ERR] GoodsList : {0}", ex.Message);
                }
            }

            driver.Close();
            driver.Dispose();

            return result;
        }

        // 상품정보 가져오기
        private void SetGoodsBasicInfo(IWebElement infoElement, GoodsInfo goods)
        {
            goods.Name = infoElement.FindElementByXPath(xpathGoodsName).Text;
            goods.Price = Utils.GetPriceFromTest(infoElement.FindElementByXPath(xpathGoodsPrice).Text);
            goods.Desc = infoElement.FindElementByXPath(xpathGoodsDesc).Text;
            goods.ThumbnailURL = infoElement.FindElementByXPath(xpathGoodsThumbnailURL).GetAttribute(attributeSrc);
        }

        // 새탭으로 클릭 후 상세 정보 가져오기
        private void OpenDetailAndGetItemDetailInfo(ChromeDriver driver, IWebElement infoElement, GoodsInfo goods)
        {
            Actions action = new Actions(driver);
            action.KeyDown(Keys.Control).MoveToElement(infoElement.FindElementByXPath(xpathGoodsName)).Click().Perform();
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driverUntil => driverUntil.FindElement(By.XPath(xpathGoodsDetailDesc)));
            goods.DescDetailHTML = driver.FindElement(By.XPath(xpathGoodsDetailDesc)).GetAttribute(attributeOuterHTML);
            goods.URL = driver.Url;

            // 탭닫고 이어서 진행
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.First());
        }

        // 다음 페이지가 있는지 확인
        private int CheckNextPageAndGo(ChromeDriver driver, string firstGoodsHTML, int currentPage)
        {
            var pagerElementList = driver.FindElements(By.XPath(xpathPagerElement));
            for (int i = 0; i < pagerElementList.Count; i++)
            {
                var page = Utils.GetInt(pagerElementList[i].Text, -1);

                // 다음페이지가 존재하면 클릭해서 이동
                if (page > currentPage)
                {
                    pagerElementList[i].Click();
                    currentPage = page;

                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until<bool>(driverUntil =>
                    {
                        // 상품목록의 첫번째가 바뀌었는지 확인하는것으로 목록이 로딩완료되었는지를 체크
                        var goodsListNew = driver.FindElements(By.XPath(xpathGoodsList));
                        return goodsListNew[0].GetAttribute(attributeOuterHTML) != firstGoodsHTML;
                    });
                    break;
                }
            }
            return currentPage;
        }
    }
}
