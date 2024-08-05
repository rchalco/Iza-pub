using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Reportes
{
    public class PrinterLineResponse
    {
        public string nombreDocumento { get; set; } = string.Empty;
        public string documentB64 { get; set; } = string.Empty;

    }
    public class PrinterLineRequest
    {
        public int PrinterId { get; set; }
    }
}
