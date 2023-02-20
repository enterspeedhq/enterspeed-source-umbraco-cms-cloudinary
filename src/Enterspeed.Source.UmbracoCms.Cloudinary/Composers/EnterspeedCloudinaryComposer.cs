using Enterspeed.Source.UmbracoCms.Cloudinary.Handlers.Media;
using Enterspeed.Source.UmbracoCms.Cloudinary.Services;
using Enterspeed.Source.UmbracoCms.Composers;
using Enterspeed.Source.UmbracoCms.DataPropertyValueConverters;
using Enterspeed.Source.UmbracoCms.Handlers.Media;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Composers
{
    [ComposeAfter(typeof(EnterspeedComposer))]
    public class EnterspeedCloudinaryComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<ICloudinaryService, CloudinaryService>();
            builder.Services.AddSingleton<ICloudinaryConfigurationService, CloudinaryConfigurationService>();

            builder.EnterspeedJobHandlers()
                .Remove<EnterspeedMediaPublishJobHandler>()
                .Remove<EnterspeedMediaTrashedJobHandler>()
                .Append<EnterspeedCloudinaryMediaPublishJobHandler>()
                .Append<EnterspeedCloudinaryMediaTrashedJobHandler>();
        }
    }
}