using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMovies.IMDb.Helpers
{
    public class HtmlHelper
    {
        public static HtmlDocument GetNewHtmlDocument()
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Empty;
            htmlDocument.OptionWriteEmptyNodes = true;
            return htmlDocument;
        }
    }
}
