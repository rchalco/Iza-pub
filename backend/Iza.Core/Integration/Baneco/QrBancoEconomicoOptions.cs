using System;
using Microsoft.Extensions.Configuration;
using PlumbingProps.Config;

namespace Iza.Core.Integration.Baneco
{
    /// <summary>
    /// Configuracion del proxy qr-banco-economico. Se lee de la seccion <c>QrBancoEconomico</c>.
    /// <para>
    /// La API Key <b>no se versiona</b>: <c>appsettings.json</c> la deja vacia y el valor real llega por
    /// la variable de entorno <c>QrBancoEconomico__ApiKey</c>. Es la credencial completa del suscriptor
    /// ante el proxy, asi que nunca se registra en bitacora ni se devuelve en una respuesta.
    /// </para>
    /// </summary>
    public sealed class QrBancoEconomicoOptions
    {
        public const string SeccionConfig = "QrBancoEconomico";
        public const string VariableApiKey = "QrBancoEconomico__ApiKey";

        public string BaseUrl { get; private set; } = string.Empty;
        public string ApiKey { get; private set; } = string.Empty;
        public string Moneda { get; private set; } = "BOB";
        public string CodigoSucursal { get; private set; } = string.Empty;

        /// <summary>Vigencia del QR en dias. 30 = "el QR del mes".</summary>
        public int DiasVigencia { get; private set; } = 30;

        /// <summary>
        /// Dias antes del vencimiento en que la cache deja de entregar el QR y emite el siguiente.
        /// Evita que un QR se entregue el mismo dia en que deja de aceptar pagos.
        /// </summary>
        public int DiasMargenRenovacion { get; private set; } = 1;

        public int TimeoutSegundos { get; private set; } = 30;

        /// <summary>
        /// Cuenta de Baneco a usar cuando el suscriptor tiene varias concedidas (cabecera
        /// <c>X-Baneco-Account</c>). Vacio si opera una sola.
        /// </summary>
        public string CuentaBaneco { get; private set; } = string.Empty;

        private static readonly Lazy<QrBancoEconomicoOptions> Instancia = new(Cargar, true);

        /// <summary>Configuracion resuelta una sola vez por proceso. Reiniciar el servicio para recargarla.</summary>
        public static QrBancoEconomicoOptions Actual => Instancia.Value;

        private static QrBancoEconomicoOptions Cargar()
        {
            IConfigurationSection seccion = ConfigManager.GetConfiguration().GetSection(SeccionConfig);

            var opciones = new QrBancoEconomicoOptions
            {
                BaseUrl = (seccion["BaseUrl"] ?? string.Empty).Trim(),
                // Precedencia: variable de entorno sobre archivo. En despliegue el archivo va vacio.
                ApiKey = (Environment.GetEnvironmentVariable(VariableApiKey) ?? seccion["ApiKey"] ?? string.Empty).Trim(),
                Moneda = Texto(seccion["Moneda"], "BOB"),
                CodigoSucursal = (seccion["CodigoSucursal"] ?? string.Empty).Trim(),
                DiasVigencia = Entero(seccion["DiasVigencia"], 30, 1, 365),
                DiasMargenRenovacion = Entero(seccion["DiasMargenRenovacion"], 1, 0, 30),
                TimeoutSegundos = Entero(seccion["TimeoutSegundos"], 30, 5, 300),
                CuentaBaneco = (seccion["CuentaBaneco"] ?? string.Empty).Trim()
            };

            if (opciones.DiasMargenRenovacion >= opciones.DiasVigencia)
            {
                // Un margen mayor que la vigencia dejaria la cache siempre vencida: cada solicitud
                // emitiria un QR nuevo contra el banco.
                opciones.DiasMargenRenovacion = 0;
            }

            return opciones;
        }

        /// <summary>
        /// Falla temprano y con un mensaje accionable. Sin esto, una configuracion incompleta se
        /// manifestaria como un 401 del proxy en medio de una venta.
        /// </summary>
        public string? Validar()
        {
            if (string.IsNullOrWhiteSpace(BaseUrl))
                return $"Falta '{SeccionConfig}:BaseUrl' en appsettings.json.";

            if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                return $"'{SeccionConfig}:BaseUrl' no es una URL http(s) valida.";

            if (string.IsNullOrWhiteSpace(ApiKey))
                return $"Falta la API Key del proxy QR. Definala en la variable de entorno '{VariableApiKey}'.";

            if (!Guid.TryParse(ApiKey, out _))
                return $"La API Key del proxy QR debe ser un GUID; revise '{VariableApiKey}'.";

            if (string.IsNullOrWhiteSpace(Moneda))
                return $"Falta '{SeccionConfig}:Moneda' en appsettings.json.";

            return null;
        }

        /// <summary>Une <see cref="BaseUrl"/> con una ruta relativa sin depender de barras finales.</summary>
        public Uri Ruta(string rutaRelativa) =>
            new(new Uri(BaseUrl.EndsWith('/') ? BaseUrl : BaseUrl + "/"), rutaRelativa.TrimStart('/'));

        private static string Texto(string? valor, string porDefecto) =>
            string.IsNullOrWhiteSpace(valor) ? porDefecto : valor.Trim();

        private static int Entero(string? valor, int porDefecto, int minimo, int maximo)
        {
            if (!int.TryParse(valor, out int resultado)) return porDefecto;
            return resultado < minimo ? minimo : resultado > maximo ? maximo : resultado;
        }
    }
}
