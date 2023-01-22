#!/bin/bash

dotnet clean src/
dotnet restore src/
dotnet build src/
