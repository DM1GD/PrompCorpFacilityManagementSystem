param($installPath, $toolsPath, $package, $project)
[System.Reflection.Assembly]::LoadFile("$($installPath)\lib\net40\System.Data.CData.Odoo.dll")
[System.Data.CData.Odoo.Nuget]::CheckNugetLicense("nuget")