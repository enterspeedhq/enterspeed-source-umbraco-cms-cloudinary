using Enterspeed.Source.UmbracoCms.Services;
using System;
using System.Linq;
using Enterspeed.Source.UmbracoCms.Cloudinary.Models;
using HtmlAgilityPack;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Templates;
using Umbraco.Cms.Core.Web;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Services
{
    public class CloudinaryUmbracoRichTextParser : IUmbracoRichTextParser
    {
        private readonly HtmlLocalLinkParser _htmlLocalLinkParser;
        private readonly HtmlImageSourceParser _htmlImageSourceParser;
        private readonly IMediaService _mediaService;
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly ICloudinaryService _cloudinaryService;

        public CloudinaryUmbracoRichTextParser(HtmlLocalLinkParser htmlLocalLinkParser, HtmlImageSourceParser htmlImageSourceParser, IMediaService mediaService, 
                                                IUmbracoContextFactory umbracoContextFactory, ICloudinaryService cloudinaryService)
        {
            _htmlLocalLinkParser = htmlLocalLinkParser;
            _htmlImageSourceParser = htmlImageSourceParser;
            _mediaService = mediaService;
            _umbracoContextFactory = umbracoContextFactory;
            _cloudinaryService = cloudinaryService;
        }

        public string ParseInternalLink(string htmlValue)
        {
            var parsedHtmlValue = _htmlLocalLinkParser.EnsureInternalLinks(htmlValue);
            parsedHtmlValue = _htmlImageSourceParser.EnsureImageSources(parsedHtmlValue);

            return parsedHtmlValue;
        }

        public string PrefixRelativeImagesWithDomain(string html, string mediaDomain)
        {
            if (string.IsNullOrWhiteSpace(html) || string.IsNullOrWhiteSpace(mediaDomain))
            {
                return html;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var imageNodes = htmlDocument.DocumentNode.SelectNodes("//img");
            var linkNodes = htmlDocument.DocumentNode.SelectNodes("//a");
            if ((imageNodes == null || !imageNodes.Any()) && (linkNodes == null || !linkNodes.Any()))
            {
                return html;
            }

            var mediaDomainUrl = new Uri(mediaDomain);
            foreach (var imageNode in imageNodes)
            {
                var src = imageNode.GetAttributeValue("src", string.Empty);
                if (src.StartsWith("/media/"))
                {
                    var publishedMedia = GetPublishedMedia(src);

                    int.TryParse(imageNode.GetAttributeValue("height", string.Empty), out var height);
                    int.TryParse(imageNode.GetAttributeValue("width", string.Empty), out var width);

                    var cloudinaryUrl = publishedMedia is not null 
                        ? _cloudinaryService.GetCloudinaryUrl(publishedMedia, new CloudinaryTransformation { Height = height, Width = width }) 
                        : null;

                    src = cloudinaryUrl ?? $"{mediaDomainUrl.AbsoluteUri.TrimEnd('/')}/{src.TrimStart('/')}";
                    imageNode.SetAttributeValue("src", src);
                }
            }

            foreach (var linkNode in linkNodes)
            {
                var href = linkNode.GetAttributeValue("href", string.Empty);
                if (href.StartsWith("/media/"))
                {
                    var publishedMedia = GetPublishedMedia(href);

                    var cloudinaryUrl = publishedMedia is not null 
                        ? _cloudinaryService.GetCloudinaryUrl(publishedMedia) 
                        : null;

                    href = cloudinaryUrl ?? $"{mediaDomainUrl.AbsoluteUri.TrimEnd('/')}/{href.TrimStart('/')}";
                    linkNode.SetAttributeValue("href", href);
                }
            }

            return htmlDocument.DocumentNode.InnerHtml;
        }

        private IPublishedContent GetPublishedMedia(string src)
        {
            var srcWithoutQueryString = src.Split('?')[0];
            var media = _mediaService.GetMediaByPath(srcWithoutQueryString);

            if (media is null)
            {
                return null;
            }

            using var context = _umbracoContextFactory.EnsureUmbracoContext();
            return context.UmbracoContext.Media.GetById(media.Id);
        }
    }
}
