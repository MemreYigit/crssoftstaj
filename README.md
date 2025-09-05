### Internship Project – Sabancı University CS395 (Crssoft)

This project was developed as part of my CS395 Internship lesson at Sabanci University, hosted at CrsSoft.

It is a web application built with:

- Frontend: React (Visual Studio Code)
- Backend: ASP.NET Core (visual Studio)
- Database: Microsoft SQL Server (SSMS)

## Requirements

- Visual Studio
- Visual Studio Code
- SQL Server Management Studio
- Node.js
- SQL Server
- IIS Express (comes with Visual Studio)

# Database Setup

1. Install SQL server on your PC and make sure it is running. If you want you can use SQL Server Configuration Manager to make sure and run the server.

2. Open SSMS and connect to your SQL Server.

# Backend Setup

1. Open the backend project in Visual Studio and delete "Migrations" folder. (We’ll recreate this folder in the next steps.)

2. From the Tools menu, go to NuGet Package Manager → Manage NuGet Packages for Solution, and install the following NuGet packages: 
   - Microsoft.AspNetCore.Authentication.JwtBearer
   - Microsoft.AspNetCore.CookiePolicy
   - Microsoft.EntityFrameworkCore
   - Microsoft.EntityFrameworkCore.Design
   - Microsoft.EntityFrameworkCore.SqlServer
   - Microsoft.EntityFrameworkCore.Tools

3. Open Package Manager Console (in Visual Studio: Tools > NuGet Package Manager > Package Manager Console).

4. Run migrations:
```
Add-Migration InitialCreate
```
```
Update-Database
```
This will create "Migrations" folder and the database schema.

5. Start the backend using IIS Express on port 5111.

Note:
If the project is not running on port 5111, you can update the configuration in one of the following ways:
- In Visual Studio, click the small drop-down arrow next to IIS Express, go to Debug Properties, and change the port.
- Alternatively, update the Program.cs file in the CORS configuration to include the correct origin:
    ```
    builder.Services.AddCors(o =>
    {
        o.AddPolicy("Spa", p => p
            .WithOrigins("http://localhost:3000", "http://localhost:5111") // your UI origins
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // needed for cookies
    });
    ```

# Frontend Setup

1. Open the frontend project in Visual Studio Code.

2. Run in Visual Studio Code terminal: 
```
npm install
```
It will create the node_modules folder.
Note: Sometimes powershell do not run this code.
- Try using Command Prompt instead.
- Or, run this command in powershell
    ```
    Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
    ```
    After use you can undo this action run this command in powershell
    ```
    Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Restricted
    ```

3. Start the development server: 
```
npm start
```
The frontend will run on [http(http://localhost:3000)]

# Running the Project

1. Make sure your SQL Server is running.
2. Start the backend in Visual Studio with IIS Express (port 5111).
3. Start the frontend with npm start (port 3000).
4. Open the website in your browser at:
```
http://localhost:3000
```

## Acknowledgements

- Crssoft for providing mentorship and internship opportunity
- My mentor Hüseyin Onur Onma