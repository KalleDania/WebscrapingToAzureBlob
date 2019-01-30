using LGH_Server.Webscraping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LGH_Server
{
    class Program
    {
        private static WebsiteScraper websiteScraper;
        private static StringToBargainConverter stringToBargainConverter;
        private static BargainToJsonConverter bargainToJsonConverter;
        private static DocumentPusher documentPusher;

        private static Dictionary<string, List<string>> allBargains;
        private static List<ABargain> cleanedUpBargainList;

        public bool finishedPushingToServer = false;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Scraping the root site https://www.tilbudsugen.dk/tilbudsavis which contains many of this weeks bargain papers from danish groceristores and sort them for easy sorting and viewing. Expect it to take anywhere from 10 to 30mins depending on your rig. \n\n");

                // 1. Scrape websites.
                websiteScraper = new WebsiteScraper();
                allBargains = websiteScraper.ScrapeNewBargains();
                Console.WriteLine("ScrapeNewBargains Done.");

                // 2. Clean up collection.
                stringToBargainConverter = new StringToBargainConverter();
                cleanedUpBargainList = stringToBargainConverter.GetConvertedDictionary(allBargains);
                Console.WriteLine("GetConvertedDictionary Done.");

                // 3. Create local backup document.
                bargainToJsonConverter = new BargainToJsonConverter();
                bargainToJsonConverter.UpdateLocalJsonFileFrom(cleanedUpBargainList);
                Console.WriteLine("UpdateLocalJsonFileFrom Done.");

                // 4. Push document to server.
                documentPusher = new DocumentPusher();
                documentPusher.PushFilTilWebsite();

                while (!documentPusher.IsFinishedPushingToServer())
                {
                    // Wait
                }
                Console.WriteLine("PushFilTilWebsite Done at:{0}, next scan in 12 hours!", DateTime.Now);

                Console.WriteLine("\n\n SCRAPING FINISHED. PRINTING RESULTS!: \n\n");

                for (int i = 0; i < cleanedUpBargainList.Count; i++)
                {
                    Console.WriteLine("Salesplace: {0} Productname: {1} Productprice: {2}", cleanedUpBargainList[i].salesPlace, cleanedUpBargainList[i].name, cleanedUpBargainList[i].price);
                }

                // Sleep application maintthread for 12 hours then repeat scan.
                Thread.Sleep(3600000 * 12);
            }
        }
    }
}
