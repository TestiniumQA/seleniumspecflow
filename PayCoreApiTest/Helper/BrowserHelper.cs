using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;
using NUnit.Framework;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using PayCoreApiTest.model;

namespace PayCoreApiTest.Helper
{
    [Binding]
    public class BrowserHelper
    {

        public RemoteWebDriver driver = null;
        DesiredCapabilities capabilities;
        public bool isRemoteDriver = false;
        private readonly string browser = "chrome";
        public Uri uri = null;

        public Dictionary<string, KeyValuePair<string, string>> keyValuePairs;
        private static string BASE_EXT = "*.json";


        public void GetWebDriver()
        {
            string browserName = TestContext.Parameters.Get("browserName");

            switch (browserName.ToLower())
            {
                case "chrome":
                    Console.WriteLine("Remote ChromeDriver will be created");

                    ChromeOptions chromeOptions = new ChromeOptions();
//chromeOptions.AddArguments("disable-popup-blocking");
//chromeOptions.AddArguments("ignore-certificate-errors");
//chromeOptions.AddArguments("--ignore-ssl-errors=yes");
//chromeOptions.AddAdditionalCapability("acceptInsecureCerts", true);
                    capabilities = chromeOptions.ToCapabilities() as DesiredCapabilities;
                    capabilities.SetCapability("testinium:browserName", "chrome");
                    capabilities.SetCapability("testinium:key", TestContext.Parameters.Get("key"));
//capabilities.SetCapability("platformName", "WIN10");
                    capabilities.SetCapability("version", "LATEST");
//capabilities.SetCapability("version", TestContext.Parameters.Get("version"));


                    break;

                case "firefox":
                    Console.WriteLine("Remote FirefoxDriver will be created");

                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddArguments("disable-popup-blocking");
                    firefoxOptions.AddArguments("ignore-certificate-errors");
                    firefoxOptions.AddAdditionalCapability("acceptInsecureCerts", true);
                    capabilities = firefoxOptions.ToCapabilities() as DesiredCapabilities;
                    capabilities.SetCapability("testinium:browserName", "firefox");
                    capabilities.SetCapability("testinium:key", TestContext.Parameters.Get("key"));
                    capabilities.SetCapability("platformName", "Windows 10");
                    break;

                case "edge":
                    Console.WriteLine("Remote EdgeDriver Ayaga will be created");
                    EdgeOptions edgeOptions = new EdgeOptions();
                    capabilities = edgeOptions.ToCapabilities() as DesiredCapabilities;
                    capabilities.SetCapability("testinium:browserName", "edge");
                    capabilities.SetCapability("testinium:key", TestContext.Parameters.Get("key"));
                    capabilities.SetCapability("platformName", "Windows 10");

                    break;

                case "safari":
                    Console.WriteLine("Remote SafariDriver will be created");
                    capabilities = new DesiredCapabilities();
                    capabilities.SetCapability("testinium:browserName", "safari");
                    capabilities.SetCapability("testinium:key", TestContext.Parameters.Get("key"));

                    break;

                default:
                    Console.WriteLine("Unsupported browser type: " + browserName);
                    return;
            }

            driver = new RemoteWebDriver(new Uri("http://172.25.1.159:4444/wd/hub"), capabilities, TimeSpan.FromSeconds(60));
            isRemoteDriver = true;
            Console.WriteLine(browserName + "created");
            //keyValuePairs = Degerver();
        }


        /*
        public void GetDriver()
        {
            Console.WriteLine("Remote ChromeDriver Ayaga kalkiyor");

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("disable-popup-blocking");
            options.AddArguments("ignore-certificate-errors");
            options.AddArguments("--ignore-ssl-errors=yes");
            Console.WriteLine("options eklendi");

            capabilities = options.ToCapabilities() as DesiredCapabilities;
            capabilities.SetCapability("testinium:browserName", browser);
            capabilities.SetCapability("testinium:key", TestContext.Parameters.Get("key"));
            capabilities.SetCapability("platformName", "Windows 10");
            capabilities.SetCapability("acceptInsecureCerts", true);
            Console.WriteLine("capability eklendi");

            driver = new RemoteWebDriver(new Uri("http://10.44.23.135:4444/wd/hub"), capabilities, TimeSpan.FromSeconds(60));
            isRemoteDriver = true;
            Console.WriteLine("ChromeDriver kalkti");
            keyValuePairs = Degerver();

        }
        */


        [Given(@"Driver'i ayaga kaldir")]
        [Obsolete]
        public void driverAwake()
        {
            if (TestContext.Parameters.Get("key") == null)
            {
                Console.WriteLine("Local ChromeDriver Ayaga kalkiyor");
                ChromeOptions option1 = new ChromeOptions();
                option1.AddArguments("disable-popup-blocking");
                option1.AddArguments("ignore-certificate-errors");
                option1.AddArguments("--ignore-ssl-errors=yes");
                option1.AddArguments("start-maximized");
                option1.AddArguments("--window-size=1024,768");
                //option1.AddExcludedArgument("enable-automation");
                //option1.AddAdditionalCapability("useAutomationExtension", false);

                String chromeYol = $"{AppDomain.CurrentDomain.BaseDirectory}\\PayCoreApiTest\\chromedriver.exe";
                //new DriverManager().SetUpDriver(new ChromeConfig());
                new DriverManager().SetUpDriver(
                "https://storage.googleapis.com/chrome-for-testing-public/128.0.6613.119/win64/chrome-win64.zip",
                    binaryPath: chromeYol,
                    binaryName: "chromedriver"
                );
                driver = new ChromeDriver(option1);
                driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);
                isRemoteDriver = false;
                Console.WriteLine("Local chromedriver created");

            }
            else
            {
                GetWebDriver();
            }
            keyValuePairs = Degerver();
        }


        public Dictionary<string, KeyValuePair<string, string>> Degerver()
        {
            Dictionary<string, KeyValuePair<string, string>> dic = new Dictionary<string, KeyValuePair<string, string>>();
            var txtFiles = Directory.EnumerateFiles(Testinium.StepImplementation.BASE_PATH_CONSTANTS, BASE_EXT);
            foreach (string currentFile in txtFiles)
            {
                var json = File.ReadAllText(currentFile);
                Dictionary<string, Element> d = JsonConvert.DeserializeObject<IEnumerable<Element>>(json).
                Select(p => (Id: p.key, Record: p)).
                 ToDictionary(t => t.Id, t => t.Record);
                //Console.WriteLine("Okunan dosya: " + currentFile + " element sayısı: " + d.Count);
                foreach (var item in d)
                {
                    dic.Add(item.Key.ToString(), new KeyValuePair<string, string>(item.Value.type, item.Value.value));
                    //Console.WriteLine("Sözlüğe eklenen element -> Key:" + item.Key + " type: " + item.Value.type + " value: " + item.Value.value);
                }

            }

            Console.WriteLine("Total Locater Count:" + dic.Count);
            return dic;
        }



    }
}
