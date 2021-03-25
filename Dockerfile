FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

WORKDIR /app


COPY ./BeautyAtHome/BeautyAtHome.csproj ./BeautyAtHome/
COPY ./ApplicationCore/ApplicationCore.csproj ./ApplicationCore/
COPY ./Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY ./BeautyAtHome.sln .

RUN dotnet restore


COPY . ./

WORKDIR /app


RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime

WORKDIR /app

COPY --from=build /app/out ./

EXPOSE 8080

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "BeautyAtHome.dll"]