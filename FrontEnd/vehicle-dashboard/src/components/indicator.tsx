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
                off: 'grey',
                idle: 'blue',
                running: 'green',
                warning: 'orange',
                fault: 'red',
            },
            battery: {
                off: 'grey',
                charging: 'green',
                discharging: 'yellow',
                warning: 'orange',
                full: 'blue',
            },
            circleParking: {
                off: 'grey',
                on: 'green',
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
