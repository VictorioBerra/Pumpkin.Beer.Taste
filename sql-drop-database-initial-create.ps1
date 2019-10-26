$proj = '.\src\Pumpkin.Beer.Taste';

# Rollback to before initial create
dotnet ef database update 0 --project $proj

Write-Output "RIP Migrations"
if(Test-Path -Path '.\src\Pumpkin.Beer.Taste\Migrations'){
	Remove-Item '.\src\Pumpkin.Beer.Taste\Migrations' -Recurse
}

dotnet ef migrations add InitialCreate --project $proj

dotnet ef database update --project $proj

# Drop Table AspNetRoleClaims;
# Drop Table AspNetUserClaims;
# Drop Table AspNetUserLogins;
# Drop Table AspNetUserRoles;
# Drop Table AspNetUserTokens;
# Drop Table BlindVote;
# Drop Table AspNetRoles;
# Drop Table BlindItem;
# Drop Table Blind;
# Drop Table AspNetUsers;
# Drop Table AspNetRoleClaims;

# clean up:
# DELETE FROM BlindVote;
# DELETE FROM BlindItem;
# DELETE FROM Blind;