FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/Shared/ ./Shared/
COPY src/Student.Service/ ./Student.Service/
RUN dotnet restore Student.Service/Student.Service.csproj
RUN dotnet publish Student.Service/Student.Service.csproj -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Student.Service.dll"]
