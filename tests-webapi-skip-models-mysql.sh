#!/bin/bash
set -e
rm -r ./tests

#Models
dotnet new nrsrx-models -n My.NRSRx1.Models -o ./tests/My.NRSRx1.Models --force
dotnet build ./tests/My.NRSRx1.Models

dotnet new nrsrx-models -n My.NRSRx2.Models -E Asset -o ./tests/My.NRSRx2.Models --force
dotnet build ./tests/My.NRSRx2.Models

#MySql WebApi
dotnet new nrsrx-webapi -n My.NRSRx1.WebApi -S true -Ev Redis -L Elasticsearch -D MySql -o ./tests/My.NRSRx1.WebApi --force
dotnet build ./tests/My.NRSRx1.WebApi

dotnet new nrsrx-webapi -n My.NRSRx2.WebApi -S true -Ev Redis -E Asset -L Splunk -D MySql -o ./tests/My.NRSRx2.WebApi --force
dotnet build ./tests/My.NRSRx2.WebApi