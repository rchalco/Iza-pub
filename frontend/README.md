# microventas-ui / iza-discoteca

Aplicación móvil y de escritorio construida con **Ionic + Angular + Capacitor**.

---

## Requisitos previos

```bash
npm install -g @ionic/cli @angular/cli @capacitor/cli
```

---

## Instalación

```bash
npm install
```

---

## Desarrollo web

```bash
# Servidor de desarrollo
npm start
# o equivalente
ionic serve

# Build de desarrollo
npm run build

# Build de producción
ionic build --prod
# o
ng build --configuration production
```

---

## Tests

```bash
# Ejecutar tests unitarios (Jest)
npm test

# Tests en modo watch
npm run test:watch

# Reporte de cobertura
npm run test:coverage

# Lint
npm run lint
```

---

## Capacitor

### Sincronizar web build con plataformas nativas

```bash
# Copia el build web (www/) y actualiza plugins en todas las plataformas
npx cap sync

# Solo copiar archivos web sin actualizar plugins
npx cap copy

# Solo actualizar plugins nativos
npx cap update
```

### Plataforma Android

```bash
# Agregar plataforma Android (solo la primera vez)
npx cap add android

# Abrir en Android Studio
npx cap open android

# Ejecutar en dispositivo/emulador Android (requiere Android Studio)
ionic capacitor run android
ionic capacitor run android --livereload --external
```

### Plataforma iOS

```bash
# Agregar plataforma iOS (solo la primera vez, requiere macOS)
npx cap add ios

# Abrir en Xcode
npx cap open ios

# Ejecutar en simulador/dispositivo iOS
ionic capacitor run ios
ionic capacitor run ios --livereload --external
```

### Flujo completo de build nativo

```bash
# 1. Compilar la app web
ionic build --prod

# 2. Sincronizar con Capacitor
npx cap sync

# 3. Abrir IDE nativo
npx cap open android   # Android Studio
npx cap open ios       # Xcode (solo macOS)
```

---

## Electron

```bash
cd electron

# Instalar dependencias de Electron
npm install

# Ejecutar en modo desarrollo con live-reload
npm run electron:start-live

# Ejecutar sin live-reload
npm run electron:start

# Empaquetar (sin instalador)
npm run electron:pack

# Generar instalador distribuible
npm run electron:make
```

---

## Ionic CLI — comandos útiles

```bash
# Ver info del entorno
ionic info

# Generar página
ionic generate page pages/nombre-pagina

# Generar componente
ionic generate component components/nombre-componente

# Generar servicio
ionic generate service services/nombre-servicio

# Generar guardia de ruta
ionic generate guard guards/nombre-guard

# Ver dispositivos conectados
ionic capacitor run android --list
```

---

## Angular CLI — comandos útiles

```bash
# Generar módulo con routing
ng generate module pages/nombre --routing

# Generar componente standalone
ng generate component components/nombre

# Generar servicio
ng generate service services/nombre

# Analizar bundle
ng build --stats-json
npx webpack-bundle-analyzer www/browser/stats.json
```

---

## Variables de entorno

| Archivo | Uso |
|---|---|
| `src/environments/environment.ts` | Desarrollo |
| `src/environments/environment.prod.ts` | Producción |

---

## Estructura del proyecto

```
src/
  app/
    components/   # Componentes reutilizables
    guards/       # Guards de rutas
    interfaces/   # Interfaces TypeScript
    pages/        # Páginas de la aplicación
    pipes/        # Pipes personalizados
    services/     # Servicios e inyectables
  assets/         # Recursos estáticos
  environments/   # Configuración por entorno
  theme/          # Variables de estilos globales
electron/         # Configuración de Electron
resources/        # Íconos y splash screens
```

---

## Generar íconos y splash screens

```bash
# Requiere @capacitor/assets
npm install -D @capacitor/assets

npx capacitor-assets generate
```
