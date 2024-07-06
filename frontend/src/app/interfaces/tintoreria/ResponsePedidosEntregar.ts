export class MDPedidosPorEntregar {
  idPedMaster: number;
  documento: string;
  nombreCliente: string;
  fechaRegistro: string;
  detallePedidosEntregar: DetallePedidosEntregar[];
  total: number;
  montoApertura: number;
  montoCierre: number;
}

export class DetallePedidosEntregar {
  cantidad: number;
  precio: number;
  producto: string;
}
