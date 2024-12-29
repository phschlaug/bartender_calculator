import React, {useState} from "react";
import {Tabs, Tab, Box, Container} from '@mui/material'
import ConfigForm from "./ConfigForm";
import Order from "./Order";

const TabsNavigation = ({ drinkPrices, updatePrices}) => {
    const [value, setValue] = useState(0);

    const handleChange = (event, newValue) => {
        setValue(newValue);
    };

    return (
        <Container>
          <Box sx={{ width: '100%', bgcolor: 'background.paper' }}>
            <Tabs value={value} onChange={handleChange} centered variant="fullWidth">
              <Tab label="Order Drinks" />
              <Tab label="Configure Prices" />
            </Tabs>
            <Box sx={{ padding: 2 }}>
              {value === 0 && <Order drinkPrices={drinkPrices} />}
              {value === 1 && <ConfigForm updatePrices={updatePrices} />}
            </Box>
          </Box>
        </Container>
      );
    };

export default TabsNavigation;