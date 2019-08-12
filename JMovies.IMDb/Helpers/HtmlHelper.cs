using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMovies.IMDb.Helpers
{
    /// <summary>
    /// Class responsible for providing utility methods related to HTML related operations
    /// </summary>
    public class HtmlHelper
    {
        /// <summary>
        /// Method responsible to initialize and configure a new Html Document
        /// </summary>
        /// <returns>Initialized and configured HtmlDocument instance</returns>
        public static HtmlDocument GetNewHtmlDocument()
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Empty;
            htmlDocument.OptionWriteEmptyNodes = true;
            return htmlDocument;
        }
    }
}
