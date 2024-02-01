# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of solutions
$solutions = (
    "basics"
)

# List of projects
$projects = (
    "IniFileLib"
)
