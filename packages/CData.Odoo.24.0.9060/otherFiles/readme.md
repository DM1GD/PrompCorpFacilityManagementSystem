# CData Odoo Provider
The CData Odoo Provider allows you to connect to Odoo sources just like SQL tables. Simply specify your credentials and access data from any Odoo source. 

### Licensing

#### .Net Framework

The NuGet package automatically downloads and installs a license so no additional action is needed. 

#### .Net Standard/.Net Core

A license must be activated before the .NET Standard Provider can be used. To activate a license, you can use install-license, a .NET Core application included with the Provider. This application is present in the 'tools' folder of the package installation directory, which on Windows can typically be found at: %USERPROFILE%\.nuget\packages\cdata.datasource

To use the install-license application run the command:

dotnet ./install-license.dll 'key'

Ensure that 'key' is your product key. This will install a license on the particular system.

### Establishing a connection

In-depth documentation on establishing a connection to Odoo can be found at [our website](https://cdn.cdata.com/help/OEK/ado/default.htm).

Odoo 2024 - Copyright (c) 2024 CData Software, Inc. - All rights reserved. - For more information, please visit our website at [www.cdata.com](http://www.cdata.com/?src=readme&bld=9060).