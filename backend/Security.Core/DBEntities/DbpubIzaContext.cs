using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Security.Core.DBEntities;

public partial class DbpubIzaContext : DbContext
{
    public DbpubIzaContext()
    {
    }

    public DbpubIzaContext(DbContextOptions<DbpubIzaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dosificacion> Dosificacions { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<FacturasDetalle> FacturasDetalles { get; set; }

    public virtual DbSet<FacturasDetalle1> FacturasDetalles1 { get; set; }

    public virtual DbSet<TAlmacene> TAlmacenes { get; set; }

    public virtual DbSet<TAlmacene1> TAlmacenes1 { get; set; }

    public virtual DbSet<TAsignacionDetail> TAsignacionDetails { get; set; }

    public virtual DbSet<TAsignacionDetail1> TAsignacionDetails1 { get; set; }

    public virtual DbSet<TAsignacionMaster> TAsignacionMasters { get; set; }

    public virtual DbSet<TAsignacionMaster1> TAsignacionMasters1 { get; set; }

    public virtual DbSet<TBarrasDeAtencion> TBarrasDeAtencions { get; set; }

    public virtual DbSet<TCaja> TCajas { get; set; }

    public virtual DbSet<TCajaChica> TCajaChicas { get; set; }

    public virtual DbSet<TCantidadDeProductosPorJornadum> TCantidadDeProductosPorJornada { get; set; }

    public virtual DbSet<TCierreBarraDetail> TCierreBarraDetails { get; set; }

    public virtual DbSet<TCierreBarraMaster> TCierreBarraMasters { get; set; }

    public virtual DbSet<TCierresCajeroDetail> TCierresCajeroDetails { get; set; }

    public virtual DbSet<TCierresCajeroMaster> TCierresCajeroMasters { get; set; }

    public virtual DbSet<TClasificador> TClasificadors { get; set; }

    public virtual DbSet<TClasificadorTipo> TClasificadorTipos { get; set; }

    public virtual DbSet<TEmpresasSucursale> TEmpresasSucursales { get; set; }

    public virtual DbSet<TFechasProceso> TFechasProcesos { get; set; }

    public virtual DbSet<TFormasDePago> TFormasDePagos { get; set; }

    public virtual DbSet<TIngrediente> TIngredientes { get; set; }

    public virtual DbSet<TJournalOperation> TJournalOperations { get; set; }

    public virtual DbSet<TLogVar> TLogVars { get; set; }

    public virtual DbSet<TMovimientosCajaChica> TMovimientosCajaChicas { get; set; }

    public virtual DbSet<TMovimientosStock> TMovimientosStocks { get; set; }

    public virtual DbSet<TOperacionDiariaCaja> TOperacionDiariaCajas { get; set; }

    public virtual DbSet<TParamPrecio> TParamPrecios { get; set; }

    public virtual DbSet<TPedidoDetail> TPedidoDetails { get; set; }

    public virtual DbSet<TPedidoFormasDePago> TPedidoFormasDePagos { get; set; }

    public virtual DbSet<TPedidoMaster> TPedidoMasters { get; set; }

    public virtual DbSet<TPersona> TPersonas { get; set; }

    public virtual DbSet<TProducto> TProductos { get; set; }

    public virtual DbSet<TPuntoDeVentum> TPuntoDeVenta { get; set; }

    public virtual DbSet<TRole> TRoles { get; set; }

    public virtual DbSet<TRolesMenu> TRolesMenus { get; set; }

    public virtual DbSet<TSesione> TSesiones { get; set; }

    public virtual DbSet<TUsuario> TUsuarios { get; set; }

    public virtual DbSet<TVersione> TVersiones { get; set; }

    public virtual DbSet<TmenuOpcione> TmenuOpciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=DBPubIZA;Persist Security Info=True;User ID=sa;Password=mikyches*123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dosificacion>(entity =>
        {
            entity.HasKey(e => e.IdDosificacion);

            entity.ToTable("Dosificacion");

            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.LlaveDosificacion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NroAutorizacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NroFacturaActual).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.NumFacturaFin).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.NumFacturaInicial).HasColumnType("numeric(18, 0)");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura);

            entity.ToTable("Factura");

            entity.Property(e => e.CodControl)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CompraVenta)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FechaAnulacion).HasColumnType("datetime");
            entity.Property(e => e.FechaEmision).HasColumnType("datetime");
            entity.Property(e => e.FechaUltModificacion).HasColumnType("datetime");
            entity.Property(e => e.NitCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NIT_Cliente");
            entity.Property(e => e.NombreFactura)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NumFactura).HasColumnType("numeric(18, 0)");

            entity.HasOne(d => d.IdDosificacionNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdDosificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Dosificacion");
        });

        modelBuilder.Entity<FacturasDetalle>(entity =>
        {
            entity.HasKey(e => e.IdFacturaDetalle).HasName("PK_FacturaDetalle");

            entity.ToTable("FacturasDetalle");

            entity.Property(e => e.Concepto)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Descuento).HasColumnType("money");
            entity.Property(e => e.Excento).HasColumnType("money");
            entity.Property(e => e.Ice)
                .HasColumnType("money")
                .HasColumnName("ICE");
            entity.Property(e => e.Monto).HasColumnType("money");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.FacturasDetalles)
                .HasForeignKey(d => d.IdFactura)
                .HasConstraintName("FK_FacturaDetalle_Factura");
        });

        modelBuilder.Entity<FacturasDetalle1>(entity =>
        {
            entity.HasKey(e => e.IdFacturaDetalle).HasName("PK_FacturaDetalle");

            entity.ToTable("FacturasDetalle", "shFabula");

            entity.Property(e => e.Concepto)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Descuento).HasColumnType("money");
            entity.Property(e => e.Excento).HasColumnType("money");
            entity.Property(e => e.Ice)
                .HasColumnType("money")
                .HasColumnName("ICE");
            entity.Property(e => e.Monto).HasColumnType("money");
        });

        modelBuilder.Entity<TAlmacene>(entity =>
        {
            entity.HasKey(e => e.IdAlmacen).HasName("PK__tAlmacen__5FC485CF766A2DBE");

            entity.ToTable("tAlmacenes", "inventarioF");

            entity.Property(e => e.IdAlmacen).HasColumnName("idAlmacen");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
        });

        modelBuilder.Entity<TAlmacene1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tAlmacenes", "shFabula");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdAlmacen)
                .ValueGeneratedOnAdd()
                .HasColumnName("idAlmacen");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdTipoAlmacen).HasColumnName("idTipoAlmacen");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
        });

        modelBuilder.Entity<TAsignacionDetail>(entity =>
        {
            entity.HasKey(e => e.IdAsignacionDetail).HasName("PK__tAsignac__76FA9D23B81F05FB");

            entity.ToTable("tAsignacionDetail", "inventarioF");

            entity.Property(e => e.IdAsignacionDetail).HasColumnName("idAsignacionDetail");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdAsignacionMaster).HasColumnName("idAsignacionMaster");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.MontoDeCompra).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TAsignacionDetail1>(entity =>
        {
            entity.HasKey(e => e.IdAsigDetail).HasName("PK__tAsignac__5F9EB0B4F7874CAD");

            entity.ToTable("tAsignacionDetail", "shFabula");

            entity.Property(e => e.IdAsigDetail).HasColumnName("idAsigDetail");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FechaDeVencimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechaDeVencimiento");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdAsigMaster).HasColumnName("idAsigMaster");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdTipoUnidad).HasColumnName("idTipoUnidad");
            entity.Property(e => e.IdfechaProceso).HasColumnName("idfechaProceso");
            entity.Property(e => e.MontoDeCompra).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.UnidadesPorCaja).HasColumnName("unidadesPorCAja");
        });

        modelBuilder.Entity<TAsignacionMaster>(entity =>
        {
            entity.HasKey(e => e.IdAsignacionMaster).HasName("PK__tAsignac__7F1ABAFFFE83F883");

            entity.ToTable("tAsignacionMaster", "inventarioF");

            entity.Property(e => e.IdAsignacionMaster).HasColumnName("idAsignacionMaster");
            entity.Property(e => e.FechaRegistroDesde)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistroDesde");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdAlmacenDesde).HasColumnName("idAlmacenDesde");
            entity.Property(e => e.IdAlmacenHasta).HasColumnName("idAlmacenHasta");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdTipoMovimiento).HasColumnName("idTipoMovimiento");
            entity.Property(e => e.IdfechaProceso).HasColumnName("idfechaProceso");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TAsignacionMaster1>(entity =>
        {
            entity.HasKey(e => e.IdAsigMaster).HasName("PK__tAsignac__948372AD45E530C5");

            entity.ToTable("tAsignacionMaster", "shFabula");

            entity.Property(e => e.IdAsigMaster).HasColumnName("idAsigMaster");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdAlmacenDesde).HasColumnName("idAlmacenDesde");
            entity.Property(e => e.IdAlmacenHasta).HasColumnName("idAlmacenHasta");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdfechaProceso).HasColumnName("idfechaProceso");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TBarrasDeAtencion>(entity =>
        {
            entity.HasKey(e => e.IdBarraDeAtencion).HasName("PK__tBarrasD__A66CF7458C027263");

            entity.ToTable("tBarrasDeAtencion", "inventarioF");

            entity.Property(e => e.IdBarraDeAtencion).HasColumnName("idBarraDeAtencion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
        });

        modelBuilder.Entity<TCaja>(entity =>
        {
            entity.HasKey(e => e.IdCaja).HasName("PK__tCajas__8BC79B3447DA9122");

            entity.ToTable("tCajas", "seguridad");

            entity.Property(e => e.IdCaja).HasColumnName("idCaja");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdTipoCaja).HasColumnName("idTipoCaja");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
        });

        modelBuilder.Entity<TCajaChica>(entity =>
        {
            entity.HasKey(e => e.IdCajaChica).HasName("PK__tCajaChi__DC38B80990CFFAE9");

            entity.ToTable("tCajaChica", "shFabula");

            entity.Property(e => e.IdCajaChica).HasColumnName("idCajaChica");
            entity.Property(e => e.Concepto)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("concepto");
            entity.Property(e => e.EntradaSalida).HasColumnName("entradaSalida");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdFechaProceso).HasColumnName("idFechaProceso");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto");
        });

        modelBuilder.Entity<TCantidadDeProductosPorJornadum>(entity =>
        {
            entity.HasKey(e => e.IdJornada).HasName("PK__tCantida__236064786DEE5A3E");

            entity.ToTable("tCantidadDeProductosPorJornada", "inventarioF");

            entity.Property(e => e.IdJornada).HasColumnName("idJornada");
            entity.Property(e => e.Cantidad)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cantidad");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaRegistroHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistroHasta");
            entity.Property(e => e.IdAlmacen).HasColumnName("idAlmacen");
            entity.Property(e => e.IdFechaProceso).HasColumnName("idFechaProceso");
            entity.Property(e => e.IdOperacion).HasColumnName("idOperacion");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
        });

        modelBuilder.Entity<TCierreBarraDetail>(entity =>
        {
            entity.HasKey(e => e.IdAsigDetail).HasName("PK__tCierreB__5F9EB0B4D64523A7");

            entity.ToTable("tCierreBarraDetail", "shFabula");

            entity.Property(e => e.IdAsigDetail).HasColumnName("idAsigDetail");
            entity.Property(e => e.CantidadEntregada).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CantidadVendida)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("CantidadVENDIDA");
            entity.Property(e => e.Diferencia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Entradaxasignaciones)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ENTRADAXAsignaciones");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdCierreBarraMaster).HasColumnName("idCierreBarraMaster");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdfechaProceso).HasColumnName("idfechaProceso");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Salidaxasignaciones)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("SALIDAXAsignaciones");
            entity.Property(e => e.StockFinal).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TCierreBarraMaster>(entity =>
        {
            entity.HasKey(e => e.IdCierreBarraMaster).HasName("PK__tCierreB__410B622ADCDF9D9A");

            entity.ToTable("tCierreBarraMaster", "shFabula");

            entity.Property(e => e.IdCierreBarraMaster).HasColumnName("idCierreBarraMaster");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdAlmacen).HasColumnName("idAlmacen");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdfechaProceso).HasColumnName("idfechaProceso");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TCierresCajeroDetail>(entity =>
        {
            entity.HasKey(e => e.IdCierreCajeroDetail).HasName("PK__tCierres__A42E722BA7D7CF5D");

            entity.ToTable("tCierresCajeroDetail", "shFabula");

            entity.Property(e => e.IdCierreCajeroDetail).HasColumnName("idCierreCajeroDetail");
            entity.Property(e => e.Diferencia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdCierreCajeroMaster).HasColumnName("idCierreCajeroMaster");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdFormaDePago).HasColumnName("idFormaDePago");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdfechaProceso).HasColumnName("idfechaProceso");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TotalDeclarado)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalDeclarado");
            entity.Property(e => e.TotalVendido).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TCierresCajeroMaster>(entity =>
        {
            entity.HasKey(e => e.IdCierreCajeroMaster).HasName("PK__tCierres__493E4EF595AF1B85");

            entity.ToTable("tCierresCajeroMaster", "shFabula");

            entity.Property(e => e.IdCierreCajeroMaster).HasColumnName("idCierreCajeroMaster");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdOperacionDiariaCaja).HasColumnName("idOperacionDiariaCaja");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdfechaProceso).HasColumnName("idfechaProceso");
            entity.Property(e => e.MontoApertura).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MontoTotalCierre).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TClasificador>(entity =>
        {
            entity.HasKey(e => e.IdClasificador).HasName("PK__tClasifi__36BC339BCB1D2E27");

            entity.ToTable("tClasificador", "shFabula");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EtiquetaDerecha)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("etiquetaDerecha");
            entity.Property(e => e.EtiquetaIzquierda)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("etiquetaIzquierda");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciHasta");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TClasificadorTipo>(entity =>
        {
            entity.HasKey(e => e.IdClasificadorTipo).HasName("PK__tClasifi__9DB6EE0B63F1CE6F");

            entity.ToTable("tClasificadorTipo", "shFabula");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciHasta");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TEmpresasSucursale>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("PK__tEmpresa__75D2CED4E5EAE471");

            entity.ToTable("tEmpresasSucursales", "shFabula");

            entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.Firma)
                .IsUnicode(false)
                .HasColumnName("firma");
            entity.Property(e => e.IdEmpresaPadre).HasColumnName("idEmpresaPadre");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.NitEmpresaVc)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.NombreSucursal)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RazonSocialVc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("razonSocialVC");
            entity.Property(e => e.ResponsableLegal)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoContribuyente)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tipoContribuyente");
            entity.Property(e => e.Zona)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TFechasProceso>(entity =>
        {
            entity.HasKey(e => e.IdFechaProceso).HasName("PK__tFechasP__C5F93068E9386656");

            entity.ToTable("tFechasProceso", "shFabula");

            entity.Property(e => e.IdFechaProceso).HasColumnName("idFechaProceso");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("datetime")
                .HasColumnName("fechaCierre");
            entity.Property(e => e.FechaDeProceso).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
        });

        modelBuilder.Entity<TFormasDePago>(entity =>
        {
            entity.HasKey(e => e.IdFormaDePago).HasName("PK__tFormasD__BD478E803C6D1480");

            entity.ToTable("tFormasDePago", "shFabula");

            entity.Property(e => e.IdFormaDePago).HasColumnName("idFormaDePago");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistroHasta).HasColumnType("datetime");
            entity.Property(e => e.FormaDePago)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OrdenDespliegue).HasColumnName("ordenDespliegue");
        });

        modelBuilder.Entity<TIngrediente>(entity =>
        {
            entity.HasKey(e => e.IdIngrediente).HasName("PK__tIngredi__563C0D334183B498");

            entity.ToTable("tIngredientes", "shFabula");

            entity.Property(e => e.IdIngrediente).HasColumnName("idIngrediente");
            entity.Property(e => e.Cantidad)
                .HasColumnType("decimal(10, 5)")
                .HasColumnName("cantidad");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdPrecio).HasColumnName("idPrecio");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.MontoIndividual)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montoIndividual");
        });

        modelBuilder.Entity<TJournalOperation>(entity =>
        {
            entity.HasKey(e => e.IdJournalClose).HasName("PK__tJournal__B4A8061D67BA52C1");

            entity.ToTable("tJournalOperation", "shFabula");

            entity.Property(e => e.IdJournalClose).HasColumnName("idJournalClose");
            entity.Property(e => e.Cantidad)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cantidad");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdFechaProceso).HasColumnName("idFechaProceso");
            entity.Property(e => e.IdOperacion).HasColumnName("idOperacion");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
        });

        modelBuilder.Entity<TLogVar>(entity =>
        {
            entity.HasKey(e => e.IdLogVar).HasName("PK__tLogVars__213BF2EB1AC8F813");

            entity.ToTable("tLogVars", "shFabula");

            entity.Property(e => e.IdLogVar).HasColumnName("idLogVar");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Vars)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("vars");
        });

        modelBuilder.Entity<TMovimientosCajaChica>(entity =>
        {
            entity.HasKey(e => e.IdMovimientoCajaChica).HasName("PK__tMovimie__DDA9E877A85B6765");

            entity.ToTable("tMovimientosCajaChica", "shFabula");

            entity.Property(e => e.IdMovimientoCajaChica).HasColumnName("idMovimientoCajaChica");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdFechaProceso).HasColumnName("idFechaProceso");
            entity.Property(e => e.IdOperacionDiariaCaja)
                .HasColumnType("datetime")
                .HasColumnName("idOperacionDiariaCaja");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.Monto)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.Motivo)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.TipoOperacion).HasColumnName("tipoOperacion");
        });

        modelBuilder.Entity<TMovimientosStock>(entity =>
        {
            entity.HasKey(e => e.IdMovStock).HasName("PK__tMovimie__A2A3B22607F4C542");

            entity.ToTable("tMovimientosStock", "shFabula");

            entity.Property(e => e.IdMovStock).HasColumnName("idMovStock");
            entity.Property(e => e.CantidadMovimiento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cantidadMovimiento");
            entity.Property(e => e.Diferencia)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("diferencia");
            entity.Property(e => e.FechaDeVencimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechaDeVencimiento");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdAlmacen).HasColumnName("idAlmacen");
            entity.Property(e => e.IdAsignacion).HasColumnName("idAsignacion");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdFechaProceso).HasColumnName("idFechaProceso");
            entity.Property(e => e.IdMovimiento).HasColumnName("idMovimiento");
            entity.Property(e => e.IdParamPrecio).HasColumnName("idParamPrecio");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.Observacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("observacion");
            entity.Property(e => e.PrecioCaja)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioCaja");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioUnitario");
        });

        modelBuilder.Entity<TOperacionDiariaCaja>(entity =>
        {
            entity.HasKey(e => e.IdOperacionDiariaCaja).HasName("PK__tOperaci__9694AACDA15DEA0D");

            entity.ToTable("tOperacionDiariaCaja", "seguridad");

            entity.Property(e => e.IdOperacionDiariaCaja).HasColumnName("idOperacionDiariaCaja");
            entity.Property(e => e.Diferencia)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("diferencia");
            entity.Property(e => e.FechaApertura)
                .HasColumnType("datetime")
                .HasColumnName("fechaApertura");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("datetime")
                .HasColumnName("fechaCierre");
            entity.Property(e => e.IdAlmacen).HasColumnName("idAlmacen");
            entity.Property(e => e.IdCaja).HasColumnName("idCaja");
            entity.Property(e => e.IdFechaProceso).HasColumnName("idFechaProceso");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.MontoApertura)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montoApertura");
            entity.Property(e => e.MontoCierre)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montoCierre");
            entity.Property(e => e.MontoCierreSis)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("MontoCierreSIS");
            entity.Property(e => e.ObservacioApertura)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("observacioApertura");
            entity.Property(e => e.ObservacionCierre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("observacionCierre");
        });

        modelBuilder.Entity<TParamPrecio>(entity =>
        {
            entity.HasKey(e => e.IdPrecio).HasName("PK__tParamPr__BF8B120C63D07AC3");

            entity.ToTable("tParamPrecios", "shFabula");

            entity.Property(e => e.IdPrecio).HasColumnName("idPrecio");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.DescripcionMenu)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Embase)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("embase");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdClasificador).HasColumnName("idClasificador");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.Orden).HasColumnName("orden");
            entity.Property(e => e.PicProducto).HasColumnName("picProducto");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioUnitario");
        });

        modelBuilder.Entity<TPedidoDetail>(entity =>
        {
            entity.HasKey(e => e.IdPedDetail).HasName("PK__tPedidoD__C138160C2D648248");

            entity.ToTable("tPedidoDetail", "shFabula");

            entity.Property(e => e.IdPedDetail).HasColumnName("idPedDetail");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdParamPrecio).HasColumnName("idParamPrecio");
            entity.Property(e => e.IdPedMaster).HasColumnName("idPedMaster");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.PrecioTotalDetalle).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TPedidoFormasDePago>(entity =>
        {
            entity.HasKey(e => e.IdPedFormaPago).HasName("PK__tPedidoF__580401BC41347338");

            entity.ToTable("tPedidoFormasDePago", "shFabula");

            entity.Property(e => e.IdPedFormaPago).HasColumnName("idPedFormaPago");
            entity.Property(e => e.Diferencia)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("diferencia");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.IdFormaDePago).HasColumnName("idFormaDePago");
            entity.Property(e => e.IdPedMaster).HasColumnName("idPedMaster");
            entity.Property(e => e.MontoCubierto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montoCubierto");
        });

        modelBuilder.Entity<TPedidoMaster>(entity =>
        {
            entity.HasKey(e => e.IdPedMaster).HasName("PK__tPedidoM__FB7979ABBC3F2575");

            entity.ToTable("tPedidoMaster", "shFabula");

            entity.Property(e => e.IdPedMaster).HasColumnName("idPedMaster");
            entity.Property(e => e.FechaBartender)
                .HasColumnType("datetime")
                .HasColumnName("fechaBartender");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdAlmacen).HasColumnName("idAlmacen");
            entity.Property(e => e.IdAmbiente).HasColumnName("idAmbiente");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdFacCliente).HasColumnName("idFacCliente");
            entity.Property(e => e.IdFacTipoPago).HasColumnName("idFacTipoPago");
            entity.Property(e => e.IdFactura).HasColumnName("idFactura");
            entity.Property(e => e.IdMesero).HasColumnName("idMesero");
            entity.Property(e => e.IdOperacionDiariaCaja).HasColumnName("idOperacionDiariaCaja");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.IdSesionBartender).HasColumnName("idSesionBartender");
            entity.Property(e => e.IdfechaProceso).HasColumnName("idfechaProceso");
            entity.Property(e => e.MontoDescuento).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MontoEntregado).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MontoPedidoTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MontoVuelto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TPersona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PK__tPersona__A478814166B6F1BC");

            entity.ToTable("tPersonas", "seguridad");

            entity.Property(e => e.IdPersona).HasColumnName("idPersona");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("celular");
            entity.Property(e => e.DocumentoDeIdentidad)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.Nombres)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TProducto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__tProduct__07F4A132F897E9CC");

            entity.ToTable("tProductos", "shFabula");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.CajaXunidades).HasColumnName("cajaXunidades");
            entity.Property(e => e.Contenido)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdClasificador).HasColumnName("idClasificador");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.Marca)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("marca");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.PicProducto).HasColumnName("picProducto");
        });

        modelBuilder.Entity<TPuntoDeVentum>(entity =>
        {
            entity.HasKey(e => e.IdPuntoDeVenta).HasName("PK__tPuntoDe__DEE4A62DD4795DCF");

            entity.ToTable("tPuntoDeVenta", "inventarioF");

            entity.Property(e => e.IdPuntoDeVenta).HasColumnName("idPuntoDeVenta");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdBarraDeAtencion).HasColumnName("idBarraDeAtencion");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
        });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__tRoles__3C872F769B5340D1");

            entity.ToTable("tRoles", "seguridad");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdSesion).HasColumnName("id_sesion");
            entity.Property(e => e.Rol)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<TRolesMenu>(entity =>
        {
            entity.HasKey(e => e.IdrolMenu).HasName("PK__tRolesMe__E7658ACBE7F2B491");

            entity.ToTable("tRolesMenu", "seguridad");

            entity.Property(e => e.IdrolMenu).HasColumnName("idrolMenu");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdMenuOpcion).HasColumnName("idMenuOpcion");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Orden).HasColumnName("orden");
        });

        modelBuilder.Entity<TSesione>(entity =>
        {
            entity.HasKey(e => e.IdSesion).HasName("PK__tSesione__DB6C2DE66AADED95");

            entity.ToTable("tSesiones", "seguridad");

            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
        });

        modelBuilder.Entity<TUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__tUsuario__645723A642453295");

            entity.ToTable("tUsuarios", "seguridad");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.IdPersona).HasColumnName("idPersona");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.Pass)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("pass");
            entity.Property(e => e.Usuario)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TVersione>(entity =>
        {
            entity.HasKey(e => e.IdVersion).HasName("PK__tVersion__BBD5F8B23055EDEE");

            entity.ToTable("tVersiones", "seguridad");

            entity.Property(e => e.IdVersion).HasColumnName("idVersion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.Version)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TmenuOpcione>(entity =>
        {
            entity.HasKey(e => e.IdMenuOpcion).HasName("PK__tmenuOpc__0C9FFA54C1BE326F");

            entity.ToTable("tmenuOpciones", "seguridad");

            entity.Property(e => e.IdMenuOpcion).HasColumnName("idMenuOpcion");
            entity.Property(e => e.Decripcion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigenciaHasta");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("icon");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.Url)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
