import './App.css';
import TabsNavigation from './TabsNavigation';
import React, {useState, useEffect} from 'react';

function App() {
  const [drinkPrices, setDrinkPrices] =useState({})

  useEffect(()=> {
    const savedPrices = JSON.parse(localStorage.getItem('drinkPrices')) || {};
    setDrinkPrices(savedPrices);
  }, []);

  const updatePrices = (newPrices) => {
    setDrinkPrices(newPrices);
  }

  return (
    <div>
      <TabsNavigation drinkPrices={drinkPrices} updatePrices={updatePrices}/>
    </div>
  );
}

export default App;
