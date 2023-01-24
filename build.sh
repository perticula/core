#!/bin/bash

dotnet clean
dotnet restore
dotnet build --no-restore
dotnet test --no-build
