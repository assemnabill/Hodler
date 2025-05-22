using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.SyncBitcoinWallet;

public record SyncBitcoinWalletCommand(
    BitcoinWalletId WalletId
) : IRequest<IResult>;