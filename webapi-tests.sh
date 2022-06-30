#!/bin/bash
rm -r ./tests

#MySql WebApi
dotnet new nrsrx-WebApi -n My.NRSRx.WebApi1 -L Elasticsearch -D MySql -E Redis -o ./tests/My.NRSRx.WebApi1 --force
dotnet build ./tests/My.NRSRx.WebApi1

dotnet new nrsrx-WebApi -n My.NRSRx.WebApi2 -L Splunk -D MySql -E Redis -o ./tests/My.NRSRx.WebApi2 --force
dotnet build ./tests/My.NRSRx.WebApi2

dotnet new nrsrx-WebApi -n My.NRSRx.WebApi3 -L ApplicationInsights -D MySql -E Redis -o ./tests/My.NRSRx.WebApi3 --force
dotnet build ./tests/My.NRSRx.WebApi3

#MsSql WebApi
dotnet new nrsrx-WebApi -n My.NRSRx.WebApi4 -L Elasticsearch -D MsSql -E Redis -o ./tests/My.NRSRx.WebApi4 --force
dotnet build ./tests/My.NRSRx.WebApi4

dotnet new nrsrx-WebApi -n My.NRSRx.WebApi5 -L Splunk -D MsSql -E Redis -o ./tests/My.NRSRx.WebApi5 --force
dotnet build ./tests/My.NRSRx.WebApi5

dotnet new nrsrx-WebApi -n My.NRSRx.WebApi6 -L ApplicationInsights -D MsSql -E Redis -o ./tests/My.NRSRx.WebApi6 --force
dotnet build ./tests/My.NRSRx.WebApi6