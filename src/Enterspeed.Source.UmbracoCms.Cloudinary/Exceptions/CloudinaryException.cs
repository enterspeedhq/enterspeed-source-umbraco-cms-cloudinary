using System;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Exceptions
{
    public class CloudinaryException : Exception
    {
        private CloudinaryException()
        {
        }

        public CloudinaryException(string message)
            : base(message)
        {
        }
    }
}