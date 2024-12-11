
using System;
using System.Threading;
using TechTalk.SpecFlow;

using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.IO;

using System.Xml;
using PayCoreApiTest;
using System.Text;

using System.Reflection;
using System.Linq;
using PayCoreApiTest.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using Tesseract;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using OpenQA.Selenium.Html5;
using AngleSharp.Dom;
using Castle.Components.DictionaryAdapter.Xml;

namespace Testinium
{

    [Binding]
    public class StepImplementation
    {

        OracleConnection con = null;
        OracleCommand command = null;
        OracleDataReader dataReader = null;
        string remoteName;
        string ImagePath = @"C:\\Users\\testinium\\Downloads\\Specflowrestsharp\\Specflowrestsharp\\"; //C:\\Users\\isofa\\source\\repos\\mybookie\\
        string gameName = "takethebank";
        Dictionary<string, object> hashMap = new Dictionary<string, object>();
        public static string BASE_PATH_CONSTANTS = "";
        private BrowserHelper _browserHelper;
        private BasePage _basePage;

        public StepImplementation()
        {

        }
        public StepImplementation(BrowserHelper browserHelper)
        {
            _browserHelper = browserHelper;
            _basePage = new BasePage(_browserHelper);
        }


        [BeforeScenario]
        [Obsolete]
        public void setUp()
        {
            if (TestContext.Parameters.Get("key") == null)
            {

                Console.WriteLine("Test localde ayağa kalkacak");
                BASE_PATH_CONSTANTS = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../") + "Constants1";
            }
            else
            {
                Console.WriteLine("Test Testiniumda ayağa kalkacak");
                remoteName = TestContext.Parameters.Get("api").ToString();
                Console.WriteLine("Testin ayağa kalktığı ortam: " + remoteName);
                BASE_PATH_CONSTANTS = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../Testinium/SpecFlowDemo/") + "Constants1";

            }


        }


        [AfterScenario]
        public void afterScenario()
        {
            try
            {
                if (null != _browserHelper.driver)
                {
                    _browserHelper.driver.Quit();
                    Console.WriteLine("Driver Quited After Scenario");
                }
            }
            catch
            {
                Console.WriteLine("Driver önceden kapatılmış");
            }
        }



        //==================================================================== Driver ==============================================================================




        [Given(@"(.*) elementine tikla")]
        public void clickElement(string key)
        {
            _basePage.clickBP(key);
        }

        [Given(@"(.*) elementine tıkla stale")]
        public void clickElementStale(string key)
        {
            _basePage.clickStale(key);
        }

        [Given(@"(.*) elementine js ile tıkla")]
        public void clickElementJS(string key)
        {
            _basePage.clickJS(key);
        }

        [Given(@"(.*),(.*) elementlerine sırayla tıkla")]
        public void clickElement(string key, string key2)
        {
            _basePage.clickDD(key, key2);
        }

        [Given(@"(.*) elementinin görünürlüğü kontrol edilir")]
        public void checkElementIsDisplayed(string key)
        {
            _basePage.checkElementIsDisplayed(key);
        }

        [Given(@"(.*) elementinin varligi kontrol edilir")]
        public void checkElementIsNull(string key)
        {
            _basePage.checkElement(key);
        }

        [Given(@"Check (.*) element is not on the page")]
        public void checkElementIsNotOnPage(string key)
        {
            if (_basePage.findElement(key) == null)
            {
                Assert.True(true, "Element seen on page");
            }
            else
            {
                Assert.Fail("Element seen on page");
            }
        }

        [Given(@"(.*) elementine (.*) textini yaz")]
        public void fillTextBox(string key, string text)
        {
            _basePage.fillTextBox(key, text);
        }

        [Given(@"(.*) adresine git")]
        public void goToUrl(string url)
        {
            _basePage.goToUrl(url);
            Console.WriteLine(url + " adresine gidildi");
        }


        [Given(@"(.*) saniye bekle")]
        public void waitBySecond(int second)
        {
            Thread.Sleep(second * 1000);
        }

        [Given(@"Geri git")]
        public void goBack()
        {
            _basePage.goBack();
        }

        [Given(@"Ileri git")]
        public void forward()
        {
            _basePage.forward();
        }

        [Given(@"Refresh Page")]
        public void refreshPage()
        {
            _basePage.refreshPage();
        }

        [Given(@"Son Sekmeye Odaklan")]
        public void switchToLastWindow()
        {
            _basePage.switchToLastWindow();
        }

        [Given(@"File Upload")]
        public void fileUpload()
        {
            _basePage.fileUpload();
        }

        [Given(@"sayfa (.*) değerini içeriyor mu")]
        public void pageContainsText(string text)
        {
            _basePage.pageContainsText(text);
        }

        [Given(@"(.*) elementi (.*) değerini içeriyor mu")]
        public void elementContainsText(string key, string text)
        {
            _basePage.elementContainsText(key, text);
        }


        [Given(@"Driver'i kapat")]
        public void driverClose()
        {
            _browserHelper.driver.Quit();
            Console.WriteLine("Driver Quited");
        }

        [Given(@"Url'de response kod ""(.*)"" olarak donuyor mu")]
        public void driverUrlResponse(string resCode)
        {
            _browserHelper.uri = new Uri(_browserHelper.driver.Url);
            string responseCodee = _browserHelper.uri.Query;
            Assert.AreEqual(responseCodee, "?responseCode=" + resCode, "Response Code eşleşmiyor");
        }


        [Given(@"Add (.*) element text to hashmap as (.*) key")]
        [Obsolete]
        public void driverGetElementText(string element, string key)
        {
            hashMap.Add(key, (_basePage.findElement(element).Text).ToString());
            Console.WriteLine("Hashmape eklenen text: " + hashMap[key]);
        }
        
        [Given(@"Add (.*) element price to hashmap as (.*) key")]
        [Obsolete]
        public void driverSetPriceHashmaoKey(string element, string key)
        {
            // Tanınan metni bir desenle eşleştirin (örneğin, bir döviz bakiyesi)
            string pattern = @"-?\$-?[\d,]+(\.\d+)?"; // $ işareti ve ardından bir veya daha fazla rakam veya virgül, isteğe bağlı ondalık kısmı içeren metni eşleştirir
            MatchCollection matches = Regex.Matches(_basePage.findElement(element).Text, pattern);

            if (matches.Count > 0)
            {
                string balanceText = matches[0].ToString(); // İlk eşleşen metni alın
                string balanceValue = balanceText.Replace("$", "").Replace(",", ""); // $ işaretini ve virgülü kaldırın
                decimal balanceAmount = decimal.Parse(balanceValue);
                addHashmapManuel(key, balanceAmount.ToString()); // Tam sayı ve ondalık kısmı koruyarak kaydedin
                Console.WriteLine("Balance: " + balanceAmount.ToString()); // Para birimi sembolü olmadan yazdırın
            }
            else
            {
                Assert.Fail("Cant find price");
            }

        }


        [Given(@"(.*) elementine hashmapteki (.*) değerini gir")]
        [Obsolete]
        public void driverSetElementText(string element, string key)
        {
            _basePage.fillTextBox(element, hashMap[key].ToString());
            Console.WriteLine("yazılan değer şu: " + hashMap[key].ToString());
        }

        [Given(@"(.*) elementine (.*) tarihini gir")]
        public void leftpress(string key, string text)
        {
            _basePage.leftPress(key, text);
            Console.WriteLine(key + "'ine " + text + " tarihi eklendi.");
        }

        [Given(@"(.*) elementini temizle")]
        public void clearElement(string key)
        {
            _basePage.clearElement(key);
            Console.WriteLine(key + "'inin text'i temizlendi");
        }


        [Given(@"(.*) keyli hashmap adresi")]
        public void goToUrlHashMap(string key)
        {
            _basePage.goToUrl(hashMap[key].ToString());
        }


        [Given(@"(.*) keyli hashmap adresine (.*) parametresi ile git")]
        public void driverGoToUrlFromHashMapviaParam(string key, string param)
        {
            _basePage.goToUrl(hashMap[key].ToString() + "&" + param);
        }


        //-------------------------------------------------------------------- Endof Driver -----------------------------------------------------------------------------------


        //==================================================================== Random Generation ==============================================================================

        [Given(@"Rastgele username yarat ve hashMap'a ""(.*)"" keyi ile ekle")]
        public void generateRandomUsername(string key)
        {
            Random rnd = new Random();
            int endOfNumber = rnd.Next(000001, 999999);
            hashMap.Add(key, "userr" + endOfNumber);
            Console.WriteLine(hashMap[key].ToString());
        }



        //---------------------------------------------------------------------- Endof Random Generation ----------------------------------------------------------------------









        //==================================================================== Database And DataReader ==============================================================================


        [Given(@"""(.*)"" bilgileri ile database connection aç")]
        public void connectDatabase(string connectionInfo)
        {
            con = new OracleConnection();
            con.ConnectionString = connectionInfo;
            con.Open();
            Console.WriteLine(remoteName + "Bilgileri ile database bağlandı" + con.ServerVersion);
        }


        [Given(@"database connection kapat")]
        public void closeDatabase()
        {
            con.Close();
            con.Dispose();
            Console.WriteLine("Database bağlantısı kapatıldı");
        }


        [Given(@"""(.*)"" query'sini database'e gönder ve data reader'a ekle")]
        public void executeDatabaseCommandAndAddObject(string query)
        {
            command = con.CreateCommand();
            command.CommandText = query;
            dataReader = command.ExecuteReader();
            Console.WriteLine("Database'e " + query + " Sorgusu yapıldı");
        }




        [Given(@"Data readerdaki ""(.*)"" değerlerini ""(.*)""  değeri ile eşleşiyor mu kontrol et")]
        public void checkDataReaderValue(string collumnName, string value)
        {
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    Assert.AreEqual(value, dataReader[collumnName].ToString(), "Veriler eşleşmiyor...");
                    Console.WriteLine(value + "değerine ile " + dataReader[collumnName].ToString() + " değeri aynı");
                }
            }
            else
            {
                Assert.Fail("Data Reader Boş..");
            }
        }

        [Given(@"Data readerdaki ""(.*)"" değerlerini ""(.*)"" keyli hashmap değeri ile eşleşiyor mu kontrol et")]
        public void checkDataReaderValueWithHashmap(string collumnName, string key)
        {
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    Assert.AreEqual(hashMap[key].ToString(), dataReader[collumnName].ToString(), "Veriler eşleşmiyor...");
                    Console.WriteLine(hashMap[key].ToString() + "değerine ile " + dataReader[collumnName].ToString() + " değeri aynı");
                }
            }
            else
            {
                Assert.Fail("Data Reader Boş..");
            }
        }



        [Given(@"Data readerdaki ""(.*)"" değerleri null mu")]
        public void checkDataReaderNull(string collumnName)
        {
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    Assert.AreEqual(DBNull.Value.ToString(), dataReader[collumnName].ToString(), "Veriler eşleşmiyor...");
                    Console.WriteLine("Değerler NULL");
                }
            }
            else
            {
                Assert.Fail("Data Reader Boş Değil..");
            }
        }



        [Given(@"Data readerdaki ""(.*)"" değerlerini hashMap'a ""(.*)"" key'i ile ekle")]
        public void addDatareaderValueToHashmap(string collumnName, string key)
        {
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    hashMap.Add(key, (object)dataReader[collumnName].ToString());
                    Console.WriteLine(key + "değerine hashmap'de " + dataReader[collumnName].ToString() + " değeri eklendi");
                }
            }
            else
            {
                Console.WriteLine("Data Reader Boş..");
            }
        }


        [Given(@"""(.*)"" query'sine ""(.*)"" keyli value'yu ekle ve database'e gönder ve data reader'a ekle")]
        public void executeDatabaseCommandAndAddObject(string query, string key)
        {
            command = con.CreateCommand();
            command.CommandText = query + "'" + hashMap[key].ToString() + "'";
            dataReader = command.ExecuteReader();
            Console.WriteLine("Database'e " + "'" + query + hashMap[key].ToString() + "'" + " Sorgusu yapıldı");
        }

        [Given(@"""(.*)"" query'sine ""(.*)"" keyli value'yu ekle ve database'e gönder ve son girdiyi data reader'a ekle")]
        public void executeDatabaseCommandAndAddLastObject(string query, string key)
        {
            command = con.CreateCommand();
            command.CommandText = query + "'" + hashMap[key].ToString() + "' ORDER BY SYSTEM_ENTRY_DATE_TIME DESC FETCH FIRST 1 ROWS ONLY";
            dataReader = command.ExecuteReader();
            Console.WriteLine("Database'e " + "'" + query + hashMap[key].ToString() + "' ORDER BY SYSTEM_ENTRY_DATE_TIME DESC FETCH FIRST 1 ROWS ONLY" + " Sorgusu yapıldı");
        }




        [Given(@"Data readardaki ""(.*)"" değeri'ni jobject olarak oluştur ve ""(.*)"" hashMap değeri jobjectdeki ""(.*)"" değeri ile eşleşiyor mu kontrol et")]
        public void checkHashmapValueViaCreateJobjectFromDataReader(string key, string value, string jobjectKey)
        {
            string json = hashMap[key].ToString();
            JObject dataObject = JObject.Parse(json);
            dataObject.SelectToken(jobjectKey).ToString();
            Assert.AreEqual(hashMap[value].ToString(), dataObject.SelectToken(jobjectKey).ToString(), "Veriler eşleşmiyor.");
        }


        [Given(@"Data readardaki ""(.*)"" değeri'ni jobject olarak oluştur ve ""(.*)"" değeri jobjectdeki ""(.*)"" değeri ile eşleşiyor mu kontrol et")]
        public void checkValueViaCreateJobjectFromDataReader(string key, string value, string jobjectKey)
        {
            string json = hashMap[key].ToString();
            JObject dataObject = JObject.Parse(json);
            dataObject.SelectToken(jobjectKey).ToString();
            Assert.AreEqual(value, dataObject.SelectToken(jobjectKey).ToString(), "Veriler eşleşmiyor.");
        }

        [Given(@"Data readardaki ""(.*)"" değeri'ni jobject olarak oluştur ve ""(.*)"" değeri bos mu kontrol et")]
        public void checkEmptyValueViaCreateJobjectFromDataReader(string key, string jobjectKey)
        {
            JObject dataObject = JObject.Parse(hashMap[key].ToString());
            Assert.IsNotEmpty(dataObject.SelectToken(jobjectKey).ToString(), "Veri boş.");
        }

        [Given(@"Data readardaki ""(.*)"" değeri'ni jobject olarak oluştur ve ""(.*)"" değeri dolu mu kontrol et")]
        public void checkNonEmptyValueViaCreateJobjectFromDataReader(string key, string jobjectKey)
        {
            JObject dataObject = JObject.Parse(hashMap[key].ToString());
            Assert.IsEmpty(dataObject.SelectToken(jobjectKey).ToString(), "Veri boş değil");
        }

        [Given(@"Data readardaki ""(.*)"" değeri'ni jobject olarak oluştur ve ""(.*)"" değeri jobjectdeki ""(.*)"" değeri ile eşleşmiyor mu kontrol et")]
        public void checkNotEqualValueViaCreateJobjectFromDataReader(string key, string value, string jobjectKey)
        {
            string json = hashMap[key].ToString();
            JObject dataObject = JObject.Parse(json);
            dataObject.SelectToken(jobjectKey).ToString();
            Assert.AreNotEqual(value, dataObject.SelectToken(jobjectKey).ToString(), "Veriler eşleşiyor.");
        }


        [Given(@"""(.*)"" query'sini database'e gönder ve geri dönüşün boş olmasını bekle")]
        public void executeDatabaseCommandCheckIsResponseEmpty2(string query)
        {
            command = con.CreateCommand();
            command.CommandText = query;
            dataReader = command.ExecuteReader();
            Console.WriteLine("Database'e " + query + " Sorgusu yapıldı");
            if (dataReader.HasRows)
            {
                Assert.Fail("Data Reader Boş Değil");
            }
            else
            {
                Console.WriteLine("Data Reader Boş..");
            }
        }


        //------------------------------------------------------------------- Endof Database And DataReader ------------------------------------------------------------







        //==================================================================== Others ==============================================================================

        [Given(@"Close windows and return to first tab")]
        public void goToFirstWindow()
        {
            _browserHelper.driver.Close();
            // Tüm açık sekmelerin tanımlarını alın
            var windowHandles = _browserHelper.driver.WindowHandles;
            _browserHelper.driver.SwitchTo().Window(windowHandles[0]);
            Console.WriteLine("Window changed to first window tab");
        }

        [Given(@"Go to last window tab")]
        public void goToLastWindow()
        {
            // Tüm açık sekmelerin tanımlarını alın
            var windowHandles = _browserHelper.driver.WindowHandles;

            // Yeni sekmenin tanımını alın
            string newTabHandle = windowHandles[windowHandles.Count - 1];

            // Yeni sekme üzerine geçiş yapın
            _browserHelper.driver.SwitchTo().Window(newTabHandle);
            Console.WriteLine("Window changed to last window tab");
        }


        [Given(@"Wait ""(.*)"" seconds")]
        public void waitSeconds(int i)
        {
            Console.WriteLine(i + " saniye bekleniyor");
            Thread.Sleep(TimeSpan.FromSeconds(i));
        }

        [Given(@"Add ""(.*)"" element value to hashmap as ""(.*)""")]
        public void addHashmapManuel(string key, string value)
        {
            hashMap.Add(key, value);
            Console.WriteLine(key + " element added to hashmap as " + value);
        }
        
        [Given(@"Get name of the game and change environment")]
        public void changeImagePath()
        {
            if(_browserHelper.driver.FindElement(By.XPath("//*[@id=\"miniGameContainer\"]//span")).Text.Contains("Super Golden"))
            {
                gameName = "goldendragon";
                Console.WriteLine("Game Name " + _browserHelper.driver.FindElement(By.XPath("//*[@id=\"miniGameContainer\"]//span")).Text);
            }
            else
            {
                gameName = "takethebank";
                Console.WriteLine("Game Name " + _browserHelper.driver.FindElement(By.XPath("//*[@id=\"miniGameContainer\"]//span")).Text);
            }
        }

        [Given(@"get (.*) and (.*) from hashmap do (.*) and save value as (.*) to hashmap")]
        public void doMathWithHashmap(string key1, string key2, string operation, string key3)
        {
            decimal firstValue = decimal.Parse(hashMap[key1].ToString());
            decimal secondValue = decimal.Parse(hashMap[key2].ToString());
            decimal calcutedValue;

            switch (operation)
            {
                case "sum":
                    calcutedValue = firstValue + secondValue;
                    addHashmapManuel(key3, calcutedValue.ToString());
                    Console.WriteLine("Operation is " + firstValue + " " + operation + " " + secondValue + " = " + calcutedValue);
                    break;
                case "minus":
                    calcutedValue = firstValue - secondValue;
                    addHashmapManuel(key3, calcutedValue.ToString());
                    Console.WriteLine("Operation is " + firstValue + " " + operation + " " + secondValue + " = " + calcutedValue);
                    break;
                case "divide":
                    calcutedValue = firstValue / secondValue;
                    addHashmapManuel(key3, calcutedValue.ToString()); 
                    Console.WriteLine("Operation is " + firstValue + " " + operation + " " + secondValue + " = " + calcutedValue);
                    break;
                case "multiply":
                    calcutedValue = firstValue * secondValue;
                    addHashmapManuel(key3, calcutedValue.ToString()); 
                    Console.WriteLine("Operation is " + firstValue + " " + operation + " " + secondValue + " = " + calcutedValue);
                    break;
                default:
                    break;
            }
        }


        [Given(@"get (.*) and (.*) from hashmap and check value1 lower than value2")]
        public void checkValuesLowerThan(string key1, string key2)
        {
            Assert.True(Decimal.Parse(hashMap[key1].ToString().Replace("$","")) < Decimal.Parse(hashMap[key2].ToString().Replace("$", "")), hashMap[key1].ToString() + " is not lower than " + hashMap[key2].ToString());
        }

        [Given(@"get (.*) and (.*) from hashmap and check value1 greater than value2")]
        public void checkValuesGreaterThan(string key1, string key2)
        {
            Assert.True(Decimal.Parse(hashMap[key1].ToString().Replace("$", "")) > Decimal.Parse(hashMap[key2].ToString().Replace("$", "")), hashMap[key1].ToString() + " is not lower than " + hashMap[key2].ToString());
        }

        [Given(@"get (.*) and (.*) from hashmap and check values are not same")]
        public void checkValuesAreNotSame(string key1,string key2)
        {
            Assert.AreNotEqual(hashMap[key1].ToString(), hashMap[key2].ToString(), "Values are matched");
        }

        [Given(@"get (.*) and (.*) from hashmap and check values are same")]
        public void checkValuesAreSame(string key1, string key2)
        {
            Assert.AreEqual(hashMap[key1].ToString(), hashMap[key2].ToString(), "Values are not matched");
        }


        [Given(@"get (.*) and (.*) from hashmap and check contains eachother")]
        public void checkValuesAreContains(string key1, string key2)
        {
            Assert.True(hashMap[key1].ToString().Contains(hashMap[key2].ToString()), "Values are not containes eachother");
            Console.WriteLine(hashMap[key1].ToString() + " contains " + hashMap[key2].ToString());
        }

        [Given(@"Get screenshot and save")]
        public void getScreenshotOfScreen()
        {
            string screenshotPath = $"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshotTemp.png";
            var screenshot = ((ITakesScreenshot)_browserHelper.driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
        }


        [Given("Click on the button with the (.*) text from screenshotname=(.*)")]
        public void findTextLocationImage(string targetText,string imgName)
        {
            Mat rawImage = Cv2.ImRead($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgName}.png", ImreadModes.Color);

            // Görüntüyü siyah-beyaz (grayscale) yapın
            Mat grayscaleImage = new Mat();
            Cv2.CvtColor(rawImage, grayscaleImage, ColorConversionCodes.BGR2GRAY);

            // Threshold işlemi uygulayın
            Cv2.Threshold(grayscaleImage, grayscaleImage, thresh: 50, maxval: 235, ThresholdTypes.Binary);

            // Yakınlaştırılmış ve eşiklenmiş görüntüyü kaydedin veya işlem yapın
            Cv2.ImWrite($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgName}.png", grayscaleImage);

            using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng+osd", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgName}.png"))
                {

                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();

                        if (text.Contains(targetText))
                        {
                            using (var iter = page.GetIterator())
                            {
                                do
                                {
                                    if (iter.TryGetBoundingBox(PageIteratorLevel.Word, out Tesseract.Rect bounds))
                                    {
                                        if (iter.GetText(PageIteratorLevel.Word) == targetText)
                                        {
                                            int left = bounds.X1;
                                            int top = bounds.Y1;
                                            int right = bounds.X2;
                                            int bottom = bounds.Y2;
                                            Actions actions = new Actions(_browserHelper.driver);
                                            actions.MoveToElement(_browserHelper.driver.FindElement(By.TagName("body")), bounds.X1 + bounds.Width, bounds.Y1 + bounds.Height).Click().Build().Perform();
                                            break;
                                        }
                                    }
                                } while (iter.Next(PageIteratorLevel.Word));
                            }
                        }
                        else
                        {
                            Console.WriteLine("Hedef metin bulunamadı.");
                        }
                    }
                }
            }
        }

        //

        [Given(@"Check is Image=(.*) contains Text=""(.*)"" filter_grayscaleimage=(.*) and filter_threshold=(.*)")]
        public void checkWithTesseractImageContainsText(string imgPath, string containsText, bool grayScale, int threshold)
        {
            Mat rawImage = Cv2.ImRead($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgPath}.png", ImreadModes.Color);
            if (threshold != 0)
            {
                Cv2.Threshold(rawImage, rawImage, thresh: threshold, maxval: 255, ThresholdTypes.Binary);
            }
            if (grayScale)
            {
                Cv2.CvtColor(rawImage, rawImage, ColorConversionCodes.BGR2GRAY);
            }
            // Yakınlaştırılmış ve eşiklenmiş görüntüyü kaydedin veya işlem yapın
            Cv2.ImWrite($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshotTemp.png", rawImage);

            using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng+osd", EngineMode.Default))
            {

                using (var img = Pix.LoadFromFile($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshotTemp.png"))
                {
                    using (var page = engine.Process(img))
                    {
                        string recognizedText = page.GetText();
                        Console.WriteLine("Text inside the image " + recognizedText);
                        Assert.True(recognizedText.Trim().Contains(containsText), "Image not contains this text requested text = " + containsText + " actual text = " + recognizedText);
                    }
                }
            }
        }

        [Given(@"Check is page contains ""(.*)"" text filter_grayscaleimage=(.*) and filter_threshold=(.*)")]
        public void checkWithTesseractPageContainsText(string containsText, bool grayScale, int threshold)
        {
            getScreenshotOfScreen();
            Mat rawImage = Cv2.ImRead($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshotTemp.png", ImreadModes.Color);
            if (threshold != 0)
            {
                Cv2.Threshold(rawImage, rawImage, thresh: threshold, maxval: 255, ThresholdTypes.Binary);
            }
            if (grayScale)
            {
                Cv2.CvtColor(rawImage, rawImage, ColorConversionCodes.BGR2GRAY);
            }
            // Yakınlaştırılmış ve eşiklenmiş görüntüyü kaydedin veya işlem yapın
            Cv2.ImWrite($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshotTemp.png", rawImage);

            using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng+osd", EngineMode.Default))
            {

                using (var img = Pix.LoadFromFile($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshotTemp.png"))
                {
                    using (var page = engine.Process(img))
                    {
                        string recognizedText = page.GetText();
                        Assert.True(recognizedText.Trim().Contains(containsText), "Page not contains this text");
                    }
                }
            }
        }


        [Given(@"Get text from image=(.*) and check text contains ""(.*)""")]
        public void getTextFromImageOCR(string imgName, string containsText)
        {
            Mat rawImage = Cv2.ImRead($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgName}.png", ImreadModes.Color);

            // Görüntüyü siyah-beyaz (grayscale) yapın
            Mat grayscaleImage = new Mat();
            Cv2.CvtColor(rawImage, grayscaleImage, ColorConversionCodes.BGR2GRAY);
            Cv2.ConvertScaleAbs(grayscaleImage, grayscaleImage, alpha: 2, beta: 0);
            // Yakınlaştırılmış ve eşiklenmiş görüntüyü kaydedin veya işlem yapın
            Cv2.ImWrite($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgName}.png", grayscaleImage);

            using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng+osd", EngineMode.Default))
            {

                using (var img = Pix.LoadFromFile($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgName}.png"))
                {
                    using (var page = engine.Process(img))
                    {
                        string recognizedText = page.GetText();
                        Assert.True(recognizedText.Trim().Contains(containsText), "Text from image doesnt contains text");
                    }
                }
            }
        }

        public string getPriceFromImageAndReturnText(string resimString)
        {
            string balanceValue = null;
            using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng+osd", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{resimString}.png"))
                {
                    using (var page = engine.Process(img))
                    {
                        string recognizedText = page.GetText();
                        Console.WriteLine("Recognized Text:");

                        // Tanınan metni bir desenle eşleştirin (örneğin, bir döviz bakiyesi)
                        string pattern = @"\$[\d.]+"; // $ işareti ve ardından bir veya daha fazla rakam veya nokta içeren metni eşleştirir
                        MatchCollection matches = Regex.Matches(recognizedText, pattern);

                        if (matches.Count > 0)
                        {
                            string balanceText = matches[0].ToString(); // İlk eşleşen metni alın
                            balanceValue = balanceText.Replace("$", ""); // $ işaretini kaldırın
                            Console.WriteLine("Balance: " + balanceValue);
                        }
                        else
                        {
                            Console.WriteLine("Price didnt appear");
                            //Assert.Fail("Balance didnt appear");
                        }
                    }
                }
            }
            return balanceValue;
        }

        [Given("get text from (.*) image set black and white threshold (.*) and save as (.*) to hashmap")]
        public void getTextChangeThresholdAndSaveToHashmap(string imgName, int threshold, string key)
        {
            Mat rawImage = Cv2.ImRead($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgName}.png", ImreadModes.Color);
            Cv2.CvtColor(rawImage, rawImage, ColorConversionCodes.BGR2GRAY);

            // Threshold işlemi uygulayın
            Cv2.Threshold(rawImage, rawImage, thresh: threshold, maxval: 235, ThresholdTypes.Binary);

            // Yakınlaştırılmış ve eşiklenmiş görüntüyü kaydedin veya işlem yapın
            Cv2.ImWrite($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgName}.png", rawImage);
            using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng+osd", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imgName}.png"))
                {
                    using (var page = engine.Process(img))
                    {
                        string recognizedText = page.GetText();
                        Console.WriteLine("Recognized Text: " + recognizedText);
                        addHashmapManuel(key, recognizedText);
                    }
                }
            }
        }

        [Given("get price from (.*) image and save as (.*) to hashmap")]
        public void getPriceFromImageAndSaveToHashmap(string resimString, string key)
        {
            using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng+osd", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{resimString}.png"))
                {
                    using (var page = engine.Process(img))
                    {
                        string recognizedText = page.GetText();
                        Console.WriteLine("Recognized Text:");

                        // Tanınan metni bir desenle eşleştirin (örneğin, bir döviz bakiyesi)
                        string pattern = @"\$[\d,]+(?:\.\d+)?"; // $ işareti ve ardından bir veya daha fazla rakam veya virgül, isteğe bağlı ondalık kısmı içeren metni eşleştirir
                        MatchCollection matches = Regex.Matches(recognizedText, pattern);

                        if (matches.Count > 0)
                        {
                            string balanceText = matches[0].ToString(); // İlk eşleşen metni alın
                            string balanceValue = balanceText.Replace("$", "").Replace(",", ""); // $ işaretini ve virgülü kaldırın
                            decimal balanceAmount = decimal.Parse(balanceValue);
                            addHashmapManuel(key, balanceAmount.ToString()); // Tam sayı ve ondalık kısmı koruyarak kaydedin
                            Console.WriteLine("Balance: " + balanceAmount.ToString()); // Para birimi sembolü olmadan yazdırın
                        }
                        else
                        {
                            Assert.Fail("ddidnt appear");
                        }}

                    }
                }
        }

        [Given("get screenshot x (.*) y (.*) width (.*) height (.*) save name (.*)")]
        public void getScreenshotSpesificSizeAndLocation(int x, int y, int width, int height, string fileName)
        {
            string screenshotPath = $"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshotTemp.png";
            var screenshot = ((ITakesScreenshot)_browserHelper.driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);

            using (Image mevcutEkranGoruntusu = Image.FromFile(screenshotPath))

            {

                // Kırpma işlemini gerçekleştirin  

                using (Bitmap kırpılmışGoruntu = new Bitmap(width, height)) {

                    using (Graphics g = Graphics.FromImage(kırpılmışGoruntu))
                    { 
                        g.DrawImage(mevcutEkranGoruntusu, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                        kırpılmışGoruntu.Save($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{fileName}.png");
                    }
                }
            }
        }

        [Given("Click (.*) times to Image=(.*)")]
        public void clickImageNTimes(int nTime, string imageName)
        {
            string screenshotPath = $"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshot.png";
            var screenshot = ((ITakesScreenshot)_browserHelper.driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);

            Mat screenShot = new Mat(screenshotPath, ImreadModes.Color);

            // Aranan resmi yükle
            Mat templateImage = new Mat($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imageName}.png", ImreadModes.Color);

            // Ekran görüntüsünde nesneyi ara
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(screenShot, templateImage, result, TemplateMatchModes.CCoeff);
                double minVal, maxVal;
                OpenCvSharp.Point minLoc, maxLoc;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                if (maxVal > 0.80)
                {
                    // Eşleşen nesneyi bulundu, tıklama işlemi yap
                    OpenCvSharp.Point location = maxLoc;
                    Actions actions = new Actions(_browserHelper.driver);
                    for (int i = 0; i < nTime; i++)
                    {
                        int centerX = location.X + templateImage.Size().Width / 2;
                        int centerY = location.Y + templateImage.Size().Height / 2;
                        int newLocationX = Convert.ToInt32(centerX);
                        int newLocationY = Convert.ToInt32(centerY);
                        actions.MoveToElement(_browserHelper.driver.FindElement(By.TagName("body")), newLocationX, newLocationY).Click().Build().Perform();
                    }
                }
            }
        }

        [Given("Click to Image=(.*)")]
        public void clickToImageWithImage(string imageName)
        {
            // Ekran görüntüsü al
            string screenshotPath = $"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshot.png";
            var screenshot = ((ITakesScreenshot)_browserHelper.driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);

            Mat screenShot = new Mat(screenshotPath, ImreadModes.Color); 


            // Aranan resmi yükle
            Mat templateImage = new Mat($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imageName}.png", ImreadModes.Color);

            // Ekran görüntüsünde nesneyi ara
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(screenShot, templateImage, result, TemplateMatchModes.CCoeffNormed);
                double minVal, maxVal;
                OpenCvSharp.Point minLoc, maxLoc;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                if (maxVal > 0.90)
                {
                    // Eşleşen nesneyi bulundu, tıklama işlemi yap
                    OpenCvSharp.Point location = maxLoc;
                    Actions actions = new Actions(_browserHelper.driver);
                    int centerX = location.X + templateImage.Size().Width / 2;
                    int centerY = location.Y + templateImage.Size().Height / 2;
                    int newLocationX = Convert.ToInt32(centerX);
                    int newLocationY = Convert.ToInt32(centerY); 
                    actions.MoveToElement(_browserHelper.driver.FindElement(By.TagName("body")), newLocationX, newLocationY).Click().Build().Perform();

                }
                else
                {
                    Assert.Fail("Searched image didnt appear on screen");
                }
            }
        }

        public bool isImageOnScreen(string imageYol)
        {
            // Ekran görüntüsü al
            string screenshotPath = $"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshot.png";
            var screenshot = ((ITakesScreenshot)_browserHelper.driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
            Mat screenCapture = Cv2.ImRead(screenshotPath, ImreadModes.Color);


            // Aranan resmi yükle
            string arananResim = $"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imageYol}.png";
            Mat templateImage = new Mat(arananResim, ImreadModes.Color);

            // Ekran görüntüsünde nesneyi ara
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(screenCapture, templateImage, result, TemplateMatchModes.CCorrNormed);
                double minVal, maxVal;
                OpenCvSharp.Point minLoc, maxLoc;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                if (maxVal > 0.90)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Given(@"image (.*) assertle")]
        public void imageAssert(string imageYol)
        {
            // Ekran görüntüsü al
             string screenshotPath = $"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshot.png";
            var screenshot = ((ITakesScreenshot)_browserHelper.driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
            Mat screenCapture = Cv2.ImRead(screenshotPath, ImreadModes.Color);


            // Aranan resmi yükle
            string arananResim = $"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\{imageYol}.png";
            Mat templateImage = new Mat(arananResim, ImreadModes.Color);

            // Ekran görüntüsünde nesneyi ara
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(screenCapture, templateImage, result, TemplateMatchModes.CCorrNormed);
                double minVal, maxVal;
                OpenCvSharp.Point minLoc, maxLoc;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                if (maxVal > 0.90)
                {
                    Assert.True(true, "eleman bulunumadı brocum");
                }
            }
        }

        [Given(@"Check the balance and lower the spin price to below the balance")]
        public void decraseSpinPriceUntilLimit()
        {
            String balance = "";
            getScreenshotSpesificSizeAndLocation(480, 590, 100, 40, "spinPriceCropped"); // get screenshot of the spinprice section
            getScreenshotSpesificSizeAndLocation(250, 580, 130, 70, "balance"); // get screenshot of the balance section
            String spinPrice = getPriceFromImageAndReturnText("spinPriceCropped");
            balance = getPriceFromImageAndReturnText("balance");
            while (decimal.Parse(balance) < decimal.Parse(spinPrice))
            {
                spinPrice = getPriceFromImageAndReturnText("spinPriceCropped");
                Console.WriteLine("Spin price greater than main balance so decraseing spin price now");
                clickToImageWithImage("spinPriceDecrase_Ingame");
            }
        }

        [Given(@"Spin (.*) times with spin cost ""(.*)"" and save total profit as (.*)")]
        public void spinNTimesWithSpesificPriceAndCheckTotalProfit(int nSpin, decimal spinPrice, string totalProfitKey)
        {
            getScreenshotSpesificSizeAndLocation(450,590,120,40,"spinPriceCropped");
            String spinPriceNow = getPriceFromImageAndReturnText("spinPriceCropped");
            decimal winTotal = 0;
            while (decimal.Parse(spinPriceNow) != spinPrice)
            {
                if (decimal.Parse(spinPriceNow) < spinPrice)
                {
                    clickToImageWithImage("spinPriceIncrease_Ingame");
                }
                else if (decimal.Parse(spinPriceNow) > spinPrice)
                {
                    clickToImageWithImage("spinPriceDecrase_Ingame");
                }
                getScreenshotSpesificSizeAndLocation(450, 590, 120, 40, "spinPriceCropped");
                spinPriceNow = getPriceFromImageAndReturnText("spinPriceCropped");
            }

            for (int i = 0; i < nSpin; i++)
            {
                clickToImageWithImage("spin");
                waitBySecond(15);
                getScreenshotSpesificSizeAndLocation(670, 590, 100, 30, "spinWinPrice");
                if (getPriceFromImageAndReturnText("spinWinPrice") != null)
                {
                    winTotal += decimal.Parse(getPriceFromImageAndReturnText("spinWinPrice"));
                }
            }
            decimal totalSpinPrice = nSpin * decimal.Parse(spinPriceNow) * -1;
            decimal totalProfit = totalSpinPrice + winTotal;
            addHashmapManuel(totalProfitKey, totalProfit.ToString());
        }

        [Given(@"Wait until slot game loading")]
        public void waitUntillSlotGameLoading()
        {
            while(!isImageOnScreen("Spin"))
            {
                Console.WriteLine("Waiting to loading game");
                Thread.Sleep(3000);
            }
        }

        [Given(@"image text bul")]
        public void zzz()
        {
            bool balanceFound = false;

            while (!balanceFound)
            {
                string screenshotPath = $"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\screenshot.png";
                var screenshot = ((ITakesScreenshot)_browserHelper.driver).GetScreenshot();
                screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                Mat screenCapture = Cv2.ImRead(screenshotPath, ImreadModes.Color);

                // Görüntüyü siyah-beyaz (grayscale) yapın
                Mat grayscaleImage = new Mat();
                Cv2.CvtColor(screenCapture, grayscaleImage, ColorConversionCodes.BGR2GRAY);

                double zoomFactor = 4.0;

                // Yeni genişlik ve yüksekliği hesaplayın
                int newWidth = (int)(grayscaleImage.Width * zoomFactor);
                int newHeight = (int)(grayscaleImage.Height * zoomFactor);

                // Yakınlaştırılmış görüntüyü oluşturun
                Mat zoomedImage = new Mat();
                Cv2.Resize(grayscaleImage, zoomedImage, new OpenCvSharp.Size(newWidth, newHeight), interpolation: InterpolationFlags.Cubic);

                // Threshold işlemi uygulayın
                Cv2.Threshold(zoomedImage, zoomedImage, thresh: 235, maxval: 255,  ThresholdTypes.Binary);

                // Yakınlaştırılmış ve eşiklenmiş görüntüyü kaydedin veya işlem yapın
                Cv2.ImWrite($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\temp_image.png", zoomedImage);


                // Tesseract ile metin tanıma işlemi
                using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng+osd", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile($"{ImagePath}\\PayCoreApiTest\\Images\\{gameName}\\temp_image.png"))
                    {
                        using (var page = engine.Process(img))
                        {
                            string recognizedText = page.GetText();
                            Console.WriteLine("Recognized Text:");

                            // Tanınan metni bir desenle eşleştirin (örneğin, bir döviz bakiyesi)
                            string pattern = @"\$[\d.]+"; // $ işareti ve ardından bir veya daha fazla rakam veya nokta içeren metni eşleştirir
                            MatchCollection matches = Regex.Matches(recognizedText, pattern);

                            if (matches.Count > 0)
                            {
                                string balanceText = matches[0].ToString(); // İlk eşleşen metni alın
                                string balanceValue = balanceText.Replace("$", ""); // $ işaretini kaldırın
                                Console.WriteLine("Bakiye: " + balanceValue);
                                balanceFound = true; // Bakiye bulundu, döngüden çık
                            }
                            else
                            {
                                Console.WriteLine("Bakiye bulunamadı. Yeniden arama yapılıyor...");
                            }
                        }
                    }
                }
            }
        }


        [Given(@"Check the URL Contains Contains The (.*) Value")]
        public void checkUrlContains(string expectedUrl)
        {

            string actualUrl = _browserHelper.driver.Url;

            if (actualUrl != null && actualUrl.Contains(expectedUrl))
            {
                Console.WriteLine("Actual URL Contains The Expected " + expectedUrl + " Value");
            }

            else
            {

                Assert.Fail("Actual URL Not Contains The Expected " + expectedUrl + " Value");
            }
        }

        [Given(@"Is The Color Of The Element (.*) Orange")]
        public void checkOrange(string key)
        {

            if (_basePage.findElement(key) != null)
            {

                string actColorCode = _basePage.findElement(key).GetCssValue("color");
                string expColorCode = "rgb(255, 131, 0)";


                Assert.AreEqual(expColorCode, actColorCode);

                Console.WriteLine("Element Color Checked ITS ORANGE");



            }
        }


    }

}


