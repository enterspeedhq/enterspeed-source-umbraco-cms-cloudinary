using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Enterspeed.Source.UmbracoCms.Cloudinary.Exceptions;
using Enterspeed.Source.UmbracoCms.Cloudinary.Models.Configuration;
using Umbraco.Cms.Core.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using System;
using Enterspeed.Source.UmbracoCms.Base.Extensions;
using Enterspeed.Source.UmbracoCms.Base.Services;
using Enterspeed.Source.UmbracoCms.Cloudinary.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly IEnterspeedConfigurationService _enterspeedConfigurationService;
        private readonly IEntityIdentityService _entityIdentityService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EnterspeedCloudinaryConfiguration _enterspeedCloudinaryConfiguration;

        public CloudinaryService(ICloudinaryConfigurationService cloudinaryConfigurationService, 
            IEnterspeedConfigurationService enterspeedConfigurationService, 
            IEntityIdentityService entityIdentityService, 
            IWebHostEnvironment webHostEnvironment)
        {
            if (!cloudinaryConfigurationService.IsCloudinaryConfigured())
            {
                throw new ConfigurationException("Missing configuration for Enterspeed:Cloudinary");
            }

            _enterspeedConfigurationService = enterspeedConfigurationService;
            _entityIdentityService = entityIdentityService;
            _webHostEnvironment = webHostEnvironment;
            _enterspeedCloudinaryConfiguration = cloudinaryConfigurationService.GetConfiguration();
        }

        public virtual void DeleteFromCloudinary(IMedia media)
        {
            var cloudinary = GetCloudinaryClient();

            var umbracoMediaId = _entityIdentityService.GetId(media);
            var cloudinaryPublicId = string.IsNullOrWhiteSpace(_enterspeedCloudinaryConfiguration.AssetFolder)
                ? umbracoMediaId
                : $"{_enterspeedCloudinaryConfiguration.AssetFolder}/{umbracoMediaId}";
            var deletionParams = new DeletionParams(cloudinaryPublicId)
            {
                Invalidate = true
            };
            var deletionResult = cloudinary.Destroy(deletionParams);

            if (deletionResult.Error is not null)
            {
                throw new CloudinaryException($"An error occurred while deleting media from Cloudinary. ${deletionResult.Error.Message}");
            }
        }

        public virtual string UploadToCloudinary(IMedia media)
        {
            var cloudinary = GetCloudinaryClient();

            var mediaStream = GetMediaStream(media);
            var umbracoMediaId = _entityIdentityService.GetId(media);

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(media.Name, mediaStream),
                PublicId = umbracoMediaId,
                DisplayName = $"{media.Name} - {umbracoMediaId}",
                Overwrite = true,
                Invalidate = true,
                AssetFolder = _enterspeedCloudinaryConfiguration.AssetFolder,
                UseAssetFolderAsPublicIdPrefix = !string.IsNullOrWhiteSpace(_enterspeedCloudinaryConfiguration.AssetFolder)
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            if (uploadResult.Error is not null)
            {
                throw new CloudinaryException($"An error occurred while uploading media to Cloudinary. ${uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public virtual string GetCloudinaryUrl(IPublishedContent umbracoMedia, CloudinaryTransformation cloudinaryTransformation = null)
        {
            var cloudinary = GetCloudinaryClient();

            var transformation = new Transformation();
            if (cloudinaryTransformation is not null && cloudinaryTransformation.Height > 0)
            {
                transformation.Height(cloudinaryTransformation.Height);
            }
            if (cloudinaryTransformation is not null && cloudinaryTransformation.Width > 0)
            {
                transformation.Width(cloudinaryTransformation.Width);
            }

            var mediaExtension = umbracoMedia.Value("umbracoExtension").ToString();
            var cloudinarySourcePath = !string.IsNullOrWhiteSpace(_enterspeedCloudinaryConfiguration.AssetFolder)
                ? $"{_enterspeedCloudinaryConfiguration.AssetFolder}/{umbracoMedia.Id}.{mediaExtension}"
                : $"{umbracoMedia.Id}.{mediaExtension}";

            var url = cloudinary.Api.UrlImgUp
                .Secure()
                .Transform(transformation)
                .BuildUrl(cloudinarySourcePath);

            return url;
        }

        private Stream GetMediaStream(IMedia media)
        {
            var umbracoMediaUri = new Uri(media.GetMediaUrl(_enterspeedConfigurationService.GetConfiguration()));

            byte[] mediaData;
            if (umbracoMediaUri.IsAbsoluteUri)
            {
                using var wc = new WebClient();
                mediaData = wc.DownloadData(umbracoMediaUri.OriginalString);
            }
            else
            {
                var wwwrootPath = _webHostEnvironment.WebRootPath;
                mediaData = System.IO.File.ReadAllBytes($"{wwwrootPath}{umbracoMediaUri.PathAndQuery}");
            }

            return new MemoryStream(mediaData);
        }

        private CloudinaryDotNet.Cloudinary GetCloudinaryClient()
        {
            var cloudinaryAccount = new Account(_enterspeedCloudinaryConfiguration.CloudName, _enterspeedCloudinaryConfiguration.ApiKey, _enterspeedCloudinaryConfiguration.ApiSecret);
            return new CloudinaryDotNet.Cloudinary(cloudinaryAccount);
        }
    }
}