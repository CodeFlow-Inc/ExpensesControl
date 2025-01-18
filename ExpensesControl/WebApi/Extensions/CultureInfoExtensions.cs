using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace ExpensesControl.WebApi.Extensions;

/// <summary>
/// Extensions for configuring localization and culture information.
/// </summary>
public static class CultureInfoExtensions
{
    // Default culture
    private const string defaultCulture = "pt-BR";

    // Secondary culture
    private const string secundaryCulture = "en-US";

    /// <summary>
    /// Creates an instance of <see cref="RequestLocalizationOptions"/> configured with the supported cultures.
    /// </summary>
    /// <remarks>
    /// Defines the default and secondary cultures, as well as fallback settings for cultures and UI cultures.
    /// </remarks>
    /// <returns>
    /// An instance of <see cref="RequestLocalizationOptions"/> configured with the supported cultures and the default culture.
    /// </returns>
    public static RequestLocalizationOptions CreateRequestLocalizationOptions()
    {
        var supportedCultures = new[]
        {
            new CultureInfo(defaultCulture),
            new CultureInfo(secundaryCulture),
        };

        var options = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            FallBackToParentCultures = false,
            FallBackToParentUICultures = false,
        };

        return options;
    }
}
