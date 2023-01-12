<div align="center">

# Version Bumper
Add automatic version bumping to your app with ease

[![wakatime](https://wakatime.com/badge/user/0ccf7ed5-30a2-486d-8ea4-6b0ca58cd9c9/project/408ed374-3631-4e04-8874-0309b7bd5921.svg)](https://wakatime.com/badge/user/0ccf7ed5-30a2-486d-8ea4-6b0ca58cd9c9/project/408ed374-3631-4e04-8874-0309b7bd5921)
![Lifecycle](https://img.shields.io/badge/lifecycle-maintaining-lime)
[![CodeQL](https://github.com/verlox/VersionBumper/actions/workflows/codeql.yml/badge.svg)](https://github.com/verlox/VersionBumper/actions/workflows/codeql.yml)
  
</div>

# Features
* CLI interface
* Easy to use and setup
* Automatically checks and makes sure changes were made

# Installation
1. Download `VersionBumper.exe` from [releases](https://github.com/verlox/VersionBumper/releases/)
2. Drop `VersionBumper.exe` into your projects solution directory (with the `.sln` file)
3. Open your project in VS and go into **Properties -> Build Events -> Post-build event command line**
4. Insert the following: `$(SolutionDir)VersionBumper.exe $(SolutionDir)$(ProjectName)\Properties\AssemblyInfo.cs --key <key>`
5. Replace `<key>` with a name, this is how it will be identified
6. Build your project and check `AssemblyInfo.cs`

# Pull Requests
All pull requests will be reviewed before merging, but are completely welcome
