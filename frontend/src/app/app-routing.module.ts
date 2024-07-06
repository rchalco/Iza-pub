import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { SessionendGuard } from './guards/sessionend.guard';
import { SessioninitGuard } from './guards/sessioninit.guard';

const routes: Routes = [
  // {
  //   path: '',
  //   redirectTo: 'folder/Inbox',
  //   pathMatch: 'full'
  // },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'compra',
    loadChildren: () =>
      import('./pages/compra/compra.module').then((m) => m.CompraPageModule),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'venta',
    loadChildren: () =>
      import('./pages/venta/venta.module').then((m) => m.VentaPageModule),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'apertura-caja',
    loadChildren: () =>
      import('./pages/apertura-caja/apertura-caja.module').then(
        (m) => m.AperturaCajaPageModule
      ),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'cierre-caja',
    loadChildren: () =>
      import('./pages/cierre-caja/cierre-caja.module').then(
        (m) => m.CierreCajaPageModule
      ),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'login',
    loadChildren: () =>
      import('./pages/login/login.module').then((m) => m.LoginPageModule),
  },
  {
    path: 'cambio-contrasena',
    loadChildren: () =>
      import('./pages/cambio-contrasena/cambio-contrasena.module').then(
        (m) => m.CambioContrasenaPageModule
      ),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'pedido-mesa',
    loadChildren: () =>
      import('./pages/pedido-mesa/pedido-mesa.module').then(
        (m) => m.PedidoMesaPageModule
      ),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'reporte-tablero',
    loadChildren: () =>
      import('./pages/reporte-tablero/reporte-tablero.module').then(
        (m) => m.ReporteTableroPageModule
      ),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'venta-tintoreria',
    loadChildren: () =>
      import(
        './pages/tintoreria/venta-tintoreria/venta-tintoreria.module'
      ).then((m) => m.VentaTintoreriaPageModule),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'home',
    loadChildren: () =>
      import('./pages/home/home.module').then((m) => m.HomePageModule),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'tintoreria-entrega',
    loadChildren: () =>
      import(
        './pages/tintoreria/tintoreria-entrega/tintoreria-entrega.module'
      ).then((m) => m.TintoreriaEntregaPageModule),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'reportes-tintoreria',
    loadChildren: () =>
      import('./pages/tintoreria/reportes/reportes.module').then(
        (m) => m.ReportesPageModule
      ),
    canActivate: [SessioninitGuard],
  },
  {
    path: 'reporte-caja',
    loadChildren: () =>
      import('./pages/tintoreria/reporte-caja/reporte-caja.module').then(
        (m) => m.ReporteCajaPageModule
      ),
  },
  {
    path: 'apertura-inventario',
    loadChildren: () =>
      import('./pages/apertura-inventario/apertura-inventario.module').then(
        (m) => m.AperturaInventarioPageModule
      ),
  },
  {
    path: 'asignacion-inventario',
    loadChildren: () =>
      import('./pages/asignacion-inventario/asignacion-inventario.module').then(
        (m) => m.AsignacionInventarioPageModule
      ),
  },
  {
    path: 'arqueo-cajero',
    loadChildren: () =>
      import('./pages/arqueo-cajero/arqueo-cajero.module').then(
        (m) => m.ArqueoCajeroPageModule
      ),
  },
  {
    path: 'cierre-operativo',
    loadChildren: () =>
      import('./pages/cierre-operativo/cierre-operativo.module').then(
        (m) => m.CierreOperativoPageModule
      ),
  },
  {
    path: 'inventario-general',
    loadChildren: () =>
      import('./pages/inventario-general/inventario-general.module').then(
        (m) => m.InventarioGeneralPageModule
      ),
  },
  {
    path: 'cierre-cajero',
    loadChildren: () => import('./pages/cierre-cajero/cierre-cajero.module').then(m => m.CierreCajeroPageModule)
  },
  {
    path: 'movimiento-stock',
    loadChildren: () => import('./pages/movimiento-stock/movimiento-stock.module').then(m => m.MovimientoStockPageModule)
  },
  {
    path: 'entradas-salidas',
    loadChildren: () => import('./pages/entradas-salidas/entradas-salidas.module').then(m => m.EntradasSalidasPageModule)
  },
  {
    path: 'personas',
    loadChildren: () => import('./pages/personas/personas.module').then(m => m.PersonasPageModule)
  },
  {
    path: 'abm-usuario',
    loadChildren: () => import('./pages/abm-usuario/abm-usuario.module').then(m => m.AbmUsuarioPageModule)
  },
  {
    path: 'habilita-caja',
    loadChildren: () => import('./pages/habilita-caja/habilita-caja.module').then(m => m.HabilitaCajaPageModule)
  },
  {
    path: 'venta-express',
    loadChildren: () => import('./pages/venta-express/venta-express.module').then(m => m.VentaExpressPageModule)
  },
  {
    path: 'cierre-global',
    loadChildren: () => import('./pages/cierre-global/cierre-global.module').then(m => m.CierreGlobalPageModule)
  },
  {
    path: 'cierre-detalle-cajero',
    loadChildren: () => import('./pages/cierre-detalle-cajero/cierre-detalle-cajero.module').then(m => m.CierreDetalleCajeroPageModule)
  },
  {
    path: 'tarjeta-read',
    loadChildren: () => import('./pages/tarjeta-read/tarjeta-read.module').then(m => m.TarjetaReadPageModule)
  },
  {
    path: 'control-tarjetas',
    loadChildren: () => import('./pages/control-tarjetas/control-tarjetas.module').then(m => m.ControlTarjetasPageModule)
  },
  {
    path: 'inventario-final',
    loadChildren: () => import('./pages/inventario-final/inventario-final.module').then(m => m.InventarioFinalPageModule)
  },
  {
    path: 'configuracion-item-menu',
    loadChildren: () =>
      import('./pages/configuracion-item-menu/configuracion-item-menu.module').then(m => m.ConfiguracionItemMenuPageModule)
  },
  {
    path: 'reposicion-producto',
    loadChildren: () => import('./pages/reposicion-producto/reposicion-producto.module').then(m => m.ReposicionProductoPageModule)
  },
  {
    path: 'reposicion-producto2',
    loadChildren: () => import('./pages/reposicion-producto2/reposicion-producto2.module').then(m => m.ReposicionProducto2PageModule)
  },
  {
    path: 'reporte-ventas-consolidado',
    loadChildren: () => import('./pages/reporte-ventas-consolidado/reporte-ventas-consolidado.module').then(m => m.ReporteVentasConsolidadoPageModule)
  },  {
    path: 'registro-huellas',
    loadChildren: () => import('./pages/registro-huellas/registro-huellas.module').then( m => m.RegistroHuellasPageModule)
  },
  {
    path: 'verificacion-huella',
    loadChildren: () => import('./pages/verificacion-huella/verificacion-huella.module').then( m => m.VerificacionHuellaPageModule)
  },
  {
    path: 'reporte-cierre-global',
    loadChildren: () => import('./pages/reporte-cierre-global/reporte-cierre-global.module').then( m => m.ReporteCierreGlobalPageModule)
  },



];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule { }
