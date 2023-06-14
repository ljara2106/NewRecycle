# NewRecycle

This web application, allows you to recycle your IIS application pools remotely from the web. It uses a NuGet package called "Microsoft.Web.Administration"
you can find more information about that package here:
[Microsoft.Web.Administration](https://learn.microsoft.com/en-us/dotnet/api/microsoft.web.administration?view=iis-dotnet)
and here:
https://www.nuget.org/packages/Microsoft.Web.Administration/

Instruction:
   - Create an app pool, assign this application identity to a user with administrative privileges.
   - Add your application pool names in the .xml file called "webapps.xml" make sure it matches correctly.
   - create an empty 'log' folder in root. (it creates a log file and log IP of who clicks on the recycle button.)
   - make sure to password protect this web page so random people dont get access to recycle your applications.

