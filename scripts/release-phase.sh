#!/bin/sh
#Line ending in this fie must be unix line ending. We unsure this via setting in '.gitattribute' file.
set -e
set -o verbose
echo "This command will be executed during 'release phase' on Heroku deployment."
if [ -z "$DefaultConnectionAutoMigrate" ]
then
    echo "DB migrations are not enabled."
else
    cd src/Webapp/ && dotnet restore && dotnet ef database update --verbose
fi
