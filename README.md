# Epiroc Challenge

The Epiroc Challenge project simulates vehicle data transmission and visualization using modern web technologies and cloud services.

## Overview

- **Vehicle Specifications**: Each vehicle has detailed motor and battery specifications.
- **Data Transmission**: The Vehicle Emulator transmits vehicle data at a frequency of 10Hz using a SignalR client.
- **Data Handling**: SignalR receives the transmitted vehicle data and forwards it to the user interface (UI) in real-time.

## Video Demonstration

https://github.com/user-attachments/assets/9c96cdc8-f01c-4e7d-aeb5-64c91d0b1fc2

## Flow

![Vehicle Data Flow](https://github.com/user-attachments/assets/ba99e895-a16f-45de-9a2c-f91dc6dbf2be)

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
