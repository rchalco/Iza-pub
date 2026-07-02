import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuGeneralDTO } from 'src/app/interfaces/general/MenuGeneral';
import { SeguridadService } from 'src/app/services/seguridad.service';
import { environment } from 'src/environments/environment';
import { NetworkQualityService } from 'src/app/services/network-quality.service';
import { ToastController } from '@ionic/angular';

@Component({
  standalone: false,
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class HomePage implements OnInit {
  menuItems: MenuGeneralDTO[] = [];
  readonly usuario = environment.UsuarioLabel || environment.Usuario;
  menuError = false;
  menuCargado = false;

  constructor(
    private seguridadService: SeguridadService,
    private router: Router,
    private networkQuality: NetworkQualityService,
    private toastController: ToastController,
  ) {}

  ngOnInit(): void {

  }

  ionViewWillEnter() {
    this.cargarMenu();
  }

  async cargarMenu(): Promise<void> {
    if (this.menuCargado && this.menuItems.length > 0) return;

    this.menuError = false;

    try {
      const obs = await this.seguridadService.obtieneMenuPorUsuario();
      obs.subscribe({
        next: (resul) => {
          this.menuItems = resul.listEntities;
          this.menuCargado = true;
          this.menuError = false;
        },
        error: () => {
          this.menuError = true;
          this.menuCargado = true;
        }
      });
    } catch {
      this.menuError = true;
      this.menuCargado = true;
    }
  }

  navigateTo(url: string): void {
    this.router.navigateByUrl(url);
  }

  async medirRed(): Promise<void> {
    const isOnline = this.networkQuality.isOnline();
    const tipo = this.networkQuality.getNetworkTypeLabel();
    const downlink = this.networkQuality.getDownlink();
    const rtt = this.networkQuality.getRtt();

    const color = isOnline ? 'success' : 'danger';
    const msg = isOnline
      ? `Red: ${tipo} | Vel: ${downlink} Mbps | RTT: ${rtt}ms`
      : 'Sin conexión a internet';

    const toast = await this.toastController.create({
      message: msg,
      duration: 4000,
      position: 'top',
      color,
    });
    toast.present();
  }
}
