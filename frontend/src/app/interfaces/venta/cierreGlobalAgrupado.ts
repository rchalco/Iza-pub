
import { ResponseCierreGlobal } from "./cierreGlobal";
import { ResponseCierreGlobalDetalle } from "./cierreGlobalDetalle";

export class ResponseCierreGlobalAgrupado {
    listaGeneral: ResponseCierreGlobal[];
    listaCortesia: ResponseCierreGlobalDetalle[];
    listaCover: ResponseCierreGlobalDetalle[];
}