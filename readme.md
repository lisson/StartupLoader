# Startup Loader

The goal of this project is to create a front end for OEM Windows vendors to run scripts and applications after the initial Windows setup.

## Running the tests

NUnit tests are provided for the registry manager.

## Deployment

Build the project under Visual Studio 2017 to generate the binary and configuration files.

### NLog.config

Specify where the log file path.

### StartupLoader.exe.config

Add AppSetting entries and each program will be loaded sequentially. Note that you can't identical AppSetting entries. Please use a .bat or .ps1 wrapper if you need duplicate entries.

The *restart* flag tells the loader to exit after the program is finished and adds a RunOnce entry to continue on reboot. I've left the restart function to the discretion of the program being ran.

## Authors

* **Andy Li** - *Initial work* - [lisson](https://github.com/lisson)

## License

This project is licensed under the MIT License

2018
