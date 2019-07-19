# BasicConfiguration
This .NET project illustrates how to maintain a minimal configuration file and initialize sensitive and non-sensitive application settings.

## Introduction
We often use configuration (`app.config` or `web.config`) files to store .NET application settings that are not likely to change. As a result, we have to deal with the configuration files that are more bloated than they need to be. There must be a better way. And there is.

## Overview
Here is the basic idea:

- Keep the application settings that are not likely to change in defined in the application source code, BUT
- In the unlikely case that they do change before the application can be rebuilt, allow the application to read them from the configuration file.

This project illustrates how this can be done. It also shows how you can handle sensitive application settings, such as passwords, encription keys, and so on.

## Code

