dotnet new --uninstall ./OData
dotnet clean ./OData
dotnet build ./OData
./reset.ps1
dotnet new --install ./OData
