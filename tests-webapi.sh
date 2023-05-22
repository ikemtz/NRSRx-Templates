#!/bin/bash
set -ex
rm -r ./tests || true

#MySql WebApi
dotnet new nrsrx-webapi -n My.NRSRx1.WebApi -L Elasticsearch -D MySql -Ev Redis -o ./tests/My.NRSRx1.WebApi --force
dotnet build ./tests/My.NRSRx1.WebApi
dotnet new nrsrx-webapi-tests -n My.NRSRx1.WebApi.Tests -D MySql  -Ev Redis -o ./tests/My.NRSRx1.WebApi.Tests --force
dotnet test ./tests/My.NRSRx1.WebApi.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover

dotnet new nrsrx-webapi -n My.NRSRx2.WebApi -L Splunk -D MySql -Ev Redis -o ./tests/My.NRSRx2.WebApi --force
dotnet build ./tests/My.NRSRx2.WebApi
dotnet new nrsrx-webapi-tests -n My.NRSRx2.WebApi.Tests -D MySql -Ev Redis -o ./tests/My.NRSRx2.WebApi.Tests --force
dotnet test ./tests/My.NRSRx2.WebApi.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover

dotnet new nrsrx-webapi -n My.NRSRx3.WebApi -L ApplicationInsights -D MySql -Ev Redis -o ./tests/My.NRSRx3.WebApi --force
dotnet build ./tests/My.NRSRx3.WebApi
dotnet new nrsrx-webapi-tests -n My.NRSRx3.WebApi.Tests -D MySql -Ev Redis -o ./tests/My.NRSRx3.WebApi.Tests --force
dotnet test ./tests/My.NRSRx3.WebApi.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover

#MsSql WebApi
dotnet new nrsrx-webapi -n My.NRSRx4.WebApi -L Elasticsearch -D MsSql -Ev Redis -o ./tests/My.NRSRx4.WebApi --force
dotnet build ./tests/My.NRSRx4.WebApi
dotnet new nrsrx-webapi-tests -n My.NRSRx4.WebApi.Tests -D MsSql -Ev Redis -o ./tests/My.NRSRx4.WebApi.Tests --force
dotnet test ./tests/My.NRSRx4.WebApi.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover

dotnet new nrsrx-webapi -n My.NRSRx5.WebApi -L Splunk -D MsSql -Ev Redis -o ./tests/My.NRSRx5.WebApi --force
dotnet build ./tests/My.NRSRx5.WebApi
dotnet new nrsrx-webapi-tests -n My.NRSRx5.WebApi.Tests -D MsSql -Ev Redis -o ./tests/My.NRSRx5.WebApi.Tests --force
dotnet test ./tests/My.NRSRx5.WebApi.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover

dotnet new nrsrx-webapi -n My.NRSRx6.WebApi -L ApplicationInsights -D MsSql -Ev Redis -o ./tests/My.NRSRx6.WebApi --force
dotnet build ./tests/My.NRSRx6.WebApi
dotnet new nrsrx-webapi-tests -n My.NRSRx6.WebApi.Tests -D MsSql -Ev Redis -o ./tests/My.NRSRx6.WebApi.Tests --force
dotnet test ./tests/My.NRSRx6.WebApi.Tests \
  --filter TestCategory=Unigration \
  --collect "XPlat Code Coverage"  \
  --logger "html;LogFileName=unit-test-results.html" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=json,cobertura,lcov,opencover
