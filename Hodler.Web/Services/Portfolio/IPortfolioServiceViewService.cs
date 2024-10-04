using Hodler.Contracts.Portfolio.PortfolioSummary;

namespace Hodler.Web.Services.Portfolio;

public interface IPortfolioServiceViewService
{
    event EventHandler<EventArgs> PortfolioSummaryChanged;

    PortfolioSummaryDto? PortfolioSummary { get; }
    
    Task InitPortfolioSummaryAsync();
}