# CommandLine #

A little library for conquering command lines

## Purpose ##

The CommandLine library gives C# application developers an easy way to feed command line parameters into their applications. As you may know, C# command line arguments are fed into applications by way of a string array. The contents of that string array are left to you to interpret as you see fit. 

That said, command lines tend to follow a loosely defined standard, and implementing that standard can be time consuming. CommandLine makes it easy for your app to present a consistent, extensible command line API to the world for minimal investment.

## Features ##

Features of CommandLine include:

* Binds command line arguments to C# class properties
* Offers lots of different argument-parsing variations
* Creates usage and error messaging automatically
* Supports required arguments and optional arguments (with default values)
* Support for custom argument validation
* .NET framework 2.0 compatibility
* ...all at the cost of writing one tiny class.

## Building ##

Just check out the project and build via Visual Studio. NUnit tests are provided; I recommend running them from Visual Studio via the NUnit test extension.

## Usage ##

For information on how to use CommandLine, see the [CommandLine User's Guide](https://github.com) Wiki page.

## Contributions ##

Feel free to fork and PR your changes for review. Useful enhancements accepted as long as they come with unit tests, comments, and clean code.

## Contact ##

* CommandLine was written and is maintained by Todd Ogrin (<toddogrin@gmail.com>).
