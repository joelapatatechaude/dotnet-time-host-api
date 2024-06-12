#!/bin/bash

set -e
run_cmd="dotnet TimeApi.dll"

# Function to check if SQL Server is ready
function wait_for_sql_server {
  until dotnet sql -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -Q "SELECT 1"; do
    >&2 echo "SQL Server is starting up"
    sleep 1
  done
}

# Wait for SQL Server to be ready before starting the application
#wait_for_sql_server

>&2 echo "SQL Server is up - executing command"
exec $run_cmd
