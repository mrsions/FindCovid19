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
        public static string BrowserBin = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

        static void Main(string[] args)
        {
            try
            {
                GetBrowser();
                GetLocation(out var x, out var y, out var minX, out var minY, out var maxX, out var maxY);

                PrintHeader();
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("▤ 검색시작! (백신을 찾으면 웹창을 띄워줍니다!)");
                Console.WriteLine();
                var system = new SearchSystem();
                system.X = x;
                system.Y = y;
                system.MinX = minX;
                system.MinY = minY;
                system.MaxX = maxX;
                system.MaxY = maxY;
                system.BrowserBin = BrowserBin;
                system.VaccineTypes = new string[] { "아스트라제네카", "화이자", "모더나" };
                system.Start();

                while (true)
                {
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
");
        }

        private static void GetBrowser()
        {
            PrintHeader();
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("▤ 브라우저 선택");
            Console.WriteLine();

            var browsers = (from b in Browser.GetBrowsers() where !b.Name.Contains("Internet Explorer") && !b.Name.Contains("Default Host Application") select b).ToList();
            if (browsers.Count == 0)
            {
                Console.WriteLine("▶ 브라우저를 찾을 수 없습니다. 브라우저를 설치한 뒤 실행 해 주세요.");
                Console.Write("종료하려면 아무 키나 누르십시오 . . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else if (browsers.Count == 1)
            {
                BrowserBin = browsers[0].Path;
            }
            else
            {
                for (int i = 0; i < browsers.Count; i++)
                {
                    Console.WriteLine($" {i}) {browsers[i].Name} ({browsers[i].Version})");
                }
                Console.WriteLine();

                while (true)
                {
                    Console.Write("사용할 브라우저 번호를 입력해 주세요. ) ");
                    try
                    {
                        int i = int.Parse(Console.ReadLine());
                        BrowserBin = browsers[i].Path;
                        break;
                    }
                    catch
                    {
                    }
                }
            }

            Console.WriteLine();
        }

        private static void GetLocation(out double x, out double y, out double minX, out double minY, out double maxX, out double maxY)
        {
            PrintHeader();
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("▤ 위치 입력");
            Console.WriteLine();

            if (File.Exists("location.txt"))
            {
                Console.WriteLine("▶ 기존에 입력된 위치가 있습니다.");
                Console.WriteLine("URL) " + File.ReadAllText("location.txt"));
                Console.WriteLine();
                Console.Write("기존 위치를 사용하시겠습니까? (Y/N)");
                while (true)
                {
                    var line = Console.ReadLine().ToLower();
                    if (line.StartsWith("y"))
                    {
                        if (SetupQuery(File.ReadAllText("location.txt", Encoding.UTF8), out x, out y, out minX, out minY, out maxX, out maxY))
                        {
                            Console.WriteLine();
                            return;
                        }
                    }
                    else if (line.StartsWith("n"))
                    {
                        break;
                    }
                }
            }

            Console.WriteLine("▶ 지시에 따라 위치를 지정하신 뒤, url을 입력해주세요.");
            Console.WriteLine("   1. 열린 네이버창을 선택한다. (혹은 입력 https://m.place.naver.com/rest/vaccine?vaccineFilter=used)");
            Console.WriteLine("   2. 지도상에서 검색할 위치 및 폭을 맞춥니다. (넓고 좁음까지 포함)");
            Console.WriteLine("   3. 상단의 [현 지도에서 검색]을 누른다.");
            Console.WriteLine("   4. url을 복사한다.");
            Console.WriteLine("   5. 검은색 콘솔창에 마우스 오른쪽 버튼을 누른다.");
            Console.WriteLine("   6. 엔터");
            Console.WriteLine();
            Browse("https://m.place.naver.com/rest/vaccine?vaccineFilter=used");

            while (true)
            {
                Console.Write("URL) ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (SetupQuery(input, out x, out y, out minX, out minY, out maxX, out maxY))
                    {
                        File.WriteAllText("location.txt", input, Encoding.UTF8);
                        Console.WriteLine();
                        return;
                    }
                    Console.WriteLine("값이 잘못입력됐습니다. url을 입력해주세요. 예) https://m.place.naver.com/rest/vaccine?vaccineFilter=used&x=127.0224661&y=37.2502279&bounds=126.9859452%3B37.2320523%3B127.0589871%3B37.2683992");
                    Console.WriteLine();
                }
            }
        }

        private static bool SetupQuery(string input, out double x, out double y, out double minX, out double minY, out double maxX, out double maxY)
        {
            const string PREFIX = "https://m.place.naver.com/rest/vaccine?";

            if (input.StartsWith(PREFIX))
            {
                string queryString = input.Substring(PREFIX.Length);
                var query = HttpUtility.ParseQueryString(queryString);
                try
                {
                    x = double.Parse(query["x"]);
                    y = double.Parse(query["y"]);
                    var bounds = query["bounds"].Split(';');
                    minX = double.Parse(bounds[0]);
                    minY = double.Parse(bounds[1]);
                    maxX = double.Parse(bounds[2]);
                    maxY = double.Parse(bounds[3]);
                    return true;
                }
                catch (Exception e)
                {
                }
            }
            x = y = minX = minY = maxX = maxY = 0;
            return false;
        }

        public static void Browse(string url)
        {
            using (var p = Process.Start(new ProcessStartInfo
            {
                FileName = BrowserBin,
                Arguments = url
            }))
            {
                p.WaitForInputIdle();
            }
        }
    }
}
