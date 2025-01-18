using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace ExpensesControl.WebApi.Extensions;

/// <summary>
/// Extensões para configurar informações de localização e cultura.
/// </summary>
public static class CultureInfoExtensions
{
    const string defaultCulture = "pt-BR";
    const string secundaryCulture = "en-US";

    /// <summary>
    /// Cria uma instância de <see cref="RequestLocalizationOptions"/> configurada para as culturas suportadas.
    /// </summary>
    /// <remarks>
    /// Define as culturas padrão e secundárias, bem como as configurações de fallback para culturas e culturas de interface do usuário.
    /// </remarks>
    /// <returns>
    /// Uma instância de <see cref="RequestLocalizationOptions"/> configurada com as culturas suportadas e a cultura padrão.
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
