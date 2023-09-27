# IIS Pool Recycles

This web application allows you to remotely recycle your IIS application pools from the web. It leverages the "Microsoft.Web.Administration" NuGet package for interacting with IIS.

## About Microsoft.Web.Administration

For more information about the "Microsoft.Web.Administration" package, you can refer to the official documentation:
- [Microsoft.Web.Administration Documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.web.administration?view=iis-dotnet)
- [NuGet Package](https://www.nuget.org/packages/Microsoft.Web.Administration/)

## Instructions

To get started, follow these steps:

1. **Create an Application Pool**: Create an IIS application pool for your web application and assign it an identity with administrative privileges.

2. **Configure Web Application Names**: Add the names of the application pools you want to recycle in the `.xml` file named "webapps.xml". Ensure that the names match exactly.

3. **Password Protect**: Ensure that your web page is password protected to prevent unauthorized access. This is essential to secure the ability to recycle your application pools.

## Note

- The provided code allows you to perform multiple application pool recycles simultaneously. When you click on the dropdown labeled "Customer X", it will initiate the recycling of the child applications specified in the "webapps.xml" file.

Please make sure to follow security best practices and restrict access to this application to authorized personnel only.

