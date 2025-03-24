using Hodler.Contracts.Shared;

namespace Hodler.Contracts.Portfolios.PortfolioSummary;

public record PortfolioSummaryDto(
    FiatAmountDto FiatNetInvested,
    decimal TotalBitcoin,
    FiatAmountDto CurrentBitcoinPrice,
    FiatAmountDto PortfolioValue,
    FiatAmountDto FiatReturnOnInvestment,
    double FiatReturnOnInvestmentPercentage,
    FiatAmountDto AverageBitcoinPrice,
    FiatAmountDto TaxFreeFiatReturnOnInvestment,
    double TaxFreeFiatReturnOnInvestmentPercentage);