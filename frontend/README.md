# gamatek-discoteca

Aplicacion Ionic + Angular + Capacitor orientada a ventas e impresion termica en Android.

## Stack principal

- Angular 18
- Ionic 8
- Capacitor
- Jest
- Android Studio para compilacion nativa

## Requisitos previos

Antes de instalar dependencias, verifica que tengas disponible:

- Node.js 18 o superior
- npm
- Ionic CLI
- Angular CLI
- Capacitor CLI
- Android Studio si vas a compilar o ejecutar Android

```bash
npm install -g @ionic/cli @angular/cli @capacitor/cli
```

## Instalacion inicial

```bash
npm install
```

## Plugin de impresion termica

Este proyecto utiliza el plugin `capacitor-thermal-printer` para impresion Bluetooth en Android.

Importante:

- Aunque trabajes con plugins de Capacitor compatibles con la linea 7, el plugin de impresion debe estar instalado en `node_modules` antes de sincronizar la plataforma nativa.
- En instalaciones limpias o cuando haya problemas de resolucion de dependencias, reinstalalo de forma explicita.
- Despues de instalar o reinstalar el plugin, ejecuta `npx cap sync android`.

```bash
npm install capacitor-thermal-printer
npx cap sync android
```

Si el proyecto ya fue instalado correctamente, este paquete tambien queda registrado en [package.json](package.json).

## Arranque rapido

Para levantar el proyecto en web:

```bash
npm install
npm start
```

Para preparar Android desde cero:

```bash
npm install
npm install capacitor-thermal-printer
npx cap sync android
npx cap open android
```

## Desarrollo web

```bash
# Servidor de desarrollo
npm start

# Alternativa con Ionic
ionic serve

# Build de desarrollo
npm run build

# Build de produccion
ng build --configuration production
```

## Tests y calidad

```bash
# Ejecutar tests unitarios
npm test

# Tests en modo watch
npm run test:watch

# Cobertura
npm run test:coverage

# Lint
npm run lint
```

## Capacitor

### Comandos base

```bash
# Copia el build web y actualiza plugins nativos
npx cap sync

# Copia solo archivos web
npx cap copy

# Actualiza plugins nativos sin copiar assets web
npx cap update
```

### Flujo recomendado para Android

Cada vez que cambies codigo web y necesites probar en Android:

```bash
# 1. Compilar web
npm run build

# 2. Sincronizar con Android
npx cap sync android

# 3. Abrir proyecto nativo
npx cap open android
```

### Primera configuracion de Android

```bash
# Solo si la plataforma no existe aun
npx cap add android

# Sincronizar plugins y assets
npx cap sync android

# Abrir en Android Studio
npx cap open android
```

### Ejecutar en dispositivo o emulador

```bash
ionic capacitor run android
ionic capacitor run android --livereload --external
```

## Impresion en Android

La aplicacion usa una impresora Bluetooth configurada desde la pantalla `config-printer`.

Consideraciones utiles:

- La impresion depende del plugin `capacitor-thermal-printer` instalado y sincronizado.
- Si Android Studio o Gradle no detectan el modulo nativo del plugin, vuelve a correr `npx cap sync android`.
- Si el proyecto Android fue regenerado o limpiado, valida nuevamente la instalacion del plugin antes de compilar.

## Estructura del proyecto

```text
src/
  app/
    components/   # Componentes reutilizables
    guards/       # Guards de rutas
    helpers/      # Utilidades como impresion
    interfaces/   # Interfaces TypeScript
    pages/        # Paginas de la aplicacion
    pipes/        # Pipes personalizados
    services/     # Servicios e inyectables
  assets/         # Recursos estaticos
  environments/   # Configuracion por entorno
  theme/          # Variables globales de estilos
android/          # Proyecto nativo Android
resources/        # Iconos y splash screens
www/              # Build web generado para Capacitor
```

## Variables de entorno

| Archivo                                | Uso        |
| -------------------------------------- | ---------- |
| `src/environments/environment.ts`      | Desarrollo |
| `src/environments/environment.prod.ts` | Produccion |

## Comandos utiles

```bash
# Ver informacion del entorno Ionic
ionic info

# Ver dispositivos Android disponibles
ionic capacitor run android --list

# Generar pagina
ionic generate page pages/nombre-pagina

# Generar componente
ionic generate component components/nombre-componente

# Generar servicio
ionic generate service services/nombre-servicio
```

## Generar iconos y splash screens

```bash
npm install -D @capacitor/assets
npx capacitor-assets generate
```

## Problemas comunes

### El plugin de impresora no aparece en Android

```bash
npm install capacitor-thermal-printer
npx cap sync android
```

### Android no refleja cambios recientes del frontend

```bash
npm run build
npx cap sync android
```

### El proyecto Android compila, pero la impresion falla

Verifica:

- que la impresora Bluetooth este emparejada
- que exista una impresora configurada en la pantalla `config-printer`
- que el plugin siga instalado y sincronizado en `android/`
