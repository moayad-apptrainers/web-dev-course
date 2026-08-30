# Tells Railway exactly how to build the C# API in backend/.
# Stage 1 — build and publish with the .NET SDK.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the project file first and restore NuGet packages.
# (Done separately so Docker can cache this step between deploys.)
COPY backend/Session19_Api.csproj ./
RUN dotnet restore

# Now copy the rest of the source and publish a release build.
COPY backend/ ./
RUN dotnet publish -c Release -o /app --no-restore

# Stage 2 — the runtime image that actually gets deployed (much smaller).
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./
CMD ["dotnet", "Session19_Api.dll"]
