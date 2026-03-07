# 🏗️ STH App Template

**A reusable project foundation for all Stronghold applications.**  
This template provides consistent tooling, configuration, and best practices used across the Stronghold app dev team. It’s designed to accelerate development and improve maintainability.

---

## 📦 Tech Stack

This template includes a modern and scalable tech stack:

- **Frontend**: [Vue 3](https://vuejs.org/), [TypeScript](https://www.typescriptlang.org/), [Pinia](https://pinia.vuejs.org/) for state management
- **Build Tool**: [Vite](https://vitejs.dev/) with HTTPS support via `mkcert`
- **API Integration**: Type-safe generated clients using [NSwag](https://github.com/RicoSuter/NSwag)
- **Authentication**: Azure AD SSO via [MSAL Browser](https://github.com/AzureAD/microsoft-authentication-library-for-js)
- **Linting & Quality**: [ESLint](https://eslint.org/), [TypeScript ESLint](https://typescript-eslint.io/)

---

## 📘 Developer Guide

Welcome! This guide is intended for developers working on this application. It provides key documentation for getting started, contributing effectively, and understanding the system architecture.

---

## 🔧 Core Topics

- [UI Guidelines](./docs/ui-guidelines.md)  
  Design principles, component structure, and frontend conventions.

- [API Guidelines](./docs/api-guidelines.md)  
  Standards for building, documenting, and integrating backend APIs.

- [Development Workflow](./docs/workflow.md)  
  Overview of our Git branching strategy, pull request process, and deployment practices.

---

## 👩‍💻 Team Onboarding

- [First Day Setup](./docs/onboarding.md)  
  Step-by-step instructions to configure your local development environment.

- [Troubleshooting & FAQs](./docs/troubleshooting.md)  
  Common issues and how to resolve them.

---

## ✅ Getting Started

If you're new to the project:

1. Begin with the [First Day Setup](./docs/onboarding.md) guide.
2. Familiarize yourself with our [Development Workflow](./docs/workflow.md).
3. Review the [UI](./docs/ui-guidelines.md) or [API](./docs/api-guidelines.md) guidelines based on your role.

We encourage all contributors to follow the practices outlined in this guide to keep the codebase clean, consistent, and scalable.

---
