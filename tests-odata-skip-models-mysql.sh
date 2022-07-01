#!/bin/bash
rm -r ./tests

#Models
dotnet new nrsrx-models -n My.NRSRx1.Models -o ./tests/My.NRSRx1.Models --force
dotnet build ./tests/My.NRSRx1.Models

dotnet new nrsrx-models -n My.NRSRx2.Models -E Asset -o ./tests/My.NRSRx2.Models --force
dotnet build ./tests/My.NRSRx2.Models

#MySql OData
dotnet new nrsrx-odata -n My.NRSRx1.OData -S true -L Elasticsearch -D MySql -o ./tests/My.NRSRx1.OData --force
dotnet build ./tests/My.NRSRx1.OData

dotnet new nrsrx-odata -n My.NRSRx2.OData -S true -E Asset -L Splunk -D MySql -o ./tests/My.NRSRx2.OData --force
dotnet build ./tests/My.NRSRx2.OData