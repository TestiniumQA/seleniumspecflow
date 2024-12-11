using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using PayCoreApiTest.Configuration;

namespace PayCoreApiTest.Helper
{
    public class BasePage
    {

        private BrowserHelper _browserHelper;
        private IJavaScriptExecutor javaScriptExecutor;
        public BasePage(BrowserHelper browserHelper)
        {
            _browserHelper = browserHelper;

        }

        public void goToUrl(string uri)
        {
            _browserHelper.driver.Navigate().GoToUrl(uri);

        }
        public void clickBP(string key)
        {
            Thread.Sleep(1000);
            findElement(key).Click();
        }
        public void clickStale(string key)
        {
            Thread.Sleep(1000);
            findElementStale(key).Click();
        }
        public void clickDD(string key, string key2)
        {
            findElement(key).Click();
            findElement(key2).Click();
        }

        public void fillTextBox(string key, string text)
        {
            findElement(key).SendKeys(text);
        }

        public void fileUpload()
        {
            var allowsDetection = _browserHelper.driver as IAllowsFileDetection;
            if (allowsDetection != null)
            {
                allowsDetection.FileDetector = new LocalFileDetector();
            }
        }

        public void checkElementIsDisplayed(string key)
        {
            Assert.IsTrue(findElement(key).Displayed, key + "'li elementi bulunamadı.");
        }
        public void checkElement(string key)
        {
            Assert.AreNotEqual(findElement(key),null, key + "'li elementi bulunamadı.");
        }

        public void checkElementNotInPage(string key)
        {
            Assert.AreEqual(findElement(key),null, key + " element seen on page.");
        }

        public void browserMaximize()
        {
            _browserHelper.driver.Manage().Window.Maximize();
        }
        public void goBack()
        {
            _browserHelper.driver.Navigate().Back();
        }

        public void forward()
        {
            _browserHelper.driver.Navigate().Forward();
        }
        public void leftPress(string key, string text)
        {
            findElement(key).SendKeys(Keys.Home);
            fillTextBox(key, text);

        }

        public void refreshPage()
        {
            _browserHelper.driver.Navigate().Refresh();
        }
        public void switchToLastWindow()

        {
            _browserHelper.driver.SwitchTo().Window(_browserHelper.driver.WindowHandles.Last());
        }

        public void pageContainsText(string text)

        {
            Assert.IsTrue(_browserHelper.driver.PageSource.Contains(text), "Sayfa " + text + " değerini içermiyor.");
        }

        public void elementContainsText(string key, string text)

        {
            Assert.AreEqual(findElement(key).Text.ToString(), text, "iki alan eşleşmiyor..");
        }


        public By generateElementBy(string by, string value)
        {
            Console.WriteLine("generateElementBy by: " + by + " value: " + value);
            By byElement = null;
            if (by.Equals(ElementType.id))
            {
                byElement = By.Id(value);
            }
            else if (by.Equals(ElementType.name))
            {
                byElement = By.Name(value);
            }
            else if (by.Equals(ElementType.className))
            {
                byElement = By.ClassName(value);
            }
            else if (by.Equals(ElementType.cssSelector))
            {
                byElement = By.CssSelector(value);
            }
            else if (by.Equals(ElementType.xpath))
            {
                byElement = By.XPath(value);
            }
            else if (by.Equals(ElementType.linkText))
            {
                byElement = By.LinkText(value);
            }
            else
            {
                Assert.Fail("No such selector.");
            }
            return byElement;
        }

        public IWebElement findElement(string key)
        {
            By by = generateElementBy(_browserHelper.keyValuePairs[key].Key, _browserHelper.keyValuePairs[key].Value);
            WebDriverWait wait = new WebDriverWait(_browserHelper.driver, TimeSpan.FromSeconds(AppConficReader.GetElementLoadTimeout()));
            IWebElement webElement = wait.Until(ExpectedConditions.ElementToBeClickable(by));
            return webElement;
        }
        public IList<IWebElement> findElements(string key)
        {
            By by = generateElementBy(_browserHelper.keyValuePairs[key].Key, _browserHelper.keyValuePairs[key].Value);
            WebDriverWait wait = new WebDriverWait(_browserHelper.driver, TimeSpan.FromSeconds(AppConficReader.GetElementLoadTimeout()));
            IList<IWebElement> webElements = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
            return webElements;
        }

        public IWebElement findElementStale(string key)
        {
            By by = generateElementBy(_browserHelper.keyValuePairs[key].Key, _browserHelper.keyValuePairs[key].Value);

            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_browserHelper.driver);
            wait.Timeout = TimeSpan.FromSeconds(AppConficReader.GetElementLoadTimeout());
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Message = "StaleElementReferenceException";
            IWebElement webElement = wait.Until(ExpectedConditions.ElementToBeClickable(by));
            return webElement;
        }


        public void scrollElement(IWebElement element)
        {
            javaScriptExecutor = (IJavaScriptExecutor)_browserHelper.driver;
            javaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView({ behaviour: 'smooth', block: 'center', inline: 'center'});", element);
        }

        public void clickJS(string key)
        {
            By by = generateElementBy(_browserHelper.keyValuePairs[key].Key, _browserHelper.keyValuePairs[key].Value);
            WebDriverWait wait = new WebDriverWait(_browserHelper.driver, TimeSpan.FromSeconds(AppConficReader.GetElementLoadTimeout()));
            IWebElement webElement = wait.Until(ExpectedConditions.ElementExists(by));
            scrollElement(webElement);
            javaScriptExecutor = (IJavaScriptExecutor)_browserHelper.driver;
            javaScriptExecutor.ExecuteScript("arguments[0].click();", webElement);

        }

        public void clearElement(string key)
        {
            findElement(key).Clear();
        }


    }
}
