#tool "nuget:?package=NUnit.Runners&version=2.6.4"
#tool "nuget:?package=EWSoftware.SHFB"
#addin "nuget:?package=NuGet.Core"

var target = Argument("target", "Default");
var version = "1.9.0";
string preRelease = "alpha";
var nugetVersion = version + (preRelease == null ? "" : "-" + preRelease);
var distDir = "./dist/" + nugetVersion;

Task("NuGetRestore")
	.Does(() =>
{
	NuGetRestore("VDS.Common.sln");
});

Task("Compile")
	.IsDependentOn("NuGetRestore")
	.Does(() =>
{
	DotNetBuild("VDS.Common.sln", settings => 
		settings.SetConfiguration("Release")
			.WithTarget("Build")
			.WithProperty("TreatWarningsAsErrors", "true"));
    var coreBuildSettings = new DotNetCoreBuildSettings {
        Configuration = "Release"
    };
    DotNetCoreRestore("./src/netcore/netcore.csproj");
    DotNetCoreBuild("./src/netcore/netcore.csproj", coreBuildSettings);
});

Task("Test")
	.IsDependentOn("Compile")
	.Does(() => 
{
	var settings = new NUnitSettings { Exclude="Timing" };
	NUnit("./test/bin/release/VDS.Common.Test.dll", settings);
});

Task("DistDir")
	.Does(() => {
	if (!DirectoryExists(distDir)) {
		CreateDirectory(distDir);
    }
});

Task("CleanNuGetLib")
	.Does(() => 
{
	if (DirectoryExists("./Build/NuGet/lib")) {
		DeleteDirectory("./Build/NuGet/lib", true);
	}
});

Task("CopyNuGetLib")
	.IsDependentOn("CleanNuGetLib")
	.IsDependentOn("Compile")
	.Does(() => 
{
	EnsureDirectoryExists("./Build/NuGet/lib/net35-client");
	CopyFiles("./src/net35-client/bin/Release/VDS.Common.*", "./Build/NuGet/lib/net35-client");
	EnsureDirectoryExists("./Build/NuGet/lib/net40-client");
    CopyFiles("./src/net40-client/bin/Release/VDS.Common.*", "./Build/NuGet/lib/net40-client");
	EnsureDirectoryExists("./Build/NuGet/lib/portable-net4+sl5+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1");
    CopyFiles("./src/portable/bin/Release/VDS.Common.*", "./Build/NuGet/lib/portable-net4+sl5+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1");
	EnsureDirectoryExists("./Build/NuGet/lib/netstandard1.4");
    CopyFiles("./src/netcore/bin/Release/netstandard1.4/VDS.Common.*", "./Build/NuGet/lib/netstandard1.4");
	EnsureDirectoryExists("./Build/NuGet/lib/netstandard1.0");
    CopyFiles("./src/netcore/bin/Release/netstandard1.0/VDS.Common.*", "./Build/NuGet/lib/netstandard1.0");
	EnsureDirectoryExists("./Build/NuGet/lib/netstandard2.0");
    CopyFiles("./src/netcore/bin/Release/netstandard2.0/VDS.Common.*", "./Build/NuGet/lib/netstandard2.0");
});

Task("NuGet")
	.IsDependentOn("CopyNuGetLib")
	.IsDependentOn("DistDir")
	.Does(() =>
{
    var packSettings = new NuGetPackSettings {
		Version = nugetVersion,
		OutputDirectory = "./dist/" + nugetVersion
	};
	NuGetPack("./Build/NuGet/VDS.Common.nuspec", packSettings);
});

Task("CopyBinaries")
	.IsDependentOn("DistDir")
	.IsDependentOn("CopyNuGetLib")
	.Does(() => {
	EnsureDirectoryExists(distDir + "/lib");
	CleanDirectory(distDir + "/lib");
	CopyDirectory("./Build/NuGet/lib", distDir + "/lib");
});

Task("Doc")
	.IsDependentOn("Compile")
	.Does(() =>
{
	MSBuild("./doc/vds-common.shfbproj");
});

Task("CopyCHM")
	.IsDependentOn("Doc")
	.IsDependentOn("DistDir")
	.Does(() =>
{
	EnsureDirectoryExists(distDir + "/doc");
	CopyFile("./doc/Help/VDS.Common.API.chm", distDir + "/doc/VDS.Common.API.chm");
});

Task("DistZip")
	.IsDependentOn("CopyBinaries")
	.IsDependentOn("CopyCHM")
	.Does(() =>
{
	var files = GetFiles(distDir+"/lib/**/*.*");
	files.Add(GetFiles(distDir + "/doc/**/*.*"));
	Zip(distDir, distDir + "/VDS.Common." + nugetVersion + "-bin.zip", files);
});

Task("Dist")
	.IsDependentOn("Test")
	.IsDependentOn("DistDir")
	.IsDependentOn("NuGet")
	.IsDependentOn("DistZip");

Task("Default")
  .IsDependentOn("Dist")
  .Does(() =>
{
});

RunTarget(target);