using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LGH_Server.Webscraping
{
    class BargainExtractor
    {

        public List<string> ExtractBargainsFromPage(string _htmlDocument)
        {
     
            List<string> GetFilteredBargainTexts(string _htmlCode)
            {
                List<string> bargainTexts = new List<string>();
                string maybeBargain = "";
                bool writingBargain = false;
                char currentChar;

                for (int i = 0; i < _htmlCode.Length; i++)
                {
                    currentChar = _htmlCode[i];

                    if (writingBargain)
                    {
                        // Last symbol of the text is '.
                        if (currentChar.ToString() == "'")
                        {
                            bargainTexts.Add(maybeBargain);
                            maybeBargain = "";
                            writingBargain = false;
                        }
                        else
                        {
                            maybeBargain = maybeBargain + currentChar;
                        }
                    }
                    else
                    {
                        // Add the new char.
                        maybeBargain = maybeBargain + currentChar;

                        if (maybeBargain.Contains("['offerText'] = '"))
                        {
                            writingBargain = true;
                            maybeBargain = "";
                        }
                    }
                }

                return bargainTexts;
            }

            List<string> shopUrls = null;

            if (_htmlDocument != null && _htmlDocument.Length != 0)
            {
                shopUrls = GetFilteredBargainTexts(_htmlDocument);
            }
            else
            {
                string debugtest = "";
            }
     

            return shopUrls;
        }



    }
}
