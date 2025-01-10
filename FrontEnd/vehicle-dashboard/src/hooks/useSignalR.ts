import { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { VehicleData } from '../models/vehicle-data.interface';

const useSignalR = (hubUrl: string): { messages: VehicleData | undefined } => {
    const [messages, setMessages] = useState<VehicleData>();
    const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl(
                hubUrl,
                {
                    transport: signalR.HttpTransportType.WebSockets,
                    skipNegotiation: false
                }
            )
            .configureLogging(signalR.LogLevel.Information)
            .withStatefulReconnect()
            .build();
        
        console.log("newConnection: ", newConnection);

        setConnection(newConnection);
    }, [hubUrl]);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    console.log('Connected to SignalR hub');

                    connection.on('ReceiveVehicleDataAsync', (data) => {
                        setMessages(data);
                    });

                    setInterval(() => {
                        connection.invoke('PingAsync').then(() => 
                            console.log('Ping sent')
                        );
                    }, 20000);
                })
                .catch((err) => console.error('Connection failed: ', err));
                  
            return () => {
                connection.stop().then(() => console.log('Disconnected from SignalR hub'));
            };
        }
    }, [connection]);

    return { messages };
};

export default useSignalR;