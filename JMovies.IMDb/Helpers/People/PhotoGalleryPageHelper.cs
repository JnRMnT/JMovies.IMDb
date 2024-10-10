using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.Settings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JMovies.IMDb.Helpers.People
{
    /// <summary>
    /// Class responsible for parsing the Media Images
    /// </summary>
    public class PhotoGalleryPageHelper
    {
        /// <summary>
        /// Main Parse method of the Photo Gallery Page
        /// </summary>
        /// <param name="person">Person instance that is populated</param>
        /// <param name="personURL">URL of the person page</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        public static void Parse(Person person, string personURL, PersonDataFetchSettings settings)
        {
            List<Image> imageContainer = new List<Image>();
            FetchPhotos(personURL, 0, settings, ref imageContainer);
            person.Photos = imageContainer;
        }

        /// <summary>
        /// Fetches all photos of nth page of the media gallery page
        /// </summary>
        /// <param name="personURL">URL of the person page</param>
        /// <param name="currentPage">Index of the current page to fetch</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <param name="imageContainer">Reference of the container that is holding the images</param>
        private static void FetchPhotos(string personURL, int currentPage, PersonDataFetchSettings settings, ref List<Image> imageContainer)
        {
            string photoGalleryURL = personURL + "/" + IMDbConstants.PhotoGalleryPath + "?page=" + (currentPage + 1);
            WebRequest photoGalleryPageRequest = HttpHelper.InitializeWebRequest(photoGalleryURL, settings);
            HtmlDocument photoGalleryPageDocument = HtmlHelper.GetNewHtmlDocument();
            using (Stream stream = HttpHelper.GetResponseStream(photoGalleryPageRequest, settings))
            {
                photoGalleryPageDocument.Load(stream, Encoding.UTF8);
            }

            if (photoGalleryPageDocument.DocumentNode != null)
            {
                HtmlNode mediaIndexNode = photoGalleryPageDocument.DocumentNode.QuerySelector("[data-testid=section-images]");
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
                            FetchPhotos(personURL, currentPage + 1, settings, ref imageContainer);
                        }
                    }
                }
            }
        }
    }
}
