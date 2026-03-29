#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Docker
WORKDIR /app
EXPOSE 80

# Build libgdiplus with pango
RUN apt-get update && apt-get install  -y --allow-unauthenticated \
     libgif-dev autoconf libtool automake build-essential gettext libglib2.0-dev libcairo2-dev libtiff-dev libexif-dev
RUN apt-get update && apt-get install -y libpango1.0-dev

RUN apt-get install -y --allow-unauthenticated git
RUN git clone https://github.com/mono/libgdiplus.git /libgdiplus
WORKDIR /libgdiplus
RUN ./autogen.sh --with-pango --prefix=/usr
RUN make
RUN make install

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Dedalo.API/Dedalo.API.csproj", "Dedalo.API/"]

COPY ["Dedalo.Application/Dedalo.Application.csproj", "Dedalo.Application/"]
COPY ["Dedalo.Domain/Dedalo.Domain.csproj", "Dedalo.Domain/"]
COPY ["Dedalo.DTO/Dedalo.DTO.csproj", "Dedalo.DTO/"]
COPY ["Dedalo.Infra.Interfaces/Dedalo.Infra.Interfaces.csproj", "Dedalo.Infra.Interfaces/"]
COPY ["Dedalo.Infra/Dedalo.Infra.csproj", "Dedalo.Infra/"]
RUN dotnet restore "Dedalo.API/Dedalo.API.csproj"
COPY . .
WORKDIR "/src/Dedalo.API"
RUN dotnet build "Dedalo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dedalo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dedalo.API.dll"]
