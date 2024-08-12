<h1> Kafka Producer and Consumer Demo </h1>

<h3> Overview </h3>
<p> This project demonstrates a Kafka <stong>producer</stong> and <stong>consumer</stong> implemented using <stong>ASP.NET Core Web API</stong>. The producer publishes inventory updates to a Kafka topic, and 
  the consumer subscribes to the topic to consume and process these updates every 5 seconds.
</p>

<h3> Repository Structure </h3>
<ul>
  <li> <strong>Producer:</strong> An ASP.NET Core Web API that publishes inventory update messages to a Kafka topic.</li>
  <li> <strong>Consumer:</strong> An ASP.NET Core Web API that subscribes to a Kafka topic and processes messages every 5 seconds.</li>
</ul>

<h3> Prerequisites </h3>
<ul>
  <li>.NET SDK 8.0 or later</li>
  <li>Apache Kafka running on localhost or a specified server</li>
  <li>Confluent.Kafka NuGet package</li>
</ul>

<h5> <strong>Kafka Setup</strong></h5>
<p> Download and Install Kafka </p>
<p>Follow the instructions on the <a href="https://kafka.apache.org/quickstart">Apache Kafka website to download and set up Kafka</a>.</p>

<h5> <strong>Start Kafka Broker</strong></h5>
<p>Make sure the Kafka broker is running. You can start Kafka with the following commands:</p><p></p>
<div class="codehilite">
<pre><code> 
    # Start ZooKeeper
      zookeeper-server-start.sh config/zookeeper.properties
    # Start Kafka Broker
      kafka-server-start.sh config/server.properties
</code></pre>
</div>

<h5> <strong>Create Kafka Topic</strong></h5>
<p>Create the topic InventoryUpdate where the producer will publish messages and the consumer will subscribe:</p><p></p>
<div class="codehilite">
<pre><code> 
      kafka-topics.sh --create --topic InventoryUpdate --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1
</code></pre>
</div>

<h3> Testing </h3>
<h5>Verify Message Consumption</h5>
<p>Check the logs of the consumer application to verify that the message is being consumed every 5 seconds.</p>

<h3> Contact </h3>
<p>For questions or feedback, please contact <a href="mailto:athandar1998@gmail.com">athandar1998@gmail.com</a>.</p>
