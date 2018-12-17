FROM microsoft/dotnet:2.1-sdk AS build-env
RUN apt-get update && apt-get dist-upgrade -y && apt-get install -y openjdk-8-jre
COPY . ./
RUN dotnet restore SonarQube.API/SonarQube.API.csproj 
RUN dotnet restore SonarQube.API.Tests/SonarQube.API.Tests.csproj  
RUN dotnet build
RUN dotnet test SonarQube.API.Tests/SonarQube.API.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

RUN dotnet tool install -g dotnet-sonarscanner
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet sonarscanner begin /k:"SonarQube" /d:sonar.host.url="http://192.168.1.37:9001" /d:sonar.verbose=true  /v:1.0.0 /d:sonar.login="77634e39b317175573ef5f03a277e52841f4a3a4" /d:sonar.cs.opencover.reportsPaths="SonarQube.API.Tests\coverage.opencover.xml" /d:sonar.coverage.exclusions="**Tests*.cs"
RUN dotnet build
RUN dotnet sonarscanner end /d:sonar.login="77634e39b317175573ef5f03a277e52841f4a3a4"
