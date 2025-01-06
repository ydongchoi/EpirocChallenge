# Epiroc Challenge

The Epiroc Challenge project simulates vehicle data transmission and visualization using modern web technologies and cloud services.


## Overview

- **Vehicle Specifications**: Each vehicle has detailed motor and battery specifications.
- **Data Transmission**: The Vehicle Emulator transmits vehicle data at a frequency of 10Hz using a SignalR client.
- **Data Handling**: SignalR receives the transmitted vehicle data and forwards it to the user interface (UI) in real-time.


## Video Demonstration

https://github.com/user-attachments/assets/b76089d3-0efb-4ed7-bfe4-b86d76cd3638


## Flow

![image](https://github.com/user-attachments/assets/e305e702-f9a5-49e7-8f52-3ee4e7d58e07)

1. **Data Transmission**: The Vehicle Emulator sends vehicle data to the SignalR client at a frequency of 10Hz.
2. **Data Processing**: The SignalR Hub processes the incoming data and forwards it to the UI in real-time.
3. **Data Storage**: The SignalR Hub handles data storage.
4. **API Processing**: The API handles additional processing as needed.
5. **UI Update**: The UI updates the vehicle dashboard with the latest data.

This flow ensures real-time data visualization and efficient data handling for the Epiroc Challenge project.


## Specifications

### Motor Configuration
The motor configuration defines the key parameters for the vehicle's motor performance. Below is the JSON representation of the motor configuration:

```json
{
    "MotorRotation": 1,
    "WheelRotation": 6,  
    "NominalPower": 520000,
    "NominalTorque": 2200
}
```

- **MotorRotation**: The rotation speed of the motor.
- **WheelRotation**: The rotation speed of the wheels, which is typically a multiple of the motor rotation.
- **NominalPower**: The nominal power output of the motor in watts.
- **NominalTorque**: The nominal torque produced by the motor in Newton-meters.

### Battery Configuration
The battery configuration specifies the parameters for the vehicle's battery system. Below is the JSON representation of the battery configuration:

```json
{
    "Capacity": 675000,
    "Charge": 675000,
    "ChargingRate": 1000,
    "Efficiency": 0.9,
    "Temperature": 0
}
```

- **Capacity**: The total capacity of the battery in watt-hours.
- **Charge**: The current charge level of the battery in watt-hours.
- **ChargingRate**: The rate at which the battery can be charged in watts.
- **Efficiency**: The efficiency of the battery, represented as a decimal.
- **Temperature**: The current temperature of the battery in degrees Celsius.

These specifications can be modified in the `appsettings.json` file to adjust the vehicle's performance characteristics.


## Features

- **Vehicle Dashboard**: The UI presents a comprehensive dashboard displaying:
    - Battery power
    - Battery status
    - Battery percentage
    - Motor RPM
    - Motor status
    - Brake status
- **Navigation**: Additional menu buttons for navigation within the application.


## BackEnd

- **Framework**: .NET 8
- **API**: RESTful API for data handling
- **Real-time Communication**: Azure SignalR for real-time data transmission
- **Database**: Azure Cosmos DB configured for MongoDB API (Request Units)


## FrontEnd

- **Framework**: React
- **Language**: TypeScript
- **UI Library**: MUI (Material-UI)
- **Build Tool**: Vite for fast development and build processes


## CI/CD

- **Automation**: GitHub Actions for continuous integration and continuous deployment


## Deployment

- **Cloud Provider**: Azure for hosting and managing the application
- **Azure App Service**: Web App for API and SignalR Hub
- **Azure Static Web Apps**: For hosting the React frontend
- **Azure Cosmos DB**: For MongoDB account (Request Units)
- **Azure SignalR**: For real-time communication


## Setup Development

To set up the development environment for the Epiroc Challenge project, follow these steps:


### Git Clone

First, clone the repository to your local machine:

```bash
git clone https://github.com/your-repo/EpirocChallenge.git
cd EpirocChallenge
```

### Backend

Navigate to the backend directory and set up the .NET environment:

```bash
cd BackEnd/src/MiningVehicle/
```

Ensure you have the correct .NET version installed:
If not, install the latest .NET SDK from the official [Microsoft .NET download page](https://dotnet.microsoft.com/download).

```bash
dotnet --version
```
```bash
dotnet restore
```
```bash
dotnet build
```
```bash
dotnet run
```
This will restore the necessary packages, build the project, and run the backend server.

### Frontend

Navigate to the frontend directory and set up the React environment:

```bash
cd FrontEnd/vehicle-dashboard/
```

Ensure you have Node.js and npm installed:
If not, install Node.js and npm from the official [Node.js download page](https://nodejs.org/).
```bash
node -v
npm -v
```


```bash
npm install
```

```bash
npm run dev
```

This will install the required dependencies and start the development server for the frontend.


## Azure Services Configuration

To set up the necessary Azure services for the Epiroc Challenge project, follow these steps:

#### Azure SignalR

1. **Create SignalR Service**:
    - Navigate to the Azure portal.
    - Create a new SignalR service instance.
    - Choose the "Free" tier for development purposes.

2. **Retrieve Primary Connection String**:
    - Once the SignalR service is created, go to the "Keys" section.
    - Copy the "Primary Connection String".

3. **Configure SignalR in `appsettings.json`**:
    - Open the `appsettings.json` file in your backend project.
    - Add the SignalR connection string under the appropriate section.

#### Azure Cosmos DB for MongoDB

1. **Create Azure Cosmos DB Account**:
    - Navigate to the Azure portal.
    - Create a new Azure Cosmos DB account with the MongoDB API.
    - Choose the "Request Units" (RU) option for throughput.

2. **Retrieve Primary Connection String**:
    - Once the Cosmos DB account is created, go to the "Connection String" section.
    - Copy the "Primary Connection String".

3. **Create Database and Collection**:
    - In the Azure portal, navigate to your Cosmos DB account.
    - Create a new database named `MiningVehicle`.
    - Within this database, create a new collection named `VehicleData`.

4. **Configure Cosmos DB in `appsettings.json`**:
    - Open the `appsettings.json` file in your backend project.
    - Add the Cosmos DB connection string under the appropriate section.

By following these steps, you will have the necessary Azure services configured and integrated with your Epiroc Challenge project.


### Configuration

Ensure that the `appsettings.json` file is properly configured with your Azure services and other necessary settings.

By following these steps, you will have the development environment set up and ready for the Epiroc Challenge project.


## Testing

- **Backend Testing**: Use xUnit for writing and running tests.
- **Frontend Testing**: Use Jest for writing and running tests.

