FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out



# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ARG DATABASE_URL_ARG
ENV DATABASE_URL=$DATABASE_URL_ARG
ARG AWS
WORKDIR /app
COPY --from=build-env /app/out .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Salmpled.dll
RUN apt-get update && apt-get install -y ffmpeg
# ENTRYPOINT ["dotnet", "Salmpled.dll"]

# ENV ASPNETCORE_URLS http://+:5000
# EXPOSE 5000