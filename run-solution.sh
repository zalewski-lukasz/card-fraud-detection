# Run docker-compose

cd deploy;
docker compose up -d;
cd ..;

# Create Java jar for Apache Flink

mvn clean package -f src/AnomalyDetector/pom.xml;

# Copy the file into taskmanager container and run it there

docker cp src/AnomalyDetector/target/AnomalyDetector-0.1.jar flink-jobmanager:/AnomalyDetector-0.1.jar;
docker exec -it flink-jobmanager flink run /AnomalyDetector-0.1.jar;