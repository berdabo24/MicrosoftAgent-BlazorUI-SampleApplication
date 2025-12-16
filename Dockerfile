# STAGE 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
# (Replace 'MyProject.csproj' with your actual project filename)
COPY ["BlazorApp2/BlazorApp2.csproj", "BlazorApp2/"]
RUN dotnet restore "BlazorApp2/BlazorApp2.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/MBlazorApp2"

# Build and Publish the app
RUN dotnet build "BlazorApp2.csproj" -c Release -o /app/build
RUN dotnet publish "BlazorApp2.csproj" -c Release -o /app/publish

# STAGE 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port Render uses (Render sets the PORT env var automatically)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Start the app
# (Replace 'MyProject.dll' with the name of your built .dll)
ENTRYPOINT ["dotnet", "BlazorApp2.dll"]