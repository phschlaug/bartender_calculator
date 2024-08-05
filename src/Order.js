import React, { useState } from "react";
import {
  Button,
  Box,
  Typography,
  Grid,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  ListItemIcon,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import drinkIcons from "./DrinkIcons";

const Order = ({ drinkPrices }) => {
  const [order, setOrder] = useState([]);

  const addDrink = (drink) =>{
    if (drink && drinkPrices[drink] && drinkPrices[drink].icon) {
      const existingPatchIndex = order.findIndex((patch) => patch.drink === drink);
      if (existingPatchIndex !== -1) {
        const updatedOrder = [...order];
        updatedOrder[existingPatchIndex].count++;
        setOrder(updatedOrder);
      } else {
        setOrder([...order, { drink, count: 1 }]);
      }
    } else {
      console.error("Invalid drink or missing icon:", drink);
    }
  };
  

  const removeDrink = (drink) => {
    const existingPatchIndex = order.findIndex((patch) => patch.drink === drink);
    if(existingPatchIndex !== -1){
      const updatedOrder = [...order];
      if(updatedOrder[existingPatchIndex].count > 1){
        updatedOrder[existingPatchIndex].count --;
      }else{
        updatedOrder.splice(existingPatchIndex, 1);
      }
      setOrder(updatedOrder);
    }
  };

  const clearOrder = () => {
    setOrder([]);
  };

  const getTotalPrice = () => {
    let totalPrice = 0;
    order.forEach(({ drink, count }) => {
      totalPrice += (drinkPrices[drink]?.price || 0) * count;
    });
    return totalPrice;
  };


  return (
    <Box>
      <Typography variant="h2">Order Drinks</Typography>
      <Grid container spacing={2}>
        {Object.keys(drinkPrices).map((drink) => (
          <Grid item xs={6} sm={4} md={3} key={drink}>
            <Button
              fullWidth
              variant="contained"
              onClick={() => addDrink(drink)}
              startIcon={drinkIcons[drinkPrices[drink]?.icon || '']}
            >
              {drink}
            </Button>
          </Grid>
        ))}
      </Grid>
      <Typography variant="h4">Current Order</Typography>
      <br />
      <Typography variant="h6">Total: ${getTotalPrice().toFixed(2)}</Typography>
      <Button variant="contained" color="secondary" onClick={clearOrder}>
        Clear
      </Button>
      <List>
        {order.map(({ drink, count }, index) => (
          <ListItem key={index} component="div" onClick= {() => removeDrink(drink)}>
            <ListItemIcon>{drinkIcons[drinkPrices[drink]?.icon]}</ListItemIcon>
            <ListItemText 
              primary={`${drink} x${count}`} 
              secondary={`$${(drinkPrices[drink]?.price * count).toFixed(2)}`}
            />

            
            <ListItemSecondaryAction>
              <IconButton
                edge="end"
                aria-label="delete"
                onClick={(e) => {
                  e.stopPropagation(); // Prevent triggering the parent onClick
                  removeDrink(drink);
                }} 
              >
              <DeleteIcon />
            </IconButton>
            </ListItemSecondaryAction>
          </ListItem>
        ))}
      </List>
    </Box>
  );
};

export default Order;
