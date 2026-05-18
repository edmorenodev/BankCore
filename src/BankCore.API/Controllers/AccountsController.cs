using BankCore.Application.Accounts.Commands.OpenAccount;
using BankCore.Application.Accounts.Commands.TransferMoney;
using BankCore.Application.Accounts.Queries.GetAccountById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AccountsController : ControllerBase
{
    /*
        ISender es una interfaz de MediatR
        Sirve pare enviar commands y queries a sus handlers correspondientes
    */
    private readonly ISender _sender;

    public AccountsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> OpenAccount(
        [FromBody] OpenAccountCommand command,
        CancellationToken cancellationToken)
    {
        var accountId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = accountId }, new { id = accountId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var account = await _sender.Send(new GetAccountByIdQuery(id), cancellationToken);
        return Ok(account);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(
        [FromBody] TransferMoneyCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(new { transactionId = result });
    }
}