# WeddingApp
Blazor website collecting and handling photos taken by all wedding guests.

## Requirements
Before starting application

1. Delete all migrations file
2. Update AddDbContext in program.cs according to database you are using

```c#
builder.Services.AddDbContext<WeddingAppUserContext>(options => {
    options.UseMySQL(builder.Configuration.GetConnectionString("MySQL"));
    options.EnableSensitiveDataLogging();
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

```
3. Update ConnectionStrings in appsettings.json
```c#
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings:MySQL": "<your_URL>"
}

```
4. In NuGet manager console
```bash
Add-Migration <migration_name>
Update-Database
```
5. To add new user just login
![Alt text](https://github.com/Tyreyn/WeddingApp/blob/main/login.png "This is login image")

## User Panel
![Alt text](https://github.com/Tyreyn/WeddingApp/blob/main/gallery.png "This is gallery panel")
![Alt text](https://github.com/Tyreyn/WeddingApp/blob/main/galleryfilled.png "This is filled gallery panel")
![Alt text](https://github.com/Tyreyn/WeddingApp/blob/main/weddingplan.png "This is wedding plan panel")
![Alt text](https://github.com/Tyreyn/WeddingApp/blob/main/weddingplanfilled.png "This is filled wedding plan panel")

## Admin Panel only for user=admin phone=admin
![Alt text](https://github.com/Tyreyn/WeddingApp/blob/main/admin1.png "This is admin panel")
![Alt text](https://github.com/Tyreyn/WeddingApp/blob/main/admin2.png "This is admin panel")
