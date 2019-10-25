$proj = '.\src\Pumpkin.Beer.Taste';

Write-Output "RIP Migrations"
if(Test-Path -Path '.\src\Pumpkin.Beer.Taste\Migrations'){
	Remove-Item '.\src\Pumpkin.Beer.Taste\Migrations' -Recurse
}

Write-Output "RIP Sqlite Database"
if(Test-Path -Path '.\src\Pumpkin.Beer.Taste\blindtastetestdb.sqlite'){
	Remove-Item '.\src\Pumpkin.Beer.Taste\blindtastetestdb.sqlite' -Recurse
}

dotnet ef migrations add InitialCreate --project $proj