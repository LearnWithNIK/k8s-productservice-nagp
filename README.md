# ProductService – Kubernetes Multi-Tier Deployment

This repository contains the source code and Kubernetes deployment configurations for a .NET 8-based ProductService API that interacts with a PostgreSQL database.

---

## 📁 Repository Structure

- `ProductService/` – Source code for the API
- `ProductService/k8s/` – Kubernetes manifests and deployment script

---

## 🚀 Deployment Summary

- **Service Tier**: .NET 8 Web API exposing CRUD endpoints at `/api/products`
- **Database Tier**: PostgreSQL with persistent volume
- **Container Registry**: Docker Hub
- **Platform**: GKE (Google Kubernetes Engine)
- **Exposure**: Kubernetes Ingress with custom host mapping

---

## 🔗 Links

- **GitHub Repository**: [https://github.com/LearnWithNIK/k8s-productservice-nagp.git](https://github.com/LearnWithNIK/k8s-productservice-nagp.git)
- **Docker Hub Image**: [`nikitrai1993/productservice:latest`](https://hub.docker.com/r/nikitrai1993/productservice)
- **Service URL**: [http://product.local/api/products](http://product.local/api/products)

> ℹ️ Ensure you map the domain manually:
> ```
> 35.184.34.101   product.local
> ```
> Add this entry to your `/etc/hosts` or local DNS settings.

---

## 🎬 Screen Recording Checklist

🎥 *[Add your screen recording link here]*

Your video should demonstrate:
- ✅ All Kubernetes objects deployed and running
- ✅ API call (`GET /api/products`) fetching data from PostgreSQL
- ✅ Deleting an API pod → auto-recovery via Deployment
- ✅ Deleting the DB pod → auto-recovery with data intact via PVC

---

## 📦 Docker Build & Push

```bash
docker build -t nikitrai1993/productservice:latest .
docker push nikitrai1993/productservice:latest
```

---

## 🐳 Dockerfile Summary

This project uses a multi-stage `Dockerfile` located in the root of the `ProductService/` directory.

- **Stage 1**: Uses `mcr.microsoft.com/dotnet/sdk:8.0` to build and publish the .NET 8 application.
- **Stage 2**: Uses `mcr.microsoft.com/dotnet/aspnet:8.0` as the runtime environment.
- **Final Image**: Exposes port 80 and runs the `ProductService.dll` on startup.

See the full `Dockerfile` in the repository for details.

---

## 🚀 One-Click Kubernetes Deployment

From the `ProductService/k8s/` directory:

```bash
cd ProductService/k8s
chmod +x deploy.sh
./deploy.sh
```

> This script applies all Kubernetes manifests in the correct order: Secrets, ConfigMap, PVC, PostgreSQL, API service, and Ingress.

---

## 🛠️ Manual Deployment (Optional)

Apply each manifest individually:

```bash
kubectl apply -f secret.yaml
kubectl apply -f configmap.yaml
kubectl apply -f pvc.yaml
kubectl apply -f postgres-deployment.yaml
kubectl apply -f postgres-service.yaml
kubectl apply -f product-deployment.yaml
kubectl apply -f product-service.yaml
kubectl apply -f ingress.yaml
```

---

## 📝 Notes

- Insert product records via `POST /api/products` after deployment.
- PostgreSQL uses a PersistentVolumeClaim to retain data.
- API is exposed externally using Kubernetes Ingress and the domain `product.local`.

---

## 📧 Author

**Nikit Rai**
