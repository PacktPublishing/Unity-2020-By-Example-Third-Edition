# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.1.0-preview.1] - 2020-06-30

 * Adds the ability to add multiple render passes to the XR renderer, configure render pass properties enable/disable render passes dynamically.
 * Adds a debug view that shows the render textures for the render passes. This should support all texture types (MP, SPI, single texture) so you can see what the render pass renders.

## [1.0.1-preview.7] - 2020-04-29

 * Enable Android support.

## [1.0.1-preview.6] - 2020-04-08

 * Still compatible with 3.0.6 management, so make that lowest dependency to avoid dependency resolution issues.

## [1.0.1-preview.5] - 2020-04-08

 * Macro around XR Management 3.2 features.

## [1.0.1-preview.4] - 2020-04-07

 * Fix `error CS0184: The given expression is never of the provided ('MockHMDBuildSettings') type`

## [1.0.1-preview.3] - 2020-04-07

 * Updated dependency: `com.unity.xr.management` 3.0.6 -> 3.2.4

## [1.0.0-preview.2] - 2020-04-06

 * Fix render mode settings not being picked up on entering play mode
 * Fix sRGB support

## [1.0.0-preview.1] - 2020-02-20

 * Moved NativeConfig to MockHMD class.
 * Added manual and API documentation.
 * Updated dependency: `com.unity.xr.management` 3.0.5 -> 3.0.6

## [0.1.0-preview.3] - 2020-01-24

 * Added ability to choose render mode at build time in Project Settings -> MockHMD.
 * Fixed stereo separation to match legacy Mock HMD.
 * Updated dependency: `com.unity.xr.management` 3.0.1 -> 3.0.5
 * Removed log spam `SinglePass ON / OFF`

## [0.1.0-preview.2] - 2019-10-29

Fixed missing meta files issue.

## [0.1.0-preview.1] - 2019-08-02

### This is the first release of *Mock HMD XR Plugin \<com.unity.xr.mock-hmd\>*.

