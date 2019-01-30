using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGH_Server
{
    public enum ItemCategory { OtherFoodstuffs, VegetablesAndFruits, Beverages, Makeup, Clothing, Other };


    class ABargain
    {
        public string salesPlace = "Føtex";
        private string mærke = "Diesel";
        public string runPeriod = "1/1/2027";
        private string startTime = "06.00";
         
        public string category;
        public string name;
        public string price;
        public string amount = "200g";
        public string endDate = "1/1/2027";
        public string discountPercent = "70%";

        public ABargain(string _category, string _salesPlace, string _runPeriod, string _name,  string _amount, string _price)
        {
            category = _category;
            salesPlace = _salesPlace;
            runPeriod = _runPeriod;
            name = _name;
            amount = _amount;
            price = _price;
        }
    }

}
