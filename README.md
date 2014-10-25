# MyCouch.AspNet.Identity #
A simple implementation of the new ASP.Net identity provider model for handling authentication and authorization in e.g. ASP.Net MVC5 using CouchDb or Cloudant as storage.

Uses [MyCouch - The asynchronous CouchDb and Cloudant client for .Net](https://github.com/danielwertheim/mycouch).

## NuGet ##
MyCouch.AspNet.Identity is distributed via NuGet. You can [find the package here](https://nuget.org/packages/MyCouch.AspNet.Identity/). But basically, in a ASP.Net MVC5 project using the [ASP.Net Identity 2.1 package](http://www.nuget.org/packages/Microsoft.AspNet.Identity.Core):

    pm:> install-package mycouch.aspnet.identity

**Please note!** Some users with old versions of NuGet has reported that some dependencies might not be resolved. The solution is to update NuGet.

## Releases ##
**NOTE!** The stable version 1.0.0 is not backwards compatible as it makes use of `IdentityUser.Email` as `UserName` and `Id`; hence your CouchDB `_id` will not match. Also, the views has been reworked.

Ensure to read the [release notes](https://github.com/danielwertheim/mycouch.aspnet.identity/wiki/release-notes).

## Usage ##
The [wiki](https://github.com/danielwertheim/mycouch.aspnet.identity/wiki) describes usage of the package.

## Get up and running with the source ##
Please note. **No NuGet packages are checked in**. If you are using the latest version of NuGet (v2.7.1+) **you should be able to just build and the packages will be restored**. If this does not work, you could install the missing NuGet packages using a simple PowerShell script [as covered here](http://danielwertheim.se/2013/08/12/nuget-restore-powershell-vs-rake)

## Issues, questions, etc ##
So you have issues or questions... Great! That means someone is using it. Use the issues function here at the project page or contact me via mail: firstname@lastname.se; or Twitter: [@danielwertheim](https://twitter.com/danielwertheim)

## License ##
The MIT License (MIT)

Copyright (c) 2014 Daniel Wertheim

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
