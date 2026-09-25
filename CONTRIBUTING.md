# Guía de Contribución — CodeBridge Backend

Gracias por contribuir al backend de CodeBridge. Este documento establece las reglas que **todo el equipo debe seguir** para mantener el código consistente y el historial de Git limpio.

---

## 1. Flujo de ramas (Git Flow simplificado)

```
main          ← Producción (protegida, solo merge via PR aprobado)
  └── QA      ← Pruebas de integración
       └── DEV    ← Desarrollo activo
            └── feature/nombre-corto   ← Tu rama de trabajo
```

### Reglas:
- **Nunca** hagas push directo a `main`, `QA` o `DEV`.
- Crea tu rama desde `DEV`: `git checkout -b feature/nombre-corto`
- Abre un **Pull Request** hacia `DEV` (o la rama que corresponda).
- El PR debe ser **aprobado por al menos 1 revisor** antes del merge.
- El pipeline de CI debe pasar con ✅ antes del merge.

---

## 2. Conventional Commits (obligatorio)

Todos los mensajes de commit deben seguir el formato [Conventional Commits](https://www.conventionalcommits.org/):

```
<tipo>(alcance opcional): descripción corta en minúsculas
```

### Tipos permitidos:

| Tipo       | Cuándo usarlo                                      | Ejemplo                                              |
|------------|-----------------------------------------------------|------------------------------------------------------|
| `feat`     | Nueva funcionalidad                                 | `feat(usuarios): agrega endpoint de registro HU-01`  |
| `fix`      | Corrección de bug                                   | `fix(auth): corrige validación de token expirado`     |
| `refactor` | Reestructuración sin cambio funcional               | `refactor(marketplace): extrae lógica a Handler`      |
| `docs`     | Solo documentación                                  | `docs: actualiza README con instrucciones de deploy`  |
| `ci`       | Cambios en pipeline o configuración de CI           | `ci: agrega job de pruebas unitarias`                 |
| `test`     | Agregar o corregir pruebas                          | `test(usuarios): agrega tests para RegistroValidator` |
| `chore`    | Tareas de mantenimiento (dependencias, configs)     | `chore: actualiza paquetes NuGet`                     |

### Reglas del mensaje:
- La descripción va en **minúsculas** y en **imperativo** ("agrega", no "agregado" ni "Agrega").
- Máximo **72 caracteres** en la primera línea.
- Si necesitas más detalle, deja una línea en blanco y escribe el cuerpo.

---

## 3. Versionado Semántico (SemVer)

El proyecto usa versionado `vX.Y.Z` basado en [Semantic Versioning](https://semver.org/):

| Componente | Cuándo se incrementa                          | Ejemplo          |
|------------|-----------------------------------------------|------------------|
| **X** (Major) | Cambio que rompe compatibilidad (breaking change) | `v2.0.0`     |
| **Y** (Minor) | Nueva funcionalidad retrocompatible            | `v1.3.0`        |
| **Z** (Patch) | Corrección de bug                              | `v1.3.1`        |

Los tags de versión se crean en `main` después de un merge exitoso:

```bash
git tag -a v1.2.0 -m "feat: búsqueda avanzada de ofertas"
git push origin v1.2.0
```

---

## 4. Arquitectura (Clean Architecture + CQRS)

Cada microservicio tiene 4 capas. Respeta siempre la **regla de dependencia**:

```
Domain ← no referencia a nadie
Application ← referencia solo a Domain
Infrastructure ← referencia a Application y Domain
Api ← referencia a Application e Infrastructure
```

> ⚠️ Si un archivo de Domain necesita un `using` de Infrastructure, **está mal**.

### Patrón obligatorio:
- **Escritura** → `Command` + `Handler` (en `Application/Commands/`)
- **Lectura** → `Query` + `Handler` (en `Application/Queries/`)
- **Controller** → Solo `_mediator.Send()`, sin lógica de negocio

---

## 5. Checklist antes de abrir un PR

- [ ] El commit sigue Conventional Commits
- [ ] La lógica de negocio está en el Handler, no en el Controller
- [ ] No hay `using` cruzados entre capas prohibidas
- [ ] Las migraciones usan `HasDefaultSchema("<servicio>")`
- [ ] Probé localmente con Swagger
- [ ] Agregué pruebas unitarias si aplica
- [ ] El pipeline de CI pasa en verde

---

## 6. Revisión de código

- Todo PR requiere **mínimo 1 aprobación** de un revisor asignado (ver `CODEOWNERS`).
- Si se suben commits nuevos después de la aprobación, **la aprobación se invalida** y se debe revisar de nuevo.
- Los comentarios del revisor se resuelven antes del merge.
