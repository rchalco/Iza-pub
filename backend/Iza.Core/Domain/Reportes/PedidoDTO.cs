﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Domain.Reportes
{
    public class PedidoDTO
    {
        public int idProducto { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }
    }
}
