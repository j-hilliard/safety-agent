# UI Guidelines

## Table of Contents
1. [Project Structure](#project-structure)
2. [Component Guidelines](#component-guidelines)
3. [Styling and CSS](#styling-and-css)
4. [State Management](#state-management)
5. [Routing](#routing)
6. [TypeScript Usage](#typescript-usage)
7. [Best Practices](#best-practices)
8. [Resources](#resources)

---

## 1. Project Structure

Your project follows a modular, scalable structure. Key directories:

- `src/components/` — Shared, reusable UI components (e.g., `BaseFormField.vue`, `TheMenu.vue`)
- `src/views/` — Page-level components, typically mapped to routes
- `src/layout/` — Layout wrappers for pages (e.g., `AppLayout.vue`)
- `src/modules/` — Feature or domain modules, each with their own `features/` and `router/`
- `src/stores/` — Pinia/Vuex stores for state management
- `src/composables/` — Reusable logic hooks (Vue 3)
- `src/assets/` — Static assets (images, fonts, etc.)
- `src/apiclient/` and `src/services/` — API and business logic

**Tip:** Organize new features as modules under `src/modules/`, with their own features and routes.

---

## 2. Component Guidelines

- **Naming:** Use PascalCase for component files and names (e.g., `BaseFormField.vue`).
- **Structure:** Each component should have `<template>`, `<script setup lang="ts">`, and `<style scoped>` sections as needed.
- **Props & Emits:** Define all props and emits with types. Use `defineProps` and `defineEmits` in `<script setup>`.
- **Reusability:** Place shared components in `src/components/`. Feature-specific components can live within their module or feature folder.
- **Base Components:** Prefix with `Base` for generic, reusable building blocks (e.g., `BaseLoader.vue`).

---

## 3. Styling and CSS

- **Framework:** Tailwind CSS is used for utility-first styling.
- **Scoped Styles:** Use `<style scoped>` for component-specific styles if needed.
- **Global Styles:** Place global styles in `src/assets/` or configure via Tailwind.
- **Class Naming:** Use Tailwind utility classes. For custom classes, use BEM or clear, descriptive names.

---

## 4. State Management

- **Store Location:** Use `src/stores/` for global stores (e.g., `userStore.ts`).
- **Module Stores:** For module-specific state, you may add stores within the module or feature folder.
- **Pinia:** Prefer Pinia for new stores (Vue 3 best practice).

---

## 5. Routing

- **Central Router:** Main routes are defined in `src/router/index.ts`.
- **Module Routes:** Each module has its own `router/index.ts` exporting its routes, which are imported into the main router.
- **Lazy Loading:** Use dynamic imports for route components for better performance.
- **Meta Titles:** Set `meta.title` for routes to update the document title.

---

## 6. TypeScript Usage

- **Type Safety:** Use TypeScript for all logic (`.ts` and `.vue` with `<script setup lang="ts">`).
- **Types:** Define shared types in `src/types.d.ts` and module-specific types within the module.
- **Shims:** Use `shims-vue.d.ts` for Vue file type support.

---

## 7. Best Practices

- **Atomic Commits:** Make small, focused commits with clear messages.
- **Linting & Formatting:** Use ESLint and Prettier for code consistency.
- **Testing:** (If applicable) Place tests alongside components or in a `tests/` directory.
- **Documentation:** Document complex components and logic with comments and JSDoc.

---

## 8. Resources

- [Vue 3 Documentation](https://vuejs.org/guide/introduction.html)
- [Pinia Documentation](https://pinia.vuejs.org/)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs/)
- [Vite Documentation](https://vitejs.dev/guide/)

---

**For questions or to propose changes to these guidelines, please open a pull request or contact the project maintainers.** 