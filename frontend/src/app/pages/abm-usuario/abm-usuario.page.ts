/* eslint-disable radix */
import { Component, OnInit } from '@angular/core';
import { PersonaDTO } from 'src/app/interfaces/general/PersonaDTO';
import { RolDTO } from 'src/app/interfaces/general/RolDTO';
import { UsuarioDTO } from 'src/app/interfaces/general/UsuarioDTO';
import { StockService } from 'src/app/services/stock.service';

@Component({
  selector: 'app-abm-usuario',
  templateUrl: './abm-usuario.page.html',
  styleUrls: ['./abm-usuario.page.scss'],
})
export class AbmUsuarioPage implements OnInit {

  showCardusuarios = true;
  usuarios: UsuarioDTO[] = [];
  selectedUsuario: UsuarioDTO;
  idPersonaSeleccionada = 0;
  idRolSeleccionado = 0;
  personas: PersonaDTO[] = [];
  roles: RolDTO[] = [];
  fechaSeleccionada: Date;

  constructor(private stockService: StockService) {}

  ngOnInit() {
    this.selectedUsuario = new UsuarioDTO();
    this.cargarUsuarios();

    this.stockService.obtenerPersonas().then((productosService) => {
      productosService.subscribe((resul) => {
        //console.log(resul);
        this.personas = resul.listEntities;
        console.log('productos', resul);
      });
    });

    this.stockService.obtenerRoles().then((productosService) => {
      productosService.subscribe((resul) => {
        //console.log(resul);
        this.roles = resul.listEntities;
        console.log('roles', resul);
      });
    });
  }

  cargarUsuarios() {
    this.stockService.obtenerUsuarios().then((productosService) => {
      productosService.subscribe((resul) => {
        //console.log(resul);
        this.usuarios = resul.listEntities;
        console.log('productos', resul);
      });
    });
  }

  modificaUsuario(usuario) {
    console.log('usuarioooo', usuario);
    this.showCardusuarios = false;
    this.selectedUsuario = usuario;
    this.idPersonaSeleccionada = this.selectedUsuario.idPersona;
    this.idRolSeleccionado = this.selectedUsuario.idRol;
    //runInThisContext.
  }
  nuevaUsuario() {
    this.selectedUsuario = new UsuarioDTO();
    this.showCardusuarios = false;
  }

  regitrarUsuario() {
    if (this.idPersonaSeleccionada <= 0) {
      this.stockService.showMessageWarning('Debe seleccionar una persona');
      return;
    }

    if (this.selectedUsuario.password !== this.selectedUsuario.passwordNuevo) {
      this.stockService.showMessageWarning('Las contraseñas deben ser iguales');
      return;
    }

    if (this.idRolSeleccionado <= 0) {
      this.stockService.showMessageWarning('Debe seleccionar un Rol');
      return;
    }
    console.log('usuario', this.selectedUsuario);
    this.selectedUsuario.idRol = this.idRolSeleccionado;
    this.selectedUsuario.idPersona = this.idPersonaSeleccionada;
    this.stockService.grabarUsuario(this.selectedUsuario).then((resul) => {
      resul.subscribe((x) => {
        this.stockService.showMessageResponse(x);
        this.selectedUsuario = new UsuarioDTO();
        this.usuarios = [];
        this.showCardusuarios = true;
        this.cargarUsuarios();
      });
    });
  }
  cancelarRegistro() {
    this.showCardusuarios = true;
  }

  selectPersona(event) {
    this.idPersonaSeleccionada = parseInt(event.detail.value);
    //console.log('persona',this.idPersonaSeleccionada);
  }

  selectRol(event) {
    this.idRolSeleccionado = parseInt(event.detail.value);
    //console.log('rollll',this.idRolSeleccionado);
  }
  selectFecha(event) {
    //this.fechaSeleccionada = event.detail.value;
    this.fechaSeleccionada = event;
  }

}
