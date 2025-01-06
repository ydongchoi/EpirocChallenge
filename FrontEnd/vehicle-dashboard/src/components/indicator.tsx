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

    const Icon = icons[type]

    return (
        <Box
            display="flex"
            justifyContent="center"
            alignItems="center"
            p={1}
            borderRadius={1}
            color={active ? 'red' : 'gray'}
        >
            <Icon
            style={{
                width: '30px',
                height: '30px',
                color: 'white',
            }}
            />
            {value !== undefined && (
            <Typography variant="body1" style={{ marginLeft: '4px', color: 'white' }}>
                {value}
            </Typography>
            )}
        </Box>
    )
}
