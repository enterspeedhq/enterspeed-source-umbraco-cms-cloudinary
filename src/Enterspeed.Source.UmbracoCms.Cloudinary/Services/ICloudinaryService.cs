using Enterspeed.Source.UmbracoCms.Cloudinary.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Services
{
    public interface ICloudinaryService
    {
        void DeleteFromCloudinary(IMedia media);
        string UploadToCloudinary(IMedia media);
        string GetCloudinaryUrl(IPublishedContent umbracoMedia, CloudinaryTransformation cloudinaryTransformation = null);
    }
}