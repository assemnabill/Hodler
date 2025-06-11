using Hodler.Application.Portfolios.Commands.ConnectBitcoinWallet;
using Hodler.Application.Portfolios.Commands.DisconnectBitcoinWallet;
using Hodler.Application.Portfolios.Queries.ConnectedWallets;
using Hodler.Contracts.Portfolios.Wallets;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.Portfolios;

[Route("api/Portfolio/[controller]")]
public class WalletsController(IMediator mediator) : ApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ConnectedWalletsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> RetrieveConnectedWalletsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var wallets = await mediator.Send(
            new ConnectedWalletsQuery(UserId),
            cancellationToken
        );

        var dto = wallets.Adapt<ConnectedWalletsDto>();

        return Ok(dto);
    }

    [HttpPost("connect")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConnectBitcoinWalletAsync(
        [FromBody] ConnectBitcoinWalletRequest dto,
        CancellationToken cancellationToken = default
    )
    {
        var command = new ConnectBitcoinWalletCommand(
            UserId,
            new BitcoinAddress(dto.Address),
            new WalletName(dto.Name),
            new WalletAvatar(new WalletIcon(dto.Icon), new WalletColor(dto.Color))
        );

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Failures);

    }

    [HttpDelete("disconnect/{walletId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DisconnectWalletAsync(
        [FromRoute] Guid walletId,
        CancellationToken cancellationToken = default
    )
    {
        var command = new DisconnectBitcoinWalletCommand(new BitcoinWalletId(walletId));
        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.Failures);
    }
}