import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
	name: 'filtro'
})
export class FiltroPipe implements PipeTransform {

	transform(arreglo: any[], texto: string, prop: string): any[] {
		if (texto === '') {
			return arreglo;
		}
		texto = texto.toLowerCase();
    console.log('FiltroPipe arreglo', arreglo);
    console.log('FiltroPipe texto', texto);
    console.log('FiltroPipe prop', prop);
		return arreglo.filter(item => item[prop]?.toLowerCase().includes(texto));
	}

}
