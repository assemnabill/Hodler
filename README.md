# Hodler 🪙

## Description

This project, developed in ASP.NET Core using C# 13.0, serves as
a Bitcoin investment tracker that simplifies portfolio management for investors, offering seamless integration with popular exchanges, tax-aware insights, and real-time profit/loss analysis. 

We aim to empower Bitcoin holders with a secure and simple tool that demystifies complex tax regulations and provides clear, actionable financial intelligence.

## Features

- Self-Hosting: Take control of your own data by self hosting hodler in your home lab
- User Authentication: create accounts and log in securely
- All your transactions in one place: Track your transactions by adding, editing and removing them
- Data Import and Synchronization: Automatic import of transaction history from connected exchanges
- Portfolio Tracking: Real-time updates of BTC prices and portfolio values, total BTC holdings, overall profit/loss and much more
- Reporting: Provide transaction history and performance reports
- Beutiful and Simple: intuitive, user-friendly interface with clear visualizations of data and insights

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
