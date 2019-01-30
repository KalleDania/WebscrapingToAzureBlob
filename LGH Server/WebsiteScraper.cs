using LGH_Server.Webscraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LGH_Server
{

    class WebsiteScraper
    {
        private const string mainWebsiteToScan = "https://www.tilbudsugen.dk/tilbudsavis";

        HTMLCopyer htmlCopyer = new HTMLCopyer();
        URLShopSorter URLShopSorter = new URLShopSorter();
        BargainExtractor bargainExtractor = new BargainExtractor();

        GetHtmlCode getHtmlCode = new GetHtmlCode();

        List<string> linksOnMainWebsite;
        List<string> storesOfInterestOnMainWebsite;

        private Dictionary<string, List<string>> allBargains = new Dictionary<string, List<string>>();

        public Dictionary<string, List<string>> ScrapeNewBargains() 
        {
            int finishedThreads = 0;

            void GetBargainsOfPaper(string _storeUrl)
            {

                lock (this)
                {
                    Console.WriteLine(_storeUrl);
                    if (_storeUrl.Contains("Aldi"))
                    {
                        string test = "";
                    }

                    if (!allBargains.ContainsKey(_storeUrl))
                    {
                        allBargains.Add(_storeUrl, new List<string>());
                    }
                    else
                    {
                        finishedThreads++;
                        return;
                    }
                }


                int pageToScan = 1;
                string unNumberedUrl = _storeUrl.Remove(_storeUrl.Length - 1);
                string finalUrl = mainWebsiteToScan + "/" + unNumberedUrl + pageToScan;
                string html = getHtmlCode.GetHtml(finalUrl);
                int pagesInPaper = Int32.Parse(GetPartOfHtmlAfter(html, "LAST_PAGE = '", "'"));
                string runningDate = getBetween(html, "Gyldig fra:</strong>\n\t\t\t\t\t\t\t<div>", "</div>");

                while (true)
                {

                    finalUrl = mainWebsiteToScan + "/" + unNumberedUrl + pageToScan;

                    // Since Tilbudsavisen/aldi har lavet så man ka gå til sider langt over det antal der er må vi lave et workafround istedet for at teste på sidenummeret.
                    html = getHtmlCode.GetHtml(finalUrl);

                    if (html != null)
                    {
                        // Scanner første side på store i.
                        foreach (var bargain in bargainExtractor.ExtractBargainsFromPage(html))
                        {
                            allBargains[_storeUrl].Add(_storeUrl + " " + bargain);
                        }
                    }


                    pageToScan++;

                    if (pageToScan > pagesInPaper)
                    {
                        string test = _storeUrl;
                        allBargains[_storeUrl].Add("Finished with store: " + _storeUrl + " where paper bargains run in this Dato period: " + runningDate);
                        break;
                    }
                }


                finishedThreads++;
            }

            // 0 lav liste med alle links på siden
            linksOnMainWebsite = htmlCopyer.GetAllShopUrlsFromTilbudsugen(mainWebsiteToScan);

            // 1 lav en liste med alle butikker aka sorter ikke butikker fra
            storesOfInterestOnMainWebsite = URLShopSorter.SortShops(linksOnMainWebsite);

            // 2 tag den første butik og klik ind på side 1 og scan
            int totalThreadCount = storesOfInterestOnMainWebsite.Count - 1;

            for (int i = 0; i < storesOfInterestOnMainWebsite.Count - 1; i++)
            {
                string storePageToScan = storesOfInterestOnMainWebsite[i];           
                Thread thread = new Thread(() => GetBargainsOfPaper(storePageToScan));
                thread.Start();
            }

            // Wait for all threads to finish before contiunig.
            while (true)
            {
                if (finishedThreads == totalThreadCount)
                {
                    break;
                }
            }


                        return allBargains;
        }

        public string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        private string GetPartOfHtmlAfter(string _htmlDocument, string _after, string _before)
        {

            string maybePageCount = "";
            bool writingPageCount = false;
            char currentChar;

            for (int i = 0; i < _htmlDocument.Length; i++)
            {
                currentChar = _htmlDocument[i];

                if (writingPageCount)
                {

                    if (currentChar.ToString() == _before)
                    {
                        return maybePageCount;
                    }
                    else
                    {
                        maybePageCount = maybePageCount + currentChar;
                    }
                }
                else
                {
                    // Add the new char.
                    maybePageCount = maybePageCount + currentChar;

                    if (maybePageCount.Contains(_after))
                    {
                        writingPageCount = true;
                        maybePageCount = "";
                    }
                }
            }
            return null;
        }
    }
}
