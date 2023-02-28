# [Enterspeed Umbraco Source Cloudinary](https://www.enterspeed.com/) [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](./LICENSE) [![NuGet version](https://img.shields.io/nuget/v/Enterspeed.Source.UmbracoCms.Cloudinary)](https://www.nuget.org/packages/Enterspeed.Source.UmbracoCms.Cloudinary/) [![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/enterspeedhq/enterspeed-source-umbraco-cms-cloudinary/pulls)

## Documentation

This package will automatically upload the Umbraco media to Cloudinary and ingest the Cloudinary url to Enterspeed instead of the Umbraco url.

The package is a add-on to [Enterspeed.Source.UmbracoCms](https://www.nuget.org/packages/Enterspeed.Source.UmbracoCms).

The only configuration specific to this add-on is the Cloudinary environment credentials. These can be configured in the `appsettings.json` file.

``` json
"Enterspeed": {
    ...
    "Cloudinary": {
      "CloudName": "", // Required
      "ApiKey": "", // Required
      "ApiSecret": "", // Required
      "AssetFolder": "" // Optional
    }
  }
```

## Changelog

See new features, fixes and breaking changes in the changelog. [Changelog](https://github.com/enterspeedhq/enterspeed-source-umbraco-cms-cloudinary/blob/develop/CHANGELOG.md)

## Contributing

Pull requests are very welcome.  
Please fork this repository and make a PR when you are ready.  

Otherwise you are welcome to open an Issue in our [issue tracker](https://github.com/enterspeedhq/enterspeed-source-umbraco-cms-cloudinary/issues).

## License

The Cloudinary add-on for Enterspeed Umbraco integration is [MIT licensed](https://github.com/enterspeedhq/enterspeed-source-umbraco-cms-cloudinary/blob/develop/LICENSE)
