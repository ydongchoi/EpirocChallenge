import React, { useEffect } from "react";
import { Chart } from "react-google-charts";

const styles = {
    dial: {
        width: `auto`,
        height: `auto`,
        color: "#000",
        border: "0.5px solid #fff",
        padding: "2px"
    },
    title: {
        fontSize: "1em",
        color: "#000"
    }
};

interface GaugeProps {
    id: string;
    value: number;
    title: string;
    min: number;
    max: number;
}

const Gauge: React.FC<GaugeProps> = ({ id, value, title, min, max }) => {
    useEffect(() => {
        // This effect will run whenever the value changes
    }, [value]);

    return (
        <div id={id} style={styles.dial}>
            <Chart
                height={300}
                chartType="Gauge"
                loader={<div>Loading Chart...</div>}
                data={[
                    ["Label", "Value"],
                    [title, value]
                ]}
                options={{
                    yellowFrom: max - (max - min) * 0.25,
                    yellowTo: max - (max - min) * 0.10,
                    redFrom: max - (max - min) * 0.10,
                    redTo: max,
                    minorTicks: 5,
                    min: min,
                    max: max,
                    animation: {
                        duration: 90,
                        easing: "inAndOut"
                    }
                }}
            />
        </div>
    );
};

export default Gauge;
