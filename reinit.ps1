dotnet new --uninstall ./src/IkeMtz.NRSRx.Templates
dotnet clean ./src/IkeMtz.NRSRx.Templates
dotnet build ./src/IkeMtz.NRSRx.Templates
./reset.ps1
dotnet new --install ./src/IkeMtz.NRSRx.Templates
