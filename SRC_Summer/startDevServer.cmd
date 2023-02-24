:start
dotnet build DocumentManager.Webapi --no-incremental --force
dotnet watch run -c Debug --project DocumentManager.Webapi
goto start