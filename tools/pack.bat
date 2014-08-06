@echo off
IF "%~1"=="" goto error

SETLOCAL

SET VERSION=%1
SET PUSH=%2
SET SRC=..\src
SET NUGET=%SRC%\.nuget\nuget.exe

%NUGET% pack %SRC%\breeze.businesstime\breeze.businesstime.nuspec -Version %VERSION%

IF "%PUSH%"=="-push" goto push

goto end

:push
%NUGET% push Breeze.BusinessTime.%VERSION%.nupkg
goto end

:error
echo ** Error: Missing version parameter.  
echo Usage: pack VERSION [-push]
echo For example: pack 1.0.3-alpha -push

:end