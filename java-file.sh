mvn clean package -f src/AnomalyDetector/pom.xml
docker cp src/AnomalyDetector/target/AnomalyDetector-0.1.jar flink-jobmanager:/AnomalyDetector-0.1.jar
docker exec -it flink-jobmanager flink run /AnomalyDetector-0.1.jar