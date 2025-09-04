FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Todo.Api.App/Todo.Api.App.csproj", "src/Todo.Api.App/"]
COPY ["src/Todo.Api.Domain/Todo.Api.Domain.csproj", "src/Todo.Api.Domain/"]
COPY ["src/Todo.Api.Infra/Todo.Api.Infra.csproj", "src/Todo.Api.Infra/"]


RUN dotnet restore "src/Todo.Api.App/Todo.Api.App.csproj"
COPY . .
WORKDIR "/src/src/Todo.Api.App"
RUN dotnet build "Todo.Api.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Todo.Api.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
ENTRYPOINT ["dotnet", "Todo.Api.App.dll"]