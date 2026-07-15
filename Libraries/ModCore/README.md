# 7D2D Mod Core
A foundational library for [7 Days to Die](https://store.steampowered.com/app/251570/7_Days_to_Die/) mod development.

ModCore provides a standardized base structure and common utilities for building 7D2D mods, eliminating boilerplate and preventing mod conflicts through automatic assembly merging.

## Quick Start

1. Install the NuGet package:

```
dotnet add package DashTheDev.7D2D.ModCore
```

2. Create your config class:

```csharp
public class MyConfig : XmlModConfig
{
    public string MyProperty { get; set; } = "default";
}
```

3. Create your mod class:

```csharp
public class MyMod : BaseMod<MyMod, MyConfig>, IModApi
{
    // Your mod implementation
}
```

4. Build and be done. Your mod is ready to deploy.

## What's Included
**BaseMod<T, TConfig>**: Base class with overrides handling mod lifecycle, config management, and Harmony patching setup.

**IModConfig/BaseModConfig**: Mod config interface or base class so that you can easily extend and create your own class that handles config (i.e JSON, etc).

**XmlModConfig**: Automatic XML serialization/deserialization for mod configuration with support for per-property comments.

**Dependency Injection Container**: Singleton and transient service registration accessible via `YourMod.Instance.Container`.

**ModLogger**: Structured logging with mod-prefixed output for debugging.

**GeneralUtility**: Common helper methods for logging, reflection, and mod utilities.

## Build Process
ModCore automatically merges into your mod DLL using ILRepack, eliminating versioning conflicts between mods. Each mod gets its own isolated copy of ModCore, so multiple mods can use different versions without clash.

## Development Setup

1. Ensure you have [Visual Studio](https://visualstudio.microsoft.com/) and [7D2D](https://store.steampowered.com/app/251570/7_Days_to_Die/) installed
2. Clone the repository
3. Create a `Local.props` file in the root:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <PropertyGroup>
        <GamePath>YOUR_GAME_PATH_HERE</GamePath>
    </PropertyGroup>
</Project>
```

4. Build the solution