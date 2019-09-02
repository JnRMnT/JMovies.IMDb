using HtmlAgilityPack;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using Fizzler.Systems.HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using JMovies.IMDb.Providers;
using System.Linq;
using JMovies.IMDb.Entities.Misc;
using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.Settings;

namespace JMovies.IMDb.Helpers.Movies
{
    /// <summary>
    /// Class responsible for parsing the Media Images
    /// </summary>
    public class PhotoGalleryPageHelper
    {
        /// <summary>
        /// Main Parse method of the Photo Gallery Page
        /// </summary>
        /// <param name="movie">Movie instance that is populated</param>
        /// <param name="documentNode">Document Node of the photo gallery page</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        public static void Parse(Movie movie, HtmlNode documentNode, ProductionDataFetchSettings settings)
        {
            if (documentNode != null)
            {
                HtmlNode mediaIndexNode = documentNode.QuerySelector("#media_index_content");
                movie.MediaImages = new List<Image>();
                if (mediaIndexNode != null)
                {
                    HtmlNode[] allImageNodes = mediaIndexNode.QuerySelectorAll("img").ToArray();
                    if (allImageNodes != null && allImageNodes.Length != 0)
                    {
                        int endIndex = allImageNodes.Length;
                        if (settings.MediaImagesFetchCount < endIndex)
                        {
                            endIndex = settings.MediaImagesFetchCount;
                        }

                        for (int i = 0; i < endIndex; i++)
                        {
                            HtmlNode imageNode = allImageNodes[i];
                            Image image = new Image
                            {
                                Title = imageNode.GetAttributeValue("title", string.Empty),
                                URL = IMDBImageHelper.NormalizeImageUrl(imageNode.GetAttributeValue("src", string.Empty))
                            };
                            if (settings.FetchImageContents)
                            {
                                image.Content = IMDBImageHelper.GetImageContent(image.URL);
                            }
                            movie.MediaImages.Add(image);
                        }
                    }
                }
            }
        }
    }
}
