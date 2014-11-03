@ECHO OFF

ECHO Preparing NuGet...
CALL ..\..\set-nuget-key.bat
del *.nupkg
pause

ECHO Publishing to NuGet...
nuget pack ..\Ministry.ReflectionHelper\Ministry.ReflectionHelper.csproj -Prop Configuration=Release
nuget push *.nupkg

pause