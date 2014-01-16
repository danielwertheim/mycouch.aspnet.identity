# MyCouch.AspNet.Identity #
A simple implementation of the new ASP.Net identity provider model for handling authentication and authorization in e.g. ASP.Net MVC5 using CouchDb or Cloudant as storage.

## NuGet ##
MyCouch.AspNet.Identity is distributed via NuGet. You can [find the CouchDb package here](https://nuget.org/packages/MyCouch.AspNet.Identity/). But basically, in any .Net4.0, .Net4.5 or Windows Store app project, open up the Package manager console, and invoke:

    pm:> install-package mycouch.aspnet.identity

**Please note!** Some users with old versions of NuGet has reported that some dependencies might not be resolved. The solution is to update NuGet.

## Get up and running with the source ##
Please note. **No NuGet packages are checked in**. If you are using the latest version of NuGet (v2.7.1+) you should be able to just build and the packages will be restored. If this does not work, you could install the missing NuGet packages using the provided PowerShell script:

    ps:> .\setup-devenv.ps1

or

    cmd:> powershell -executionpolicy unrestricted .\setup-devenv.ps1

For the script to work, you need to have [the NuGet command line](http://nuget.codeplex.com/releases) `(NuGet.exe) registrered in the environment path`, or you need to tweak the script so it knows where it will find your NuGet.exe.

## Issues, questions, etc ##
So you have issues or questions... Great! That means someone is using it. Use the issues function here at the project page or contact me via mail: firstname@lastname.se; or Twitter: [@danielwertheim](https://twitter.com/danielwertheim)

## License ##
The MIT License (MIT)

Copyright (c) 2014 Daniel Wertheim

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
