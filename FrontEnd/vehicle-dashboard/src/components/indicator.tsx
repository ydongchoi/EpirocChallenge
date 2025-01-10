import { type FC } from 'react'
import { Battery, Lightbulb, Plug, CircleParking } from 'lucide-react'
import { Box, Typography } from '@mui/material'

interface IndicatorProps {
    type: 'battery' | 'circleParking' | 'light' | 'power'
    value?: string | number
    active?: boolean
}

export const Indicator: FC<IndicatorProps> = ({ type, value, active = true }) => {
    const icons = {
        battery: Battery,
        circleParking: CircleParking,
        light: Lightbulb,
        power: Plug,
    }

    const getColor = () => {
        const colorMap: { [key: string]: { [key: string]: string } } = {
            light: {
            off: '#B0BEC5', // grey
            idle: '#42A5F5', // blue
            running: '#66BB6A', // green
            warning: '#FFCA28', // orange
            fault: '#EF5350', // red
            },
            battery: {
            off: '#B0BEC5', // grey
            charging: '#66BB6A', // green
            discharging: '#FFEB3B', // yellow
            warning: '#FFCA28', // orange
            full: '#42A5F5', // blue
            },
            circleParking: {
            off: '#B0BEC5', // grey
            on: '#66BB6A', // green
            },
        }

        return colorMap[type]?.[value as string]
    }

    const Icon = icons[type]

    return (
        <Box
            alignItems="center"
            borderRadius={1}
            color={active ? 'red' : 'gray'}
            display="flex"
            justifyContent="center"
            p={1}
            position={'fixed'}
        >
            <Icon
                style={{
                    width: '30px',
                    height: '30px',
                    color: getColor(),
                }}
            />
            {value !== undefined && (
                <Typography variant="body1" style={{ marginLeft: '4px', color: getColor() }}>
                    {value}
                </Typography>
            )}
        </Box>
    )
}
