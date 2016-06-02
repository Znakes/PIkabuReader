using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Web.Http;
using HttpStatusCode = Windows.Web.Http.HttpStatusCode;

namespace PIkabuReader.Core.ContentLoader
{

    public enum PikabuSection
    {
        Best,
        Hot,
        Fresh
    }

    public class PikabuApi
    {
        public const string SiteUrl = @"http://m.pikabu.ru";
        
        private const string HotSection = "hot";
        private const string BestSection = "best";
        private const string FreshSection = "new";
        private const string Page = "page";
        private const string Delimeter = "/";

        HttpClient http = new HttpClient();

        public async Task<PikabuPost[]> GetPostsByPage(int pageNumber, PikabuSection section)
        {
            string sectionStr = string.Empty;

            switch (section)
            {
                case PikabuSection.Best:
                    sectionStr = BestSection;
                    break;
                case PikabuSection.Hot:
                    sectionStr = HotSection;
                    break;
                case PikabuSection.Fresh:
                    sectionStr = FreshSection;
                    break;
            }

            var requestString = $@"{SiteUrl}/{sectionStr}?{Page}={pageNumber}";

            var responce = await http.GetAsync(new Uri(requestString));
            responce.EnsureSuccessStatusCode();
            return ParsePageToObjects(WebUtility.HtmlDecode(await responce.Content.ReadAsStringAsync()));

        }

        private PikabuPost[] ParsePageToObjects(string htmltext)
        {
            PikabuPostsParser parser = new PikabuPostsParser();

            return parser.GetPosts(htmltext);
        }
    }
}
