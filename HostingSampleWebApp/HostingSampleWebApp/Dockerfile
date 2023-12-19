# Use the official .NET SDK image as a build environment.
# This image includes the .NET SDK and all the tools needed to build and publish your app.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory to /app
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . ./

# Build the app
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image as the base image for the final image.
# This image includes the .NET runtime and all the libraries needed to run your app.
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Set the working directory to /app
WORKDIR /app

# Copy the published app from the build environment to the final image
COPY --from=build-env /app/out .

# Expose port 80 to the outside world
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "HostingSampleWebApp.dll"]