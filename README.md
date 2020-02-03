# UnityPluginXcodeTemplate
![Platform](https://img.shields.io/badge/platform-%20iOS%20%7C%20macOS%20-lightgrey.svg)
![Unity](https://img.shields.io/badge/unity-2018-green.svg)
![Xode](https://img.shields.io/badge/xcode-xcode11-green.svg)

* iOS & macOS Unity Native Plugin Xcode Template
* Generate Xcode projects from setting files by [xcodegen](https://github.com/yonaskolb/XcodeGen)

## Installed Repositories
* https://github.com/fuziki/KeyboarInputDetector

## Requirements
* [xcode11+](https://apps.apple.com/jp/app/xcode/id497799835?mt=12)
* [Unity 2018+](https://unity.com/)
* [xcodegen](https://github.com/yonaskolb/XcodeGen)
* [rust](https://www.rust-lang.org/)  (if change script)

## setting.yml

``` setting.yml
PROJECT_NAME: XyzProject #Project Name
FRAMEWORK_IOS: XyzProject_iOS #Framework(for iOS)
FRAMEWORK_MACOS: XyzProject_macOS #Framework(for macOS)
BUNDLE_MACOS: XyzProject_bundle #Bundle Name(for macOS)
EXAMPLE_IOS: Example_iOS #iOS Example
EXAMPLE_MACOS: Example_macOS #macOS Example
```

* Update project with setting file

``` Makefile
make setup
```

## Xcode/

* open xcodeproj

```
open PROJECT_NAME.xcodeproj
```

## Unity/
* LibraryUser
  * test created native plugin

* make unitypackage
```
make pkg
```

## Dependency
* [yaml-rust](https://crates.io/crates/yaml-rust)
* [linked-hash-map](https://github.com/contain-rs/linked-hash-map)
* [rand](https://github.com/rust-random/rand)
