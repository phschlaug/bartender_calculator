# Bartender Calculator

<p align="center" width="100%">
    <img width="33%" src="./img/bartender_calculator_icon.png">
</p>

The Bartender App is a user-friendly tool designed to streamline the workflow for bartenders, making it easier to manage and process drink orders with efficiency and accuracy.This app simplifies the process of managing drink orders, reducing errors, and speeding up service, which enhances the overall efficiency of bartenders.

Fast and Easy Checkout at Festivals – With Our App! 🎡💰
Our app makes checkout seamless at festivals, markets, and events! 🚀

✔ Manage Products – Create and store items with custom prices.

✔ Easy Order Processing – Select products with a tap and create orders in seconds.

✔ Automatic Price Calculation – The total price is displayed instantly – no manual calculations needed!

✔ Fast and Efficient – Reduce waiting times and streamline your sales process.

Make checkout stress-free and focus on your customers! Download now and get started! 🎉

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![GitHub Issues](https://img.shields.io/github/issues/phschlaug/bartender_calculator)](https://github.com/phschlaug/artender_calculator/issues)

## Table of content

- [Bartender Calculator](#bartender-calculator)
  - [Table of content](#table-of-content)
    - [Summary](#summary)
  - [MAUI - App](#maui---app)
    - [Appium Tests](#appium-tests)
      - [Good to Know](#good-to-know)
    - [Useful Links](#useful-links)
  - [React Prototype](#react-prototype)
    - [Required  Software](#required--software)
    - [Technical Summary](#technical-summary)
    - [How to run App on an Android Device](#how-to-run-app-on-an-android-device)

### Summary

The initial prototype was developed using React and Capacitor to get a first understanding how such an app could look like.
All relevant information of the prototype can be found [here](#react-prototype).
The Android App was developed using the [MAUI-Framework](https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui?view=net-maui-9.0) from Microsoft using C#.
All relevant information can be found [here](#maui---app).

## MAUI - App

This is an approach to implement the [React Prototype](#react-prototype) with C# and the MAUI-Framework.

### Appium Tests

To be able to run Appium tests Appium needs to be installed via NPM.
Please install it using below command:

```shell
npm install -g appium
```

Besides installing Appium locally to be able to run the tests a driver is also needed, to install the used one use the below command:

```shell
appium driver install uiautomator2
```

Please be aware that the test might only run on an english UI.

#### Good to Know

With the help of the Appium Inspector it is possible to check the UI to do so first start the Appium server then start afterward start the Appium Inspector and check the configuration.
Here is an example

```json
{
  "platformName": "Android",
  "platformVersion": "14.0",
  "deviceName": "emulator-5554",
  "app": "/path/to/your/app.apk",
  "automationName": "UIAutomator2"
}
```

### Useful Links

- [Material Google Icons](https://fonts.google.com/icons?icon.size=24&icon.color=%235f6368&icon.platform=web)

## React Prototype

This can be found in the [react_prototype](src/react_prototype/)

### Required  Software

- [Android Studio](https://developer.android.com/studio)
- [Visual Studio Code](https://code.visualstudio.com)
- [Capacitor](https://capacitorjs.com)

### Technical Summary

- __Frontend Framework:__ React
- __Mobile Deployment:__ Capacitor(for running the React app on Android devices)
- __UI Components:__ Material-UI consistent and visually appealing design
- __Local Storage:__ Use local storage to persists drink configuration and prices

### How to run App on an Android Device

To run this app on an Android device capacitor is used, to be able to run this tiny app on an Android device follow the below commands

```shell
# Build the application
npm run build

# Copy code to the Android folders
npx cap copy

# Run it with Android Studio
npx cap open android
```

This three commands are combined into the npm script called 'run-on-android', so the easiest way is to run the following command:

```shell
npm run run-on-android
```
