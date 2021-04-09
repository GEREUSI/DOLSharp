FROM mono:6.10
COPY . /app
RUN /app/build.sh
CMD [ "/app/run.sh" ]