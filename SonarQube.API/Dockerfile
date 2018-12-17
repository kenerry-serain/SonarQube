FROM microsoft/dotnet:2.1-sdk AS build-env
RUN apt-get update && apt-get dist-upgrade -y && apt-get install -y openjdk-8-jre
COPY . ./
RUN dotnet restore SonarQube.API/SonarQube.API.csproj 
RUN dotnet restore SonarQube.API.Tests/SonarQube.API.Tests.csproj  
RUN dotnet build
RUN dotnet test SonarQube.API.Tests/SonarQube.API.Tests.csproj

RUN dotnet tool install -g dotnet-sonarscanner
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet sonarscanner begin /k:"SonarQube" /d:sonar.host.url="http://192.168.1.37:9000" /d:sonar.verbose=true  /v:1.0.0 /d:sonar.login="532677245a12d6ffcfea89a70e32834ab88deed8" /d:sonar.sourceEncoding="UTF-8"
RUN dotnet build ./SonarQube.sln
RUN dotnet sonarscanner end /d:sonar.login="532677245a12d6ffcfea89a70e32834ab88deed8"
