using Invoice.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Business.InvoiceKind.BasicExpenses
{
    public class DetailServicioBasico: IDetailFacturable
    {

        // Actividad económica registrada en el Padrón Nacional de Contribuyentes relacionada al NIT.
        /// SI
        /// </summary>
        public string actividadEconomica { get; set; }

        /// <summary>
        /// Homologado a los códigos de productos genéricos enviados por el SIN a través del servicio de sincronización.
        /// SI
        /// </summary>
        public long codigoProductoSin { get; set; }

        /// <summary>
        /// Código que otorga el contribuyente a su servicio o producto.
        /// SI
        /// </summary>
        public string codigoProducto { get; set; }

        /// <summary>
        /// Descripción que otorga el contribuyente a su servicio o producto
        /// SI
        /// </summary>
        public string descripcion { get; set; }

        /// <summary>
        /// Cantidad del producto o servicio otorgado. En caso de servicio este valor debe ser 1.
        /// SI
        /// </summary>
        public decimal cantidad { get; set; }

        /// <summary>
        /// Valor de la paramétrica que identifica la unidad de medida.
        /// SI
        /// </summary>
        public long unidadMedida { get; set; }

        /// <summary>
        /// Precio que otorga el contribuyente a su servicio o producto.
        /// SI
        /// </summary>
        public decimal precioUnitario { get; set; }

        /// <summary>
        /// Monto de descuento sobre el producto o servicio específico,  Si no aplica deberá ser nulo
        /// NO
        /// </summary>
        public decimal? montoDescuento { get; set; }

        /// <summary>
        /// El subtotal es igual a la (cantidad * precio unitario) – descuento.
        /// SI
        /// </summary>
        public decimal subTotal { get; set; }
    }
}
