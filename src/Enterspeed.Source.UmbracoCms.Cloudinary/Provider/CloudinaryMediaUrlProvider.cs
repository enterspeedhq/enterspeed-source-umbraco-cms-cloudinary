﻿using Enterspeed.Source.UmbracoCms.Cloudinary.Services;
using Enterspeed.Source.UmbracoCms.Providers;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Provider
{
    public class CloudinaryMediaUrlProvider : IUmbracoMediaUrlProvider
    {
        private readonly ICloudinaryService _cloudinaryService;

        public CloudinaryMediaUrlProvider(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public string GetUrl(IPublishedContent media)
        {
            return _cloudinaryService.GetCloudinaryUrl(media);
        }
    }
}
