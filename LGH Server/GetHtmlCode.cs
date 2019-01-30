using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LGH_Server.Webscraping
{
    class GetHtmlCode
    {

        public string GetHtml(string _url)
        {
            WebRequest request = WebRequest.Create(_url);

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
    }
}
