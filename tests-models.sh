#!/bin/bash
set -ex
rm -r ./tests || true

#MySql OData
dotnet new nrsrx-models -n My.NRSRx1.Models -o ./tests/My.NRSRx1.Models --force
dotnet build ./tests/My.NRSRx1.Models

dotnet new nrsrx-models -n My.NRSRx2.Models -E Asset -o ./tests/My.NRSRx2.Models --force
dotnet build ./tests/My.NRSRx2.Models