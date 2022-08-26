#!/bin/bash
set -e
rm -r ./tests || true

#Models
dotnet new nrsrx-models -n My.NRSRx1.Models -o ./tests/My.NRSRx1.Models --force
dotnet build ./tests/My.NRSRx1.Models

dotnet new nrsrx-models -n My.NRSRx2.Models -E Asset -o ./tests/My.NRSRx2.Models --force
dotnet build ./tests/My.NRSRx2.Models

#MsSql OData
dotnet new nrsrx-odata -n My.NRSRx1.OData -S true -L Elasticsearch -D MsSql -o ./tests/My.NRSRx1.OData --force
dotnet build ./tests/My.NRSRx1.OData

dotnet new nrsrx-odata -n My.NRSRx2.OData -S true -E Asset -L Splunk -D MsSql -o ./tests/My.NRSRx2.OData --force
dotnet build ./tests/My.NRSRx2.OData

#MsSql OData Tests
dotnet new nrsrx-odata-tests -n My.NRSRx1.OData.Tests -S true -o ./tests/My.NRSRx1.OData.Tests --force
dotnet build ./tests/My.NRSRx1.OData.Tests
dotnet test ./tests/My.NRSRx1.OData.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover

dotnet new nrsrx-odata-tests -n My.NRSRx2.OData.Tests -S true -E Asset -o ./tests/My.NRSRx2.OData.Tests --force
dotnet build ./tests/My.NRSRx2.OData.Tests
dotnet test ./tests/My.NRSRx2.OData.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover
