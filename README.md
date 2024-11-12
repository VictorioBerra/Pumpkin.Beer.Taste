# Blind Taste Test App

## Contributing

Spin up the MSSQL container using `docker compose up pumpkinbeertaste-db -d`. This is a one-time thing unless you want to blow away the container and volume and start again.

## Docker

- `docker build . --file .\src\Pumpkin.Beer.Taste\Dockerfile`

Build using Docker Compose

- `docker-compose up --remove-orphans --build`

## Manual SQL Server Container Creation

- https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash
- `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=myStong_Password123!" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest`
- `docker exec -it sql1 "bash"`
- `/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "myStong_Password123!"`
- `CREATE DATABASE BlindTasteTest; GO;`
- `ALTER LOGIN sa ENABLE; GO;`
- `ALTER LOGIN sa WITH PASSWORD = 'myStong_Password123!'; GO;`

## Deployment

- Automated by GitHub Actions

# Migrations

- `cd ./src/Pumpkin.Beer.Taste`
- `dotnet ef migrations add Whatever`

## TODO

- Use the new docker nuget to remove dockerfile? https://learn.microsoft.com/en-us/dotnet/core/docker/publish-as-container?pivots=dotnet-8-0#add-nuget-package
- Dockerfile build does not work with `--no-restore` flag
- User profiles
    - Num tastings done, etc.

TimeZones are not implemented at all basically. This app fully assumes CT time zone. Docker compose sets DB to TZ=America/Chicago.

The audit properties save with a zero offset.

When a user creates a tasting, the start and closed date is saved with the offset.

When you use the clock service to get the UtcNow, it comes back without an offset. - TODO did this change when I upgraded TimeProvider? Need to test...
