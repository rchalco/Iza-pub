export class ProductoPrecioVenta {
    idSesion: number = 0;
    idProducto: number = 0;
    idPrecio: number = 0;
    producto: string = '';
    embase: string = '';
    picProductoB64: string = '';
    marca: string = '';
    contenido: string = '';
    enStock: number = 0;
    precio: number = 0;
    cantidadCaja: number = 0;
    cantidadVendida: number = 0;
    cantidad: number = 0;
    precioUnitario: number = 0;
    total: number = 0;
    categoria: string = '';
    idcCategoria: number = 0;
    picCategoria: string = '';
    contenidoCategoria: string = '';
    esParaMenu: boolean = false;
    activo: boolean = false;
    nombreProducto: string = '';
    descripcionProducto: string = '';
    picProductoComboB64: string = '';
    detalleProductos: ProductoPrecioVenta[]=[];
  
  }