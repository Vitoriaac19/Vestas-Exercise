# Sensor Data Simulation for Test Bench Environment

This project simulates sensor data for a test bench environment. The data generated includes **rotation speed**, **stress level**, and **temperature** over a simulation time. This data is stored in an **SQLite database** and can be retrieved for analysis. The simulation provides a basic example of how to simulate and store time-series data in a relational database.

---

## 🚀 Features

- **Sensor Data Simulation**: Generates a list of simulated sensor data based on initial rotation speed, simulation time, speed increment, and temperature.
- **Database Storage**: Stores the generated sensor data into an SQLite database (`bearing_tests.db` by default).
- **Data Retrieval**: Retrieves stored data from the SQLite database for further analysis.
- **Stress Calculation**: Calculates the stress level based on the rotation speed using a simplified quadratic formula.

---

## 🛠 Classes and Methods

### `SimulationService`
This class contains the logic for simulating sensor data and managing the interaction with the database.

- **SimulateTestBench**: Simulates sensor data for a specified time period. It calculates rotation speed, stress level, and temperature at each second and stores them in a list.
- **CalculateStressLevel**: A simplified formula to calculate the stress level based on rotation speed. Stress increases quadratically with rotation speed.
- **RunSimulationAndSave**: Simulates the data and saves the results into the database.
- **RetrieveAndDisplayData**: Retrieves all sensor data from the database for display or analysis.

### `BearingDatabase`
This class manages the interaction with the SQLite database.

- **CreateDatabase**: Creates the SQLite database and the required table (`SensorData`) if they don't already exist.
- **InsertSensorData**: Inserts a single sensor data entry into the database.
- **InsertSensorData(List<SensorData>)**: Inserts multiple sensor data entries into the database using a transaction for performance.
- **GetAllData**: Retrieves all sensor data from the database.
- **GetDataByPeriod**: Retrieves sensor data within a specific time range.

### `SensorData`
This class holds the sensor data.

#### Properties:
- **Timestamp**: The timestamp when the data was collected.
- **RotationSpeed**: The rotation speed of the sensor in revolutions per minute (rpm).
- **StressLevel**: The stress level (force/torque) of the sensor in Newton-meters (N.m).
- **Temperature**: The temperature of the sensor in degrees Celsius (°C).

---

## 💻 How to Run the Code

1. Clone the repository and open the project in Visual Studio or your preferred IDE.
2. Ensure that you have the `System.Data.SQLite` library installed. You can install it via NuGet.
3. The program will automatically create the database file (`bearing_tests.db`) and the required table if they don't exist.
4. To run the simulation and save the generated data, call the `RunSimulationAndSave` method, passing the initial parameters (initial rotation speed, simulation time, speed increment, initial temperature).
5. To retrieve the stored data, use the `RetrieveAndDisplayData` method.

---

## ☁️ Scaling to a Cloud-Based Environment (Azure)

To scale this solution to a cloud-based environment, we would move from using a local **SQLite** database to using a more scalable database solution provided by **Azure**. Below are the steps and recommendations for scaling the solution:

### 1. Switch to a Cloud Database (Azure SQL Database)
**Problem with SQLite**: SQLite is a lightweight, file-based database suited for small-scale applications or local storage. However, it lacks scalability, fault tolerance, and multi-user capabilities.  
**Solution**: Migrate the database to **Azure SQL Database**, a fully managed relational database service. This provides automatic scaling, backups, and high availability.

#### Steps:
- Set up an **Azure SQL Database** instance.
- Modify the `BearingDatabase` class to use the Azure SQL Database connection string and SQL commands (e.g., `SqlConnection`, `SqlCommand` in C#).
- Update the **CreateDatabase** and **InsertSensorData** methods to use SQL Server-specific syntax and prepared statements to handle data storage in the cloud.

### 2. Implement Distributed Data Storage
If the volume of sensor data is large or continuous, consider using a **data lake** or **Azure Blob Storage** for raw data storage. This allows for easier management of large datasets.

#### Steps:
- Store sensor data in **Azure Blob Storage** as raw data (e.g., JSON or CSV files).
- Use **Azure Data Factory** or **Azure Logic Apps** to ingest and transform data before storing it in **Azure SQL Database** for further processing.

### 3. Azure Functions for Automation
Use **Azure Functions** to run the simulation in the cloud. Functions can be triggered by an event (e.g., a time-based trigger or HTTP request) to run the simulation and store the results in Azure SQL Database. This removes the need for a dedicated server and enables scaling based on demand.

### 4. Implement Scaling with Azure App Services
Azure **App Service** can host the application backend if a web-based interface is needed.  
Configure **auto-scaling** to adjust compute resources as the simulation load increases.  
Use **Azure Logic Apps** or **Azure Functions** to process large-scale simulations asynchronously.

### 5. Azure Monitoring and Alerts
Integrate **Azure Monitor** to track the performance of the simulation and database storage.  
Set up alerts for high resource usage or errors to ensure that the simulation runs smoothly in a cloud environment.

### 6. Cost Optimization
Use **Azure Reserved Instances** for the database to optimize costs.  
Store raw data in **Azure Blob Storage** and only move it to the SQL database when needed for processing or querying.

### 7. Security
Use **Azure Active Directory (AAD)** for authentication and authorization to access the Azure SQL Database and other resources.  
Enable **transparent data encryption (TDE)** and **managed backups** in **Azure SQL Database** to ensure data security and compliance.

---

## 📈 Conclusion

This solution can easily be scaled to a cloud-based environment using **Azure** services. By transitioning from a local SQLite database to **Azure SQL Database**, utilizing **Azure Functions** for automation, and integrating other **Azure services** for data management, you can build a scalable and reliable cloud-based simulation system for sensor data.

---

## 💬 Contact

For any questions, feel free to open an issue or contact me directly at [vitoriaac19@gmail.com](mailto:vitoriaac19@gmail.com).
