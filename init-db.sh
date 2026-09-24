#!/bin/bash
set -e

# Script de inicialización de PostgreSQL.
# Se ejecuta automáticamente SOLO la primera vez que se crea el contenedor.
# Crea las bases de datos individuales para cada microservicio.

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE DATABASE freelancer_usuarios;
    CREATE DATABASE freelancer_admin;
    CREATE DATABASE freelancer_chat;
    CREATE DATABASE freelancer_marketplace;
    CREATE DATABASE freelancer_resenas;
EOSQL

echo "✅ Todas las bases de datos creadas correctamente."
