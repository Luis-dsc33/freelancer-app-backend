using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using Marketplace.Domain.Entities;

namespace Marketplace.Infrastructure.Persistence;

internal static class CatalogoSkillsInicial
{
    private static readonly string[] NombresCategorias =
    [
        "Lenguajes de programación", "Desarrollo frontend", "Desarrollo backend y APIs", "Bases de datos",
        "Desarrollo móvil y multiplataforma", "Inteligencia artificial y ciencia de datos", "Cloud, DevOps y despliegue",
        "Ciberseguridad y redes", "Diseño UI/UX y diseño gráfico", "Producción audiovisual y multimedia",
        "Marketing digital y comunicación", "Ofimática, gestión y análisis empresarial", "Testing y calidad de software",
        "Hardware, soporte e IoT", "Habilidades profesionales y transversales"
    ];

    private static readonly string[][] NombresSkills =
    [
        ["C", "C++", "C#", "Java", "Python", "JavaScript", "TypeScript", "PHP", "Ruby", "Go", "Rust", "Kotlin", "Swift", "Dart", "R", "MATLAB", "Visual Basic .NET", "SQL", "Bash", "PowerShell"],
        ["HTML5", "CSS3", "Sass / SCSS", "Tailwind CSS", "Bootstrap", "React", "Next.js", "Angular", "Vue.js", "Nuxt.js", "Svelte", "Astro", "jQuery", "Redux", "Zustand", "Responsive Design", "Progressive Web Apps (PWA)", "Web Accessibility", "Web Components"],
        ["ASP.NET Core", "ASP.NET MVC", ".NET", "Node.js", "Express.js", "NestJS", "Laravel", "Django", "Flask", "FastAPI", "Spring Boot", "Ruby on Rails", "REST API", "GraphQL", "gRPC", "Microservicios", "WebSockets", "Autenticación JWT", "OAuth 2.0", "Entity Framework Core", "Dapper"],
        ["SQL Server", "MySQL", "PostgreSQL", "MariaDB", "SQLite", "Oracle Database", "MongoDB", "Firebase Firestore", "Redis", "Supabase", "Firebase", "PL/SQL", "Diseño de bases de datos", "Modelado entidad-relación", "Normalización de bases de datos", "Optimización de consultas SQL"],
        ["Android Development", "iOS Development", "Flutter", "React Native", ".NET MAUI", "Kotlin Multiplatform", "SwiftUI", "Jetpack Compose", "Ionic", "Xamarin", "Desarrollo de aplicaciones híbridas", "Integración de APIs móviles", "Diseño de interfaces móviles"],
        ["Machine Learning", "Deep Learning", "Redes neuronales", "Procesamiento de lenguaje natural (NLP)", "Computer Vision", "Generative AI", "Prompt Engineering", "Modelos de lenguaje (LLM)", "Inteligencia artificial generativa", "Análisis de datos", "Ciencia de datos", "Minería de datos", "Data Visualization", "Big Data", "Estadística", "Análisis predictivo", "Regresión", "Clasificación", "Clustering", "Scikit-learn", "TensorFlow", "PyTorch", "Pandas", "NumPy", "OpenCV", "Power BI", "Tableau", "Jupyter Notebook"],
        ["Git", "GitHub", "GitLab", "GitHub Actions", "Docker", "Kubernetes", "CI/CD", "Linux", "Windows Server", "AWS", "Microsoft Azure", "Google Cloud Platform", "Vercel", "Railway", "Netlify", "Nginx", "Apache", "Administración de servidores", "Configuración de entornos", "Despliegue de aplicaciones", "Monitoreo de aplicaciones", "Infraestructura como código (IaC)"],
        ["Fundamentos de ciberseguridad", "Ethical Hacking", "Pentesting", "Seguridad de aplicaciones web", "OWASP Top 10", "Análisis de vulnerabilidades", "Seguridad de APIs", "Criptografía", "Seguridad de bases de datos", "Gestión de identidades y accesos", "Active Directory", "Redes TCP/IP", "Configuración de routers", "Configuración de switches", "Administración de redes", "Firewalls", "VPN", "Wireshark", "Kali Linux", "Seguridad informática"],
        ["UI Design", "UX Design", "UX Research", "Diseño de interfaces", "Diseño de experiencia de usuario", "Prototipado", "Wireframing", "Design Systems", "Figma", "Adobe XD", "Sketch", "Adobe Photoshop", "Adobe Illustrator", "Adobe InDesign", "Canva", "Diseño editorial", "Diseño de identidad visual", "Branding", "Diseño de logotipos", "Diseño de material publicitario", "Diseño de presentaciones", "Diseño responsivo"],
        ["Edición de video", "Producción audiovisual", "Fotografía", "Edición fotográfica", "Animación 2D", "Animación 3D", "Motion Graphics", "Modelado 3D", "Renderizado 3D", "Blender", "Autodesk Maya", "Cinema 4D", "Adobe Premiere Pro", "Adobe After Effects", "DaVinci Resolve", "Adobe Audition", "Producción musical", "Diseño sonoro", "Guionismo", "Storytelling", "Creación de contenido digital", "Ilustración digital"],
        ["Marketing digital", "SEO", "SEM", "Social Media Management", "Community Management", "Marketing de contenidos", "Copywriting", "Email Marketing", "Google Ads", "Meta Ads", "Google Analytics", "Estrategia de marca", "Investigación de mercado", "Gestión de campañas publicitarias", "Marketing estratégico", "Redacción de contenidos", "Comunicación digital", "Gestión de redes sociales", "E-commerce", "WordPress", "Shopify"],
        ["Microsoft Excel", "Excel avanzado", "Microsoft Word", "Microsoft PowerPoint", "Microsoft Access", "Google Sheets", "Google Workspace", "Power Query", "Power Pivot", "Gestión de proyectos", "Scrum", "Kanban", "Agile", "Jira", "Trello", "Notion", "Análisis de requerimientos", "Documentación técnica", "Modelado de procesos", "UML", "BPMN", "Levantamiento de requerimientos", "Gestión de equipos", "Elaboración de reportes"],
        ["Pruebas manuales", "Pruebas automatizadas", "Testing funcional", "Testing de integración", "Testing de APIs", "Testing de rendimiento", "Testing de seguridad", "Selenium", "Cypress", "Playwright", "Postman", "Swagger / OpenAPI", "JUnit", "NUnit", "xUnit", "Jest", "Vitest", "Control de calidad de software", "Depuración de código", "Análisis estático de código"],
        ["Soporte técnico", "Mantenimiento preventivo", "Mantenimiento correctivo", "Ensamblaje de computadoras", "Diagnóstico de hardware", "Arduino", "ESP32", "Raspberry Pi", "Internet of Things (IoT)", "Sistemas embebidos", "Electrónica básica", "Programación de microcontroladores", "Sensores y actuadores", "Automatización", "Configuración de dispositivos", "Instalación de sistemas operativos"],
        ["Comunicación efectiva", "Trabajo en equipo", "Pensamiento crítico", "Resolución de problemas", "Creatividad", "Adaptabilidad", "Organización", "Gestión del tiempo", "Liderazgo", "Autonomía", "Pensamiento analítico", "Atención al detalle", "Aprendizaje autónomo", "Negociación", "Gestión de conflictos", "Comunicación escrita", "Comunicación oral", "Orientación a resultados"]
    ];

    public static IEnumerable<SkillCategory> Categories => NombresCategorias.Select((nombre, index) => new SkillCategory
    {
        Id = CategoryId(index), Nombre = nombre, Descripcion = $"Habilidades de {nombre.ToLowerInvariant()}", Activo = true
    });

    public static IEnumerable<Skill> Skills => NombresSkills.SelectMany((skills, categoryIndex) => skills.Select(nombre => new Skill
    {
        Id = SkillId(categoryIndex, nombre), Nombre = nombre, NombreNormalizado = Normalizar(nombre),
        Descripcion = nombre, CategoriaId = CategoryId(categoryIndex), Activo = true
    }));

    private static Guid CategoryId(int index) => Guid.Parse($"70000000-0000-0000-0000-{index + 1:000000000000}");
    private static Guid SkillId(int categoryIndex, string name)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes($"codebridge-skill:{categoryIndex}:{name}"));
        return new Guid(hash[..16]);
    }

    private static string Normalizar(string value) => string.Concat(value.Normalize(NormalizationForm.FormD).Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)).Normalize(NormalizationForm.FormC).ToUpperInvariant();
}
