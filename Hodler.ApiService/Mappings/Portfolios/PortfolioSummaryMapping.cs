using Hodler.Contracts.Portfolios.PortfolioSummary;
using Hodler.Contracts.Shared;
using Hodler.Domain.Portfolios.Models;
using Mapster;

namespace Hodler.ApiService.Mappings.Portfolios;

public class PortfolioSummaryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<PortfolioSummaryInfo, PortfolioSummaryDto>()
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