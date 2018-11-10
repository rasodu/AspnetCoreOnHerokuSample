#!/bin/sh
#Line ending in this fie must be unix line ending. 'vi <file>' => :set fileformat=unix => wq
set -e
set -o verbose
echo "This command will be executed during 'release phase' on Heroku deployment."
if [ -z "$DefaultConnectionAutoMigrate" ]
then
    echo "DB migrations are not enabled."
else
    cd src/Webapp/ && dotnet ef database update
fi
