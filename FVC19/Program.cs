using LitJson;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace OVC19
{
    class Program
    {
        public const string PATH_LAST_INFO = "lastinfo.txt";

        //public static string BrowserBin = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

        public static BrowserSystem browser;
        public static SearchSystem search;

        static void Main(string[] args)
        {
            try
            {
                //GetBrowser();
                search = new SearchSystem();
                browser = new BrowserSystem();

                ValidateSignIn();
                ValidateAuth();
                SelectLocation();
                SelectVaccine();

                browser.Hide();

                PrintHeader();
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("▤ 검색시작! (백신을 찾으면 웹창을 띄워줍니다!)");
                Console.WriteLine("  - 인터넷 창을 종료하지 마세요.");
                Console.WriteLine("  - 인터넷 창을 종료하면 프로그램이 종료됩니다.");
                Console.WriteLine();
                search.Start();

                while (browser.IsRun())
                {
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
        }

        private static OpenQA.Selenium.Chrome.ChromeDriver ValidateSignIn()
        {
            PrintHeader();
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("▤ 로그인");
            Console.WriteLine(" 브라우저를 통해 로그인해주세요.");
            var driver = browser.Open("https://nid.naver.com/nidlogin.login");
            while (driver.Url.StartsWith("https://nid.naver.com/nidlogin.login")) ;
            return driver;
        }

        private static OpenQA.Selenium.Chrome.ChromeDriver ValidateAuth()
        {
            OpenQA.Selenium.Chrome.ChromeDriver driver;
            PrintHeader();
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("▤ 인증");
            Console.WriteLine(" 브라우저를 통해 인증해주세요.");

            string authUrl = "https://v-search.nid.naver.com/reservation/info?key=H6X1QDNHE3rv7Zz";
            if (File.Exists(PATH_LAST_INFO)) File.ReadAllText(PATH_LAST_INFO, Encoding.UTF8);

            driver = browser.Open(authUrl);
            try
            {
                while (driver.Url.StartsWith("https://v-search.nid.naver.com/reservation/info")
                    || driver.Url.StartsWith("https://v-search.nid.naver.com/reservation/auth"))
                {
                    if (driver.Url.StartsWith("https://v-search.nid.naver.com/home"))
                    {
                        throw new Exception("");
                    }
                }
            }
            catch
            {
                Console.WriteLine();
                Console.WriteLine("브라우저 인증 주소가 손상됐습니다. 실제 예약상황에 들어갔을 때 인증을 실시합니다.");
            }
            return driver;
        }

        private static void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine(@"########################################################
##                                                    ##
##                    COVID-19                        ##
##               잔여백신을 알려줘!                   ##
##                                                    ##
##                                 Made by SIONS      ##
##                               github.com/mrsions   ##
##                                                    ##
########################################################

※ 이 앱은 Naver Graph API를 사용합니다.
※ 이 앱은 학습목적으로 개발된 앱이며, 사용시 발생하는 형사, 민사상를 포함한 모든 법적인 문제는 사용자에게 있습니다.
※ 이 앱은 학습목적으로 개발된 앱이며 판매, 배포, 수정 등을 금지합니다.
※ 종료하시려면 CTRL + C 를 눌러주세요
");
        }

        private static void SelectLocation()
        {
            PrintHeader();
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("▤ 위치 입력");
            Console.WriteLine();

            string url = "https://m.place.naver.com/rest/vaccine?vaccineFilter=used";
            if (File.Exists("location.txt"))
            {
                string newUrl = File.ReadAllText("location.txt", Encoding.UTF8);
                if (newUrl.StartsWith("https://m.place.naver.com/rest/vaccine"))
                {
                    var lines = newUrl.Replace("\r", "").Split('\n');
                    url = lines[0];
                }
            }

            Console.WriteLine("▶ 지시에 따라 위치를 정해주세요.");
            Console.WriteLine("   1. 열린 백신 네이버 지도를 확인합니다.");
            Console.WriteLine("   2. 지도상에서 검색할 위치 및 폭을 맞춥니다. (넓고 좁음까지 포함)");
            Console.WriteLine("   3. 상단의 [현 지도에서 검색]을 누른다.");
            Console.WriteLine();
            var driver = browser.Open(url);

            while (true)
            {
                if (SetupQuery(driver.Url))
                {
                    Console.Write("▶ 위치를 찾았습니다. 이곳으로 할까요? (Y/N) ");
                    string input = Console.ReadLine().ToLower();
                    if (input.StartsWith("y"))
                    {
                        SetupQuery(driver.Url);
                        File.WriteAllText("location.txt", driver.Url, Encoding.UTF8);
                        Console.WriteLine();
                        return;
                    }
                }
            }
        }

        private static bool SetupQuery(string input)
        {
            const string PREFIX = "https://m.place.naver.com/rest/vaccine?";

            if (input.StartsWith(PREFIX))
            {
                string queryString = input.Substring(PREFIX.Length);
                var query = HttpUtility.ParseQueryString(queryString);
                try
                {
                    search.X = double.Parse(query["x"]);
                    search.Y = double.Parse(query["y"]);
                    var bounds = query["bounds"].Split(';');
                    search.MinX = double.Parse(bounds[0]);
                    search.MinY = double.Parse(bounds[1]);
                    search.MaxX = double.Parse(bounds[2]);
                    search.MaxY = double.Parse(bounds[3]);
                    return true;
                }
                catch (Exception e)
                {
                }
            }
            return false;
        }


        private static void SelectVaccine()
        {
            var vaccines = new string[] { "아스트라제네카", "얀센", "화이자", "모더나" };
            int index = vaccines.Length;

            List<string> selected = new List<string>();
            if (File.Exists("vaccines.txt"))
            {
                selected = JsonMapper.ToObject<List<string>>(File.ReadAllText("vaccines.txt", Encoding.UTF8));
            }
            else
            {
                selected.AddRange(vaccines);
            }

            while (true)
            {
                PrintHeader();
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("▤ 백신 선택 (방향키, 스페이스)");
                Console.WriteLine();

                for (int i = 0; i <= vaccines.Length; i++)
                {
                    var msg = "   ";

                    if (i == vaccines.Length)
                    {
                        msg += "[ 예약시작 ]";
                    }
                    else if (selected.Contains(vaccines[i]))
                    {
                        msg += "[ V ] " + vaccines[i];
                    }
                    else
                    {
                        msg += "[   ] " + vaccines[i];
                    }

                    var bc = Console.ForegroundColor;
                    Console.ForegroundColor = (index == i ? ConsoleColor.Yellow : ConsoleColor.Gray);
                    Console.WriteLine(msg);
                    Console.ForegroundColor = bc;
                }

                var key = Console.ReadKey();
                int listLength = vaccines.Length + 1;
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        index = (index - 1 + listLength) % listLength;
                        break;
                    case ConsoleKey.DownArrow:
                        index = (index + 1 + listLength) % listLength;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        try
                        {
                            if (index == vaccines.Length)
                            {
                                search.VaccineTypes = selected.ToArray();
                                return;
                            }
                            else
                            {
                                if (selected.Contains(vaccines[index]))
                                {
                                    selected.Remove(vaccines[index]);
                                }
                                else
                                {
                                    selected.Add(vaccines[index]);
                                }
                            }
                        }
                        finally
                        {
                            File.WriteAllText("vaccines.txt", JsonMapper.ToJson(selected), Encoding.UTF8);
                        }
                        break;
                }
            }
        }
    }
}