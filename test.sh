#!/bin/bash
set -ex
dotnetVersion=$(dotnet --version)
export dotnetVersion
echo "$dotnetVersion"

if [ "$dotnetVersion" == "9.0.103" ]
then
  dotnet new install ./src/IkeMtz.NRSRx.Templates/Models --force
  dotnet new install ./src/IkeMtz.NRSRx.Templates/OData --force
  dotnet new install ./src/IkeMtz.NRSRx.Templates/'OData Tests' --force
  dotnet new install ./src/IkeMtz.NRSRx.Templates/WebApi --force
  dotnet new install ./src/IkeMtz.NRSRx.Templates/'WebApi Tests' --force
else 
  dotnet new --install ./src/IkeMtz.NRSRx.Templates/Models --force
  dotnet new --install ./src/IkeMtz.NRSRx.Templates/OData --force
  dotnet new --install ./src/IkeMtz.NRSRx.Templates/'OData Tests' --force
  dotnet new --install ./src/IkeMtz.NRSRx.Templates/WebApi --force
  dotnet new --install ./src/IkeMtz.NRSRx.Templates/'WebApi Tests' --force
fi
rm -r ./TestResults || true

set -e
echo --- Executing tests-models.sh ---
/bin/bash -e ./tests-models.sh
echo --- Executing tests-odata-skip-models-mssql.sh ---
/bin/bash -e ./tests-odata-skip-models-mssql.sh
echo --- Executing tests-odata-skip-models-mysql.sh ---
/bin/bash -e ./tests-odata-skip-models-mysql.sh
echo --- Executing tests-odata-skip-models-oracle.sh ---
/bin/bash -e ./tests-odata-skip-models-oracle.sh
echo --- Executing tests-odata.sh ---
/bin/bash -e ./tests-odata.sh
echo --- Executing tests-webapi-skip-models-mssql-redis.sh ---
/bin/bash -e ./tests-webapi-skip-models-mssql-redis.sh
echo --- Executing tests-webapi-skip-models-mysql-redis.sh ---
/bin/bash -e ./tests-webapi-skip-models-mysql-redis.sh
echo --- Executing tests-webapi-skip-models-oracle-noevents.sh ---
/bin/bash -e ./tests-webapi-skip-models-oracle-noevents.sh
echo --- Executing tests-webapi-skip-models-oracle-redis.sh ---
/bin/bash -e ./tests-webapi-skip-models-oracle-redis.sh
echo --- Executing tests-webapi.sh ---
/bin/bash -e ./tests-webapi.sh

echo  ------ FINISHED ------