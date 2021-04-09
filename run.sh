#!/bin/sh

mkdir -p /app/Release/config
sed "s/{{password}}/${MYSQL_PASSWORD}/g;s/{{db}}/${MYSQL_DATABASE}/g;s/{{user}}/${MYSQL_USER}/g" /app/serverconfig.xml > /app/Release/config/serverconfig.xml

mono /app/Release/DOLServer.exe