# Stage 1: Build the React app
FROM node:18 AS build

WORKDIR /app

# Copy package.json and package-lock.json to install dependencies
COPY package.json package-lock.json ./

# Install the app dependencies
RUN npm install

# Copy the rest of the project files
COPY . .

# Build the React app for production
RUN npm run build

# Stage 2: Serve the app with Nginx
FROM nginx:alpine

# Copy the built app files to the directory where Nginx looks for static files
COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80 (default port for Nginx)
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]