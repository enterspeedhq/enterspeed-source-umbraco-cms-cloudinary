using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.UmbracoCms.Data.Models;
using Enterspeed.Source.UmbracoCms.Exceptions;
using Enterspeed.Source.UmbracoCms.Handlers.Media;
using Enterspeed.Source.UmbracoCms.Models;
using Enterspeed.Source.UmbracoCms.Providers;
using System.Net;
using Umbraco.Cms.Core.Services;
using Enterspeed.Source.UmbracoCms.Cloudinary.Services;
using Microsoft.Extensions.Logging;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Handlers.Media
{
    public class EnterspeedCloudinaryMediaTrashedJobHandler : EnterspeedMediaTrashedJobHandler
    {
        private readonly IEnterspeedIngestService _enterspeedIngestService;
        private readonly IEnterspeedConnectionProvider _enterspeedConnectionProvider;
        private readonly IMediaService _mediaService;
        private readonly ICloudinaryService _cloudinaryService;

        public EnterspeedCloudinaryMediaTrashedJobHandler
        (
            IEnterspeedIngestService enterspeedIngestService,
            IEnterspeedConnectionProvider enterspeedConnectionProvider,
            ILogger<EnterspeedMediaTrashedJobHandler> logger,
            IMediaService mediaService,
            ICloudinaryService cloudinaryService)
            : base(
                enterspeedIngestService,
                enterspeedConnectionProvider,
                logger)
        {
            _enterspeedIngestService = enterspeedIngestService;
            _enterspeedConnectionProvider = enterspeedConnectionProvider;
            _mediaService = mediaService;
            _cloudinaryService = cloudinaryService;
        }

        public override void Handle(EnterspeedJob job)
        {
            var parsed = int.TryParse(job.EntityId, out var parsedId);
            var media = parsed ? _mediaService.GetById(parsedId) : null;

            _cloudinaryService.DeleteFromCloudinary(media);

            var deleteResponse = _enterspeedIngestService.Delete(media?.Id.ToString(), _enterspeedConnectionProvider.GetConnection(ConnectionType.Publish));
            if (!deleteResponse.Success && deleteResponse.Status != HttpStatusCode.NotFound)
            {
                throw new JobHandlingException($"Failed deleting entity ({job.EntityId}/{job.Culture}). Message: {deleteResponse.Message}");
            }
        }
    }
}