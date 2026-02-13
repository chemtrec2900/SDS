# Troubleshooting Guide

## Access Denied Error When Running Application

If you encounter the error:
```
System.ComponentModel.Win32Exception (5): Access is denied
```

This is typically caused by Windows security settings or antivirus software blocking execution.

### Solutions

#### Option 1: Run as Administrator
1. Open PowerShell or Command Prompt as Administrator
2. Navigate to the project directory
3. Run: `dotnet run --project src/SDS.API`

#### Option 2: Unblock the Executable
Run this PowerShell command:
```powershell
Unblock-File -Path "src\SDS.API\bin\Debug\net9.0\SDS.API.exe"
```

Or unblock the entire directory:
```powershell
Get-ChildItem -Path "src\SDS.API\bin\Debug\net9.0" -Recurse | Unblock-File
```

#### Option 3: Use DLL Instead of EXE
The project has been configured to not create an executable. Run using:
```powershell
dotnet exec src/SDS.API/bin/Debug/net9.0/SDS.API.dll
```

#### Option 4: Check Windows Defender/Antivirus
1. Add the project folder to Windows Defender exclusions:
   - Open Windows Security
   - Go to Virus & threat protection
   - Click "Manage settings"
   - Add exclusion → Folder
   - Select: `C:\Users\tesfaye.gari\source\repos\SDS`

#### Option 5: Check Group Policy
If you're on a corporate network, check with IT if there are group policies blocking execution of unsigned executables.

#### Option 6: Clean and Rebuild
```powershell
dotnet clean
dotnet build
dotnet run --project src/SDS.API
```

### Alternative: Use Visual Studio
If command line continues to have issues:
1. Open the solution in Visual Studio 2022
2. Set `SDS.API` as the startup project
3. Press F5 to run

### Verify the Fix
Once running, you should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5141
```

Then access Swagger UI at: `http://localhost:5141/swagger`
