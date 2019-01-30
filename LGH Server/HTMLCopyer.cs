using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;


namespace LGH_Server.Webscraping
{
    class HTMLCopyer
    {


        public List<string> GetAllShopUrlsFromTilbudsugen(string url)
        {
            List<string> GetFilteredURLs(string _htmlCode)
            {
                List<string> innerUrls = new List<string>();
                string maybeUrl = "";
                bool writingUrl = false;
                char currentChar;

                for (int i = 0; i < _htmlCode.Length; i++)
                {
                    currentChar = _htmlCode[i];

                    if (writingUrl)
                    {
                        // Last symbol of the URLs is '.
                        if(currentChar.ToString() == "'")
                        {
                            innerUrls.Add(maybeUrl);
                            maybeUrl = "";
                            writingUrl = false;
                        }
                        else
                        {
                            maybeUrl = maybeUrl + currentChar;
                        }                   
                    }
                    else
                    {
                        // Add the new char.
                        maybeUrl = maybeUrl + currentChar;

                        if (maybeUrl.Contains("<a href='/tilbudsavis/"))
                        {
                            writingUrl = true;
                            maybeUrl = "";
                        }
                    }
                }

                return innerUrls;
            }

            string GetHtmlCode()
            {
                WebRequest request = WebRequest.Create(url);

                StreamReader reader;
                try
                {
                    reader = new StreamReader(request.GetResponse().GetResponseStream());
                }
                catch (Exception)
                {
                    Console.WriteLine("Webserver does not allow this call.");
                    return null;
                    throw;
                }

                return reader.ReadToEnd();
            }


            string htmlCode = GetHtmlCode();

            List<string> shopUrls = GetFilteredURLs(htmlCode);

            return shopUrls;
        }
    }
}


