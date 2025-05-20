# Stage 1: Build the React client app
FROM node:18-alpine AS client-build-env
WORKDIR /app/client-app

# Copy package.json and package-lock.json (or yarn.lock if you use Yarn)
COPY client-app/package.json ./
COPY client-app/package-lock.json ./
# If using yarn:
# COPY client-app/yarn.lock ./

# Install dependencies
RUN npm install
# If using yarn:
# RUN yarn install

# Copy the rest of the client app source code
COPY client-app/ ./

# Build the client app
RUN npm run build

# Stage 2: Build the .NET backend app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS backend-build-env
# Note: Ensure '9.0' matches your project's .NET version.
# If your project uses .NET 8, change to mcr.microsoft.com/dotnet/sdk:8.0, etc.
WORKDIR /src

# Copy .csproj and restore dependencies
COPY ["DataTransform.csproj", "./"]
RUN dotnet restore "./DataTransform.csproj"

# Copy all other source files
COPY . .
WORKDIR "/src/."
RUN dotnet build "DataTransform.csproj" -c Release -o /app/build

# Publish the .NET app
FROM backend-build-env AS publish
RUN dotnet publish "DataTransform.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final image - Serve the .NET app and React client
FROM mcr.microsoft.com/dotnet/aspnet:9.0
# Note: Ensure '9.0' matches your project's .NET version and the SDK version used above.
WORKDIR /app

# Copy built .NET backend from the publish stage
COPY --from=publish /app/publish .

# Copy built React client app from the client-build-env stage
# The React build output is typically in /app/client-app/build
# We copy it to wwwroot in the final image, which ASP.NET Core serves by default
COPY --from=client-build-env /app/client-app/build ./wwwroot

# Expose the port the app runs on (ASP.NET Core default is 8080 if ASPNETCORE_URLS is not set)
EXPOSE 8080
# If your app is configured to run on port 80 internally, use EXPOSE 80

ENTRYPOINT ["dotnet", "DataTransform.dll"]