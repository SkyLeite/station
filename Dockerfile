FROM mcr.microsoft.com/dotnet/core/sdk
WORKDIR /app
COPY . /app
RUN dotnet restore --verbosity detailed
ENTRYPOINT ["dotnet", "run"]
