# FULLSTACK MONGODB E-COMMERCE APPLICATION

This project involves designing and implementing the wpf frontend of an online sports shoe store, focusing on the purchasing process. 
The application needs to allow users to browse products,  view detailed product information, manage a shopping cart, and complete the payment process.

The main objective of this project is the use of the no-sql database MongoDb with C# and study the functionalities no-sql databases.

## Aplication Functionalities
- Product Browse: Users can search for products by category (a hierarchical tree defined in the database), product name, price range, or size. Products are displayed in a paginated grid, showing variations in color, price, and discounts.

- Product Detail: A dedicated page for each product displays up to four images, an HTML description, and options to select colors and available sizes (based on stock).
Shopping Cart: The cart summarizes selected products, quantities, sizes, and colors, detailing line item prices, discounts, shipping costs, and the total. Shipping costs are determined by user-selected methods, with varying prices, minimum order thresholds for free shipping, and associated VAT types.

- Payment: Users will log in to access their personal and shipping/billing details. Payment is made via credit card, with validation of the card type and number. Upon successful payment, a confirmation message is displayed, and an invoice is sent via email.

- Invoice Management: Invoices must be printable at any time, with all economic data (quantities, prices, discounts, VAT percentages) frozen at the time of purchase. VAT types and percentages are defined in the database. Store identification data for invoices are also stored in the database for flexibility.


## Project Structure

### StoreFrontDb
This project/module holds the database connection context and client. It facilitates the data connection between mongodb and the rest of the modules. Data is supplied to the connection parameters through appsettings.json
with the following syntax  
```{
  "ConnectionStrings": {
    "storefront": "mongodb://username:password@ipaddress:port"
  }
}
````

### StoreFrontModel
A C# module that holds all the business model classes to be used by the application. Since they are to be used by mongodb, they must be annotated with mongodb annotations depending on their purpose.

### StoreFrontRepository
Module responsible for using the db connection to make db queries and forward the result to the rest of the application. It consists of an interface and its implemantation

### StoreFrontUi
A c# wpf project that defines the interface that the user will use to interact with our backend service.


## Tech Stack
### MongoDb. 
As this project is mainly focused on learning a non relational database, mongodb was the obvious choice due to its evergrowing popularity and relative ease of use. The database server is hosted in a docker container

### Jasper Reports
A jasper reports server running in a docker container is used to create invoices for every purchase and emailed to the client using smpt in C#

### Wpf
Since its a .net application the obvious choice of frontend is wpf. Winforms is old and has no xaml support, uwp is slow and sandboxed. 

### Docker
The application database server and the report server are hosted in docker. The following commands were used

```
docker network create botiga
docker volume create --name mariadb_data
docker run -d --name mariadb `
  -p 7777:3306 `
  --env ALLOW_EMPTY_PASSWORD=yes `
  --env MARIADB_USER=bn_jasperreports `
  --env MARIADB_PASSWORD=bitnami `
  --env MARIADB_DATABASE=bitnami_jasperreports `
  --network botiga `
  --volume mariadb_data:/bitnami/mariadb `
  bitnami/mariadb:latest

docker volume create --name jasperreports_data
docker run -d --name jasperreports `
  -p 8080:8080 `
  --env JASPERREPORTS_DATABASE_USER=bn_jasperreports `
  --env JASPERREPORTS_DATABASE_PASSWORD=bitnami `
  --env JASPERREPORTS_DATABASE_NAME=bitnami_jasperreports `
  --network botiga `
  --volume jasperreports_data:/bitnami/jasperreports `
  bitnami/jasperreports:latest

credentials
8080
jasperadmin
bitnami


docker run -d --name storefront `
  -p 27017:27017 `
  -v E:\Documents\GradoSuperiorDam2\Proyecto2\MongoDb\Db:/data/db `
  -e MONGO_INITDB_ROOT_USERNAME=[mongouser] `
  -e MONGO_INITDB_ROOT_PASSWORD=[mongopassword] `
  --network botiga `
  mongo:latest


credentials
27017
[mongosuser]
[mongopassword]

mongodb://mongouser:[mongopassword]@localhost:27017/StoreFront?authSource=admin

```
In the root folder of this repository under dbCollection I have provided the json files of the coolections used in this program.
In order for the frontend to send mails we need to pass smtp parameters and download the reports from jasper we do that through serversettings.json

```

﻿{
  "JasperSettings": {
    "ServerUrl": "http://localhost:8080/jasperserver",
    "Username": "jasperadmin",
    "Password": "bitnami",
    "ReportPath": "/StoreFrontReports/storefrontInvoice",
    "ParameterName": "invoiceNum"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "SenderEmail": "",
    "SenderPassword": "",
    "UseSSL": true
  }
}


```

## Sample Demo

[![Watch the video](https://img.youtube.com/vi/bpE_u2WnqWo/maxresdefault.jpg)](https://youtu.be/bpE_u2WnqWo)

### [Watch this video on YouTube](https://youtu.be/bpE_u2WnqWo)

  
