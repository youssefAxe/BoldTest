# Application Setup Guide

This application is built using .NET 8. Follow the instructions below to set up and run the application.

## Prerequisites

Before you start, ensure that you have the following:

- .NET 8 installed on your machine.
- Access to the SQL script required to add the necessary table.

## Setup Instructions

### 1. Execute SQL Script

The application requires a database table to be created before running. 

- Locate the `sqlscript` file in the repository.
- Execute the script on your SQL server to add the required table.

### 2. Run the Application

Once the database setup is complete, you can run the application. 

To access the app, use the following URL format in your web browser or HTTP client:

http://<ipaddressOrDOMAIN>:<port>/reporting?ReportPath=Test.rdl
- Replace `<ipaddressOrDOMAIN>` with the actual IP address or domain name of the server hosting the app.
- Replace `<port>` with the port number that the application is running on (e.g., `8080`).
- Ensure the `Test.rdl` report file is available and properly configured.

### 3. Example URL

For example, if your application is hosted on `192.168.1.100` on port `8080`, the URL to access the report would look like:
http://192.168.1.100:8080/reporting?ReportPath=Test.rdl
## Troubleshooting

- **Network Issues**: Make sure the IP address and port are accessible from your network.
- **Report Not Found**: Ensure the report file `Test.rdl` is in the correct directory and the path is specified correctly.

## License
