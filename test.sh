#!/bin/bash
echo --- Executing tests-models.sh ---
/bin/bash ./tests-models.sh
echo --- Executing tests-odata-skip-models-mssql.sh ---
/bin/bash ./tests-odata-skip-models-mssql.sh
echo --- Executing tests-odata-skip-models-mysql.sh ---
/bin/bash ./tests-odata-skip-models-mysql.sh
echo --- Executing tests-odata.sh ---
/bin/bash ./tests-odata.sh
echo --- Executing tests-webapi-skip-models-mssql.sh ---
/bin/bash ./tests-webapi-skip-models-mssql.sh
echo --- Executing tests-webapi-skip-models-mysql.sh ---
/bin/bash ./tests-webapi-skip-models-mysql.sh
echo --- Executing tests-webapi.sh ---
/bin/bash ./tests-webapi.sh