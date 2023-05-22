#!/bin/bash
set -ex
rm -r ./tests || true

#MySql OData
dotnet new nrsrx-odata -n My.NRSRx1.OData -L Elasticsearch -D MySql -o ./tests/My.NRSRx1.OData --force
dotnet build ./tests/My.NRSRx1.OData

dotnet new nrsrx-odata -n My.NRSRx2.OData -L Splunk -D MySql -o ./tests/My.NRSRx2.OData --force
dotnet build ./tests/My.NRSRx2.OData

dotnet new nrsrx-odata -n My.NRSRx3.OData -L ApplicationInsights -D MySql -o ./tests/My.NRSRx3.OData --force
dotnet build ./tests/My.NRSRx3.OData

#MsSql OData
dotnet new nrsrx-odata -n My.NRSRx4.OData -L Elasticsearch -D MsSql -o ./tests/My.NRSRx4.OData --force
dotnet build ./tests/My.NRSRx4.OData

dotnet new nrsrx-odata -n My.NRSRx5.OData -L Splunk -D MsSql -o ./tests/My.NRSRx5.OData --force
dotnet build ./tests/My.NRSRx5.OData

dotnet new nrsrx-odata -n My.NRSRx6.OData -L ApplicationInsights -D MsSql -o ./tests/My.NRSRx6.OData --force
dotnet build ./tests/My.NRSRx6.OData

#MySql OData Tests
dotnet new nrsrx-odata-tests -n My.NRSRx1.OData.Tests -o ./tests/My.NRSRx1.OData.Tests -D MySql --force
dotnet build ./tests/My.NRSRx1.OData.Tests
dotnet test ./tests/My.NRSRx1.OData.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover

dotnet new nrsrx-odata-tests -n My.NRSRx2.OData.Tests -o ./tests/My.NRSRx2.OData.Tests -D MySql --force
dotnet build ./tests/My.NRSRx2.OData.Tests
dotnet test ./tests/My.NRSRx2.OData.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover
  
dotnet new nrsrx-odata-tests -n My.NRSRx3.OData.Tests -o ./tests/My.NRSRx3.OData.Tests -D MySql --force
dotnet build ./tests/My.NRSRx3.OData.Tests
dotnet test ./tests/My.NRSRx3.OData.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover
  
#MsSql OData Tests
dotnet new nrsrx-odata-tests -n My.NRSRx4.OData.Tests -o ./tests/My.NRSRx4.OData.Tests --force
dotnet build ./tests/My.NRSRx4.OData.Tests
dotnet test ./tests/My.NRSRx4.OData.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover
  
dotnet new nrsrx-odata-tests -n My.NRSRx5.OData.Tests -o ./tests/My.NRSRx5.OData.Tests --force
dotnet build ./tests/My.NRSRx5.OData.Tests
dotnet test ./tests/My.NRSRx5.OData.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover
  
dotnet new nrsrx-odata-tests -n My.NRSRx6.OData.Tests -o ./tests/My.NRSRx6.OData.Tests --force
dotnet build ./tests/My.NRSRx6.OData.Tests
dotnet test ./tests/My.NRSRx6.OData.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover
  