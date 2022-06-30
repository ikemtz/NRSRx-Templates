#!/bin/bash
rm -r ./tests

#MySql OData
dotnet new nrsrx-odata -n My.NRSRx.OData1 -L Elasticsearch -D MySql -o ./tests/My.NRSRx.OData1 --force
dotnet build ./tests/My.NRSRx.OData1

dotnet new nrsrx-odata -n My.NRSRx.OData2 -L Splunk -D MySql -o ./tests/My.NRSRx.OData2 --force
dotnet build ./tests/My.NRSRx.OData2

dotnet new nrsrx-odata -n My.NRSRx.OData3 -L ApplicationInsights -D MySql -o ./tests/My.NRSRx.OData3 --force
dotnet build ./tests/My.NRSRx.OData3

#MsSql OData
dotnet new nrsrx-odata -n My.NRSRx.OData4 -L Elasticsearch -D MsSql -o ./tests/My.NRSRx.OData4 --force
dotnet build ./tests/My.NRSRx.OData4

dotnet new nrsrx-odata -n My.NRSRx.OData5 -L Splunk -D MsSql -o ./tests/My.NRSRx.OData5 --force
dotnet build ./tests/My.NRSRx.OData5

dotnet new nrsrx-odata -n My.NRSRx.OData6 -L ApplicationInsights -D MsSql -o ./tests/My.NRSRx.OData6 --force
dotnet build ./tests/My.NRSRx.OData6