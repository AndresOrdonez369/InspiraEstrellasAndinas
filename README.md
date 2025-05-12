# Inspira Estrellas Andinas ???

[![Unity Version](https://img.shields.io/badge/Unity-2022.3%2B%20LTS-blueviolet)](https://unity.com/releases/editor/archive) [![XR Interaction Toolkit](https://img.shields.io/badge/XR%20Interaction%20Toolkit-3.1.1-blue)](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.1/manual/index.html) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) <!-- ¡Asegúrate de añadir un archivo LICENSE.txt con la licencia MIT! -->

Un juego educativo en Realidad Virtual (VR) desarrollado en Unity, diseñado para inspirar a niñas y jóvenes mujeres a explorar el fascinante mundo STEM (Ciencia, Tecnología, Ingeniería y Matemáticas) y la tecnología a través de historias interactivas y desafíos estimulantes.
![Logo Inspira Estrellas Andinas](image/estrellasandinas.png)
---

## Tabla de Contenidos

*   [?? Sobre el Proyecto](#-sobre-el-proyecto)
    *   [?? Core Gameplay](#-core-gameplay)
    *   [??? Construido Con](#?-construido-con)
*   [?? Empezando](#-empezando)
    *   [?? Prerrequisitos](#-prerrequisitos)
    *   [?? Instalación](#?-instalación)
*   [?? Estructura del Proyecto](#-estructura-del-proyecto)
    *   [?? Carpetas Principales](#-carpetas-principales)
    *   [?? Escenas Principales](#-escenas-principales)
*   [?? Paquetes de Unity Clave](#-paquetes-de-unity-clave)
*   [?? Uso](#-uso)
*   [?? Contribuciones](#-contribuciones)
*   [?? Licencia](#-licencia)
*   [?? Contacto](#-contacto)
*   [?? Agradecimientos](#-agradecimientos)

---

## ?? Sobre el Proyecto

**Inspira Estrellas Andinas** sumerge a la jugadora en una aventura narrativa VR donde, como una joven exploradora, descubre entornos virtuales interactivos, conoce las historias de mujeres inspiradoras en campos STEM y resuelve desafíos basados en conceptos científicos y tecnológicos. El objetivo es fomentar la curiosidad, la confianza y el interés por estas áreas de una manera lúdica y significativa.

### ?? Core Gameplay

El juego se basa en los siguientes pilares de interacción:

1.  **Explorar:** Navegar por entornos VR interactivos como una plataforma de lanzamiento y un laboratorio espacial.
2.  **Interactuar:** Activar hologramas, escuchar narrativas, manipular consolas y objetos virtuales.
3.  **Aprender:** Absorber información sobre figuras históricas (como Sally Ride, Ada Lovelace, Maria Augusta Urrutia), conceptos STEM presentados de forma visual y práctica.
4.  **Resolver:** Completar desafíos interactivos aplicando conceptos aprendidos (ej. programar distribución de agua).
5.  **Sentir:** Experimentar la emoción del descubrimiento, la satisfacción de resolver problemas y la inspiración de las historias.

### ??? Construido Con

Este proyecto utiliza las siguientes tecnologías y herramientas principales:

*   **[Unity Engine (6000.0.33f1)](https://unity.com/)**: Plataforma de desarrollo principal.
*   **[XR Interaction Toolkit (v3.1.1)](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.1/manual/index.html)**: Para gestionar las interacciones VR (agarre, UI, locomoción).
*   **[Universal Render Pipeline (URP)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/)**: Para gráficos optimizados y flexibles.
*   **[OpenXR Plugin](https://docs.unity3d.com/Packages/com.unity.xr.openxr@latest/)**: Para compatibilidad con diversos dispositivos VR (Meta Quest, etc.).
*   **[Unity Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/)**: Para la gestión de entradas.
*   **Git & Git LFS**: Para el control de versiones y manejo de archivos grandes.
*   _(GDD)_ Software 3D (Blender), Software de Texturizado (Substance Painter/Designer, Krita/Photoshop), DAW (Reaper, Pro Tools, Audacity).

---

## ?? Empezando

Sigue estos pasos para obtener una copia local del proyecto y empezar a trabajar.

### ?? Prerrequisitos

Asegúrate de tener instalado el siguiente software:

*   **Git:** Necesario para clonar el repositorio. [Descargar Git](https://git-scm.com/downloads)
*   **Git LFS:** Esencial para manejar los archivos grandes del proyecto.
    ```bash
    # Instalar Git LFS (solo una vez por sistema)
    git lfs install --system
    ```
*   **Unity Hub:** Para gestionar las instalaciones de Unity Editor. [Descargar Unity Hub](https://unity.com/download)
*   **Unity Editor (Versión 2022.3 LTS o compatible):** La versión de Unity utilizada para el desarrollo. Se recomienda usar la misma versión o una compatible superior que soporte los paquetes listados. Verifica el archivo `ProjectSettings/ProjectVersion.txt` en el repositorio para la versión exacta utilizada.

### ?? Instalación

1.  **Clona el repositorio:**
    ```bash
    git clone https://github.com/AndresOrdonez369/InspiraEstrellasAndinas.git
    ```
2.  **Navega al directorio del proyecto:**
    ```bash
    cd InspiraEstrellasAndinas
    ```
    *(Git LFS debería descargar automáticamente los archivos grandes durante el clonado si está instalado correctamente. Si tienes problemas, ejecuta `git lfs pull` dentro del directorio).*
3.  **Abre el proyecto con Unity Hub:**
    *   Abre Unity Hub.
    *   Haz clic en "Open" (Abrir) -> "Add project from disk" (Añadir proyecto desde disco).
    *   Selecciona la carpeta `InspiraEstrellasAndinas` que clonaste.
    *   Asegúrate de seleccionar la versión correcta de Unity Editor (2022.3 LTS o la indicada en `ProjectVersion.txt`).
4.  **Espera a que Unity importe:** La primera vez que abras el proyecto, Unity necesitará importar todos los assets y paquetes. Esto puede tardar un tiempo.

---

## ?? Estructura del Proyecto

Una visión general de cómo está organizado el proyecto dentro de la carpeta `Assets`:

### ?? Carpetas Principales

*   `Assets/Inspira/`: **Carpeta principal del proyecto.** Contiene la lógica, assets específicos y escenas desarrolladas para este juego.
    *   `Animations/`: Archivos de animación y controladores.
    *   `Audio/`: Clips de sonido, música, configuración de AudioMixer.
    *   `Characters/`: Modelos, prefabs y lógica de personajes (hologramas).
    *   `Fonts/`: Archivos de fuentes personalizadas.
    *   `Materials/`: Materiales utilizados en los modelos y UI.
    *   `Models/`: Archivos de modelos 3D (FBX, OBJ, etc.).
    *   `Prefabs/`: GameObjects pre-configurados listos para usar.
    *   `Scenes/`: **¡Aquí viven las escenas del juego!**
    *   `Scripts/`: Todos los scripts C# personalizados.
    *   `Settings/`: Archivos de configuración (Input Actions, etc.).
    *   `Shaders/`: Shaders personalizados (si los hay).
    *   `Textures/`: Archivos de textura (PNG, TGA, JPG, PSD).
    *   `Timeline/`: Assets de Unity Timeline para cinemáticas o secuencias.
    *   `UI/`: Elementos de interfaz de usuario (Canvas, imágenes, prefabs de UI).
    *   `Vegetation/`, `Rocks/`, `River/`, `House/` etc.: Assets específicos de entorno.
*   `Assets/[Otras Carpetas]/`: Pueden incluir assets descargados de la Asset Store (ej. `Gogo Casual Pack`, `Abandoned World`), paquetes esenciales (`TextMesh Pro`, `XR`), etc.
*   `Packages/`: Contiene el `manifest.json` y las dependencias de paquetes gestionadas por Unity Package Manager (UPM).

### ?? Escenas Principales

Ubicadas generalmente en `Assets/Inspira/Scenes/`:

1.  **UI_Inicio:** Escena de bienvenida e introducción al juego. Contiene UI principal, LevelManager, AudioManager.
2.  **UI_difficult:** Escena para la selección de dificultad (si aplica) o configuración inicial. Similar a UI_Inicio en estructura base.
3.  **nave:** Escena principal del Módulo 1 y 2 (Puente y Laboratorio). Contiene el entorno interactivo, XR Rig, Managers de juego/escena/audio, triggers y elementos dinámicos/estáticos del escenario.
4.  **Recuerdo_2:** Escena principal del Módulo 3 (Recuerdo). Similar a `nave` en estructura, pero con el contenido específico del módulo 3.
5.  **UI_Final:** Escena de conclusión, mostrando resultados o un mensaje final. Contiene UI, LevelManager, posiblemente partículas o efectos visuales.

*Nota: Todas las escenas jugables contienen un `XR Origin (XR Rig)` para la representación y seguimiento del jugador en VR y un `XR Interaction Manager` para gestionar las interacciones.*

---

## ?? Paquetes de Unity Clave

El archivo `Packages/manifest.json` define las dependencias del proyecto. Algunos de los paquetes más importantes son:

*   **`com.unity.xr.interaction.toolkit` (v3.1.1):** Fundamental para toda la interacción VR. Provee componentes para agarrar objetos, interactuar con UI en VR, teletransporte, movimiento continuo, etc.
*   **`com.unity.xr.openxr` (v1.14.3):** Implementación del estándar OpenXR, permitiendo que la aplicación funcione en una amplia gama de hardware VR compatible con OpenXR (Meta Quest, SteamVR, WMR, etc.).
*   **`com.unity.inputsystem` (v1.14.0):** Sistema moderno para manejar entradas de diversos dispositivos (mandos VR, teclado, ratón).
*   **`com.unity.render-pipelines.universal` (v17.1.0):** Define el pipeline de renderizado. URP ofrece un buen balance entre calidad visual y rendimiento para plataformas como VR standalone (Quest).
*   **`com.unity.timeline` (v1.8.7):** Usado para crear cinemáticas, secuencias de eventos o animaciones complejas.
*   **`com.unity.xr.hands` (v1.5.0):** (Si se utiliza) Soporte para seguimiento de manos.
*   **Módulos de Unity (`com.unity.modules.*`):** Funcionalidades básicas del motor (Audio, Physics, UI, Animation, etc.).

---

## ?? Uso

1.  Abre el proyecto en Unity Editor (versión compatible).
2.  Navega a la carpeta de Escenas (probablemente `Assets/Inspira/Scenes/`).
3.  Abre la escena inicial, por ejemplo `UI_Inicio.unity`.
4.  Asegúrate de tener un dispositivo VR compatible conectado y configurado con OpenXR en Unity (`Project Settings` > `XR Plug-in Management`).
5.  Presiona el botón **Play** en el editor de Unity para ejecutar la experiencia en tu dispositivo VR.
6.  Para crear una build ejecutable, ve a `File` > `Build Settings`, selecciona tu plataforma destino (ej. Android para Quest, Windows para PC VR), configura los ajustes y haz clic en `Build`.

---

## ?? Contribuciones

¡Las contribuciones son bienvenidas! Si deseas mejorar "Inspira Estrellas Andinas", por favor sigue estos pasos:

1.  **Haz un Fork** del Proyecto haciendo clic en el botón "Fork" en la esquina superior derecha.
2.  **Crea tu Rama de Característica:**
    ```bash
    git checkout -b feature/TuCaracteristicaIncreible
    ```
3.  **Realiza tus Cambios:** Desarrolla tu característica o corrección.
4.  **Confirma tus Cambios:**
    ```bash
    git commit -m 'Añade una Característica Increíble'
    ```
5.  **Sube tus Cambios a tu Rama:**
    ```bash
    git push origin feature/TuCaracteristicaIncreible
    ```
6.  **Abre una Pull Request:** Ve a tu fork en GitHub y abre una Pull Request hacia la rama `main` de este repositorio original.

Por favor, asegúrate de que tu código sigue las convenciones existentes y está debidamente comentado cuando sea necesario. Para cambios significativos, considera abrir un "Issue" primero para discutir la idea.

---

## ?? Licencia

Este proyecto se distribuye bajo la Licencia MIT. Consulta el archivo `LICENSE.txt` para más detalles.

*(**Nota:** Debes crear un archivo llamado `LICENSE.txt` en la raíz de tu proyecto y pegar el texto de la Licencia MIT allí. Puedes encontrarlo fácilmente buscando "MIT License text").*

---

## ?? Contacto

Andrés Ordóñez - [@AndresOrdonez369](https://github.com/AndresOrdonez369)

Enlace del Proyecto: [https://github.com/AndresOrdonez369/InspiraEstrellasAndinas](https://github.com/AndresOrdonez369/InspiraEstrellasAndinas)

---

## ?? Agradecimientos

*   A todas las mujeres pioneras en STEM que inspiran este proyecto.
*   A la comunidad de Unity y XR.
*   _(Añade aquí otros agradecimientos si lo deseas: assets específicos, tutoriales, colaboradores, etc.)_

---