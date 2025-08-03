#!/bin/bash

# Apply Kubernetes manifests in order

kubectl apply -f secret.yaml
kubectl apply -f configmap.yaml
kubectl apply -f pvc.yaml
kubectl apply -f postgres-deployment.yaml
kubectl apply -f postgres-service.yaml
# sleep 20
kubectl apply -f product-deployment.yaml
kubectl apply -f product-service.yaml
kubectl apply -f ingress.yaml

echo "All resources applied."

# echo "Waiting for pods to be ready..."

# kubectl wait --for=condition=Ready pod -l app=postgres --timeout=120s
# kubectl wait --for=condition=Ready pod -l app=product --timeout=120s

# echo "Pods are ready."

# kubectl get pods
# kubectl get svc
# kubectl get ingress
