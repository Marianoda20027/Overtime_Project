# Tiquetes Frontend (React + Vite + TS)

Estructura alineada a tu guía (adapters, assets, components, contexts, hooks, models, pages, services, utilities) + ESLint, Prettier, Husky y Commitlint.

## Requisitos
- Node 18+ (LTS)
- npm

## Inicio Rápido
```bash
npm i
npm run prepare   # instala hooks de Husky
npm run dev
```

## Scripts
- `npm run dev` servidor de desarrollo
- `npm run build` build de producción
- `npm run preview` servir build local
- `npm run lint` ESLint
- `npm run format` Prettier
- `npm run typecheck` TypeScript sin emitir

## Variables de entorno
Copia `.env.example` a `.env` y ajusta:
```
VITE_API_URL=http://localhost:3000
```

## Notas
- React 18 para máxima compatibilidad con librerías de UI.
- Puedes agregar más páginas repitiendo la estructura `pages/<Page>/{index.tsx,hooks.ts,types.ts}`.
