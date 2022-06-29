dotnet new --uninstall ./IkeMtz.NRSRx.Templates
dotnet clean ./IkeMtz.NRSRx.Templates
dotnet build ./IkeMtz.NRSRx.Templates
./reset.ps1
dotnet new --install ./IkeMtz.NRSRx.Templates
