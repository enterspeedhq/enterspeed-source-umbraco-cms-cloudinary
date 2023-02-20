using System;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.UmbracoCms.Cloudinary.Services;
using Enterspeed.Source.UmbracoCms.Data.Models;
using Enterspeed.Source.UmbracoCms.Exceptions;
using Enterspeed.Source.UmbracoCms.Handlers.Media;
using Enterspeed.Source.UmbracoCms.Models;
using Enterspeed.Source.UmbracoCms.Providers;
using Enterspeed.Source.UmbracoCms.Services;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Handlers.Media
{
    public class EnterspeedCloudinaryMediaPublishJobHandler : EnterspeedMediaPublishJobHandler
    {
        private readonly IEnterspeedPropertyService _enterspeedPropertyService;
        private readonly IEntityIdentityService _entityIdentityService;
        private readonly IEnterspeedConfigurationService _enterspeedConfigurationService;
        private readonly ICloudinaryService _cloudinaryService;

        public EnterspeedCloudinaryMediaPublishJobHandler(
            IEnterspeedPropertyService enterspeedPropertyService,
            IEnterspeedIngestService enterspeedIngestService,
            IEntityIdentityService entityIdentityService,
            IEnterspeedGuardService enterspeedGuardService,
            IEnterspeedConnectionProvider enterspeedConnectionProvider,
            IMediaService mediaService,
            IEnterspeedConfigurationService enterspeedConfigurationService,
            ICloudinaryService cloudinaryService)
            : base(
                enterspeedPropertyService,
                enterspeedIngestService,
                entityIdentityService,
                enterspeedGuardService,
                enterspeedConnectionProvider,
                mediaService,
                enterspeedConfigurationService)
        {
            _enterspeedPropertyService = enterspeedPropertyService;
            _entityIdentityService = entityIdentityService;
            _enterspeedConfigurationService = enterspeedConfigurationService;
            _cloudinaryService = cloudinaryService;
        }

        public override void Handle(EnterspeedJob job)
        {
            var media = GetMedia(job);
            if (!CanIngest(media, job))
            {
                return;
            }

            var cloudinaryMediaUrl = _cloudinaryService.UploadToCloudinary(media);
            var umbracoData = CreateMediaEntity(media, cloudinaryMediaUrl, job);

            Ingest(umbracoData, job);
        }

        protected UmbracoMediaEntity CreateMediaEntity(IMedia media, string cloudinaryMediaUrl, EnterspeedJob job)
        {
            try
            {
                return new UmbracoMediaEntity(media, _enterspeedPropertyService, _entityIdentityService, _enterspeedConfigurationService, cloudinaryMediaUrl);
            }
            catch (Exception e)
            {
                throw new JobHandlingException($"Failed creating entity ({job.EntityId}). Message: {e.Message}. StackTrace: {e.StackTrace}");
            }
        }
    }
}