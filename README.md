# vertical-csdependencies

Finds all C# projects in a directory, and displays them in dependency order.

## Installation

```
dotnet tool install vertical-dependencies
```

## Quick Start

Given you are positioned in the root of a multi-project directory, and your sources are in a folder called 'src', the following example lists the full paths to each project in *dependency* order.

```
$ csdeps ./src
```

## Use Case

This tool was designed to aggregate projects that have relationships with each other via package references (not P2P references), and display them in the order in which they should be built, given one is a dependency of another.

For example, say we have the following project structure:

/src/vertical.core/vertical.core.csproj:

```xml
<Project Sdk="Microsoft.Sdk">
    <PropertyGroup>
        <IsPackable>true</IsPackable>
        <PackageId>vertical.core</PackageId>
    </PropertyGroup>
</Project>
```

/src/vertical.logging.abstractions/vertical.logging.abstractions.csproj:

```xml
<Project Sdk="Microsoft.Sdk">
    <PropertyGroup>
        <IsPackable>true</IsPackable>
        <PackageId>vertical.logging.abstractions</PackageId>
    </PropertyGroup>
</Project>
```

/src/vertical.diagostics.abstractions/vertical.diagnostics.abstractions.csproj:

```xml
<Project Sdk="Microsoft.Sdk">
    <PropertyGroup>
        <IsPackable>true</IsPackable>
        <PackageId>vertical.diagnostics.abstractions</PackageId>
    </PropertyGroup>
</Project>
```

/src/vertical.diagostics/vertical.diagnostics.csproj:

```xml
<Project Sdk="Microsoft.Sdk">
    <PropertyGroup>
        <IsPackable>true</IsPackable>
        <PackageId>vertical.diagnostics</PackageId>
    </PropertyGroup>
    <ItemGroup>
        <PackageReference Include="vertical.diagnsotics.abstractions" />
        <PackageReference Include="vertical.logging.abstractions" />
    </ItemGroup>
</Project>
```

If we ran the utility and pointed it at `/src`, we would see the following output:

```
$ csdeps ./src
/src/vertical.core/vertical.core.csproj
/src/vertical.logging.abstractions/vertical.logging.abstractions.csproj
/src/vertical.diagostics.abstractions/vertical.diagnostics.abstractions.csproj
/src/vertical.diagostics/vertical.diagnostics.csproj
```

We could take this output and feed it to a build script...

```PS
foreach ($project in (& csdeps .\src)) {
    dotnet pack $project -c Release
}
```

## Particulars

### How dependency names are resolved

The utility resolves package identifiers using the following order of precedence:

    1. <PackageId> element text within the <PropertyGroup> element
    2. <AssemblyName> element text within the <PropertyGroup> element
    3. Project name - which is the name of project file excluding the extension

### Command line usage

```
$ csdeps <path [,...]> [OPTIONS]
```

#### Arguments

path - One or more source directories to look for project files. Paths are rooted to the current working directory

#### Options

--debug - Display diagnostic information

-h, --help - Displays the help file

-i, --include &lt;pattern&gt; - A glob pattern that matches the files to include. Defaults to **/*.csproj is not specified.

-o, --output &lt;option&gt; - Specifies which nomenclature of the project should be used in the output. Choices are: assemblyName, directory, fileName, projectFile, projectName

--tree - Display the output as a tree

-x, --exclude &lt;pattern&gt; - One or more glob patterns used to match files that should be excluded from aggregation.

## Contributing
Yes! Help from the community is highly appreciated. Please [create an issue](https://github.com/verticalsoftware/vertical-csdependencies/issues/new) though so we can discuss the bug or feature.