import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filtro',
})
export class FiltroPipe implements PipeTransform {
  /**
   * Filtra un arreglo por una propiedad del nivel superior.
   * Opcionalmente busca dentro de un sub-arreglo por una propiedad de sus items.
   *
   * @param arreglo      - Lista a filtrar
   * @param texto        - Texto de búsqueda
   * @param prop         - Propiedad del nivel superior (ej. 'categoria')
   * @param subArrayProp - (Opcional) Nombre del sub-arreglo (ej. 'detalle')
   * @param subItemProp  - (Opcional) Propiedad a buscar en cada item del sub-arreglo (ej. 'nombreProducto')
   */
  transform(
    arreglo: any[],
    texto: string,
    prop: string,
    subArrayProp?: string,
    subItemProp?: string,
  ): any[] {
    if (!arreglo || !texto || texto.trim() === '') {
      return arreglo;
    }

    const busqueda = texto.toLowerCase();

    // Si no se especifican parámetros de sub-arreglo, filtrado simple (comportamiento original)
    if (!subArrayProp || !subItemProp) {
      return arreglo.filter((item) =>
        item[prop]?.toLowerCase().includes(busqueda),
      );
    }

    // Filtrado con búsqueda en sub-arreglo
    const resultado: any[] = [];

    for (const item of arreglo) {
      // Si la categoría coincide, incluir el item completo
      if (item[prop]?.toLowerCase().includes(busqueda)) {
        resultado.push(item);
        continue;
      }

      // Buscar dentro del sub-arreglo
      const subArray: any[] = item[subArrayProp];
      if (Array.isArray(subArray)) {
        const coincidencias = subArray.filter((sub) =>
          sub[subItemProp]?.toLowerCase().includes(busqueda),
        );
        if (coincidencias.length > 0) {
          // Clonar el item con solo los detalles que coinciden
          resultado.push({ ...item, [subArrayProp]: coincidencias });
        }
      }
    }

    return resultado;
  }
}
