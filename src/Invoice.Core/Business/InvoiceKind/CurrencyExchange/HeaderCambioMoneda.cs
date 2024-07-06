using Invoice.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoice.Core.Business.InvoiceKind.CurrencyExchange
{
    [Serializable, XmlRoot("cabecera")]
    public class HeaderCambioMoneda : IHeaderFacturable
    {
        /// <summary>
        /// Número de NIT registrado en el Padrón Nacional de Contribuyentes que corresponde a la persona o empresa que emite la factura.
        /// SI
        /// </summary>
        public long nitEmisor
        {
            get;
            set;
        }

        /// <summary>
        /// Razón Social o nombre registrado en el Padrón Nacional de Contribuyentes de la persona o empresa que emite la factura
        /// SI
        /// </summary>
        public string razonSocialEmisor
        {
            get;
            set;
        }

        /// <summary>
        /// Lugar registrado en el Padrón Nacional de contribuyentes.
        /// SI
        /// </summary>
        public string municipio
        {
            get;
            set;
        }

        /// <summary>
        /// Teléfono registrado en el Padrón Nacional de contribuyentes
        /// NO
        /// </summary>
        public string telefono
        {
            get;
            set;
        }

        /// <summary>
        /// Número asignado a la factura.
        /// SI
        /// </summary>
        public int numeroFactura
        {
            get;
            set;
        }

        /// <summary>
        /// Código único de facturación (CUF) debe ser generado por el emisor siguiendo el algoritmo indicado.
        /// SI
        /// </summary>
        public string cuf
        {
            get;
            set;
        }

        /// <summary>
        /// Código único de facturación diario (CUFD), valor único que se obtiene al consumir el servicio web correspondiente.
        /// SI
        /// </summary>
        public string cufd
        {
            get;
            set;
        }

        /// <summary>
        /// Código de la sucursal registrada en el Padrón y en la cual se está emitiendo la factura. Por ejemplo: sucursal = 0 (casa matriz).
        /// SI
        /// </summary>
        public long codigoSucursal
        {
            get;
            set;
        }

        /// <summary>
        /// Dirección de la sucursal registrada en el Padrón Nacional de Contribuyentes.
        /// SI
        /// </summary>
        public string direccion
        {
            get;
            set;
        }

        /// <summary>
        /// Código del punto de Venta creado mediante un servicio web y en el cual se emite la factura.
        /// NO
        /// </summary>
        public long? codigoPuntoVenta
        {
            get;
            set;
        }

        /// <summary>
        /// Fecha y hora en la cual se emite la factura. Expresada en formato UTC Extendido, por ejemplo: “2020-02-15T08:40:12.215”
        /// SI
        /// </summary>
        public string fechaEmision
        {
            get;
            set;
        }

        /// <summary>
        /// Razón Social o nombre de la persona u empresa a la cual se emite la factura.
        /// NO
        /// </summary>
        public string nombreRazonSocial
        {
            get;
            set;
        }

        /// <summary>
        /// Valor de la paramétrica que identifica el Tipo de Documento utilizado para la emisión de la factura. Puede contener valores que van del 1 al 9.  (Ejemplo 1 que representa al CI).
        /// SI
        /// </summary>
        public long codigoTipoDocumentoIdentidad
        {
            get;
            set;
        }

        /// <summary>
        /// Número que corresponde al Tipo de Documento Identidad utilizado y al cual se realizará la facturación.
        /// SI
        /// </summary>
        public string numeroDocumento
        {
            get;
            set;
        }

        /// <summary>
        /// Valor que otorga el SEGIP en casos de cédulas de identidad con número duplicado, En otro caso enviar un valor nulo agregando en la Etiqueta xsi:nil=”true”.
        /// NO
        /// </summary>
        public string complemento
        {
            get;
            set;
        }

        /// <summary>
        /// Código de identificación único del cliente, deberá ser asignado por el sistema de facturación del contribuyente.
        /// SI
        /// </summary>
        public string codigoCliente
        {
            get;
            set;
        }
        /// <summary>
        /// Código de identificación único del cliente, deberá ser asignado por el sistema de facturación del contribuyente.
        /// SI
        /// </summary>
        public long codigoTipoOperacion
        {
            get;
            set;
        }
        /// <summary>
        /// Valor de la paramétrica que identifica el método de pago utilizado para realizar la compra. Por ejemplo 1 que representa a un pago en efectivo.
        /// SI
        /// </summary>
        public long codigoMetodoPago
        {
            get;
            set;
        }

        /// <summary>
        /// Cuando el método de pago es 2 (Tarjeta), debe enviarse este valor pero ofuscado con los primeros y últimos 4 dígitos en claro y ceros al medio. Ej: 4797000000007896, en otro caso, debe enviarse un valor nulo
        /// NO
        /// </summary>
        public long? numeroTarjeta
        {
            get;
            set;
        }

        ///// <summary>
        ///// Es el monto que cobra la entidad financiera en caso de servicios de arrendamiento, por el cual se cobra una comisión o similar, arrendamiento o leasing.   Si no aplica debe ser nulo.
        ///// NO
        ///// </summary>
        //public  decimal? montoTotalArrendamientoFinanciero
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Monto total por el cual se realiza el hecho generador.
        /// SI
        /// </summary>
        public decimal montoTotal
        {
            get;
            set;
        }
        /// <summary>
        /// Monto base para el cálculo del crédito fiscal.
        /// SI
        /// </summary>
        public decimal montoTotalSujetoIva
        {
            get;
            set;
        }
        /// <summary>
        /// Monto base para el cálculo del crédito fiscal.
        /// SI
        /// </summary>
        public decimal ingresoDiferenciaCambio
        {
            get;
            set;
        }

        /// <summary>
        /// Valor de la paramétrica que identifica la moneda en la cual se realiza la transacción.
        /// SI
        /// </summary>
        public long codigoMoneda
        {
            get;
            set;
        }

        /// <summary>
        /// Tipo de cambio de acuerdo a la moneda en la que se realiza el hecho generador, si el código de moneda es boliviano deberá ser igual a 1.
        /// SI
        /// </summary>
        public decimal tipoCambio
        {
            get;
            set;
        }

        /// <summary>
        /// Es el Monto Total expresado en el tipo de moneda, si el código de moneda es boliviano deberá ser igual al monto total.
        /// SI
        /// </summary>
        public decimal montoTotalMoneda
        {
            get;
            set;
        }


        ///// <summary>
        ///// Monto Adicional al descuento por item
        ///// NO
        ///// </summary>
        public decimal? descuentoAdicional
        {
            get;
            set;
        }


        /// <summary>
        /// Valor que se envía para autorizar el registro de una factura con NIT inválido. Por defecto, enviar cero (0) o nulo y uno (1) cuando se autorice el registro.
        /// NO
        ///// </summary>
        public long? codigoExcepcion
        {
            get;
            set;
        }

        /// <summary>
        /// Código de Autorización de Facturas por Contingencia
        /// NO
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string cafc
        {
            get;
            set;
        }

        /// <summary>
        /// Leyenda asociada a la actividad económica.
        /// 
        /// SI
        /// </summary>
        public string leyenda
        {
            get;
            set;
        }

        /// <summary>
        /// Identifica al usuario que emite la factura, deberá ser descriptivo. Por ejemplo JPEREZ
        /// SI
        /// </summary>
        public string usuario
        {
            get;
            set;
        }
        /// <summary>
        /// Valor de la paramétrica que identifica el tipo de factura que se está emitiendo. Para este tipo de factura este valor es 15.
        /// SI
        /// </summary>
        public decimal tipoCambioOficial
        {
            get;
            set;
        }

        /// <summary>
        /// Valor de la paramétrica que identifica el tipo de factura que se está emitiendo. Para este tipo de factura este valor es 15.
        /// SI
        /// </summary>
        public int codigoDocumentoSector
        {
            get;
            set;
        }
    }
}
