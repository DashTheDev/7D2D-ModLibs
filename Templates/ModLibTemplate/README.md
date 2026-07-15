# ModLibTemplate
A [7 Days to Die](https://store.steampowered.com/app/251570/7_Days_to_Die/) mod library.

## Development Setup
1. Ensure you have [Visual Studio](https://visualstudio.microsoft.com/) and [7D2D](https://store.steampowered.com/app/251570/7_Days_to_Die/) installed
2. Clone the repository
3. Create a `Local.props` file in the root of the project with the following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <PropertyGroup>
        <!-- Insert game path here (i.e Z:\SteamLibrary\steamapps\common\7 Days To Die) -->
        <GamePath>YOUR_GAME_PATH_GOES_HERE</GamePath>
    </PropertyGroup>
</Project>
```

4. Open the solution in [Visual Studio](https://visualstudio.microsoft.com/) and build.