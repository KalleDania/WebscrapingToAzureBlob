using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGH_Server.Webscraping
{


    class URLShopSorter
    {

        private readonly string[] shopCategoriesToKeep = new string[]
        {
            "Dagligvarer",
            "Online+Shopping",
            "Gr%C3%A6nsehandel",
            "Sundhed+og+personlig+pleje"
        };

        public List<string> SortShops(List<string> _allLinksOnMainWebsite)
        {

            List<string> sortedShops = new List<string>();

            foreach (var item in _allLinksOnMainWebsite)
            {
                //1 kun gem dem der har side_x til sidst da de er gateway knapperne ind til selve aviserne.
                if (item.Contains("side"))
                {
                    //2 kun gem dem der har noget med mad, grøntsager, drikkevarer, makeup, tøj eller tekstil at gøre.
                    for (int i = 0; i < shopCategoriesToKeep.Length; i++)
                    {
                        if (item.Contains(shopCategoriesToKeep[i]))
                        {
                            sortedShops.Add(item);
                            break;
                        }
                    }
                }
            }

            //3 hav dem alfabetisk sorteret.
            sortedShops.Sort();

            return sortedShops;
        }





        private readonly string[] shopsToKeep = new string[]
        {
            "Aldi",
            "Bilka",
            "Coop",
            "Brugsen",
            "fakta",
            "Kvickly",
            "Føtex",
            "Harald Nyborg",
            "Helsam",
            "Irma",
            "Let-Køb",
            "Lidl",
            "Løvbjerg",
            "MENY",
            "Købmand"
        };



        public List<string> SortShopMineTilbudDKs(List<string> _allLinksOnMainWebsite)
        {

            List<string> sortedShops = new List<string>();

            foreach (var item in _allLinksOnMainWebsite)
            {
                    //2 kun gem dem der har noget med mad, grøntsager, drikkevarer, makeup, tøj eller tekstil at gøre.
                    for (int i = 0; i < shopsToKeep.Length; i++)
                    {
                        if (item.ToLower().Contains(shopsToKeep[i].ToLower()))
                        {
                            sortedShops.Add(item);
                            break;
                        }
                    }
            }

            //3 hav dem alfabetisk sorteret.
            sortedShops.Sort();

            return sortedShops;
        }

    }
}
