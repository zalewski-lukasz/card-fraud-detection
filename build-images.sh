# Build and push image for transaction data generator

cd ./src/TransactionSimulator/;
docker build -t xlukzalx/psd-transaction-simulator:latest .;
docker push xlukzalx/psd-transaction-simulator:latest;
cd ../../;

# Build and push image for anomaly reader

cd ./src/AnomalyInfoConsumer/;
docker build -t xlukzalx/psd-anomaly-consumer:latest .;
docker push xlukzalx/psd-anomaly-consumer:latest;
cd ../../;

# Build and push image for web application

#cd ./src/AdminDashboard/dashboard-app/;
#ls;
#docker build -t xlukzalx/psd-admin-dashboard:latest .;
#docker push xlukzalx/psd-admin-dashboard:latest;

