# Configuring GitHub as a NuGet Package Source

This guide explains how to configure GitHub as a NuGet package source by modifying the `NuGet.Config` file to include your GitHub repository credentials.

---

## **Steps to Add the GitHub Source**

1. **Locate the `NuGet.Config` File**:
   - The `NuGet.Config` file can typically be found in one of the following locations:
     - Global: `%APPDATA%\NuGet\NuGet.Config` (Windows)
     - Project-specific: In the root folder of your .NET project.

2. **Edit the `NuGet.Config` File**:
   - Open the `NuGet.Config` file in a text editor or IDE of your choice.
   - Add the following configuration for GitHub:

   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <packageSources>
       <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
       <add key="Microsoft Visual Studio Offline Packages"
            value="C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\" />
       <add key="github" value="https://nuget.pkg.github.com/CodeFlow-Inc/index.json" />
     </packageSources>
     <packageSourceCredentials>
       <github>
         <add key="Username" value="NicolasBuscarini" />
         <add key="ClearTextPassword" value="TOKEN_GITHUB" />
       </github>
     </packageSourceCredentials>
     <packageRestore>
       <add key="enabled" value="True" />
       <add key="automatic" value="True" />
     </packageRestore>
     <bindingRedirects>
       <add key="skip" value="False" />
     </bindingRedirects>
     <packageManagement>
       <add key="format" value="0" />
       <add key="disabled" value="False" />
     </packageManagement>
   </configuration>
   ```

   - Replace:
     - `NicolasBuscarini` with your GitHub username.
     - `TOKEN_GITHUB` with your personal access token (PAT).

3. **Verify Your Personal Access Token (PAT)**:
   - Make sure the token has the following scopes:
     - `read:packages` (for reading packages)
     - `repo` (if the repository is private)

   You can generate or verify tokens in your [GitHub Developer Settings](https://github.com/settings/tokens).

4. **Save the File**:
   - Save the changes and close the editor.

---

## **Testing the Configuration**

To confirm the configuration is working, try adding a package from your GitHub NuGet source:

```bash
dotnet add package <PackageName>
```

Replace `<PackageName>` with the name of the package you want to install.

---

## **Troubleshooting**

1. **401 Unauthorized**:
   - Ensure the token is active and has the correct scopes.
   - Check that the username and token in `NuGet.Config` are correct.

2. **Connectivity Issues**:
   - Ensure you have access to `https://nuget.pkg.github.com/CodeFlow-Inc/index.json`.

3. **Validate Configuration**:
   - Use the following command to list package sources and verify the GitHub source is added:
     ```bash
     dotnet nuget list source
     ```

---

## **Additional Resources**

- [GitHub Packages Documentation](https://docs.github.com/en/packages)
- [NuGet Configuration Documentation](https://learn.microsoft.com/en-us/nuget/reference/nuget-config-file)
