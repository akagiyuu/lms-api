FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/Shared/ ./Shared/
COPY src/Course.Service/ ./Course.Service/
RUN dotnet restore Course.Service/Course.Service.csproj
RUN dotnet publish Course.Service/Course.Service.csproj -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Course.Service.dll"]
