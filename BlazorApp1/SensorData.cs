    // Defines the SensorData class, which stores data collected from a sensor in a rotation simulation
    public class SensorData
    {
        // Property to store the timestamp (date and time) when the data was collected
        public DateTime Timestamp { get; set; }

        // Property to store the rotation speed of the sensor in revolutions per minute (rpm)
        public double RotationSpeed { get; set; }

        // Property to store the stress level (likely torque or force) of the sensor in Newton-meters (N.m)
        public double StressLevel { get; set; }

        // Property to store the temperature of the sensor in degrees Celsius (°C)
        public double Temperature { get; set; }

        // Overridden ToString() method to return a readable string representation of the sensor data
    }

