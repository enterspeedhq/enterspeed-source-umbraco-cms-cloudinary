using Enterspeed.Source.UmbracoCms.Cloudinary.Models.Configuration;
using Microsoft.Extensions.Configuration;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Services
{
    public class CloudinaryConfigurationService : ICloudinaryConfigurationService
    {
        private readonly IConfiguration _configuration;

        private EnterspeedCloudinaryConfiguration _enterspeedCloudinaryConfiguration;

        public CloudinaryConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EnterspeedCloudinaryConfiguration GetConfiguration()
        {
            if (_enterspeedCloudinaryConfiguration != null)
            {
                return _enterspeedCloudinaryConfiguration;
            }

            _enterspeedCloudinaryConfiguration = GetConfigurationFromSettingsFile();

            return _enterspeedCloudinaryConfiguration;
        }

        public bool IsCloudinaryConfigured()
        {
            var configuration = GetConfiguration();
            return configuration is not null;
        }

        private EnterspeedCloudinaryConfiguration GetConfigurationFromSettingsFile()
        {
            var configuration = _configuration.GetSection("Enterspeed").GetSection("Cloudinary").Get<EnterspeedCloudinaryConfiguration>();

            if (configuration is null 
                || string.IsNullOrWhiteSpace(configuration.CloudName) 
                || string.IsNullOrWhiteSpace(configuration.ApiKey) 
                || string.IsNullOrWhiteSpace(configuration.ApiSecret))
            {
                return null;
            }

            return configuration;
        }
    }
}