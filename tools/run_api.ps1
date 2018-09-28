
$projDir = '..\src\idsrv.api\'

Start-Process -FilePath 'dotnet' -WorkingDirectory $projDir -ArgumentList 'run'
