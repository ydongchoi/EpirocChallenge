import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder, HttpTransportType, LogLevel } from '@microsoft/signalr';
import { VehicleData } from '../models/vehicle-data.interface';
import Gauge from './gauge';
import BatteryGauge from 'react-battery-gauge';
import { Indicator } from './indicator';
import { AppBar, Toolbar, Typography, Box, Grid, Slider, Card, CardContent, CardHeader, BottomNavigation, BottomNavigationAction } from '@mui/material';
import { Battery, Cog, Menu as MenuIcon, Omega, PlugZap } from 'lucide-react';

const marks = [
  { value: -1, label: 'Off' },
  { value: 0, label: '0' },
  { value: 1, label: '1' },
  { value: 2, label: '2' },
  { value: 3, label: '3' },
  { value: 4, label: '4' },
];

function valuetext(value: number) {
  return `${value}`;
}

// const apiUrl = process.env.NODE_ENV === 'production' 
// ? process.env.REACT_APP_VEHICLE_API_URL_PROD 
// : process.env.REACT_APP_VEHICLE_API_URL_DEV;

// const signalRUrl = process.env.NODE_ENV === 'production'
// ? process.env.REACT_APP_VEHICLE_SIGNALR_URL_PROD
// : process.env.REACT_APP_VEHICLE_SIGNALR_URL_DEV;

const requestCharging = async () => {
  try {
    const response = await fetch(`https://mining-vehicle.azurewebsites.net/api/vehicle/chargeBattery`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });

    console.log('request charging response: ', response);
    
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

  } catch (error) {
    console.error('Error fetching data: ', error);
    throw error;
  }
};

const requestSpeed = async (event: Event, speed: number) => {
  console.log('event' + event);
  try {
    const response = await fetch(`https://mining-vehicle.azurewebsites.net/api/vehicle/adjustSpeed`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ speed }),
    });

    console.log("request speed: ", speed);

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

  } catch (error) {
    console.error('Error fetching data: ', error);
    throw error;
  }
}

const VehicleDataReceiver: React.FC = () => {
  const [messages, setMessages] = useState<VehicleData>();
  const [connection, setConnection] = useState<any>(null);

  const motorStatusMap = ['off', 'idle', 'running', 'warning', 'fault'];
  const batteryStatusMap = ['off', 'charging', 'discharging', 'warning', 'full'];
  const breakStatusMap = ['off', 'on'];

  useEffect(() => {
    const fetchData = async () => {
      const newConnection = new HubConnectionBuilder()
        .withUrl(`https://mining-vehicle.azurewebsites.net/vehicleDataHub`, 
          {
            transport: HttpTransportType.WebSockets,
            skipNegotiation: false
          }
        )
        .configureLogging(LogLevel.Information)
        .withStatefulReconnect()
        .build();
      
        newConnection.serverTimeoutInMilliseconds = 300000;
        newConnection.keepAliveIntervalInMilliseconds = 300000;

      console.log('newConnection: ', newConnection);
      console.log(newConnection.baseUrl);
      console.log(newConnection.connectionId);

      setConnection(newConnection);
    };

    fetchData();
  },[]);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          console.log('Connection started');
        })
      .catch((error: any) => console.error('Connection failed: ', error));
    
      connection.on('ReceiveVehicleDataAsync', (message: VehicleData) => {
        connection.invoke('GetConnectionId').then((id: any) => {
          console.log('ConnectionId: ', id);
        });
  
        console.log('Received message: ', message);
        setMessages(message);
      });
    }
  }, [connection]);

  return (
    <div>
      <AppBar position="static">
        <Toolbar style={{ display: 'flex', justifyContent: 'space-around' }}>
          <Box display="flex" flexDirection="column" alignItems="center">
            <Indicator type="circleParking" value={breakStatusMap[Number(messages?.breakData?.status ?? 0)]} />
          </Box>
          <Box display="flex" flexDirection="column" alignItems="center">
            <Indicator type="light" value={motorStatusMap[Number(messages?.motorData?.status ?? 0)]} />
          </Box>
          <Box display="flex" flexDirection="column" alignItems="center">
            <Indicator type="battery" value={batteryStatusMap[Number(messages?.batteryData?.status ?? 0)]} />
          </Box>
        </Toolbar>
      </AppBar>

      <Grid container spacing={2} style={{ marginTop: '10px' }}>
        <Grid container item justifyContent='center' spacing={3}>
          <Grid item xs={12} sm={6} md={3} style={{ display: 'flex', justifyContent: 'center' }}>
            <Gauge id="kw" value={(messages?.batteryData.power || 0) / 1000} title="kW" min={-300} max={300} />
          </Grid>
          <Grid item xs={12} sm={6} md={3} style={{ display: 'flex', justifyContent: 'center' }}>
            <Gauge id="rpm" value={messages?.motorData.rpm || 0} title="Rpm" min={0} max={800} />
          </Grid>
        </Grid>

        <Grid container item spacing={2}>
          <Grid item xs={12} sm={6} md={2}>
            <Card style={{ height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <CardHeader title={<Typography variant="h6">Gear Ratio 1:6</Typography>} />
              <CardContent>
                <Cog size={100} />
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={2}>
            <Card style={{ height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <CardHeader title={<Typography variant="h6">Battery Level</Typography>} />
              <CardContent>
                <BatteryGauge value={(messages?.batteryData.percentage || 0) * 100} orientation='vertical' size={100} />
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={2}>
            <Card style={{ height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <CardHeader title={<Typography variant="h6">Battery Temperature</Typography>} />
              <CardContent>
                <BatteryGauge value={(messages?.batteryData.temperature || 0) * 100} orientation='vertical' size={100} />
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={6}>
            <Card style={{ height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <CardHeader title={<Typography variant="h6">Speed Control</Typography>} />
              <CardContent style={{ width: '90%' }}>
              <Slider
                aria-label="Custom marks"
                defaultValue={-1}
                getAriaValueText={valuetext}
                step={1}
                valueLabelDisplay="auto"
                marks={marks}
                min={-1}
                max={4}
                style={{ width: '100%' }}
                onChange={(event, value) => requestSpeed(event, value as number)}
              />
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      </Grid>

      <BottomNavigation style={{ justifyContent: 'space-between' }}>
        <BottomNavigationAction label="Gear" icon={<Cog />} />
        <BottomNavigationAction label="Engine" icon={<Omega />} />
        <BottomNavigationAction label="Menu" icon={<MenuIcon />} style={{ flexGrow: 1, textAlign: 'center' }} />
        <BottomNavigationAction label="BatteryTemperature" icon={<Battery />} />
        <BottomNavigationAction label="Charge" icon={<PlugZap onClick={requestCharging} />} />
      </BottomNavigation>
    </div>
  );
};

export default VehicleDataReceiver;
