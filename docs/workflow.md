# 🔄 Development Workflow

This guide outlines the branching strategy, pull request process, and deployment flow used for this project. It follows a modified [GitHub Flow](https://docs.github.com/en/get-started/quickstart/github-flow) approach optimized for our team's collaboration.

---

## 🪴 Branching Strategy

### Main Branches

- **`main`**
    - The default branch.
    - CI/CD builds and releases are triggered from here.
    - Protected branch — all changes go through PRs.

### Working Branches

- **`feature/{story-number}-{short-description}`**  
  For new features related to a user story.  
  Example: `feature/123-login-form-validation`

- **`bugfix/{bug-number}-{short-description}`**  
  For fixing tracked bugs.  
  Example: `bugfix/456-fix-header-overlap`

---

## 🚧 Development Process

1. **Create a branch**
    - Start from `main`
    - Use a `feature/` or `bugfix/` prefix depending on the task

2. **Stay up-to-date**
    - Pull from `main` regularly (daily or whenever a new PR is merged)

3. **Develop against a single story**
    - Keep the scope of each branch focused to make reviews easy

4. **Open a Pull Request (PR) into `main`**
    - Include the relevant story or bug number in the PR title or description
    - Ensure the branch is up-to-date with `main`

5. **Verify the build**
    - CI build must complete successfully
    - Ensure your database migrations (if any) are added **last** to avoid conflicts

6. **Code Review**
    - PRs are reviewed by **Joshua**

7. **Deployment**
    - Upon approval, the solution is built and automatically deployed to the dev environment

8. **Clean up**
    - After deployment, delete both the **remote** and **local** feature or bugfix branches

---

## 📚 References

- 🔗 [GitHub Flow Documentation](https://docs.github.com/en/get-started/quickstart/github-flow)

---

Following this workflow helps us maintain high-quality, stable code while enabling fast iteration and team collaboration.
