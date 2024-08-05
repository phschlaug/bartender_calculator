# Bartender Calculator

<p align="center" width="100%">
    <img width="33%" src="./img/cask.png">
</p>

The Bartender App is a user-friendly tool designed to streamline the workflow for bartenders, making it easier to manage and process drink orders with efficiency and accuracy. It is built using React and can be deployed as a mobile application on Android devices using Capacitor.
This app simplifies the process of managing drink orders, reducing errors, and speeding up service, which enhances the overall efficiency of bartenders.

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![GitHub Issues](https://img.shields.io/github/issues/phschlaug/bartender_calculator)](https://github.com/phschlaug/artender_calculator/issues)

## Table of content

- [Bartender Calculator](#bartender-calculator)
  - [Table of content](#table-of-content)
  - [Required  Software](#required--software)
  - [Technical Summary](#technical-summary)
  - [How to run App on an Android Device](#how-to-run-app-on-an-android-device)
  - [Usage](#usage)
  - [Useful Links](#useful-links)

## Required  Software

- [Android Studio](https://developer.android.com/studio)
- [Visual Studio Code](https://code.visualstudio.com)
- [Capacitor](https://capacitorjs.com)

## Technical Summary

- __Frontend Framework:__ React
- __Mobile Deployment:__ Capacitor(for running the React app on Android devices)
- __UI Components:__ Material-UI consistent and visually appealing design
- __Local Storage:__ Use local storage to persists drink configuration and prices

## How to run App on an Android Device

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

## Usage

ToDo: Some code example how to use it.

## Useful Links

- [Icon Generator](https://www.flaticon.com)
