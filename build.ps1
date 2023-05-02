$version = "2.1"

$currentDir = (Get-Item $MyInvocation.MyCommand.Path).Directory.FullName
. dotnet.exe restore $currentDir
. dotnet.exe publish -c Release -r win10-x64 $currentDir/SubLink/SubLink.csproj /p:Version=$version /p:SkipInvalidConfigurations=true /p:PublishSingleFile=true /p:PublishReadyToRun=true /p:IncludeNativeLibrariesForSelfExtract=true /p:EnableCompressionInSingleFile=true --self-contained true

New-Item build-$version -ItemType directory
Copy-Item -Path "SubLink\bin\Release\net7.0\win10-x64\publish\SubLink.exe" -Destination build-$version
Copy-Item -Path "SubLink\SubLink.cs" -Destination build-$version

if (-not (Test-Path -Path "builds")) {
    New-Item -Path "builds" -ItemType Directory
}

if (Test-Path builds\SubLink-$version.zip) {
    Remove-Item builds\SubLink-$version.zip
}
Compress-Archive -Path "build-$version\*" -destinationpath "builds\SubLink-$version.zip" -compressionlevel optimal
Remove-Item build-$version -Recurse