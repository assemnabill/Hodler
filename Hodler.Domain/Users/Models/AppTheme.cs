using System.ComponentModel;

namespace Hodler.Domain.Users.Models;

public enum AppTheme
{
    [Description("Dark")] Dark,
    [Description("Light")] Light,
    [Description("Systen")] System
}