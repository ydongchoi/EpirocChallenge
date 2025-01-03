import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { VehicleData } from '../models/vehicle-data.interface';
import Gauge from './gauge';
import BatteryGauge from 'react-battery-gauge';
import { Indicator } from './indicator';
import { AppBar, Toolbar, Typography, Grid, Slider, Card, CardContent, CardHeader, BottomNavigation, BottomNavigationAction, Menu } from '@material-ui/core';
import Box from '@material-ui/core/Box';
import { Battery, Cog, MenuIcon, Omega, PlugZap, Thermometer } from 'lucide-react';

const marks = [
  {
    value: -1,
    label: 'Off'
  },
  {
    value: 0,
    label: '0',
  },
  {
    value: 1,
    label: '1',
  },
  {
    value: 2,
    label: '2',
  },
  {
    value: 3,
    label: '3',
  },
];

function valuetext(value: number) {
  return `${value}`;
}

const requestCharging = async () => {
  try {
    const response = await fetch('http://localhost:5140/api/vehicle/chargebattery', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const data = await response.json();
    return data;

  } catch (error) {
    console.error('Error fetching data: ', error);
    throw error;
  }
};

const VehicleDataReceiver: React.FC = () => {
  const [messages, setMessages] = useState<VehicleData>();
  const [connection, setConnection] = useState<any>(null);

  const motorStatusMap = ['off', 'idle', 'running', 'warning', 'fault'];
  const batteryStatusMap = ['off', 'charging', 'discharging', 'warning', 'full'];
  const breakStatusMap = ['off', 'on'];

  useEffect(() => {
    const connectToSignalR = async () => {
      const newConnection = new HubConnectionBuilder()
        .withUrl('http://localhost:5140/vehicleDataHub') // URL of the SignalR hub
        .build();

      newConnection.on('ReceiveVehicleDataAsync', (message: VehicleData) => {
        console.log('Received message: ', message.batteryData.power);
        setMessages(message);
      });

      try {
        await newConnection.start();
        console.log('Connected to SignalR Hub');
      } catch (err) {
        console.error('Error while starting connection: ', err);
      }

      setConnection(newConnection);

      // TODO: Stop Connection
    };

    connectToSignalR();

    // Cleanup function moved inside connectToSignalR

    return () => {
      // Clean up the connection on component unmount
      if (connection) {
        connection.stop();
      }
    };
  }, [connection]);

  return (
    <div>
      <AppBar position="static">
        <Toolbar style={{ display: 'flex', gap: '70px' }}>
          <Indicator type="circleParking" value={breakStatusMap[Number(messages?.breakData?.status ?? 0)]} />
          <Indicator type="light" value={motorStatusMap[Number(messages?.motorData?.status ?? 0)]} />
          <Indicator type="battery" value={batteryStatusMap[Number(messages?.batteryData?.status ?? 0)]} />
        </Toolbar>
      </AppBar>

      <Grid container>
        <Grid container item justifyContent='center' spacing={3} style={{ marginTop: '10px' }}>
          <Grid item style={{ marginRight: '100px' }}>
                <Gauge id="kw" value={(messages?.batteryData.power || 0) / 1000} title="kW" min={-300} max={300} />
          </Grid>
          <Grid item>
                <Gauge id="rpm" value={messages?.motorData.rpm || 0} title="Rpm" min={0} max={800} />
          </Grid>
        </Grid>

        <Grid container item style={{ marginTop: '10px' }}>
          <Grid item xs={12} sm={2} md={2} justifyContent='center'>
            <Card style={{ height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <CardHeader title={<Typography variant="h6">Gear Ratio</Typography>} />
              <CardContent>
                <Cog
                  size={100}
                />
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={2} md={2}>
            <Card style={{ height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <CardHeader title={<Typography variant="h6">Battery Level</Typography>} />
              <CardContent>
                <BatteryGauge
                  value={(messages?.batteryData.percentage || 0) * 100}
                  orientation='vertical'
                  size={100}
                />
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={2} md={2}>
            <Card style={{ height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <CardHeader title={<Typography variant="h6">Battery Temperature</Typography>} />
              <CardContent>
                <BatteryGauge
                  value={(messages?.batteryData.temperature || 0) * 100}
                  orientation='vertical'
                  size={100}
                />
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={6} justifyContent='center'>
            <Card style={{ height: '100%', flexDirection: 'column', alignItems: 'center' }}>
              <CardHeader title={<Typography variant="h6">Speed Control</Typography>} />
              <CardContent>
                <Slider
                  aria-label="Custom marks"
                  defaultValue={0}
                  getAriaValueText={valuetext}
                  step={1}
                  valueLabelDisplay="auto"
                  marks={marks}
                  min={-1}
                  max={4}
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


      {/* <div style={{ display: 'flex', gap: '30px', justifyContent: 'center' }}>
        <Gauge id="kw" value={(messages?.batteryData.power || 0) / 1000} title="kW" min={-300} max={300} />
        <Gauge id="rpm" value={messages?.motorData.rpm || 0} title="Rpm" min={0} max={800} />
      </div>

      <div style={{ display: 'flex', gap: '30px', justifyContent: 'center' }}>
        <BatteryGauge 
          value={(messages?.batteryData.percentage || 0) * 100} 
          orientation='vertical' 
          size={100} 
        />
        <PlugZap size={100} onClick={requestCharging} />
      </div> */}

    </div>
  );
};

export default VehicleDataReceiver;
