#!/bin/bash

if [ -z "$SQLServerSAPassword" ]; then
  echo "Error: SQLServerSAPassword environment variable is not set."
  exit 1
fi

if [ -z "$SQLServerUserPassword" ]; then
  echo "Error: SQLServerUserPassword environment variable is not set."
  exit 1
fi

sleep 30s

echo "running set up script"

/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "$SQLServerSAPassword" -d master -i db-init.sql -v password="$SQLServerUserPassword"
