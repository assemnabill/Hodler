using System.ComponentModel.DataAnnotations;

namespace Hodler.Web.Components.Pages.Transactions;

public class TransactionViewModel
{
    [Required] public DateTime Date { get; set; }

    [Required] public string Type { get; set; }

    [Required]
    [Range(0.00000001, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    public decimal Total { get; set; }
}