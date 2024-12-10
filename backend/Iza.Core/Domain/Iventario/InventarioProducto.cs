using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Iventario
{
    public class InventarioProducto
    {
        public int idProducto { get; set; }
        public string nombreProducto { get; set; }
        public string categoria { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidadCaja { get; set; }
        public decimal enStock { get; set; }
        public int idProveedor { get; set; }
        /// <summary>
        /// precio unitario
        /// </summary>
        public decimal montoCompra { get; set; }

        /// <summary>
        /// si es caja o unidades
        /// </summary>
        public int idTipo { get; set; }

        public decimal montoTotal { get; set; }
        public DateTime fechaDeVencimiento { get; set; }
    }
}
