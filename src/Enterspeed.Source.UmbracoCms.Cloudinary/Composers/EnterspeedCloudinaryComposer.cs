using Enterspeed.Source.UmbracoCms.Base.DataPropertyValueConverters;
using Enterspeed.Source.UmbracoCms.Base.Handlers.Media;
using Enterspeed.Source.UmbracoCms.Base.Providers;
using Enterspeed.Source.UmbracoCms.Base.Services;
using Enterspeed.Source.UmbracoCms.Cloudinary.Handlers.Media;
using Enterspeed.Source.UmbracoCms.Cloudinary.Provider;
using Enterspeed.Source.UmbracoCms.Cloudinary.Services;
using Enterspeed.Source.UmbracoCms.V9Plus;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;

namespace Enterspeed.Source.UmbracoCms.Cloudinary.Composers
{
    [ComposeAfter(typeof(EnterspeedComposer))]
    public class EnterspeedCloudinaryComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<ICloudinaryService, CloudinaryService>();
            builder.Services.AddSingleton<ICloudinaryConfigurationService, CloudinaryConfigurationService>();
            builder.Services.AddUnique<IUmbracoRichTextParser, CloudinaryUmbracoRichTextParser>();
            builder.Services.AddUnique<IUmbracoMediaUrlProvider, CloudinaryMediaUrlProvider>();

            builder.EnterspeedJobHandlers()
                .Remove<EnterspeedMediaPublishJobHandler>()
                .Remove<EnterspeedMediaTrashedJobHandler>()
                .Append<EnterspeedCloudinaryMediaPublishJobHandler>()
                .Append<EnterspeedCloudinaryMediaTrashedJobHandler>();
        }
    }
}