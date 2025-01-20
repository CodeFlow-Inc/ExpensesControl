# Installation Guide: WSL, Docker, and Portainer

This guide explains how to install WSL (Windows Subsystem for Linux), set up Docker within WSL, and configure Portainer to start automatically whenever WSL is launched.

---

## **1. Installing WSL**

1. **Enable WSL on Windows:**
   Open PowerShell as Administrator and run:
   ```powershell
   wsl --install
   ```
   - This command installs WSL 2 and sets Ubuntu as the default distribution.
   - Restart your computer if prompted.

2. **Install a Specific Distribution:**
   If you want to install another distribution (e.g., Debian or Alpine), run:
   ```powershell
   wsl --install -d <DistributionName>
   ```
   Example to install Ubuntu:
   ```powershell
   wsl --install -d Ubuntu
   ```

3. **Check the WSL Version:**
   Ensure your distribution is running on WSL 2:
   ```powershell
   wsl --list --verbose
   ```
   If it is not using WSL 2, convert the distribution:
   ```powershell
   wsl --set-version <DistributionName> 2
   ```

---

## **2. Installing Docker in WSL**

### **Option 1: Using Docker Desktop**

1. **Download and Install Docker Desktop:**
   - Download Docker Desktop from: [https://www.docker.com/products/docker-desktop/](https://www.docker.com/products/docker-desktop/)
   - During installation, ensure you select the option **"Use WSL 2 instead of Hyper-V"**.

2. **Enable Docker Integration with WSL:**
   - Open Docker Desktop.
   - Go to **Settings > Resources > WSL Integration**.
   - Enable integration for your desired Linux distribution (e.g., Ubuntu).

3. **Test the Docker Installation:**
   Open the WSL terminal and run:
   ```bash
   docker --version
   docker run hello-world
   ```
   If Docker is working correctly, you will see a success message from the "hello-world" container.

### **Option 2: Installing Docker Directly in WSL**

If you prefer to install Docker directly inside WSL without Docker Desktop, follow these steps:

1. **Update the Package List:**
   ```bash
   sudo apt update
   ```

2. **Install Required Packages:**
   ```bash
   sudo apt install -y ca-certificates curl gnupg lsb-release
   ```

3. **Add Docker’s GPG Key and Repository:**
   ```bash
   sudo mkdir -p /etc/apt/keyrings
   curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
   echo \
     "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
     $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
   ```

4. **Install Docker Engine:**
   ```bash
   sudo apt update
   sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
   ```

5. **Add Your User to the Docker Group:**
   ```bash
   sudo usermod -aG docker $USER
   ```

6. **Restart WSL and Test Docker:**
   Restart your WSL session:
   ```bash
   wsl --shutdown
   ```
   Then reopen WSL and test:
   ```bash
   docker run hello-world
   ```

---

## **3. Installing and Configuring Portainer**

1. **Download the Portainer Image:**
   Run the following command in the WSL terminal:
   ```bash
   docker pull portainer/portainer-ce
   ```

2. **Create a Volume for Portainer Data:**
   ```bash
   docker volume create portainer_data
   ```

3. **Run the Portainer Container:**
   ```bash
   docker run -d \
     --name=portainer \
     --restart=always \
     -p 8000:8000 \
     -p 9443:9443 \
     -v /var/run/docker.sock:/var/run/docker.sock \
     -v portainer_data:/data \
     portainer/portainer-ce
   ```

4. **Access Portainer:**
   - Open your browser and go to: [https://localhost:9443](https://localhost:9443)
   - Set up an admin username and password.

---

## **4. Configuring a Script to Start Portainer Automatically**

1. **Create the Startup Script:**
   In your WSL terminal, create a new script file:
   ```bash
   sudo nano /etc/profile.d/start-portainer.sh
   ```

2. **Add the Command to Start Portainer:**
   Paste the following content into the file:
   ```bash
   #!/bin/bash
   if ! docker ps | grep -q portainer; then
       docker start portainer
   fi
   ```

3. **Make the Script Executable:**
   ```bash
   sudo chmod +x /etc/profile.d/start-portainer.sh
   ```

4. **Test the Script:**
   Restart your WSL session:
   ```bash
   wsl --shutdown
   ```
   Open the terminal again and check if Portainer is running:
   ```bash
   docker ps
   ```

---

## **5. Troubleshooting: Docker Permissions in WSL**

If you encounter the error `permission denied while trying to connect to the Docker daemon socket`, follow these steps:

1. **Add Your User to the Docker Group:**
   ```bash
   sudo usermod -aG docker $USER
   ```

2. **Restart Your WSL Session:**
   ```bash
   wsl --shutdown
   ```

3. **Verify Docker Permissions:**
   ```bash
   docker run hello-world
   ```

4. **Start Docker Manually (if required):**
   ```bash
   sudo service docker start
   ```

5. **Check Docker Group Membership:**
   ```bash
   groups
   ```
   Ensure `docker` appears in the list of groups.

---

Now you have WSL, Docker, and Portainer configured properly! Let me know if you need further assistance.

