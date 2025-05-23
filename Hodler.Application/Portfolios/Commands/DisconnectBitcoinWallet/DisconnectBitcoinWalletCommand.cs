using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Shared.Results;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.DisconnectBitcoinWallet;

public record DisconnectBitcoinWalletCommand(BitcoinWalletId WalletId) : IRequest<IResult>;