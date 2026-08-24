@ECHO OFF
ECHO Updating Database.
dotnet Internalexamportal.Web.dll /hook:commands create-database,stop
PAUSE