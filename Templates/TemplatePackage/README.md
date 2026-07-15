# DashTheDev.7D2D.Templates
A collection of project templates for **7 Days to Die** modding.

## Templates Included
| Template | Short Name | Description |
|---|---|---|
| 7D2D Mod | `7D2DMod` | A ready-to-go mod project targeting .NET Framework 4.8 |
| 7D2D Mod Library | `7D2DModLib` | A class library project for shared mod utilities targeting .NET Framework 4.8 |

## Requirements
- .NET SDK (any modern version)

## Installation
Install the template package via the .NET CLI:

```bash
dotnet new install DashTheDev.7D2D.Templates
```

To verify the templates are installed:

```bash
dotnet new list 7D2D
```

## Usage

### Create a new Mod project
```bash
dotnet new 7D2DMod -n MyMod
```

### Create a new Mod Library project
```bash
dotnet new 7D2DModLib -n MyModUtils
```

Both commands will scaffold a project with your chosen name, with namespaces, assembly names, and file names all set up correctly.

All you will need to do is to create and update the `local.props` file to point to your 7D2D installation. For more information read the `README.md` file that is generated.

## Updating
To update to the latest version of the templates:

```bash
dotnet new update
```

## Uninstalling
```bash
dotnet new uninstall DashTheDev.7D2D.Templates
```