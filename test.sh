#!/bin/bash
echo --- Executing tests-models.sh ---
/bin/bash -e ./tests-models.sh
echo --- Executing tests-odata-skip-models-mssql.sh ---
/bin/bash -e ./tests-odata-skip-models-mssql.sh
echo --- Executing tests-odata-skip-models-mysql.sh ---
/bin/bash -e ./tests-odata-skip-models-mysql.sh
echo --- Executing tests-odata.sh ---
/bin/bash -e ./tests-odata.sh
echo --- Executing tests-webapi-skip-models-mssql.sh ---
/bin/bash -e ./tests-webapi-skip-models-mssql.sh
echo --- Executing tests-webapi-skip-models-mysql.sh ---
/bin/bash -e ./tests-webapi-skip-models-mysql.sh
echo --- Executing tests-webapi.sh ---
/bin/bash -e ./tests-webapi.sh