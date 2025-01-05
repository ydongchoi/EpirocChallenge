# Epiroc Challenge

The Epiroc Challenge project simulates vehicle data transmission and visualization using modern web technologies and cloud services.

## Overview

- **Vehicle Specifications**: Each vehicle has detailed motor and battery specifications.
- **Data Transmission**: The Vehicle Emulator transmits vehicle data at a frequency of 10Hz using a SignalR client.
- **Data Handling**: SignalR receives the transmitted vehicle data and forwards it to the user interface (UI) in real-time.

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