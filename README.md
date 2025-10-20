# Practica1

This project is a C# web application for managing users and their physical activities. It includes validation utilities for NIF, IBAN, email, and phone numbers, a data layer for storing users and activities, internationalization support, and a web interface with login and dashboard.

## Cloning the Repository

```bash
git clone https://github.com/Joseleelsuper/VyP-Practica1.git
```

## Running the Tests

The project includes unit tests for the logic and database layers using MSTest.

To run the tests in Visual Studio:

1. Open the solution file in Visual Studio.
2. Build the solution.
3. Go to Test > Run All Tests or use the Test Explorer.

## Running the Web Application on IIS

The web application is located in the `www` folder.

To run it on IIS:

1. Open IIS Manager.
2. Create a new site or application.
3. Set the physical path to the `www` folder in the project directory.
4. Ensure the application pool is configured for .NET Framework 4.8.
5. Browse to the site URL.

For development in Visual Studio, you can open the `www` folder as a web site project.

Ensure that the Database and Logica projects are built and their assemblies are accessible to the web application.