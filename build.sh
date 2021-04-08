#!/bin/bash

cd /app
msbuild -t:restore
msbuild

chmod -R 777 /app/Debug
chmod -R 777 /app/build