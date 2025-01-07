## Installation

Before getting started, ensure that you have .NET SDK 8 installed.

To verify if you have the SDK 8 installed, run the following command in your terminal:

```
dotnet --version
```

## Quick Start

1. Clone this repository to your local machine:

```
git clone https://github.com/GitHubUser10F2C/Catedra3Backend.git
```

2. Navigate to the project folder:

```
cd Catedra3Backend
```

3. Restore the project dependencies:

```
dotnet restore
```

4. Create a migration and update:

```
dotnet ef migrations add InitialCreate
Dotnet ef database update
```

5. Run the project

```
dotnet run
```