
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build


WORKDIR /app


COPY Behoof.csproj ./ 


RUN dotnet restore "Behoof.csproj"


COPY . .


RUN dotnet publish "Behoof.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final


WORKDIR /app


COPY --from=build /app/publish .


EXPOSE 80


# 5. Запускаем приложение
ENTRYPOINT ["dotnet", "Behoof.dll"]
