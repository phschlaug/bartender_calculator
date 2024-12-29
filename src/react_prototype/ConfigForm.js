import React, { useState, useEffect } from 'react';
import { TextField, Button, Box, Grid, IconButton, MenuItem, Select, InputLabel, FormControl, ListItemIcon } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import drinkIcons from './DrinkIcons';


const ConfigForm = ({ updatePrices }) => {
  const [drinkPrices, setDrinkPrices] = useState({});
  const [newDrink, setNewDrink] = useState('');
  const [newPrice, setNewPrice] = useState('');
  const [newIcon, setNewIcon] = useState('');
  const [editingDrink, setEditingDrink] = useState('');
  const [tempName, setTempName] = useState('');

  useEffect(() => {
    const savedPrices = JSON.parse(localStorage.getItem('drinkPrices')) || {};
    setDrinkPrices(savedPrices);
  }, []);

  const handlePriceChange = (e) => {
    const { name, value } = e.target;
    setDrinkPrices({ 
        ...drinkPrices, 
        [name]: { ...drinkPrices[name], price: parseFloat(value)},
    });
  };

  const handleIconChange = (drink, icon) => {
    setDrinkPrices({
        ...drinkPrices,
        [drink]: {...drinkPrices[drink], icon: icon},
    });
  };

  const handleNewDrinkChange = (e) => {
    setNewDrink(e.target.value);
  };

  const handleNewPriceChange = (e) => {
    setNewPrice(e.target.value);
  };

  const handleNewIconChange = (e) => {
    setNewIcon(e.target.value);
  };

  const handleNameEdit = (drink) => {
    setEditingDrink(drink);
    setTempName(drink);
  };

  const handleNameChange = (e) => {
    setTempName(e.target.value);
  };

  const saveNameChange = (oldName) => {
    const updatedPrices = {...drinkPrices};
    updatedPrices[tempName] = updatedPrices[oldName];
    delete updatedPrices[oldName];
    setDrinkPrices(updatedPrices);
    localStorage.setItem('drinkPrices', JSON.stringify(updatedPrices));
    updatePrices(updatedPrices);
    setEditingDrink('');
  };

  const handleNameBlur = (oldName) => {
    saveNameChange(oldName);
  };

  const handleNameKeyPress = (e, oldName) => {
    if (e.key === 'Enter'){
        saveNameChange(oldName);
    }
  };

  const addNewDrink = () => {
    if (newDrink && newPrice && !isNaN(newPrice) && newIcon) {
        setDrinkPrices({ 
            ...drinkPrices, 
            [newDrink]: { price: parseFloat(newPrice), icon: newIcon },
        });
        setNewDrink('');
        setNewPrice('');
        setNewIcon('');
    }
  };

  const savePrices = () => {
    localStorage.setItem('drinkPrices', JSON.stringify(drinkPrices));
    updatePrices(drinkPrices);
  };

  const removeDrink = (drink) => {
    const updatedPrices = { ...drinkPrices };
    delete updatedPrices[drink];
    setDrinkPrices(updatedPrices);
    localStorage.setItem('drinkPrices', JSON.stringify(updatedPrices));
    updatePrices(updatedPrices);
  };

  return (
    <Box>
      <h2>Configure Drink Prices</h2>
      <Grid container spacing={2}>
        {Object.keys(drinkPrices).map((drink) => (
          <Grid item xs={12} key={drink}>
            <Grid container spacing={2} alignItems="center">
              <Grid item xs={2}>
                {drinkIcons[drinkPrices[drink].icon]}
              </Grid>
              <Grid item xs={3}>
                {editingDrink === drink ? (
                  <TextField
                    fullWidth
                    value={tempName}
                    onChange={handleNameChange}
                    onBlur={() => handleNameBlur(drink)}
                    onKeyDown = {(e) => handleNameKeyPress(e, drink)}
                    autoFocus
                  />
                ) : (
                  <Button fullWidth onClick={() => handleNameEdit(drink)}>
                    {drink}
                  </Button>
                )}
              </Grid>
              <Grid item xs={3}>
                <TextField
                  fullWidth
                  label="Price"
                  type="number"
                  name={drink}
                  value={drinkPrices[drink].price}
                  onChange={handlePriceChange}
                />
              </Grid>
              <Grid item xs={3}>
                <FormControl fullWidth>
                  <InputLabel>Icon</InputLabel>
                  <Select
                    value={drinkPrices[drink].icon}
                    onChange={(e) => handleIconChange(drink, e.target.value)}
                    label="Icon"
                  >
                    {Object.keys(drinkIcons).map((iconKey) => (
                      <MenuItem key={iconKey} value={iconKey}>
                        <ListItemIcon>{drinkIcons[iconKey]}</ListItemIcon>
                        {iconKey}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>
              <Grid item xs={1}>
                <IconButton edge="end" aria-label="delete" onClick={() => removeDrink(drink)}>
                  <DeleteIcon />
                </IconButton>
              </Grid>
            </Grid>
          </Grid>
        ))}
        <Grid item xs={12} sm={4}>
          <TextField
            fullWidth
            label="New Drink"
            value={newDrink}
            onChange={handleNewDrinkChange}
          />
        </Grid>
        <Grid item xs={12} sm={4}>
          <TextField
            fullWidth
            label="Price"
            type="number"
            value={newPrice}
            onChange={handleNewPriceChange}
          />
        </Grid>
        <Grid item xs={12} sm={4}>
          <FormControl fullWidth>
            <InputLabel>Icon</InputLabel>
            <Select
              value={newIcon}
              onChange={handleNewIconChange}
              label="Icon"
            >
              {Object.keys(drinkIcons).map((iconKey) => (
                <MenuItem key={iconKey} value={iconKey}>
                  <ListItemIcon>{drinkIcons[iconKey]}</ListItemIcon>
                  {iconKey}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={12}>
          <Button fullWidth variant="contained" onClick={addNewDrink}>
            Add Drink
          </Button>
        </Grid>
        <Grid item xs={12}>
          <Button fullWidth variant="contained" color="primary" onClick={savePrices}>
            Save Prices
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
};



export default ConfigForm;
