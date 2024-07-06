export class RequestRegistroPedido {
  detallePedido: TypeDetailPedidoTintoreria[];
  idSesion: number;
  idFacCliente: string;
  idEmpresa: number;
  idOperacionDiariaCaja: number;
  idAmbiente: number;
  idPedMaster: number;
  observaciones: string;
}

export class TypeDetailPedidoTintoreria {
  idProducto: number | null;
  cantidad: number | null;
  precioFinal: number | null;
}
