using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Artefacts.Invoice
{
    public class CUFDHeaderRequest
    {
        /// <summary>
        ///  NIT del Contribuyente.(Emisor)
        /// </summary>
        public  long NIT { get; set; }

        /// <summary>
        /// Fecha y Hora del Emisor  yyyyMMddHHmmssSSS
        /// </summary>
        public  long FechaHora { get; set; }

        /// <summary>
        /// Fecha y Hora de emision de la factura
        /// </summary>
        public  DateTime FechaEmisionFactura { get; set; }

        /// <summary>
        /// SUCURSAL (de donde se emite la factura)
        /// 0 = Casa Matriz
        /// 1 = Sucursal 1
        /// 2 = Sucursal 2
        /// N = Sucursal N
        /// </summary>
        public  long Sucursal { get; set; }
        /// <summary>
        /// 1 = Electrónica en Línea     
        /// 2 = Computarizada en Línea
        /// 3 = Portal Web en Línea
        /// </summary>
        public  long Modalidad { get; set; }

        /// <summary>
        /// 1 = Online
        /// 2 = Offline
        /// 3 = Masiva
        /// </summary>
        public  long TipoEmision { get; set; }

        /// <summary>
        /// TIPO FACTURA / DOCUMENTO AJUSTE
        ///1 = Factura con Derecho a Crédito Fiscal
        ///2 = Factura sin Derecho a Crédito Fiscal
        ///3 = Documento de Ajuste
        /// </summary>
        public  long TipoFactura { get; set; }

        /// <summary>
        /// 1 = Factura Compra Venta
        ///2 = Recibo de Alquiler de Bienes Inmuebles
        ///…….
        ///24 = Nota Crédito - Débito
        /// </summary>
        public  long TipoDocumentoSector { get; set; }

        /// <summary>
        /// NÚMERO DE FACTURA
        /// </summary>
        public  long NumeroFactura { get; set; } = 0;

        /// <summary>
        /// PUNTO DE VENTA (POS)
        /// Número de punto de venta
        /// 0 = No corresponde
        /// 1,2,3,4,….n
        /// </summary>
        public  long PuntoVenta { get; set; }

        /// <summary>
        /// CUFD
        /// </summary>
        public  string CUFD { get; set; }

        /// <summary>
        /// CUFD codigo de control
        /// </summary>
        public  string CUFDCodigoControl { get; set; }

        /// <summary>
        /// codigo cacf para facturacion manual
        /// </summary>
        public string? cafc { get; set; } = null;

    }
}
