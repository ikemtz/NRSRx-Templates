#!/bin/bash
rm -r ./tests

#MySql OData
dotnet new nrsrx-models -n My.NRSRx.Models1 -o ./tests/My.NRSRx.Models1 --force
dotnet build ./tests/My.NRSRx.Models1

dotnet new nrsrx-models -n My.NRSRx.Models2 -E Asset -o ./tests/My.NRSRx.Models2 --force
dotnet build ./tests/My.NRSRx.Models2