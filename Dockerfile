FROM mono:6.10
COPY . /app
# TODO: Add configs
# TODO: Build Release Version
RUN /app/build.sh
CMD [ "mono",  "/app/Debug/DOLServer.exe" ]



