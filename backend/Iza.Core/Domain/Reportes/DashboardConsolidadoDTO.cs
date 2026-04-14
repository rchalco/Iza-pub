using System.Collections.Generic;

namespace Iza.Core.Domain.Reportes
{
    /// <summary>
    /// 
    /// </summary>
    public class DashboardConsolidadoDTO
    {
        public List<DashboardFormaPagoDTO> FormasDePago { get; set; } = new();
        public List<DashboardMenuUsuarioDTO> VentasPorMenuYCajero { get; set; } = new();
        public List<DashboardMenuPrecioDTO> VentasPorPrecio { get; set; } = new();
        public List<DashboardIngredienteDTO> IngredientesConsumidos { get; set; } = new();
    }
}
