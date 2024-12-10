import { typeIngredientesDeMenu } from "./typeIngredientesDeMenu";

export class IngredientesDeMenuGeneralDTO {
    idSesion: number = 0;
    idFechaProceso: number = 0;
    idPrecio: number = 0;
    descripcionMenu: string = '';
    embase: string = '';
    precio: number = 0;
    idIngrediente: number = 0;
    idProducto: number = 0;
    nombreProducto: string = '';
    idCategoria: number = 0;
    categoria: string = '';
    marca: string = '';
    contenido: string = '';
    embaseXUnidades: number = 0;
    activo: boolean = false;
    medidaUnitaria: number = 0;
    unidaDeMedida: string = '';
    depliegueDerecha: boolean = false;
    precioUnitario: number = 0;
    //precioVenta: number = 0;
    esParaMenu: boolean = false;
    esProducto: boolean = false;
    cantidad: number = 0;
    subTotal: number = 0;
    detalle: typeIngredientesDeMenu[]=[];
}