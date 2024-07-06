using Invoice.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoice.Core.Business.InvoiceKind.BasicExpenses
{
    [Serializable]
    [XmlRoot("cabecera")]
    public class HeaderServicioBasico : IHeaderFacturable
    {
        /// <summary>
        /// Número de NIT registrado en el Padrón Nacional de Contribuyentes que corresponde a la persona o empresa que emite la factura.
        /// SI
        /// </summary>
        public long nitEmisor { get; set; }

        /// <summary>
        /// Razón Social o nombre registrado en el Padrón Nacional de Contribuyentes de la persona o empresa que emite la factura
        /// SI
        /// </summary>
        public string razonSocialEmisor { get; set; }

        /// <summary>
        /// Lugar registrado en el Padrón Nacional de contribuyentes.
        /// SI
        /// </summary>
        public string municipio { get; set; }

        /// <summary>
        /// Teléfono registrado en el Padrón Nacional de contribuyentes
        /// NO
        /// </summary>
        public string telefono { get; set; }

        /// <summary>
        /// Número asignado a la factura.
        /// SI
        /// </summary>
        public int numeroFactura { get; set; }

        /// <summary>
        /// Código único de facturación (CUF) debe ser generado por el emisor siguiendo el algoritmo indicado.
        /// SI
        /// </summary>
        public string cuf { get; set; }
        /// <summary>
        /// Código único de facturación diario (CUFD), valor único que se obtiene al consumir el servicio web correspondiente.
        /// SI
        /// </summary>
        public string cufd { get; set; }

        /// <summary>
        /// Código de la sucursal registrada en el Padrón y en la cual se está emitiendo la factura. Por ejemplo: sucursal = 0 (casa matriz).
        /// SI
        /// </summary>
        public long codigoSucursal { get; set; }

        /// <summary>
        /// Dirección de la sucursal registrada en el Padrón Nacional de Contribuyentes.
        /// SI
        /// </summary>
        public string direccion { get; set; }

        /// <summary>
        /// Código del punto de Venta creado mediante un servicio web y en el cual se emite la factura.
        /// NO
        /// </summary>
        public long? codigoPuntoVenta { get; set; }

        /// <summary>
        /// Mes correspondiente al periodo por el cual se está facturando el servicio.
        /// NO
        /// </summary>
        public string mes { get; set; }

        /// <summary>
        /// Corresponde a la gestión que se está facturando.
        /// NO
        /// </summary>
        public string gestion { get; set; }

        /// <summary>
        /// Nombre de la ciudad donde se encuentra el domicilio del cliente al que se le factura.
        /// NO
        /// </summary>
        public string ciudad { get; set; }

        /// <summary>
        /// Nombre de la zona donde se encuentra el domicilio del cliente al que se le factura.
        /// NO
        /// </summary>
        public string zona { get; set; }

        /// <summary>
        /// Número de medidor del cliente al que se le factura, máximo de 100 caracteres.
        /// NO
        /// </summary>
        public string numeroMedidor { get; set; }

        /// <summary>
        /// Fecha y hora en la cual se emite la factura. Expresada en formato UTC Extendido, por ejemplo: “2020-02-15T08:40:12.215”
        /// SI
        /// </summary>
        public string fechaEmision { get; set; }

        /// <summary>
        /// Razón Social o nombre de la persona u empresa a la cual se emite la factura.
        /// NO
        /// </summary>
        public string nombreRazonSocial { get; set; }

        /// <summary>
        /// Dirección donde se halla ubicado el domicilio.
        /// NO
        /// </summary>
        public string domicilioCliente { get; set; }

        /// <summary>
        /// Valor de la paramétrica que identifica el Tipo de Documento utilizado para la emisión de la factura. Puede contener valores que van del 1 al 9.  (Ejemplo 1 que representa al CI).
        /// SI
        /// </summary>
        public long codigoTipoDocumentoIdentidad { get; set; }

        /// <summary>
        /// Número que corresponde al Tipo de Documento Identidad utilizado y al cual se realizará la facturación.
        /// SI
        /// </summary>
        public string numeroDocumento { get; set; }

        /// <summary>
        /// Valor que otorga el SEGIP en casos de cédulas de identidad con número duplicado, En otro caso enviar un valor nulo agregando en la Etiqueta xsi:nil=”true”.
        /// NO
        /// </summary>
        public string complemento { get; set; }

        /// <summary>
        /// Código de identificación único del cliente, deberá ser asignado por el sistema de facturación del contribuyente.
        /// SI
        /// </summary>
        public string codigoCliente { get; set; }

        /// <summary>
        /// Valor de la paramétrica que identifica el método de pago utilizado para realizar la compra. Por ejemplo 1 que representa a un pago en efectivo.
        /// SI
        /// </summary>
        public long codigoMetodoPago { get; set; }

        /// <summary>
        /// Cuando el método de pago es 2 (Tarjeta), debe enviarse este valor pero ofuscado con los primeros y últimos 4 dígitos en claro y ceros al medio. Ej: 4797000000007896, en otro caso, debe enviarse un valor nulo
        /// NO
        /// </summary>
        public long numeroTarjeta { get; set; }

        /// <summary>
        /// Monto total por el cual se realiza el hecho generador.
        /// SI
        /// </summary>
        public decimal montoTotal { get; set; }

        /// <summary>
        /// Monto base para el cálculo del crédito fiscal.
        /// SI
        /// </summary>
        public decimal montoTotalSujetoIva { get; set; }

        /// <summary>
        /// Consumo del servicio en el periodo.
        /// SI
        /// </summary>
        public decimal consumoPeriodo { get; set; }

        /// <summary>
        /// Número de carnet de identidad del beneficiario.
        /// SI
        /// </summary>
        public decimal beneficiarioLey1886 { get; set; }

        /// <summary>
        /// Descuento que aplica a la tercera edad.
        /// SI
        /// </summary>
        public decimal montoDescuentoLey1886 { get; set; }

        /// <summary>
        /// Monto descuento al consumo de energía eléctrica.
        /// SI
        /// </summary>
        public decimal montoDescuentoTarifaDignidad { get; set; }

        /// <summary>
        /// Tasa de limpieza.
        /// NO
        /// </summary>
        public decimal tasaAseo { get; set; }

        /// <summary>
        /// Tasa por alumbrado público.
        /// NO
        /// </summary>
        public decimal tasaAlumbrado { get; set; }

        /// <summary>
        /// Descuentos otorgados que no afectan al monto sujeto IVA.
        /// NO
        /// </summary>
        public decimal ajusteNoSujetoIva { get; set; }

        /// <summary>
        /// Detalle en formato json del monto de Ajustes que no esta sujetos al IVA y que se consideran para el calculo del monto a pagar. JSON en formato campo, valor. Ej:  {"campo": valor 1, "campo": valor 2} 
        /// NO
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string detalleAjusteNoSujetoIva { get; set; }

        /// <summary>
        /// Descuentos otorgados que si afectan al monto sujeto IVA.
        /// NO
        /// </summary>
        public decimal ajusteSujetoIva { get; set; }

        /// <summary>
        /// Detalle en formato json del monto de Ajustes que estan sujetos al IVA y que se consideran para el calculo del monto a pagar. JSON en formato campo, valor. Ej:  {"campo": valor 1, "campo": valor 2} 
        /// NO
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string detalleAjusteSujetoIva { get; set; }

        /// <summary>
        /// Otros pagos que se realizan sin afectar al monto sujeto IVA.
        /// NO
        /// </summary>
        public decimal otrosPagosNoSujetoIva { get; set; }

        /// <summary>
        /// Detalle en formato json del monto de los otros pagos realizados que no afectan al IVA, pero si al monto a pagar. 
        /// JSON en formato campo, valor. Ej:  {"campo": valor 1, "campo": valor 2} 
        /// NO
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string detalleOtrosPagosNoSujetoIva { get; set; }

        /// <summary>
        /// Sumatoria de otras tasas
        /// </summary>
        public decimal otrasTasas { get; set; }

        /// <summary>
        /// Valor de la paramétrica que identifica la moneda en la cual se realiza la transacción.
        /// SI
        /// </summary>
        public long codigoMoneda { get; set; }

        /// <summary>
        /// Tipo de cambio de acuerdo a la moneda en la que se realiza el hecho generador, si el código de moneda es boliviano deberá ser igual a 1.
        /// SI
        /// </summary>
        public decimal tipoCambio { get; set; }

        /// <summary>
        /// Es el Monto Total expresado en el tipo de moneda, si el código de moneda es boliviano deberá ser igual al monto total.
        /// SI
        /// </summary>
        public decimal montoTotalMoneda { get; set; }

        /// <summary>
        /// Monto Adicional al descuento por item
        /// NO
        /// </summary>
        public decimal descuentoAdicional { get; set; }

        /// <summary>
        /// Valor que se envía para autorizar el registro de una factura con NIT inválido. Por defecto, enviar cero (0) o nulo y uno (1) cuando se autorice el registro.
        /// NO
        /// </summary>
        public int codigoExcepcion { get; set; }

        /// <summary>
        /// Código de Autorización de Facturas por Contingencia
        /// NO
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string? cafc { get; set; }

        /// <summary>
        /// Leyenda asociada a la actividad económica.
        /// SI
        /// </summary>
        public string leyenda { get; set; }

        /// <summary>
        /// Identifica al usuario que emite la factura, deberá ser descriptivo. Por ejemplo JPEREZ.
        /// NO
        /// </summary>
        public string usuario { get; set; }

        /// <summary>
        /// Valor de la paramétrica que identifica el tipo de factura que se está emitiendo. Para este tipo de factura este valor es 13.
        /// SI
        /// </summary>
        public int codigoDocumentoSector { get; set; }
        
    }
}
