# Project Name

## Description

This project, developed in ASP.NET Core using C# 13.0, serves as
a [briefly describe purpose: e.g., backend for a web application, API service, etc.]. The repository is built on the
.NET 9.0 framework, ensuring modern and efficient development practices tailored for scalable and robust web
applications.

## Features

- [Feature 1: e.g., Authentication and Authorization mechanism]
- [Feature 2: e.g., Secure data handling and storage]

## Prerequisites

Ensure you have the following installed:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download)

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/assemnabill/Hodler.git
    cd Hodler
    ```
2. Restore .NET dependencies:
    ```bash
    dotnet restore
    ```
3. Configure CoinDesk Api:

Update the `appsettings.json` file with your personal api key.
```json
{
   "ExternalApis": {
      "CoinDesk": {
         "ApiKey": "your_api_key_here"
      }
   }
}
```

## Usage

1. Run the application in the development mode:
    ```bash
    dotnet run
    ```

## Testing
Run unit tests using the following command:

```bash
  dotnet test
```

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature:
    ```bash
    git checkout -b feature-name
    ```
3. Commit your changes and push to your forked repository.
4. Open a Pull Request in this repository, describing your changes.

## License

This project is licensed under the [LICENSE NAME] license. See the `LICENSE` file for more details.

## Acknowledgments

- [List libraries, frameworks, or support resources you used in the project, e.g., ASP.NET Core, Entity Framework Core, etc.]

## Contact

For queries or collaborations, reach out at [your-email@example.com].
