using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.DisconnectBitcoinWallet;

public record DisconnectBitcoinWalletCommand(BitcoinWalletId WalletId) : IRequest<IResult>;