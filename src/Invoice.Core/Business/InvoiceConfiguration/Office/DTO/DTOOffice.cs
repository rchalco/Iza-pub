using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Business.InvoiceConfiguration.Office.DTO
{
    public class DTOOffice
    {
        public int IdOffice { get; set; }
        public int IdOficinaExterna { get; set; }
        public string Descripcion { get; set; }
        public string NombrePuntoVenta { get; set; }
        public bool IsCasaMatriz { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        /// <summary>
        /// Tipo de punto de venta
        ///1. Punto Venta Comisionista
        ///2. Punto Venta Ventanilla de Cobranza
        ///3. Punto de Venta Móviles
        ///4. Punto de Venta YPFB
        ///5. Punto de Venta Cajeros
        ///6. Punto de Venta Conjunta
        /// </summary>
        public int TipoPuntoVenta { get; set; } = 2;
    }
}
