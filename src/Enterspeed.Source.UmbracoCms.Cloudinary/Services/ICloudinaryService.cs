using Umbraco.Cms.Core.Models;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Services
{
    public interface ICloudinaryService
    {
        void DeleteFromCloudinary(IMedia media);
        string UploadToCloudinary(IMedia media);
    }
}