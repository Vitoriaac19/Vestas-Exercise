using System.Collections.Generic;

    public class SimulationService
    {
        // Function to simulate test data for the test bench
        // It generates a list of SensorData by incrementing rotation speed and simulating stress and temperature values
        public List<SensorData> SimulateTestBench(double initialRotationSpeed, double simulationTimeSeconds, double speedIncrement, double initialTemperature)
        {
            // Initialize a list to store the generated sensor data
            List<SensorData> sensorData = new List<SensorData>();

            // Variables to track the current rotation speed, temperature, and stress level
            double rotationSpeed = initialRotationSpeed;
            double temperature = initialTemperature;
            double stressLevel;

            // Define the starting time for the simulation (current time)
            DateTime startTime = DateTime.Now;

            // Simulate data every second for the duration of the simulation
            for (int i = 0; i < simulationTimeSeconds; i++)
            {
                // Calculate stress level based on rotation speed (simplified simulation)
                stressLevel = CalculateStressLevel(rotationSpeed);

                // Temperature increases gradually with rotation speed
                temperature += 0.01 * rotationSpeed;

                // Add the current sensor data entry with the timestamp
                sensorData.Add(new SensorData
                {
                    Timestamp = startTime.AddSeconds(i),  // Set the timestamp for each second of the simulation
                    RotationSpeed = rotationSpeed,  // Store the current rotation speed
                    StressLevel = stressLevel,  // Store the calculated stress level
                    Temperature = temperature  // Store the current temperature
                });

                // Increase rotation speed for the next iteration (simulating acceleration)
                rotationSpeed += speedIncrement;
            }

            // Return the generated list of sensor data
            return sensorData;
        }

        // Function to calculate stress level based on rotation speed
        // This is a simplified formula where stress increases with the square of the rotation speed
        public double CalculateStressLevel(double rotationSpeed)
        {
            // Simplified formula: stress increases quadratically with rotation speed (simulation)
            return 0.05 * Math.Pow(rotationSpeed, 2);
        }


        // Function to perform the simulation and save the generated data to a database
        // It uses a database manager to insert the simulated data into a SQLite database
        public void RunSimulationAndSave(double initialRotationSpeed, double simulationTimeSeconds,
                                                double speedIncrement, double initialTemperature,
                                                string databaseName = "bearing_tests.db")
        {
            // Simulate the sensor data by calling SimulateTestBench
            List<SensorData> simulatedData = SimulateTestBench(
                initialRotationSpeed,
                simulationTimeSeconds,
                speedIncrement,
                initialTemperature);

            // Create an instance of the database manager for interacting with the database
            BearingDatabase databaseManager = new BearingDatabase(databaseName);

            // Save the simulated data into the SQLite database
            databaseManager.InsertSensorData(simulatedData);
        }

        // Function to retrieve all sensor data from the database and display it
        // It returns the list of data and can be used to visualize the stored results
        public List<SensorData> RetrieveAndDisplayData(string databaseName = "bearing_tests.db")
        {
            // Create an instance of the database manager to access the data
            BearingDatabase databaseManager = new BearingDatabase(databaseName);

            // Retrieve all sensor data from the database and return it
            return databaseManager.GetAllData();
        }
    }
