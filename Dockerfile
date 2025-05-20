FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime-env
WORKDIR /app

COPY --from=build-env /app/out .

# Variable de entorno para que Kestrel escuche en todos los interfaces de red
# en el puerto que Railway.app proveer� a trav�s de la variable de entorno PORT.
# Si no se establece ASPNETCORE_URLS, Kestrel por defecto escucha en http://*:8080 y https://*:8081
# Railway necesita que tu app escuche en el puerto que �l asigna.
# ENV ASPNETCORE_URLS=http://+:80 
# No es estrictamente necesario EXPOSE aqu� para Railway, ya que �l mapear� los puertos.
# EXPOSE 80 
# EXPOSE 443 # Si fueras a manejar HTTPS dentro del contenedor, lo cual Railway tambi�n puede hacer externamente.

# Comando para iniciar la aplicaci�n cuando el contenedor se ejecute.
# Reemplaza 'Invoicing.API.dll' con el nombre exacto de la DLL de tu proyecto API.
# (Normalmente es NombreDelProyecto.dll)
ENTRYPOINT ["dotnet", "Invoicing.API.dll"]