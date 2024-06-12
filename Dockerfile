FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out
#RUN dotnet tool install --global dotnet-ef
RUN dotnet tool install --tool-path /app/tools dotnet-ef


# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
#RUN dotnet tool restore
# Copy the EF tools to the runtime environment
COPY --from=build-env /app/tools /app/tools
#COPY --from=build-env /root/.dotnet/tools /root/.dotnet/tools
#ENV PATH="$PATH:/root/.dotnet/tools"
ENV PATH="$PATH:/app/tools"

COPY --from=build-env /App/out .
COPY entrypoint.sh .
EXPOSE 8080

RUN chown -R 1001:0 /App && chmod -R g+rwX /App
RUN chown -R 1001:0 /app/tools && chmod -R g+rwX /app/tools

USER 1001


ENTRYPOINT ["./entrypoint.sh"]
#ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]
