## Descripción

<!-- Describe brevemente qué hace este PR y por qué es necesario -->

## Módulo / Microservicio

<!-- Indica a qué microservicio(s) afecta: Usuarios, Marketplace, Chat, Reseñas, Admin -->

- [ ] Usuarios
- [ ] Marketplace
- [ ] Chat
- [ ] Reseñas
- [ ] Admin

## Ticket / Issue

<!-- Enlaza el issue o historia de usuario relacionada -->

Closes #

## Tipo de cambio

- [ ] `feat`: Nueva funcionalidad
- [ ] `fix`: Corrección de bug
- [ ] `refactor`: Refactorización (sin cambio funcional)
- [ ] `docs`: Documentación
- [ ] `ci`: Cambios en pipeline CI/CD
- [ ] `test`: Agregar o corregir pruebas

## Checklist de calidad

- [ ] Mi código sigue la **Guía de Desarrollo CodeBridge** (Clean Architecture, CQRS, convenciones de nombres)
- [ ] La lógica de negocio está en el **Handler**, no en el Controller
- [ ] No hay `using` de Infrastructure o Api dentro de Domain o Application
- [ ] Los Controllers solo llaman a `_mediator.Send()` sin `if` de reglas de negocio
- [ ] Agregué/actualicé las pruebas unitarias correspondientes
- [ ] Las migraciones usan `HasDefaultSchema("<servicio>")` y apuntan a `db_codebridge`
- [ ] El commit sigue el formato **Conventional Commits** (`feat:`, `fix:`, etc.)
- [ ] Probé localmente con Swagger y los endpoints responden correctamente

## Evidencia

<!-- Pega capturas de pantalla o GIFs que demuestren el funcionamiento -->

