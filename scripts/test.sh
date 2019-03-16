#!/bin/sh
#Line ending in this fie must be unix line ending. We unsure this via setting in '.gitattribute' file.
set -e
set -o verbose
echo "This command will be execute all tests for the repository"
cd "test/Webapp.Tests" && dotnet test
