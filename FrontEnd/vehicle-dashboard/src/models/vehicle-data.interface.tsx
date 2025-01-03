// Define the data types based on your C# models
interface BatteryData {
    capacity: number;
    charge: number;
    chargingRate: number;
    power: number;
    efficiency: number;
    percentage: number;
    status: string;  // Assuming BatteryStatus is a string, you can modify based on actual enum
    temperature: number;
  }
  
  interface BreakData {
    status: string | null;  // Assuming BreakStatus is a string, modify as necessary
  }
  
  interface MotorData {
    gearRatio: number;
    status: string;  // Assuming MotorStatus is a string, modify as necessary
    speed: number;
    rpm: number;
  }
  
  interface VehicleData {
    timestamp: string;  // Use Date if you prefer Date objects
    batteryData: BatteryData;
    breakData: BreakData;
    motorData: MotorData;
  }

  export type { VehicleData, BatteryData, BreakData, MotorData };
  