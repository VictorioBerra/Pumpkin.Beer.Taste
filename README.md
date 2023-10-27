# Blind Taste Test App

## SQL Container Creation

- https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash
- `sudo docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest`
- `sudo docker exec -it sql1 "bash"`
- `/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "<enterStrongPasswordHere>"`
- `CREATE DATABASE BlindTasteTest; GO;`
- `ALTER LOGIN sa ENABLE; GO;`
- `ALTER LOGIN sa WITH PASSWORD = '<enterStrongPasswordHere>'; GO;`

## TODO

- SQLServer Docker for development
- Cake
- Remove all Identity stuff
- FluentValidation
