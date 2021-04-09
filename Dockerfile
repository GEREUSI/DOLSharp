FROM mono:6.10
COPY . /app
# TODO: Add configs
RUN /app/build.sh
CMD [ "mono",  "/app/Release/DOLServer.exe" ]



