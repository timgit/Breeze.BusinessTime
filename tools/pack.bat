@echo off
IF "%~1"=="" goto error

SETLOCAL

SET VERSION=%1
SET PUSH=%2
SET NUGET=..\src\.nuget\nuget.exe

SET PACKAGE_SPEC=..\src\breeze.businesstime\breeze.businesstime.nuspec
SET PACKAGE_NAME=Breeze.BusinessTime

%NUGET% pack %PACKAGE_SPEC% -Version %VERSION%

IF "%PUSH%"=="-push" goto push

goto end

:push
%NUGET% push %PACKAGE_NAME%.%VERSION%.nupkg
goto end

:error
echo ** Error: Missing version parameter.  
echo Usage: pack VERSION [-push]
echo For example: pack 1.0.3-alpha -push

:end