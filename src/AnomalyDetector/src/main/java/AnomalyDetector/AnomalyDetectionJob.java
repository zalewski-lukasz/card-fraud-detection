/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package AnomalyDetector;

import org.apache.flink.api.common.eventtime.WatermarkStrategy;
import org.apache.flink.connector.kafka.sink.KafkaRecordSerializationSchema;
import org.apache.flink.connector.kafka.sink.KafkaSink;
import org.apache.flink.connector.kafka.source.KafkaSource;
import org.apache.flink.connector.kafka.source.enumerator.initializer.OffsetsInitializer;
import org.apache.flink.streaming.api.datastream.DataStream;
import org.apache.flink.streaming.api.datastream.KeyedStream;
import org.apache.flink.streaming.api.environment.StreamExecutionEnvironment;
import org.apache.flink.streaming.api.windowing.assigners.ProcessingTimeSessionWindows;
import org.apache.flink.streaming.api.windowing.time.Time;

public class AnomalyDetectionJob {
	public static void main(String[] args) throws Exception {

		StreamExecutionEnvironment env = StreamExecutionEnvironment.getExecutionEnvironment();

		KafkaSource<Transaction> dataSource = KafkaSource.<Transaction>builder()
				.setBootstrapServers("kafka:29092")
				.setTopics("Transakcje")
				.setGroupId("transaction-group")
				.setStartingOffsets(OffsetsInitializer.earliest())
				.setValueOnlyDeserializer(new TransactionDeserializationSchema())
				.build();

		DataStream<Transaction> dataStream = env.fromSource(dataSource, WatermarkStrategy.noWatermarks(), "transaction-data-source");

		KeyedStream<Transaction, Integer> keyedCardTransactions = dataStream
				.keyBy(Transaction::getCardId);

		KeyedStream<Transaction, Integer> keyedUserTransactions = dataStream
				.keyBy(Transaction::getUserId);

		DataStream<Alert> valueOverTheLimitAlerts = keyedCardTransactions
				.process(new OverTheLimitDetector())
						.name("over-the-limit-alerts");

		DataStream<Alert> multipleTransactionsAlerts = keyedUserTransactions
				.process(new MultipleTransactionsAnomaly())
				.name("multiple-transactions-alerts");

		DataStream<Alert> generalOutlierAlerts = keyedCardTransactions
				.process(new GeneralOutlierDetector())
				.name("general-outlier-alerts");

		DataStream<Alert> locationChangeAlerts = keyedUserTransactions
				.window(ProcessingTimeSessionWindows.withGap(Time.seconds(3)))
				.process(new LocationChangeDetector())
				.name("location-change-alerts");

		DataStream<Alert> foundAlerts = valueOverTheLimitAlerts
				.union(generalOutlierAlerts)
				.union(multipleTransactionsAlerts)
				.union(locationChangeAlerts);

		KafkaSink<Alert> dataSink = KafkaSink.<Alert>builder()
						.setBootstrapServers("kafka:29092")
								.setRecordSerializer(KafkaRecordSerializationSchema.builder()
										.setTopic("Alerty")
										.setValueSerializationSchema(new AlertSerializationSchema())
										.build())
										.build();

		foundAlerts
				.sinkTo(dataSink)
						.name("alerts-sink");

		env.execute("Anomaly detection");
	}
}
