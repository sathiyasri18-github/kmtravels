# Stops SnlEngineering.Api and SnlEngineering.Web dev processes so dotnet build can copy DLLs.
$names = @('SnlEngineering.Api', 'SnlEngineering.Web')
foreach ($name in $names) {
    Get-Process -Name $name -ErrorAction SilentlyContinue | ForEach-Object {
        Write-Host "Stopping $($_.ProcessName) (PID $($_.Id))..."
        Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
    }
}
# Also shut down dotnet build servers that may hold locks
dotnet build-server shutdown 2>$null
Write-Host "Done. You can now run: cd src/SnlEngineering.Api; dotnet run"
