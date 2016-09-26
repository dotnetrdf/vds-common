#tool "nuget:?package=NUnit.Runners&version=2.6.4"
#addin "nuget:?package=NuGet.Core&version=2.12.0"
#addin "Cake.ExtendedNuGet"

var target = Argument("target", "Default");
var version = "1.7.0";
string preRelease = null;
var nugetVersion = version + (preRelease == null ? "" : "-" + preRelease);
var distDir = "./dist/" + nugetVersion;

Task("Compile")
	.Does(() =>
{
	DotNetBuild("VDS.Common.sln", settings => 
		settings.SetConfiguration("Release")
			.WithTarget("Build")
			.WithProperty("TreatWarningsAsErrors", "true"));
});

Task("Test")
	.IsDependentOn("Compile")
	.Does(() => 
{
	NUnit("./test/bin/release/VDS.Common.Test.dll");
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

Task("BinariesZip")
	.IsDependentOn("CopyBinaries")
	.Does(() =>
{
	var files = GetFiles(distDir+"/lib/**/*.*");
	files.Add(GetFiles(distDir + "/doc/**/*.*"));
	Zip(distDir, "VDS.Common." + nugetVersion + "-bin.zip", files);
});

Task("Dist")
	.IsDependentOn("DistDir")
	.IsDependentOn("NuGet")
	.IsDependentOn("BinariesZip");

Task("Default")
  .IsDependentOn("Dist")
  .Does(() =>
{
  Information("Hello World!");
});

RunTarget(target);