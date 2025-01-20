# Git Flow Guidelines for Our Team

## Branch Structure

Our team follows a structured Git Flow to maintain code quality and streamline the development process. Below is the description of our branch strategy:

### Protected Branches
1. **release**
   - This branch is used for deployment purposes.
   - Only stable and tested code should be merged here.

2. **master**
   - This branch contains the production-ready code.
   - Only thoroughly reviewed and tested code should be merged here.

### Feature Development
- For new features, create a branch using the following naming convention:
  ```
  feature/<issue-number>/<branch-name>
  ```
  Example: `feature/1234/add-login-button`

### Bug Fixes
- For bug fixes, create a branch using the following naming convention:
  ```
  bug-fix/<bug-number>/<branch-name>
  ```
  Example: `bug-fix/5678/fix-login-crash`

## Commit Messages

To maintain a clean and consistent commit history, use the following prefixes for commit messages:

- **CHORE:** For maintenance tasks or updates that don’t modify functionality (e.g., updating dependencies, configurations).
  Example: `CHORE: update npm packages`

- **STYLE:** For formatting or styling changes that do not affect functionality.
  Example: `STYLE: improve button spacing`

- **FEAT:** For adding new features.
  Example: `FEAT: implement user authentication`

- **FIX:** For fixing bugs.
  Example: `FIX: resolve login issue on mobile`

- **DOC:** For documentation updates.
  Example: `DOC: update README with setup instructions`

## Workflow Summary

1. **Start Development**
   - Create a branch for your feature or bug fix based on the conventions above.
   - Work on your changes locally and commit them with appropriate messages.

2. **Code Review and Testing**
   - Push your branch to the repository and open a Pull Request (PR) to the `release` branch.
   - Ensure the PR is reviewed by your peers and all automated tests pass.

3. **Merging and Deployment**
   - After the PR is approved and tested, merge it into the `release` branch.
   - Once the changes in the `release` branch are stable and ready for production, create a PR to merge into the `master` branch.
   - Deployments to production are made from the `master` branch.

## Additional Notes
- Always pull the latest changes from the `release` branch before starting work on a new feature or bug fix.
- Avoid direct commits to `release` and `master` branches.
- Keep your branches small and focused to simplify code reviews.
- Add meaningful descriptions to your pull requests to help reviewers understand your changes.

By following these guidelines, we can ensure a smooth and efficient development process while maintaining high-quality code.

