# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [4.0.0 - 2025-01-20]
### Breaking
- Updated dependency for `Enterspeed.Source.UmbracoCms` to require 5.0.0 or above

### Changed 
- Updated `CloudinaryDotNet` to 1.25.2

## [3.0.0 - 2025-01-20]
### Breaking
- Updated dependency for `Enterspeed.Source.UmbracoCms` to require 4.3.0 or above and below 5.0.0

## [2.1.0 - 2023-12-19]
### Added
- Medias in Rich Text Editor are now also using the Cloudinary URL and images has the height and width paramters as the Umbraco URL if an image is scaled in the editor.
- References to media items in property editors like `Media Picker`, `Multi URL Picker`, `Multi Node Tree Picker` and medias in `Grid editor` now also uses the Cloudinary URL. It's no longer only the media entity it self, that has the Cloudinary URL.

## [2.0.0 - 2023-12-14]
### Breaking
- Updated dependency for `Enterspeed.Source.UmbracoCms` to require 4.0.0 or above

### Changed 
- Updated `CloudinaryDotNet` to 1.24.0

## [1.0.0]
### Added
- Initial release
