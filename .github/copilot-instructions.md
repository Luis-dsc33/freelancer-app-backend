# Copilot Instructions

## Directrices del proyecto
- Durante este desarrollo, las funcionalidades exclusivas del Estudiante deben quedar preparadas con archivos base y comentarios detallados identificados como `ERJ - 15/11/2026`; las funcionalidades del Cliente deben implementarse completamente dentro de la arquitectura existente. Los Controllers no evalúan claims: delegan al Mediator; Application define ICurrentUserService y los handlers autorizan al usuario. 
- El catálogo de habilidades debe ser único y relacional, separando habilidades técnicas de transversales, distinguiendo requeridas/deseables en ofertas, y evitando relaciones EF/FK entre schemas de microservicios; el nivel de dominio pertenece al perfil del Estudiante y queda fuera de Marketplace.

## Reglas arquitectónicas
- Los Controllers no deben evaluar claims ni usar User.FindFirstValue; deben delegar directamente al Mediator.
- Application debe definir ICurrentUserService con UserId e IsAuthenticated.
- Infrastructure debe implementarlo mediante IHttpContextAccessor y registrarlo como Scoped.
- Los Handlers deben obtener y autorizar al usuario autenticado.