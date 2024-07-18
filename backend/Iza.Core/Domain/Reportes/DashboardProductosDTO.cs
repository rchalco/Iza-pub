using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Reportes
{
    public class DashboardProductosDTO
    {
        public long idProducto { get; set; }
        public string clasificador { get; set; }
        public string nombreProducto { get; set; }
        public decimal stockAnterior { get; set; }
        public decimal ingresos { get; set; }
        public decimal disponibleAlmacenCentral { get; set;}
        public decimal asignadoABarra1PV_1 { get; set; }
        public decimal cantidadVendidaBarra1PV_1 { get; set; }
        public decimal disponibleBarra1PV_1 { get; set; }
        public decimal asignadoABarra1PV_2 { get; set; }
        public decimal cantidadVendidaBarra1PV_2 { get; set; }
        public decimal disponibleBarra1PV_2 { get; set; }
        public decimal asignadoABarra2PV_1 { get; set; }
        public decimal cantidadVendidaBarra2PV_1 { get; set; }
        public decimal disponibleBarra2PV_1 { get; set; }
        public decimal asignadoABarra2PV_2 { get; set; }
        public decimal cantidadVendidaBarra2PV_2 { get; set; }
        public decimal disponibleBarra2PV_2 { get; set; }

        public decimal asignadoABarra3PV_1 { get; set; }
        public decimal cantidadVendidaBarra3PV_1 { get; set; }
        public decimal disponibleBarra3PV_1 { get; set; }
        public decimal asignadoABarra3PV_2 { get; set; }
        public decimal cantidadVendidaBarra3PV_2 { get; set; }
        public decimal disponibleBarra3PV_2 { get; set; }
        public decimal totalDisponible { get; set; }

    }
}
