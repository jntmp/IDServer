
$projDir = '..\src\idsrv.server\bin\Debug\netcoreapp2.1\'

Start-Process -FilePath 'dotnet' -WorkingDirectory $projDir -ArgumentList '.\idsrv.server.dll'