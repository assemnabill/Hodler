using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Shared.Results;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.SyncBitcoinWallet;

public record SyncBitcoinWalletCommand(
    BitcoinWalletId WalletId
) : IRequest<IResult>;