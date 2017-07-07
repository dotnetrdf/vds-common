
# Introduction

VDS.Common is a lightweight, dependency-free library of useful advanced data structures such as Trees, Tries and indexing tailored dictionaries.

It is based on code originally from in the [dotNetRDF Project][1] (see past code history [here][2] and [here][3]) but was split off into a separate library once it became sufficiently generic to be more broadly useful.

VDS.Common is built for a variety of .Net versions and profiles, currently we have builds for the following:

- .Net 3.5 Full/Client Profile
- .Net 4.0 Full/Client Profile
- Portable Class Library Profile 328
- .NET Core netstandard1.4
- .NET Core netstandard1.0

# License

VDS.Common is licensed under the MIT License

# Usage and Documentation

Releases can be found [here on GitHub](https://github.com/dotnetrdf/vds-common/releases). From 1.7.0 the GitHub release includes a ZIP file containing the compiled binaries and a CHM of the API documentation.

To use VDS.Common simply add a reference to the DLL for the appropriate .Net version to your project.  If you are using NuGet simply search for VDS.Common and install it that way.

[![NuGet](https://img.shields.io/nuget/v/VDS.Common.svg?maxAge=2592000)](https://www.nuget.org/packages/VDS.Common)

For documentation on the features this library provides please see the [Wiki][4].

# Build Status

[Master Branch](https://github.com/dotnetrdf/vds-common/tree/master): [![Master Branch](https://ci.appveyor.com/api/projects/status/3bru28e2e3j18hm9/branch/master)](https://ci.appveyor.com/project/kal/vds-common/branch/master)

# Acknowledgements

VDS.Common is developed primarily Rob Vesse with some contributions from Kal Ahmed and Mike Davies
Other contributions were aslo received from the following GitHub users:

- @amardeepsingh

[1]: http://dotnetrdf.github.io/
[2]: https://bitbucket.org/dotnetrdf/dotnetrdf/src/4365cd7d087158b72c2e4053879bede2e194cdec/Libraries/core/net40/Common?at=default
[3]: https://bitbucket.org/dotnetrdf/dotnetrdf/src/3378cdd89cc59dedb294657085da648946d76bb4/Libraries/core/Common?at=default
[4]: https://github.com/dotnetrdf/vds-common/wiki/Home