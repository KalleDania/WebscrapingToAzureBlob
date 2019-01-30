using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGH_Server.Webscraping
{
    class StringToBargainConverter
    {
        List_ProductsOfInterest wordList = new List_ProductsOfInterest();


        public List<ABargain> GetConvertedDictionary(Dictionary<string, List<string>> _allBargainsByStore)
        {

            List<ABargain> convertedToBargains = new List<ABargain>();


            foreach (KeyValuePair<string, List<string>> storeList in _allBargainsByStore)
            {
                string salesPlace = getBetween(storeList.Key, "/", "/");
                string runPeriod = GetRestFrom(storeList.Value[storeList.Value.Count - 1], "period: ");

                for (int i = 0; i < storeList.Value.Count - 1; i++) // -1 da vi ikke vil lave et ABargain entry ud af den sidste da det bare er den der older dato.
                {
                    string source = storeList.Value[i];

                    for (int j = 0; j < wordList.productsOfInterest.Count; j++)
                    {
                        if (source.Contains(wordList.productsOfInterest[j]))
                        {
                            string category = wordList.productsOfInterest[j];
                            string name = getName(source);
                            if (name.Length != 0)
                            {
                                source = source.Replace(name, "");
                            }
                            else
                            {
                                name = "BAD NAME";
                            }
                            string amount = getBetween(source, " ", ".");
                            if (amount.Length != 0)
                            {
                                source = source.Replace(amount, "");
                            }
                            else
                            {
                                amount = "BAD AMOUNT";
                            }
                            string price = GetRestFrom(source, " ");

                            // Fjern danske bogstaver da JSON ikke kan læse dem. 
                            category = category.Replace("æ", "ae").Replace("ø", "oe").Replace("å", "aa").Replace("Æ", "Ae").Replace("Ø", "Oe").Replace("Å", "Aa");
                            salesPlace = salesPlace.Replace("æ", "ae").Replace("ø", "oe").Replace("%", "oe").Replace("å", "aa").Replace("Æ", "Ae").Replace("Ø", "Oe").Replace("Å", "Aa");
                            name = name.Replace("æ", "ae").Replace("ø", "oe").Replace("å", "aa").Replace("Æ", "Ae").Replace("Ø", "Oe").Replace("Å", "Aa");
                            amount = name.Replace("æ", "ae").Replace("ø", "oe").Replace("å", "aa").Replace("Æ", "Ae").Replace("Ø", "Oe").Replace("Å", "Aa");

                            price = new string(price.Where(c => char.IsDigit(c) || c.ToString() == "." || c.ToString() == ",").ToArray()); // remove all none-numbers.
                            category = new string(category.Where(c => char.IsLetter(c)).ToArray());  // remove all none-numbers and none legal letters like æ ø å;
                            salesPlace = new string(salesPlace.Where(c => char.IsLetter(c)).ToArray());
                            name = new string(name.Where(c => char.IsLetter(c)).ToArray());
                            amount = new string(amount.Where(c => char.IsLetter(c)).ToArray());

                            convertedToBargains.Add(new ABargain(category, salesPlace, runPeriod, name, amount, price));
                            // Since we found a matching word and this listentry then is acecpted, skip the execution of the rest of the iteration.
                            goto NextListEntry;
                        }
                    }


                    storeList.Value.RemoveAt(i);
                NextListEntry:
                    continue;
                }
            }

            //sorter list items og returner.
            // ... Sort the numbers by their first digit.
            //     We use ToString on each number.
            //     We access the first character of the string and compare that.
            //     This uses a lambda expression.
            convertedToBargains.Sort((a, b) => (a.name.CompareTo(b.name)));

            return convertedToBargains;
        }

        public string getName(string strSource)
        {
            string strStart = "side_1 ";
            string strEnd = "1";

            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                if (End <= 0)
                {
                    return "";
                }
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }


        public string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);

                if (End <= 0)
                {
                    return "";
                }
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }


        public string GetRestFrom(string strSource, string strStart)
        {
            int Start;

            if (strSource.Contains(strStart))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                return strSource.Substring(Start, strSource.Length - Start);
            }
            else
            {
                return "";
            }
        }

    }
}
