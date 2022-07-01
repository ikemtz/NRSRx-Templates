#!/bin/bash
set -e
rm -r ./tests

#MySql WebApi
dotnet new nrsrx-WebApi -n My.NRSRx1.WebApi -L Elasticsearch -D MySql -Ev Redis -o ./tests/My.NRSRx1.WebApi --force
dotnet build ./tests/My.NRSRx1.WebApi

dotnet new nrsrx-WebApi -n My.NRSRx2.WebApi -L Splunk -D MySql -Ev Redis -o ./tests/My.NRSRx2.WebApi --force
dotnet build ./tests/My.NRSRx2.WebApi

dotnet new nrsrx-WebApi -n My.NRSRx3.WebApi -L ApplicationInsights -D MySql -Ev Redis -o ./tests/My.NRSRx3.WebApi --force
dotnet build ./tests/My.NRSRx3.WebApi

#MsSql WebApi
dotnet new nrsrx-WebApi -n My.NRSRx4.WebApi -L Elasticsearch -D MsSql -Ev Redis -o ./tests/My.NRSRx4.WebApi --force
dotnet build ./tests/My.NRSRx4.WebApi

dotnet new nrsrx-WebApi -n My.NRSRx5.WebApi -L Splunk -D MsSql -Ev Redis -o ./tests/My.NRSRx5.WebApi --force
dotnet build ./tests/My.NRSRx5.WebApi

dotnet new nrsrx-WebApi -n My.NRSRx6.WebApi -L ApplicationInsights -D MsSql -Ev Redis -o ./tests/My.NRSRx6.WebApi --force
dotnet build ./tests/My.NRSRx6.WebApi