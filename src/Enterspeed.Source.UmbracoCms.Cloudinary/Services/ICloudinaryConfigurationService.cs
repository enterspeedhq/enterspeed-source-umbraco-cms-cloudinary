using Enterspeed.Source.UmbracoCms.Cloudinary.Models.Configuration;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Services
{
    public interface ICloudinaryConfigurationService
    {
        EnterspeedCloudinaryConfiguration GetConfiguration();
        bool IsCloudinaryConfigured();
    }
}