using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.Settings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JMovies.IMDb.Helpers.Movies
{
    /// <summary>
    /// Class responsible for parsing the Media Images
    /// </summary>
    public static class PhotoGalleryPageHelper
    {
        /// <summary>
        /// Main Parse method of the Photo Gallery Page
        /// </summary>
        /// <param name="movie">Movie instance that is populated</param>
        /// <param name="movieURL">URL of the movie page</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        public static void Parse(Movie movie, string movieURL, ProductionDataFetchSettings settings)
        {
            List<Image> imageContainer = new List<Image>();
            FetchPhotos(movieURL, 0, settings, ref imageContainer);
            movie.MediaImages = imageContainer;
        }

        /// <summary>
        /// Fetches all photos of nth page of the media gallery page
        /// </summary>
        /// <param name="movieURL">URL of the movie page</param>
        /// <param name="currentPage">Index of the current page to fetch</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <param name="imageContainer">Reference of the container that is holding the images</param>
        private static void FetchPhotos(string movieURL, int currentPage, ProductionDataFetchSettings settings, ref List<Image> imageContainer)
        {
            string photoGalleryURL = movieURL + "/" + IMDbConstants.PhotoGalleryPath + "?page=" + (currentPage + 1);
            WebRequest photoGalleryPageRequest = HttpHelper.InitializeWebRequest(photoGalleryURL, settings);
            HtmlDocument photoGalleryPageDocument = HtmlHelper.GetNewHtmlDocument();
            using (Stream stream = HttpHelper.GetResponseStream(photoGalleryPageRequest, settings))
            {
                photoGalleryPageDocument.Load(stream, Encoding.UTF8);
            }

            if (photoGalleryPageDocument.DocumentNode != null)
            {
                HtmlNode mediaIndexNode = photoGalleryPageDocument.DocumentNode.QuerySelector("#media_index_content");
                if (mediaIndexNode != null)
                {
                    HtmlNode[] allImageNodes = mediaIndexNode.QuerySelectorAll("img").ToArray();
                    if (allImageNodes != null && allImageNodes.Length != 0)
                    {
                        int endIndex = allImageNodes.Length;
                        if (settings.MediaImagesFetchCount < imageContainer.Count + endIndex)
                        {
                            endIndex = settings.MediaImagesFetchCount - imageContainer.Count;
                        }

                        for (int i = 0; i < endIndex; i++)
                        {
                            HtmlNode imageNode = allImageNodes[i];
                            Image image = new Image
                            {
                                Title = imageNode.GetAttributeValue("alt", string.Empty).Prepare(),
                                URL = IMDBImageHelper.NormalizeImageUrl(imageNode.GetAttributeValue("src", string.Empty))
                            };
                            if (settings.FetchImageContents)
                            {
                                image.Content = IMDBImageHelper.GetImageContent(image.URL);
                            }
                            imageContainer.Add(image);
                        }

                        //Continue on other pages
                        if (settings.MediaImagesFetchCount > imageContainer.Count)
                        {
                            FetchPhotos(movieURL, currentPage + 1, settings, ref imageContainer);
                        }
                    }
                }
            }
        }
    }
}
