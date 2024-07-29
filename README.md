# C# Cloud AWS Day One

## Learning Objectives

- Understand how to deploy an API to an AWS Elastic Beanstalk
- Understand how to spin up an AWS Postgres instance

## Instructions

1. Fork this repository
2. Clone your fork to your machine

## Core Activity

For this exercise you should get a a backend and database working in AWS. You can create something simple and new or you may use an existing application, e.g. the cinema challenge and build a simple front end using technology you choose. You should build using the following approach:

- Back end API using C# and the `dotnet new webapi` template with at least 1 endpoint
- Database using PostgresSQL hosted in AWS
- Build you application `dotnet publish -c Release -o out`
- Compress your application 
- Deploying your Application to AWS follow the next steps

## Steps
### 1. Open the AWS Management Console.
- Navigate to the Elastic Beanstalk service.
- Click "Create Application."
- Enter the application name (e.g., MyApiApp).
- Choose the .NET platform.
- Click "Create application."

### 2. Upload and Deploy the Application:

- After creating the application, navigate to the "Environments" section.
- Click "Create environment."
- Choose "Web server environment."
- Enter the environment name (e.g., MyApiEnv).
- For the platform, select ".NET Core."
- Under "Application code," choose "Upload your code."
- Upload the MyApi.zip file.
- Click "Create environment."


## Extension 1
Adding of a Database for an extension mark
## Steps
### 1. Set Up Amazon RDS for PostgreSQL

- Create an RDS Instance:
- Open the AWS Management Console and navigate to the RDS service.
- Click "Create database."
- Choose the "Standard Create" option.
- Select the PostgreSQL engine.
- Configure the DB instance settings (DB instance identifier, master username, password).
- Choose the instance type and allocated storage.
- Configure Database Connectivity:
- In the "Additional configuration" section, configure the database name.
- Click "Create database."
- After the database is created, navigate to the "Connectivity & security" tab.
- Note the "Endpoint" and "Port."
- Configure the security group to allow access from your Elastic Beanstalk environment.

`appsettings.*.json`
```json
"ConnectionStrings": {
    "DefaultConnection": "Host=mydbinstance.endpoint;Database=mydatabase;Username=myadmin;Password=mypassword"
  }
```

## Package to Solution
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

## Code Changes
`startup.cs`
```csharp
services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

```

## Create DBContext
```csharp
public class MyModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<MyModel> MyModels { get; set; }
}

```

### EntityFramework Commands
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
