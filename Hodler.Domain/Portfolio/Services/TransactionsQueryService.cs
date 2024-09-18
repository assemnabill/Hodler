using System.Reflection;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.User.Models;
using Microsoft.Extensions.Logging;

namespace Hodler.Domain.Portfolio.Services;

public class TransactionsQueryService : ITransactionsQueryService
{
    private readonly ILogger<TransactionsQueryService> _logger;
    private readonly IBitPandaTransactionParser _bitPandaTransactionParser;
    private readonly IKrakenTransactionParser _krakenTransactionParser;
    private readonly ICurrentPriceProvider _currentPriceProvider;

    public TransactionsQueryService(
        ILogger<TransactionsQueryService> logger,
        IBitPandaTransactionParser bitPandaTransactionParser,
        IKrakenTransactionParser krakenTransactionParser,
        ICurrentPriceProvider currentPriceProvider)
    {
        _logger = logger;
        _bitPandaTransactionParser = bitPandaTransactionParser;
        _krakenTransactionParser = krakenTransactionParser;
        _currentPriceProvider = currentPriceProvider;
    }

    public async Task<PortfolioSummary> GetPortfolioSummaryAsync(
        UserId userId,
        CancellationToken cancellationToken)
    {
        // todo: get from portfolio repository
        var transactions = new Transactions(
            ParseBitPandaTransactions()
                .Concat(ParseKrakenTransactions())
                .Where(x => x.Type == TransactionType.Buy)
                .ToList()
        );

        // todo: call GetSummaryReportAsync on portfolio
        var summaryReport = await transactions.GetSummaryReportAsync(_currentPriceProvider, cancellationToken);

        return summaryReport;
    }

    public Task<ITransactions> GetTransactionsAsync(CancellationToken cancellationToken)
    {
        var transactions = new Transactions(
            ParseBitPandaTransactions()
                .Concat(ParseKrakenTransactions())
                .ToList()
        );

        return Task.FromResult<ITransactions>(transactions);
    }

    private ITransactions ParseBitPandaTransactions()
    {
        var transactionsStream = GetResourceAsStream("Hodler.Domain.Portfolio.Resources.bitpanda.csv");
        var transactionRecords = ReadTransactionsStream(transactionsStream!);

        return _bitPandaTransactionParser.ParseTransactions(transactionRecords);
    }

    private ITransactions ParseKrakenTransactions()
    {
        var transactionsStream = GetResourceAsStream("Hodler.Domain.Portfolio.Resources.kraken.csv");
        var transactionRecords = ReadTransactionsStream(transactionsStream!);

        return _krakenTransactionParser.ParseTransactions(transactionRecords);
    }

    private Stream? GetResourceAsStream(string resourceName)
    {
        var transactionsCsv = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(resourceName);

        if (transactionsCsv is null)
        {
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            _logger.LogWarning("Couldn't find resource {0}\nAvailable resources: {1}", resourceName,
                resources.Aggregate((a, b) => $"{a}, {b}"));
        }

        return transactionsCsv;
    }

    private List<string[]> ReadTransactionsStream(Stream transactionsStream)
    {
        using var reader = new StreamReader(transactionsStream);

        return reader
            .ReadToEnd()
            .Split('\n')
            .Skip(1)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(x => x.Split(','))
            .ToList();
    }
}