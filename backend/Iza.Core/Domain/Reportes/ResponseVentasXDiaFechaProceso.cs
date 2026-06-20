namespace Iza.Core.Domain.Reportes
{
    public class ResponseVentasXDiaFechaProceso
    {
        public string fechaProceso { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public long idProducto { get; set; }
        public string menu { get; set; } = string.Empty;
        public string cantidad { get; set; } = string.Empty;
    }
}