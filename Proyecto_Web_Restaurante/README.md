# Proyecto Web Restaurante

He preparado la estructura básica de carpetas recomendada para un proyecto moderno (usando el estándar de Next.js/React). 

## Estructura de carpetas creada:

*   **`/public`**: Aquí guardaremos todas las imágenes estáticas (el logo del restaurante, fotos de los platos, iconos, etc.). Es público y accesible directamente desde el navegador.
*   **`/src`**: El corazón del código. Todas las piezas de nuestra web irán aquí dentro para mantenerlo organizado.
    *   **`/src/app`**: Aquí irán las "páginas" o rutas de nuestra web. Por ejemplo, aquí crearemos `page.tsx` (la página de inicio) y la carpeta `/reservas` (para la página del calendario).
    *   **`/src/components`**: Pequeños bloques visuales que reutilizaremos. Ejemplo: el `Button` de reservar, la `Card` de cada plato, el menú de navegación (`Navbar`), o el pie de página (`Footer`).
    *   **`/src/lib`**: Lógica pura, utilidades extra, o la configuración para conectarnos a la base de datos (Supabase, por ejemplo).

---

### ⚠️ ¡Aviso Importante!
He intentado instalar el proyecto completo con **Next.js** de forma automática, pero el sistema me indica que **no tienes Node.js instalado** en tu ordenador. Node.js es imprescindible para programar con React/Next.js hoy en día.

**Para poder continuar con el desarrollo real (arrancar la web y verla):**
1. Ve a la web oficial: [nodejs.org](https://nodejs.org/)
2. Descarga e instala la versión recomendada (LTS).
3. Una vez instalado, reinicia tu editor de código.
4. Avísame por aquí diciendo "¡Ya está instalado Node!" y ejecutaré los comandos mágicos para inicializar todo el código al instante.

¡Dime qué te parece la estructura y si vas a poder instalar Node.js para que le demos caña!
