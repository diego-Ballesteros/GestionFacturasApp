# Etapa 1: Compilación de la aplicación (SDK de .NET)
# Usamos la imagen oficial de .NET SDK correspondiente a tu proyecto (.NET 8).
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar el archivo .csproj y restaurar las dependencias primero.
# Asegúrate que 'Facturacion.API.csproj' sea el nombre correcto y esté en el contexto.
COPY Facturacion.API.csproj ./
RUN dotnet restore

# Copiar el resto del código fuente de la aplicación al contenedor.
# Esto copiará todo desde la carpeta donde está el Dockerfile (y sus subdirectorios) a /app en el contenedor.
COPY . ./

# Publicar la aplicación en modo Release. La salida irá a la carpeta 'out'.
# Especifica el archivo .csproj para ser explícito.
RUN dotnet publish Facturacion.API.csproj -c Release -o out

# Etapa 2: Construir la imagen final de runtime (ASP.NET Runtime)
# Usamos la imagen de runtime de ASP.NET, que es más ligera que la del SDK.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime-env
WORKDIR /app

# Copiar la salida publicada de la etapa de compilación ('build-env') a la imagen final.
COPY --from=build-env /app/out .

# Comando para iniciar la aplicación cuando el contenedor se ejecute.
ENTRYPOINT ["dotnet", "Facturacion.API.dll"]