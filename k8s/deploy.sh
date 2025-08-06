#!/bin/bash

# Apply Kubernetes manifests in order

kubectl apply -f secret.yaml
kubectl apply -f configmap.yaml
kubectl apply -f pvc.yaml
kubectl apply -f postgres-deployment.yaml
kubectl apply -f postgres-service.yaml
kubectl apply -f product-deployment.yaml
kubectl apply -f product-service.yaml
kubectl apply -f ingress.yaml

echo "All resources applied."
