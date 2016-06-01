using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient http = new HttpClient();

            var response = http.GetAsync(@"http://m.pikabu.ru/new?page=2").Result;
        }
    }
}
