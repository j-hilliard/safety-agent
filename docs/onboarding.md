# Onboarding Guide

Welcome to the project! This guide will help you set up your development environment, understand the project structure, and follow best practices for contributing.

---

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Project Structure Overview](#project-structure-overview)
3. [Getting Started](#getting-started)
    - [Cloning the Repository](#cloning-the-repository)
    - [Setting Up the Frontend (webapp)](#setting-up-the-frontend-webapp)
    - [Setting Up the Backend (Api, Data, Shared)](#setting-up-the-backend-api-data-shared)
4. [Running the Application](#running-the-application)
5. [Development Workflow](#development-workflow)
6. [Code Quality & Standards](#code-quality--standards)
7. [Troubleshooting](#troubleshooting)
8. [Further Resources](#further-resources)

---

## 1. Prerequisites

- **Node.js** (v16 or higher)
- **npm** (v8 or higher)
- **.NET 6 SDK** (or higher)
- **SQL Server** (or your configured database)
- **Git** (for version control)
- Recommended: VS Code or JetBrains Rider

---

## 2. Project Structure Overview

- `/webapp/` — Vue 3 frontend (Vite, Tailwind CSS, TypeScript)
- `/Api/` — ASP.NET Core Web API (controllers, services, validators, etc.)
- `/Data/` — Entity Framework Core data access (DbContext, migrations, models)
- `/Shared/` — Shared code (constants, enums, attributes) for backend projects
- `/docs/` — Project documentation

---

## 3. Getting Started

### Cloning the Repository

```sh
git clone <your-repo-url>
cd <project-root>
```

### Setting Up the Frontend (webapp)

```sh
cd webapp
npm install
```

- Copy any required `.env` files if not present.
- To start the dev server:
  ```sh
  npm run dev
  ```

### Setting Up the Backend (Api, Data, Shared)

```sh
dotnet restore
```

- Ensure `appsettings.json` and (optionally) `appsettings.Local.json` are configured.
- Update the database (if needed):
  ```sh
  cd Data
  dotnet ef database update
  ```

---

## 4. Running the Application

- **Frontend:**  
  From `/webapp`, run `npm run dev` and visit [http://localhost:5173](http://localhost:5173)
- **Backend:**  
  From `/Api`, run `dotnet run` (default ports: 5000/5001)
- **API Docs:**  
  Visit `/swagger` or `/nswag` endpoint if enabled

---

## 5. Development Workflow

- Create a new branch for your feature or bugfix.
- Make atomic, well-documented commits.
- Follow the [UI Guidelines](./ui-guidelines.md) and [API Guidelines](./api-guidelines.md).
- Run linters and tests before pushing.
- Open a pull request for review.

---

## 6. Code Quality & Standards

- **Frontend:**  
  - Use TypeScript and `<script setup lang="ts">` in Vue files.
  - Run `npm run lint` and `npm run type-check`.
  - Use Tailwind CSS for styling.
- **Backend:**  
  - Follow C# and .NET conventions.
  - Use dependency injection for services.
  - Write XML comments for public APIs.
- **General:**  
  - Keep code modular and well-documented.
  - Add or update documentation as needed.

---

## 7. Troubleshooting

- See [Troubleshooting Guide](./troubleshooting.md) for common issues and solutions.
- If you're stuck, check the README, search issues, or ask a maintainer.

---

## 8. Further Resources

- [Vue 3 Documentation](https://vuejs.org/guide/introduction.html)
- [Vite Documentation](https://vitejs.dev/guide/)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)

---

**Welcome aboard! If you have any questions, don't hesitate to ask your team or open an issue.**
