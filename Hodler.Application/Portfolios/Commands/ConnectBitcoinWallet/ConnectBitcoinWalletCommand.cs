using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.ConnectBitcoinWallet;

public record ConnectBitcoinWalletCommand(
    UserId UserId,
    BitcoinAddress Address,
    string WalletName,
    BlockchainNetwork Network
) : IRequest<IResult>;