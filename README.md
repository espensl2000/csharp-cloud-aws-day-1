# C# Cloud AWS - Day One

## Learning Objectives

- Understand how to deploy an API to AWS Elastic Beanstalk
- Understand how to spin up an AWS PostgreSQL instance
- Host frontend static files on AWS S3

## Instructions

1. Fork this repository.
2. Clone your fork to your machine.

# Core Activity

## Set Up Amazon RDS for PostgreSQL

### Steps

1. **Create an RDS Instance:**

   - Open the AWS Management Console and navigate to the RDS service.
   - Click "Create database."
   - Choose the "Standard Create" option.
   - Select the `PostgreSQL` engine specifically.
   - Under `Templates` choose `Free Tier`.
   - Under `Settings` assign a db name that includes your name; keep the default master username, choose `Self Managed` for credentials and type in a master password (make sure to save this somewhere).
   - Under `Instance configuration` choose `db.t3.micro`.
   - Keep the default settings for `Storage`.
   - Under `Connectivity` keep all default settings, except select `Yes` for `Public access`.
   - Keep default settings for `Tags` and `Dataabase authentication`.
   - Under `Monitoring` uncheck `Turn on Performance Insights`, thus disabling monitoring.
   - Click "Create database."

2. **Configure Database Connectivity:**

   - After the database is created, navigate to the "Connectivity & security" tab in the newly created database's dashboard. The database's dashboard can be accessed by visiting RDS in the AWS console, then clicking Databases so that you see the list of all databases, followed by clicking the name of the database for which you want to see the dashboard.
   - Make a note of the "Endpoint" and "Port" of the database, you will need these to create the DB connection string.

3. **Create `appsettings.json`:**
   - Create `appsettings.json` with the contents from `_example.appsettings.json`
   - Replace the `Host` with the AWS RDS Endpoint
   - Choose your own database name and assign to `Database`
   - Replace DB `User Id` and `Password` based on the root username and password you created for RDS

```json
   "ConnectionStrings": {
       "DefaultConnection": "Host=mydbinstance.endpoint;Database=mydatabase;User Id=myadmin;Password=mypassword"
   }
```

4. **Install Required Packages:**
   - The solution already comes with the required packages installed
   - Install the packages via: `dotnet restore` command

These are the packages that were added to the project:

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package newtonsoft.json
```

5. **Apply Entity Framework Migrations:**
   - To Run EntityFramework migration use the commands below
   - The sample project contains a Todo table, with relevant Db Context
   - The DB Context seeds 3 Todos when the database is updated
   - The API has 4 endpoints for CRUD operations on Todos
   - READ ALL: `GET /todos`
   - CREATE Todo: `POST /todos` with payload of `Title` and `Completed`
   - UPDATE Todo: `PUT /todos/:id` with payload of `Title` and `Completed`
   - DELETE Todo: `DELETE /todos/:id`

```bash
dotnet ef database update
```

## Deploy Backend API

### Steps
1. **Build API and Compress**
   - Publish the application:

```bash
dotnet publish -c Release -o out
```

- Compress the published files:

```bash
cd out
zip -r MyApi.zip .
```

2. **Open the AWS Management Console:**

   - Navigate to the Elastic Beanstalk service.
   - Click "Create Application."
   - Enter the application name (e.g., aws-day-1-{studentName}-api).
   - Description is Optional.
   - Click Create

3. **Create and Environment for you Application**
   - Click on **Create new Environment**.
   - Follow the steps **EXACTLY**.
      1. ### Configure environment
         - Environment tier choose **Web Server environment**.
         - Platform choose **.Net Core on Linux**.
         - Application Code pick **Upload your code**.
            - Then choose your local compress file.
            - Add your own version Label (e.g. v1, v1.1 etc).
         - Preset pick **High availability**
         - Click on **Next**
      2. ### Configure service access
         - Leave everything as it is.
         - No Changes to be made. 
         - Click on **Skip to review**
      3. ### Edit Step 4 
         - Root volume type **General Purpose 3(SSD)**.
         - Instance types remove **t3.small**. (Only **t3.micro** should be left.)
         - Click on **Skip to review**
      3. ### Edit Step 5 (Optional to run Development environment)
         - Environment properties Add a key value pair **ASPNETCORE_ENVIRONMENT: Development**.
         - Click on **Next**
   - Click on **Submit**

## Deploy Frontend UI

### Prerequisites

- AWS Account
- Frontend application built locally (e.g., React app)

### Steps

1. **Create an S3 Bucket:**

   - Open the AWS Management Console and navigate to the **S3**.
   - Click on the **Create bucket** button.
   - Enter a unique bucket name. (ex **aws-{Firstname+LastName-day-1**) `No Spaces`
   - Disable Block **Public Access setting for this bucket** (Tick that you Acknowledge this change)
   - Leave the default settings for the remaining options.
   - Click "Create bucket".

2. **Upload Frontend Files:**
   - Build your frontend application locally (assuming it’s a React app):

```bash
npm run build
```

- In the AWS Management Console, navigate to the S3 bucket you created.
- Click on the "Upload" button.
- Click "Add files" and select the files from the build folder.
- Click "Add folder" and select the **assests** folder from the build folder.
- Click "Upload" to upload the files to the S3 bucket.

3. **Configure Static Website Hosting:**

   - In the AWS Management Console, navigate to your S3 bucket.
   - Go to the "Properties" tab.
   - Scroll down to the "Static website hosting" section.
   - Click "Edit".
   - Enable static website hosting.
   - Set the index document name (e.g., index.html).
   - Click "Save changes".

4. **Set Bucket Policy for Public Access:**
   - Navigate to the "Permissions" tab of your S3 bucket.
   - Click on "Bucket Policy".
   - Add the following policy to allow public read access:
     json

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "PublicReadGetObject",
      "Effect": "Allow",
      "Principal": "*",
      "Action": "s3:GetObject",
      "Resource": "arn:aws:s3:::aws-day-1-ajdewilzin/*"
    }
  ]
}
```

- Replace your-bucket-name with the name of your S3 bucket.
- Click "Save".

5. **Access the Static Site:**
   - After configuring static website hosting, note the "Bucket website endpoint" URL provided in the static website hosting section.
