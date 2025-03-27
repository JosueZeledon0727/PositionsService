# Etapa 1: Construir la app de React
FROM node:18 AS build

# Directorio de trabajo dentro del contenedor
WORKDIR /app

# Copiar package.json y package-lock.json para instalar dependencias
COPY package.json package-lock.json ./

# Instalar las dependencias de la app
RUN npm install

# Copiar el resto de los archivos del proyecto
COPY . .

# Construir la app de React para producción
RUN npm run build

# Etapa 2: Servir la app con Nginx
FROM nginx:alpine

# Copiar los archivos de la app construida al directorio donde Nginx busca los archivos estáticos
COPY --from=build /app/build /usr/share/nginx/html

# Exponer el puerto 80 (puerto por defecto de Nginx)
EXPOSE 80

# Ejecutar Nginx en el contenedor
CMD ["nginx", "-g", "daemon off;"]