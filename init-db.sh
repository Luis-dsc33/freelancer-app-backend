#!/bin/bash
set -e

# Script de inicialización de PostgreSQL.
# Se ejecuta automáticamente SOLO la primera vez que se crea el contenedor.
# Ya no se crean bases separadas: todos los microservicios comparten
# la misma BD ($POSTGRES_DB) y se separan por schemas.

echo "✅ Base de datos '$POSTGRES_DB' lista. Los schemas se crean vía EF Core migrations."
