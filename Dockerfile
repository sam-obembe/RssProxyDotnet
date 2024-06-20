FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /rssProxy
COPY . .
RUN dotnet clean
RUN dotnet restore
RUN dotnet publish -c Release -o api


FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
WORKDIR /api
RUN ls -a
COPY --from=build /rssProxy/api/* .
CMD ["./RssProxyDotnet"]