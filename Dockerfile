FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .
COPY nuget.config .
COPY AspNet.Core.RedisSession.Public/*.csproj ./AspNet.Core.RedisSession.Public/
COPY AspNet.Core.RedisSession.Repository/*.csproj ./AspNet.Core.RedisSession.Repository/
COPY AspNet.Core.RedisSession.Service/*.csproj ./AspNet.Core.RedisSession.Service/
COPY AspNet.Core.RedisSession.Web/*.csproj ./AspNet.Core.RedisSession.Web/
COPY AspNet.Core.RedisSession.UnitTest/*.csproj ./AspNet.Core.RedisSession.UnitTest/
RUN dotnet restore

COPY AspNet.Core.RedisSession.Public/. ./AspNet.Core.RedisSession.Public/
COPY AspNet.Core.RedisSession.Repository/. ./AspNet.Core.RedisSession.Repository/
COPY AspNet.Core.RedisSession.Service/. ./AspNet.Core.RedisSession.Service/
COPY AspNet.Core.RedisSession.Web/. ./AspNet.Core.RedisSession.Web/
COPY AspNet.Core.RedisSession.UnitTest/. ./AspNet.Core.RedisSession.UnitTest/
WORKDIR /src/AspNet.Core.RedisSession.UnitTest
RUN dotnet build && dotnet test
WORKDIR /src/AspNet.Core.RedisSession.Web
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "AspNet.Core.RedisSession.Web.dll" ]
