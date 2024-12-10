using Iza.Core.Domain.Iventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Venta
{
    public class ResulProductoPrecioVenta
    {
        public int idProducto { get; set; }
        public int idPrecio { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string embase { get; set; } = string.Empty;
        //public byte[] picProducto { get; set; }
        public string marca { get; set; } = string.Empty;
        public string contenido { get; set; } = string.Empty;
        public decimal enStock { get; set; }
        public decimal precio { get; set; }
        public decimal precioUnitario { get; set; }
        public int CantidadCaja { get; set; }
        public string nombreProducto { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
        public string NivelStock { get; set; } = string.Empty;

        ////Nuevos campos para probar
        public int idPrecioDerecha { get; set; }
        public int idPrecioIzquierda { get; set; }
        public decimal precioDerecha { get; set; }
        public decimal precioIzquierda { get; set; }
        public int slide { get; set; }
        public string etiquetaDerecha { get; set; } = string.Empty;
        public string etiquetaIzquierda { get; set; } = string.Empty;


        /// <summary>
        /// NUEVAS PROPIEDADES PARA ABM 
        /// </summary>
        //public List<ResulProductoPrecioVenta> detalleProductos { get; set; }
        //public int idSesion { get; set; }
        //public int idcCategoria { get; set; }
        //public string producto { get; set; }
        //public bool activo { get; set; }
        //public bool esParaMenu { get; set; }
        //public int cantidad { get; set; }
    }
    public class ResulProductoPrecioVentaComplex
    {
        public string categoria { get; set; }
        public string etiquetaDerecha { get; set; }
        public string etiquetaIzquierda { get; set; }
        public int slide { get; set; }
        public List<ResulProductoPrecioVenta> detalle { get; set; }
    }

    public class ResulProductoPrecioInventarioComplex
    {
        public string categoria { get; set; }
        public List<InventarioProducto> detalle { get; set; }
    }

}
