FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS build

RUN apk add git \
&& git clone https://github.com/saviosg/Projeto-UNINOVE.git /projeto

RUN cd /projeto/Axulus.API \
&& dotnet publish -c release -o /app \
&& cp Application.db /app/

FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine

WORKDIR /app

COPY --from=build /app ./

CMD ASPNETCORE_HTTPS_PORT=443 ASPNETCORE_URLS=http://*:$PORT ./Axulus.API

