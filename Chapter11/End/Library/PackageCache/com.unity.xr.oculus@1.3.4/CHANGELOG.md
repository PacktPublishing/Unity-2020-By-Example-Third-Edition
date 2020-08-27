# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).


## [1.3.4] - 2020-05-12
### Changes
- When Oculus Android is enabled in XR Management, Vulkan is removed from the Android graphics API list. It can manually be added back in to the list.

### Fixes
- Stats.PluginVersion wasn't properly null terminating the version string. It is now the correct length.

## [1.3.3] - 2020-04-08
### Changes
- Change XR Management dependency to 3.0.6 to resolve a package manager issue.

## [1.3.2] - 2020-04-08
### Changes
- Fixed a breaking change involving an incompatibility with versions of XR Management earlier than 3.2.4.

## [1.3.1] - 2020-04-03
### Changes
- Updated XR Management dependency to 3.2.4

## [1.3.0] - 2020-03-16
### Changes
- Bundles up 1.2.5-preview.x changes for release.
- Minor version bump to add a loader callback API method.

## [1.2.5-preview.3] - 2020-03-09
### Changes
- Updated XR Management dependency to 3.2.0-preview.8
- When Oculus Android is enabled in XR Management, Vulkan is removed from the Android graphics API list. It can manually be added back in to the list.

## [1.2.5-preview.2] - 2020-02-27
### Changes
- Updated XR Management dependency to 3.2.0-preview.3
- Renamed Oculus loader

## [1.2.5-preview.1] - 2020-02-26
### Changes
- Updated XR Management dependency to 3.2.0-preview.2
- Implement XR Management Metadata interfaces

## [1.2.0] - 2020-02-25
### Changes
- Remove preview tag
- Add missing release notes for 1.1.5

## [1.2.0-preview.1] - 2020-02-14
### Changes
- Cleans up plugin graphics thread lifecylce
- Cleans up documentation
- Update to Oculus 1.44 plugin

### Adds
- Public foveation setting API
- Public Oculus statistics APIs
- Improved recentering support

### Fixes
- There was a crash on exit when using single threaded rendering.  This has been fixed.
- Fixed BeginFrame log spew.

## [1.1.5] - 2019-12-20
### Changes
- Cleans up documentation

## [1.1.5-preview] - 2019-12-19
### Changes
- Cleans up plugin graphics thread lifecylce
- Fixed a manifest merging issue with the 1.44 Oculus Integration assets

## [1.1.4] - 2019-12-13
- No changes, version rev only.

## [1.1.4-preview.1] - 2019-12-12
### Changes
- Expands internal performance profiling tooling
- Re-enables GLES2

### Fixes
- [Quest] Fixes an issue where resting then waking the device with the power button caused a black screen in the application (v12 quest runtime and up) 

## [1.1.4-preview] - 2019-12-03
### Fixes
- Occlusion mesh no longer renders in the preview view unless specified by the user.
- Fixed an issue where some entries in a custom AndroidManifest.xml were getting removed when V2 signing was enabled.

## [1.1.3] - 2019-11-27
- UNRELEASED
- No changes version rev only.

## [1.1.3-preview.1] - 2019-11-27
### Fixes
- Fixes a crash that occured when building an app without the android loader in the XR Management list

## [1.1.3-preview] - 2019-11-27
### Changes
- Adds FFR hookup for when using Quest and Vulkan 

## [1.1.2] - 2019-11-25
### Changes
- updates documentation
- updates minimum Unity version required (for Vulkan support)

## [1.1.2-preview] - 2019-11-25
### Fixes
- Enables Vulkan support on Quest and Go
- provider now uses correct occlusion mesh

### Known Issues
- Vulkan on Quest/Go does not currently support Multiview, this will be supported in a later release of the Unity Editor
- FFR on Vulkan on Quest/Go is not currently supported, this feature will be supported in a later release of the Unity Editor

## [1.1.1] - 2019-11-21
### Fixes
- viewport scale in the mirror view now uses scaled uvs
- fixed a potential manifest collision issue when using v2 signing

### Changes
- update XR Management dependency to 3.0.4
- increased the callbackOrder on the Android build processor script so that other scripts can execute first if need be
- renamed plugin libraries and cleaned up various error messages

## [1.1.0] - 2019-10-17
- version bump to 1.1.0, no code changes

## [1.1.0-preview] - 2019-10-17
### Fixes
- semver to reflect new backwards compatible functionality (color scale API, input subsystem layouts)

## [1.0.3-preview.1] - 2019-10-15
### Fixes
- thread safe color scale
- screenshot artifacts with SPI

## [1.0.3-preview] - 2019-09-27
### Fixes
- Fixed msaa issues on quest
- Fixed side-by-side screenshot functionality

### Changes
- Disables main framebuffer flag to save memory (~36MB on Quest)
- Input subsystem layouts to package

### Adds
- Color scale and offset api and helper class
- More oculus statistics (accessible via display subsystem api)
- User presence usage when using new input system 

## [1.0.2] - 2019-09-03
### Changes
- XR Plugin Management dependency

## [1.0.1] - 2019-08-28
### Fixes
- Input bugs
  - Go reported sceondary button when it should have reported a menu button
  - Quest and Rift S reported a thumbrest when they did not have one
  - Oculus Remote would never connect
- Timing issues upon pausing/resuming app on standalone HMDs
- V2signing checkbox for properly signed apks on quest

## [1.0.0] - 2019-07-10
### Changes
- Removed preview tag
- Update com.unity.xr.management dependency version
- Migrate away from experience subsystem
- Update Boundary Points when recentering and changing the tracking space origin mode
- Fixed spatializer .meta files

## [0.9.0-preview] - 2019-06-17
### Added
- Oculus 1.37 runtime upgrade
- Oculus audio spatializer plugin

## [0.8.4-preview] - 2019-06-10
### Added
- Single Pass Instancing support on PC DX11

## [0.8.0-preview] - 2019-06-06
### Added
- Rendering and input support
- Arm64 support for mobile builds
- Depth support
- Render viewport scale
- Eye texture resolution scale
- Culling pose pullback
- Win32 compatibility
- Updates minimum unity version to 2019.2
- Input tracking reference node reporting
- Updates to Oculus plugin 1.34
- XRStats support
- Device relative eye positions
- Recenter functionality
- Registration of tracking references
- Tests to package
- Haptics Functionality
