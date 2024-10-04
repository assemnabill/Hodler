using Hodler.Contracts.Shared;
using Hodler.Domain.Portfolio.Models;
using Mapster;
using Hodler.Contracts.Portfolio.PortfolioSummary;

namespace Hodler.ApiService.Mappings;

public class PortfolioSummaryRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<PortfolioSummary, PortfolioSummaryDto>()
            .MapWith(x => new PortfolioSummaryDto(
                x.FiatNetInvested.Adapt<FiatAmountDto>(),
                x.TotalBitcoin.Amount,
                x.CurrentBitcoinPrice.Adapt<FiatAmountDto>(),
                x.PortfolioValue.Adapt<FiatAmountDto>(),
                x.FiatReturnOnInvestment.Adapt<FiatAmountDto>(),
                x.FiatReturnOnInvestmentPercentage,
                x.AverageBitcoinPrice.Adapt<FiatAmountDto>(),
                x.TaxFreeFiatReturnOnInvestment.Adapt<FiatAmountDto>(),
                x.TaxFreeFiatReturnOnInvestmentPercentage
            ));
    }
}