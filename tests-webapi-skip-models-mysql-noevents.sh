#!/bin/bash
set -ex
rm -r ./tests || true

#Models
dotnet new nrsrx-models -n My.NRSRx1.Models -o ./tests/My.NRSRx1.Models --force
dotnet build ./tests/My.NRSRx1.Models

dotnet new nrsrx-models -n My.NRSRx2.Models -E Asset -o ./tests/My.NRSRx2.Models --force
dotnet build ./tests/My.NRSRx2.Models

#MsSql WebApi
dotnet new nrsrx-webapi -n My.NRSRx1.WebApi -S true -Ev NoEvents -L Elasticsearch -D MsSql -o ./tests/My.NRSRx1.WebApi --force
dotnet build ./tests/My.NRSRx1.WebApi

dotnet new nrsrx-webapi -n My.NRSRx2.WebApi -S true -Ev NoEvents -E Asset -L Splunk -D MsSql -o ./tests/My.NRSRx2.WebApi --force
dotnet build ./tests/My.NRSRx2.WebApi


#MsSql WebApi Tests
dotnet new nrsrx-webapi-tests -n My.NRSRx1.WebApi.Tests -S true  -Ev NoEvents -D MsSql -o ./tests/My.NRSRx1.WebApi.Tests --force
dotnet build ./tests/My.NRSRx1.WebApi.Tests
dotnet test ./tests/My.NRSRx1.WebApi.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover
  
dotnet new nrsrx-webapi-tests -n My.NRSRx2.WebApi.Tests -S true -Ev NoEvents -D MsSql -E Asset -o ./tests/My.NRSRx2.WebApi.Tests --force
dotnet build ./tests/My.NRSRx2.WebApi.Tests
dotnet test ./tests/My.NRSRx2.WebApi.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover
  