import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuGeneralDTO } from 'src/app/interfaces/general/MenuGeneral';
import { SeguridadService } from 'src/app/services/seguridad.service';
import { environment } from 'src/environments/environment';

@Component({
  standalone: false,
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class HomePage implements OnInit {
  menuItems: MenuGeneralDTO[] = [];
  readonly usuario = environment.UsuarioLabel || environment.Usuario;

  constructor(
    private seguridadService: SeguridadService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.seguridadService.obtieneMenuPorUsuario().then((obs) =>
      obs.subscribe((resul) => {
        this.menuItems = resul.listEntities;
      })
    );
  }

  navigateTo(url: string): void {
    this.router.navigateByUrl(url);
  }
}
