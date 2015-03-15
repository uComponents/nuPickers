ECHO off

SET /P APPVEYOR_BUILD_NUMBER=Please enter a build number (e.g. 134):
SET /P PACKAGE_VERSION=Please enter your package version (e.g. 1.0.5):
SET /P UMBRACO_PACKAGE_PRERELEASE_SUFFIX=Please enter your package release suffix or leave empty (e.g. beta):

SET /P APPVEYOR_REPO_BRANCH=If you want to simulate a specific branch (e.g. master):

if "%APPVEYOR_BUILD_NUMBER%" == "" (
  SET APPVEYOR_BUILD_NUMBER=48
)
if "%PACKAGE_VERSION%" == "" (
  SET PACKAGE_VERSION=1.1.0
)

SET APPVEYOR_BUILD_VERSION=%PACKAGE_VERSION%.%APPVEYOR_BUILD_NUMBER%

build.bat

@IF %ERRORLEVEL% NEQ 0 GOTO err
@EXIT /B 0
:err
@PAUSE
@EXIT /B 1