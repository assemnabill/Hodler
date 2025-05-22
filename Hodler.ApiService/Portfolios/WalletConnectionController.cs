using Hodler.Application.Portfolios.Commands.ConnectBitcoinWallet;
using Hodler.Application.Portfolios.Commands.DisconnectBitcoinWallet;
using Hodler.Application.Portfolios.Queries.ConnectedWallets;
using Hodler.Contracts.WalletConnections;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.Portfolios;

public class WalletConnectionController(IMediator mediator) : ApiController
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConnectBitcoinWalletAsync(
        [FromBody] ConnectBitcoinWalletRequest dto,
        CancellationToken cancellationToken = default
    )
    {
        var command = new ConnectBitcoinWalletCommand(
            UserId,
            new BitcoinAddress(dto.Address),
            dto.Name,
            new BlockchainNetwork(dto.ChainId)
        );

        var wallet = await mediator.Send(command, cancellationToken);
        var response = wallet.Adapt<ConnectedWalletDto>();

        return Created(
            Url.Action(nameof(ConnectBitcoinWalletAsync), new { walletId = response.Id }),
            response
        );

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