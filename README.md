# Blind Taste Test App

## Docker

- `docker build . --file .\src\Pumpkin.Beer.Taste\Dockerfile`

Develop in Docker Compose

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

- Login to ECR (docker login token expires after 12 hours) `aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 246282979715.dkr.ecr.us-east-1.amazonaws.com`
- Push latest `docker build . --file .\src\Pumpkin.Beer.Taste\Dockerfile -t 246282979715.dkr.ecr.us-east-1.amazonaws.com/pumpkintasting:latest --push`

Head to portainer and reload stack with a fresh pull.

## TODO

- Use the new docker nuget to remove dockerfile?
- Cake
- Dockerfile build does not work with `--no-restore` flag
- docker-compose traefik development cert. Maybe try this? https://gist.github.com/pyrou/4f555cd55677331c742742ee6007a73a 


Think about time zones a little more...

The audit properties save with a zero offset.

When a user creates a tasting, the start and closed date is saved with the offset.

When you use the clock service to get the UtcNow, it comes back without an offset.
