using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGH_Server.Webscraping
{
    class List_ProductsOfInterest
    {

        private string myPath;
        private string MyPath
        {
            get
            {
                if (myPath == null)
                {
                    string systemPath = AppDomain.CurrentDomain.BaseDirectory;

                    myPath = Path.Combine(systemPath, "wordlistProductsOfInterest.txt");
                }
                return myPath;
            }
        }


        public readonly List<string> productsOfInterest;

        public List_ProductsOfInterest()
        {
            productsOfInterest = ConvertListToCS();
        }


        private List<string> ConvertListToCS()
        {
            List<string> tempList = new List<string>();

            string fullDocument = File.ReadAllText(MyPath);
            string productOfInterest = "";

            for (int i = 0; i < fullDocument.Length; i++)
            {
                string c = fullDocument[i].ToString();

                if (c.Equals(","))
                {   
                    productOfInterest = productOfInterest.Replace("\r", "").Replace("\n", "");
                    tempList.Add(productOfInterest);
                    productOfInterest = "";
                }

                else if (!c.Equals(" "))
                {
                    productOfInterest = productOfInterest + fullDocument[i];
                }
            }

            return tempList;
        }
    }
}
