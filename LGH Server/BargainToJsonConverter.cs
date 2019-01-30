using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGH_Server
{
    class BargainToJsonConverter
    {
        private string myPath;
        private string MyPath
        {
            get
            {
                if (myPath == null)
                {
                    string systemPath = AppDomain.CurrentDomain.BaseDirectory;

                    myPath = Path.Combine(systemPath, "AllBargains.txt");
                }
                return myPath;
            }
        }

        private string newDocument;

        public void UpdateLocalJsonFileFrom(List<ABargain> _allBargains)
        {

            //skriv til json dokument i samme folder som ordlisten ligger, brug dne til at opdatere dokumnentet lokalt før push

            StringBuilder sb = new StringBuilder();

            sb.AppendLine().Append("{");

            for (int i = 0; i < _allBargains.Count; i++)
            {
                // If the last bargain finish document.SSS
                if (i == _allBargains.Count - 1)
                {
                    sb.AppendLine().Append("'{0}':{ 'Category':'{1}','Name':'{2}','Price':'{3}','Amount':'{4}', 'RunPeriod':'{5}', 'SalesPlace':'{6}'}")
                    .Replace("{0}", i.ToString())
                    .Replace("{1}", _allBargains[i].category)
                    .Replace("{2}", _allBargains[i].name)
                    .Replace("{3}", _allBargains[i].price)
                    .Replace("{4}", _allBargains[i].amount)
                    .Replace("{5}", _allBargains[i].runPeriod)
                    .Replace("{6}", _allBargains[i].salesPlace);
                }
                else
                {
                    sb.AppendLine().Append("'{0}':{ 'Category':'{1}','Name':'{2}','Price':'{3}','Amount':'{4}', 'RunPeriod':'{5}', 'SalesPlace':'{6}'},")
                     .Replace("{0}", i.ToString())
                     .Replace("{1}", _allBargains[i].category)
                     .Replace("{2}", _allBargains[i].name)
                     .Replace("{3}", _allBargains[i].price)
                     .Replace("{4}", _allBargains[i].amount)
                     .Replace("{5}", _allBargains[i].runPeriod)
                     .Replace("{6}", _allBargains[i].salesPlace);
                }

            }

            sb.AppendLine().Append("}");
            newDocument = sb.Replace("'", "\"").ToString();
            File.WriteAllText(MyPath, newDocument); //tjek om der blir skrevet rigtigt til filen
        }
    }
}

