#!/bin/bash

git pull
git submodule update --init --recursive

dotnet restore
screen -dmS metasite bash ./start.sh
