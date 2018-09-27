
$projDir = '..\src\Yunify.Auth.Server\'

if([System.IO.File]::Exists($projDir + 'db.identityconfig.sqlite')){
    Start-Process -FilePath 'dotnet' -WorkingDirectory $projDir -ArgumentList 'ef database drop -f -c ConfigurationStoreContext' -Wait
}

Start-Process -FilePath 'dotnet' -WorkingDirectory $projDir -ArgumentList 'ef database update -c ConfigurationStoreContext' -Wait
Start-Process -FilePath 'dotnet' -WorkingDirectory $projDir -ArgumentList 'ef database update -c UserDbContext' -Wait