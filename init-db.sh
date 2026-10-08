#!/bin/bash
set -e

# Script de inicialización de PostgreSQL.
# Se ejecuta automáticamente SOLO la primera vez que se crea el contenedor.
# PostgreSQL crea POSTGRES_DB. Los servicios comparten esa base y usan schemas.

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE SCHEMA IF NOT EXISTS usuarios;
    CREATE SCHEMA IF NOT EXISTS admin;
    CREATE SCHEMA IF NOT EXISTS chat;
    CREATE SCHEMA IF NOT EXISTS marketplace;
    CREATE SCHEMA IF NOT EXISTS resenas;
EOSQL

echo "✅ Schemas creados en la base compartida."