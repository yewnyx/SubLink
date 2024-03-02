$version = "2.2.0"

$currentDir = (Get-Item $MyInvocation.MyCommand.Path).Directory.FullName
. dotnet.exe restore $currentDir
. dotnet.exe publish -c Release -r win10-x64 $currentDir/SubLink/SubLink.csproj /p:Version=$version /p:SkipInvalidConfigurations=true /p:PublishSingleFile=true /p:PublishReadyToRun=true /p:IncludeNativeLibrariesForSelfExtract=true /p:EnableCompressionInSingleFile=true --self-contained true
. dotnet.exe publish -c Release -r win10-x64 $currentDir/SubLinkKick/SubLinkKick.csproj /p:Version=$version /p:SkipInvalidConfigurations=true /p:PublishSingleFile=true /p:PublishReadyToRun=true /p:IncludeNativeLibrariesForSelfExtract=true /p:EnableCompressionInSingleFile=true --self-contained true
. dotnet.exe publish -c Release -r win10-x64 $currentDir/SubLinkStreampad/SubLinkStreamPad.csproj /p:Version=$version /p:SkipInvalidConfigurations=true /p:PublishSingleFile=true /p:PublishReadyToRun=true /p:IncludeNativeLibrariesForSelfExtract=true /p:EnableCompressionInSingleFile=true --self-contained true
. dotnet.exe publish -c Release -r win10-x64 $currentDir/SubLinkStreamElements/SubLinkStreamElements.csproj /p:Version=$version /p:SkipInvalidConfigurations=true /p:PublishSingleFile=true /p:PublishReadyToRun=true /p:IncludeNativeLibrariesForSelfExtract=true /p:EnableCompressionInSingleFile=true --self-contained true
. dotnet.exe publish -c Release -r win10-x64 $currentDir/SubLinkFansly/SubLinkFansly.csproj /p:Version=$version /p:SkipInvalidConfigurations=true /p:PublishSingleFile=true /p:PublishReadyToRun=true /p:IncludeNativeLibrariesForSelfExtract=true /p:EnableCompressionInSingleFile=true --self-contained true

New-Item build-$version -ItemType directory
Copy-Item -Path "SubLink\bin\Release\net7.0\win10-x64\publish\SubLink.exe" -Destination build-$version
Copy-Item -Path "SubLinkKick\bin\Release\net7.0\win10-x64\publish\SubLinkKick.exe" -Destination build-$version
Copy-Item -Path "SubLinkStreampad\bin\Release\net7.0\win10-x64\publish\SubLinkStreamPad.exe" -Destination build-$version
Copy-Item -Path "SubLinkStreamElements\bin\Release\net7.0\win10-x64\publish\SubLinkStreamElements.exe" -Destination build-$version
Copy-Item -Path "SubLinkFansly\bin\Release\net7.0\win10-x64\publish\SubLinkFansly.exe" -Destination build-$version
Copy-Item -Path "SubLinkCommon\SubLink.cs" -Destination build-$version

if (-not (Test-Path -Path "builds")) {
    New-Item -Path "builds" -ItemType Directory
}

if (Test-Path builds\SubLink-$version.zip) {
    Remove-Item builds\SubLink-$version.zip
}
Compress-Archive -Path "build-$version\*" -destinationpath "builds\SubLink-$version.zip" -compressionlevel optimal
Remove-Item build-$version -Recurse