using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

public class BearingDatabase
{
    private string _connectionString;
    private string _databasePath;

    // Constructor to initialize the database path and connection string
    public BearingDatabase(string databasePath = "bearing_tests.db")
    {
        _databasePath = databasePath;
        _connectionString = $"Data Source={databasePath};Version=3;";

        // Create the database and table if they do not exist
        CreateDatabase();
    }

    // Method to create the database and table if they do not exist
    private void CreateDatabase()
    {
        // Check if the database file already exists
        bool createNewTable = !File.Exists(_databasePath);

        // Create a new connection
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            // If the file did not exist, create the table
            if (createNewTable)
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS SensorData (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Timestamp TEXT NOT NULL,
                        RotationSpeed REAL NOT NULL,
                        StressLevel REAL NOT NULL,
                        Temperature REAL NOT NULL
                    );"
                ;

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Database and table successfully created.");
            }
        }
    }

    // Method to insert a single sensor data entry
    internal void InsertSensorData(SensorData data)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string insertQuery = @"
                INSERT INTO SensorData (Timestamp, RotationSpeed, StressLevel, Temperature)
                VALUES (@Timestamp, @RotationSpeed, @StressLevel, @Temperature);"
            ;

            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                // Add the data parameters to the SQL command
                command.Parameters.AddWithValue("@Timestamp", data.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@RotationSpeed", data.RotationSpeed);
                command.Parameters.AddWithValue("@StressLevel", data.StressLevel);
                command.Parameters.AddWithValue("@Temperature", data.Temperature);

                // Execute the insert command
                command.ExecuteNonQuery();
            }
        }
    }

    // Method to insert multiple sensor data entries at once
    internal void InsertSensorData(List<SensorData> dataList)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            // Use a transaction to improve performance when inserting multiple records
            using (var transaction = connection.BeginTransaction())
            {
                string insertQuery = @"
                    INSERT INTO SensorData (Timestamp, RotationSpeed, StressLevel, Temperature)
                    VALUES (@Timestamp, @RotationSpeed, @StressLevel, @Temperature);"
                ;

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    // Define parameters for the insert command
                    var timestampParam = command.Parameters.Add("@Timestamp", System.Data.DbType.String);
                    var rotationParam = command.Parameters.Add("@RotationSpeed", System.Data.DbType.Double);
                    var stressParam = command.Parameters.Add("@StressLevel", System.Data.DbType.Double);
                    var temperatureParam = command.Parameters.Add("@Temperature", System.Data.DbType.Double);

                    // Loop through each data entry and insert it into the database
                    foreach (var data in dataList)
                    {
                        timestampParam.Value = data.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                        rotationParam.Value = data.RotationSpeed;
                        stressParam.Value = data.StressLevel;
                        temperatureParam.Value = data.Temperature;

                        command.ExecuteNonQuery();
                    }
                }

                // Commit the transaction
                transaction.Commit();
            }

            Console.WriteLine($"{dataList.Count} records successfully inserted.");
        }
    }

    // Method to retrieve all stored sensor data
    internal List<SensorData> GetAllData()
    {
        List<SensorData> results = new List<SensorData>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT Timestamp, RotationSpeed, StressLevel, Temperature FROM SensorData ORDER BY Timestamp;";

            using (var command = new SQLiteCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    // Read each record from the database and add it to the results list
                    while (reader.Read())
                    {
                        SensorData data = new SensorData
                        {
                            Timestamp = DateTime.Parse(reader.GetString(0)),
                            RotationSpeed = reader.GetDouble(1),
                            StressLevel = reader.GetDouble(2),
                            Temperature = reader.GetDouble(3)
                        };

                        results.Add(data);
                    }
                }
            }
        }

        return results;
    }

    // Method to retrieve data within a specific time range
    internal List<SensorData> GetDataByPeriod(DateTime startDate, DateTime endDate)
    {
        List<SensorData> results = new List<SensorData>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string selectQuery = @"
                SELECT Timestamp, RotationSpeed, StressLevel, Temperature 
                FROM SensorData 
                WHERE Timestamp BETWEEN @StartDate AND @EndDate 
                ORDER BY Timestamp;"
            ;

            using (var command = new SQLiteCommand(selectQuery, connection))
            {
                // Add the parameters for the time range to the query
                command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd HH:mm:ss"));

                using (var reader = command.ExecuteReader())
                {
                    // Read each record that falls within the time range and add it to the results list
                    while (reader.Read())
                    {
                        SensorData data = new SensorData
                        {
                            Timestamp = DateTime.Parse(reader.GetString(0)),
                            RotationSpeed = reader.GetDouble(1),
                            StressLevel = reader.GetDouble(2),
                            Temperature = reader.GetDouble(3)
                        };

                        results.Add(data);
                    }
                }
            }
        }

        return results;
    }
}
