using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Shared.Results;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.ConnectBitcoinWallet;

public record ConnectBitcoinWalletCommand(
    UserId UserId,
    BitcoinAddress Address,
    WalletName WalletName,
    WalletAvatar Avatar
) : IRequest<IResult>;