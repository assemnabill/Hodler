using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Queries.ConnectedWallets;

public record ConnectedWalletsQuery(UserId UserId)
    : IRequest<IReadOnlyCollection<IBitcoinWallet>>;