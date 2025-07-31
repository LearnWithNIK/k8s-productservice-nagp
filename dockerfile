# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy .csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code
COPY . ./

# Publish the app
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published app from build stage
COPY --from=build /out .

# Listen on port 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "ProductService.dll"]
