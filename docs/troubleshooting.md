# Troubleshooting Guide

This guide provides solutions to common issues you may encounter while developing or running this project.

---

## Table of Contents

1. [General Issues](#general-issues)
2. [Frontend (webapp) Issues](#frontend-webapp-issues)
3. [Backend (Api, Data, Shared) Issues](#backend-api-data-shared-issues)
4. [Database Issues](#database-issues)
5. [Build & Deployment Issues](#build--deployment-issues)
6. [Other Resources](#other-resources)

---

## 1. General Issues

### Problem: **Dependencies Not Installing or Outdated**
- **Solution:**  
  - For Node.js: Run `npm install` in the `webapp` directory.
  - For .NET: Run `dotnet restore` in the solution root.
  - Delete `node_modules/` or `bin/`/`obj/` folders and reinstall if issues persist.

### Problem: **Environment Variables Not Loaded**
- **Solution:**  
  - Ensure `.env` files (frontend) or `appsettings.json` (backend) are present and correctly configured.
  - For local development, check for `appsettings.Local.json` and override as needed.

---

## 2. Frontend (webapp) Issues

### Problem: **Vite Server Fails to Start**
- **Solution:**  
  - Ensure Node.js (v16+) is installed.
  - Run `npm install` before `npm run dev`.
  - Check for port conflicts (default: 5173).

### Problem: **Tailwind CSS Not Working**
- **Solution:**  
  - Ensure `tailwind.config.js` and `postcss.config.cjs` are present.
  - Restart the dev server after changes to Tailwind config.

### Problem: **TypeScript Errors**
- **Solution:**  
  - Run `npm run type-check` to see errors.
  - Ensure all `.vue` files use `<script setup lang="ts">` if using TypeScript.

### Problem: **ESLint/Prettier Not Formatting**
- **Solution:**  
  - Run `npm run lint` or check your editor's integration.
  - Ensure `.eslintrc.js` and `.prettierrc` are present.

### Problem: **Hot Module Reload (HMR) Not Working**
- **Solution:**  
  - Restart the Vite dev server.
  - Clear browser cache.

---

## 3. Backend (Api, Data, Shared) Issues

### Problem: **API Not Starting**
- **Solution:**  
  - Run `dotnet build` to check for compilation errors.
  - Ensure all required configuration files are present.
  - Check for port conflicts (default: 5000/5001).

### Problem: **Database Connection Errors**
- **Solution:**  
  - Verify connection strings in `appsettings.json` or `appsettings.Local.json`.
  - Ensure the database server is running and accessible.

### Problem: **Migrations Not Applying**
- **Solution:**  
  - Run `dotnet ef database update` in the `Data` project.
  - Ensure `DesignTimeContextFactory.cs` is correctly configured.

### Problem: **NSwag/Swagger Not Generating**
- **Solution:**  
  - Ensure `nswag.json` is present and valid.
  - Run `nswag run` or use the integrated tooling.

---

## 4. Database Issues

### Problem: **Migrations Out of Sync**
- **Solution:**  
  - Run `dotnet ef migrations add <MigrationName>` to create new migrations.
  - Run `dotnet ef database update` to apply migrations.
  - If issues persist, try deleting the database and reapplying migrations (for local/dev only).

### Problem: **Seeding/Initialization Fails**
- **Solution:**  
  - Check `DBInitializer.cs` for errors or missing data.
  - Ensure the database user has appropriate permissions.

---

## 5. Build & Deployment Issues

### Problem: **Build Fails**
- **Solution:**  
  - For frontend: Run `npm run build` and check for errors.
  - For backend: Run `dotnet publish` and check for errors.

### Problem: **Static Files Not Served**
- **Solution:**  
  - Ensure files are in the correct `public/` or `wwwroot/` directory.
  - Check server/static file middleware configuration.

### Problem: **Production Environment Variables Not Set**
- **Solution:**  
  - Ensure all required environment variables are set in the deployment environment.

---

## 6. Other Resources

- [Vue 3 Troubleshooting](https://vuejs.org/guide/scaling-up/tooling.html#troubleshooting)
- [Vite Troubleshooting](https://vitejs.dev/guide/troubleshooting.html)
- [ASP.NET Core Troubleshooting](https://docs.microsoft.com/en-us/aspnet/core/test/troubleshoot)
- [Entity Framework Core Troubleshooting](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/troubleshooting)
- [NSwag Documentation](https://github.com/RicoSuter/NSwag)

---

**Still stuck?**  
- Check the project README for setup instructions.
- Search the issue tracker or open a new issue with detailed error messages and steps to reproduce.
- Contact the project maintainers for further help.
