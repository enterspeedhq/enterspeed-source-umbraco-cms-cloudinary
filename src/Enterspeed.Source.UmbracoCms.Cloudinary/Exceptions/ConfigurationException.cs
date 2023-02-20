using System;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Exceptions
{
    public class ConfigurationException : Exception
    {
        private ConfigurationException()
        {
        }

        public ConfigurationException(string message)
            : base(message)
        {
        }
    }
}