#!/bin/bash

cd /app
msbuild -p:configuration=Release -t:restore
msbuild -p:configuration=Release

chmod -R 777 /app/Release
chmod -R 777 /app/build