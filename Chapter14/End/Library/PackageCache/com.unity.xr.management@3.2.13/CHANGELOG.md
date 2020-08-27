# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.2.13] - 2020-07-01

* Package installation is only for verified packages now.
* Removed progress dialogs and reduced the number of blocking API calls required over lifetime.
* Fix stacking calls to Client.List that was causing incorrect error reporting about API timeout.
* Auto create settings for plug in packages if missing.

## [3.2.12] - 2020-06-05
* Fix testing definitions to allows us to remove tests as separate packages.
  
## [3.2.11] - 2020-06-04
* Documentation updates for clarity and correctness.
* Block use of deprecated APIs on 2020.2 and later.
  
## [3.2.11-preview.1] - 2020-05-15
* Fix FB 1242581 : Fix a number of issue around cache rebuilding and persistent UI display of cache rebuilding even though nothing was happening.
* Fix FB 1245181 : Fix nullderef access of settings manager instance.

## [3.2.10] - 2020-04-24
* Release 3.2.10

## [3.2.10-preview.1] - 2020-04-20
* Fix Unity Frame Debugger by not stopping loaders on Pause / Stop.
* Don't stop loaders when XRGeneralSettings is disabled.
* Fix UI issue where third party providers would sometimes disappear from the provider selection UI.
 

## [3.2.9] - 2020-04-18
* Fix linux tests in CI.

## [3.2.8] - 2020-04-17
* Fix double click to untoggle Magic Leap bug.
* Fix name of Magic Leap Zero Iteration entry for standalone.

## [3.2.7] - 2020-04-14
* Fix issue where the wrong time value was being used to test for a timeout.

## [3.2.6] - 2020-04-07
* Add log message to clarify the error when dealing with the un-bundled AR packages.

## [3.2.5] - 2020-04-07
* Fixes LIH version
* Fixes linked LIH documentation version

## [3.2.4] - 2020-04-02
* Add documentation related to re-ordering loaders.

## [3.2.3] - 2020-03-27
* Pick up updated Legacy Input Handlers.

## [3.2.2] - 2020-03-26
* Re-enable Mac automation testing
* Add force removal of legacy LIH package to deal with package management resolution issue.
  
## [3.2.1] - 2020-03-23
* CI issues causing an update to LIH, which mean we need to spin an update for that as well.

## [3.2.0] - 2020-03-13
* Release of new management workflow.
  
## [3.2.0-preview.9] - 2020-03-11
* Disable legacy vr if we install XR Management. This doesn't lock the UI out but it at least keeps it from being activated at the same time as XR.
* Stop nagging users to uninstall if not a plugin is unassigned. Instead just add text to the above the fold copy to point out users need to use Pack Man UI instead.


## [3.2.0-preview.8] - 2020-03-09
* Cleanup of the uninstaller code. Will eventually do the same thing for the package metadata code as well.

## [3.2.0-preview.7] - 2020-03-06
* Add utility for requesting uninstall of the currently installed built in VR pacakges.
* Add callbacks to XRLoaderHelper to allow clients to handle assignment/unassigment of their loader to a build target in the Editor.

## [3.2.0-preview.6] - 2020-03-04
* Make documentation link color differentiate between Personal and Professional editor themes.
* More UI tweaks
  
## [3.2.0-preview.5] - 2020-03-01
* Fix state management for queue processing.
* Add ability for users to remove plug-ins that are no longer actively referenced.
* Add documentation link to management pane in Player Settings.

## [3.2.0-preview.4] - 2020-02-26
* Further Documentation review changes.
* Fix bug allowing user to enable/disable loaders at play time.
* Fix bug with compilation errors causing proggress bar to hang around.

## [3.2.0-preview.3] - 2020-02-25
* Modify metadata store to hold packages and not just metadatas. This allows us to call the settings instance initializer function even after initialization.
* Fixed some minor known package naming issues.
  
## [3.2.0-preview.2] - 2020-02-24
* Add in Mock HMD pacakge to known packages.
* Fix up some asynchronous issues with checking for installable packages.
* Change UI a little to try to differentiate installable from installed packages.
* Correct documentation after review.
* Modify sorted list to make MOck HMD last item always.
* Modify assignment to make sure that the order of the items is always the same order as in the UI.

## [3.2.0-preview.1] - 2020-02-20
* Entire re-write of the UI and backing data store to provide a better user experience more inline with the previous Built In XR Settings UI.
* Removed the Legacy Input Helpers sub pane and replaced with a more integrated set of menu uptions in the Assets menu (provided by the Legacy Input Helpers package).
  
## [3.1.0] - 2020-02-07
* Preparation for verification release.
  
## [3.1.0-preview.2] - 2019-12-17
* Fix FB 1206103: Serialized loader list is not saved correctly when a new loader asset is created as part of the add operation.
* Fix package checking system to also look at installed packages and not just remotely registered packages. This allows us to see packages the user has locally installed on disk that may not be registered in the package registry.
* Add check to make sure we pick up any class in the project that derives from XRLoader and not just those in packages. This allows a developer to create a loader in their assets folder and use that regardless of installed packages.

## [3.1.0-preview.1] - 2019-12-06
* Fixes an issue where subsystems could not be initialized before awake in the editor
* Fixes an issue where subsystems were not re-started after a pause in the editor
* Re-add build target filtering into management.
* Documentation copy review and edit.
 
## [3.1.0-preview] - 2019-09-11
* Adds generic gfx capabilities method to XRLoader class
 
## [3.0.5] - 2019-12-06
* Fix package validation errors.
* Release for verification.

## [3.0.5-preview.4] - 2019-12-06
* Release for verification.
* Remove build target filtering support. Will be added back into 3.1.0.

## [3.0.5-preview.3] - 2019-11-22
* Correct Samples code to make sure that it compiles correctly.
* Make some documentation fixes for inline code.
* Replace XR SDK text with just XR or other appropriate messaging.

## [3.0.5-preview.2] - 2019-11-18
* New attribute was incorrectly placed into Runtime instead of Editor. Moved to Editor where it belongs.

## [3.0.5-preview.1] - 2019-11-13
* UI rework to provide for simpler installation and management of XR Plug-in Providers.
* Reworked the underlying data handling and maintenance to be more streamlines and less coupled.

## [3.0.4] - 2019-11-04
* Release package for verification.

## [3.0.4-preview.3] - 2019-10-29
* Update minimum compatible Editor version to 2019.3.0b9
* Fixes an issue where subsystems could not be initialized before awake in the editor
* Fixes an issue where subsystems were not re-started after a pause in the editor
* Removes dialog boxes for creating Loaders and initializing settings.

## [3.0.4-preview.2] - 2019-10-23
* Modifies wording of LIH inclusion page from "required" to recommended.

## [3.0.4-preview.1] - 2019-09-20
* Public API InitializeLoaderAsync was erroneously made internal. Move back to being publicly accessible.
* Fix editor application perf issue due to not unhooking update callback.

## [3.0.3] - 2019-08-29
* Fix package dependency version for subsystem registrion pacakge.

## [3.0.2] - 2019-08-29
* Release package for verification.

## [3.0.2-preview.3] - 2019-08-29
* Update to reflect changes in downstream subsystem definitions in 19.3+.

## [3.0.2-preview.2] - 2019-08-23
* Change legacy input helpers version to 1.*
* Fix documentation validation errors.
* Allow 3.x to work with Unity 2019.2.
* This package will not work with 2019.3a1 - a11.

## [3.0.2-preview.1] - 2019-08-06
* Remove asset menu creation entry for XR Settings as it is unsupported now.
* Fix an issue with downloading packages that could allow PackMan toget corrupted, forcing the user to reload Unity.

## [3.0.1] - 2019-07-11
* Update base Unity release version after namespace changes.

## [3.0.0] - 2019-07-09
* Update docs to add more information around correct usage.
* add useful names to sub objects of general settings.
* Add Magic Leap to curated packages list.

## [2.99.0-preview.2] - 2019-06-19
* Pick up 2019.2 preview changes that are applicable to 2019.3.
* Fix up the code after Experimental namespace change.

## [2.99.0-preview.1] - 2019-06-14
* Update package to support 2019.3+ only.

## [2.99.0-preview] - 2019-06-14
* Update package to support 2019.3+ only.
* Rev version to almost 3. This is to make space for 2019.2 preview versions and in acknowledgement of the breaking changes that will happen soon.

## [2.0.0-preview.24] - 2019-06-14
* Tie version to 2019.2 exclusively for preview.
* Strip document revision history.
* Remove third party notice as unneeded.

## [2.0.0-preview.23] - 2019-06-10
* Add promotion pipeline yaml file to get promotion to production working again.

## [2.0.0-preview.22] - 2019-06-11
* Revert Legacy Input Helpers dependency to newly pushed 1.3.2 production version.

## [2.0.0-preview.21] - 2019-06-10
* Downgrade Legacy Input Helpers dependency to correct production version.

## [2.0.0-preview.20] - 2019-06-10
* Downgrade Legacy Input Helpers dependency to help get package to production.

## [2.0.0-preview.19] - 2019-06-04
* Fix package name and description.

## [2.0.0-preview.18] - 2019-06-03
* Minor corrections in samples header file.
* Remove Windows from log message.
* Remove tutorial UI and unsupported data.

## [2.0.0-preview.17] - 2019-05-28
* Fix issue where no settings object would cause an error to be logged at build time incorrectly.
* Add helper method to get XRGeneralSettings instance for a specific build target.

## [2.0.0-preview.16] - 2019-05-28
* Move PR template to correct location.

## [2.0.0-preview.15] - 2019-05-23
* Fix the readme help page to only appear once on initial add of package.
* Fix up test namespaces to use correct namespace naming

## [2.0.0-preview.14] - 2019-05-23
* updating number for yamato, adds depednency to com.unity.xr.legacyinputhelpers

## [2.0.0-preview.13] - 2019-05-09
* Fix more output logging for Yamato.

## [2.0.0-preview.12] - 2019-05-09
* Add support for Yamato
* Fix unit tests broken with streamlined workflow changes.

## [2.0.0-preview.10] - 2019-04-19
* Add ability for users to disable auto initialize at start. This should allow for hybrid applications that want to start in non-XR mode and manually switch.
* Fix play mode initialization so that we can guarantee that XR has been initialized (or at least attempted initialization) by the time the Start method is called on MonoBehaviours.
* Documentation updated to cover the above.
* Fixed a bug in the new Readme script code that caused a crash in headless mode. Seems the code was launching an Editor window and causing UIElements to crash on an attempt to repaint. We have a workaround to make sure we don't load the window if in headless mode and a bug is filed with the responsible team to correct the crash.

## [2.0.0-preview.9] - 2019-04-10
* Fix package validation console errors.

## [2.0.0-preview.8] - 2019-04-10
* Fix package validation compilation errors.
* Remove .github folder from npm packaging.

## [2.0.0-preview.7] - 2019-04-10
* Streamlining of the management system. Move XR Manager to a singleton instance on XRGeneralSettings that is populated by an XRManagerSettings instance that the user can switch in and out. __NOTE: This removes the ability to use XRManagement for per scene situations. For hybrid or manual scenes the user will be responsible for instantiating/loading the XRManagerSettings instance they want and dealing with lifecycle themselves.__

## [2.0.0-preview.6] - 2019-03-29
* Fix up package repo information for rel mgmt.

## [2.0.0-preview.5] - 2019-02-05
* Split documentation into separate audience files for End Users and Providers.
* Update package target Unity version to Unity 2019.1.

## [2.0.0-preview.4] - 2019-02-05
* Fix an issue with with an NRE in the build processor.

## [2.0.0-preview.3] - 2019-01-22
* Add missing repo url to package json file
* Fix NRE issue in build processor

## [2.0.0-preview.3] - 2019-01-22
* Fix error in general build processor due to a potential null deref.
* Fix missing check for unity version when referencing UIElements.

## [2.0.0-preview.2] - 2018-12-19
* Fix package validation issues.
* Fix bug due to preinit code that would cause a null ref exception.

## [2.0.0-preview.1] - 2018-12-19
* Upated to support loading integrated and standalone subsystems.
* Add support for pre-init framework to allow for setting handling things like LUID setup pre-gfx setup.
* Add ability for general settings to be set per platform and not just globally.
* Tagged with release preview build. This should be the base on which we move to release for 2019.1

## [0.2.0-preview.9] - 2018-11-27
* Fixed some issues with boot time and general setting.

## [0.2.0-preview.8] - 2018-10-29
* Fix an API breaking change to UnifiedSettings api
* Fix a NRE in XRGeneralSettings if the user has set an XRManager Component on a scene game object and didn't setup general settings.

## [0.2.0-preview.7] - 2018-10-29
* Hopefully all CI issues are resolved now.
  
## [0.2.0-preview.4] - 2018-10-24
* Merged in gneral settings support. Initial implentation allows for ability to assign an XR Manager instance for loading XR SDK at boot launch time.

## [0.2.0-preview.3] - 2018-10-24
* Merged in Unified Settings dependent changes.

## [0.1.0-preview.9] - 2018-07-30
* Add missing .npmignore file

## [0.1.0-preview.8] - 2018-07-30

* Updated UI for XR Manager to allow for adding, removing and reordering loaders. No more need for CreateAssetMenu attributes on loaders.
* Updated code to match formatting and code standards.

## [0.1.0-preview.7] - 2018-07-25

* Fix issue #3: Add ASMDEFs for sample code to get it to compile. No longer need to keep copy in project.
* Fix Issue #4: Update documentation to reflect API changes and to expand information and API documentation.
* Fix Issue #5: Move boilerplate loader code to a common helper base class that can be used if an implementor wants to.

## [0.1.0-preview.6] - 2018-07-17

### Added runtime tests for XRManager

### Updated code to reflect name changes for XR Subsystem types.

## [0.1.0-preview.5] - 2018-07-17

### Simplified settings for build/runtime

Since we are 2018.3 and later only we can take advantage of the new PlayerSettings Preloaded Assets API. This API allows us to stash assets in PLayerSettings that are preloaded at runtime. Now, instead of figuring out where to write a file for which build target we just use the standard Unity engine and code access to get the settings we need when we need them.

## [0.1.0-preview.4] - 2018-07-17

### Added samples and abiity to load settings

This change adds a full fledged sample base that shows how to work with XR Management from start to finish, across run and build. This includes serializing and de-serializing the settings.

## [0.1.0-preview.3] - 2018-07-17


## [0.1.0-preview.2] - 2018-06-22

### Update build settings management

Changed XRBuildData froma class to an attribute. This allows providers to use simpler SO classes for build data and not forece them to subclass anything.
Added a SettingsProvider subclass that wraps each of these attribute tagged classes. We use the display name from the attribute to populate the path in Unified Settings. The key in the attribute is used to store a single instance of the build settings SO in EditorBuildSettings as a single point to manage the instance.
Added code to auto create the first SO settings instance using a file panel since the Editor build settings container requires stored instances be backed in the Asset DB. There is no UI for creating the settings (unless added by the Provider) so this should allow us to maintain the singleton settings. Even if a user duplicates the settings instance, since it won't be in the Editor build settings container we won't honor it.

## [0.1.0-preview.1] - 2018-06-21

### This is the first release of *Unity Package XR Management*.
