using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Core.DBEntities;

public partial class DbinvoiceSwitchContext : DbContext
{
    public DbinvoiceSwitchContext()
    {
    }

    public DbinvoiceSwitchContext(DbContextOptions<DbinvoiceSwitchContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CaptionsInvoice> CaptionsInvoices { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Cufd> Cufds { get; set; }

    public virtual DbSet<Cui> Cuis { get; set; }

    public virtual DbSet<EventoSignificativo> EventoSignificativos { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<LogBatchProcess> LogBatchProcesses { get; set; }

    public virtual DbSet<LogClassifierSin> LogClassifierSins { get; set; }

    public virtual DbSet<LogProductSin> LogProductSins { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<Parameter> Parameters { get; set; }

    public virtual DbSet<TestInvoice> TestInvoices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=155.138.212.216;Initial Catalog=DBInvoiceSwitch;Persist Security Info=True;User ID=sa;Password=m1k1ches*123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CaptionsInvoice>(entity =>
        {
            entity.HasKey(e => e.IdLeyenda).HasName("PK__ListaLey__6171E717D9C2EBB5");

            entity.ToTable("CaptionsInvoice");

            entity.Property(e => e.CodigoActividad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DescripcionLeyenda)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.FechaVigenciaHasta).HasColumnType("datetime");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.IdCompany).HasName("PK_Comapny");

            entity.ToTable("Company");

            entity.HasIndex(e => e.IdCompany, "IXKey_Company").IsUnique();

            entity.Property(e => e.IdCompany).ValueGeneratedNever();
            entity.Property(e => e.CompanyKey)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nit)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NIT");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SincodeEnviroment).HasColumnName("SINCodeEnviroment");
            entity.Property(e => e.SincodeModeInvoice)
                .HasComment("1 = Electrónica en Línea   2 = Computarizada en Línea 3 = Portal Web en Línea")
                .HasColumnName("SINCodeModeInvoice");
            entity.Property(e => e.SincodeSystem)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SINCodeSystem");
            entity.Property(e => e.Sintoken)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("SINToken");
            entity.Property(e => e.SocialReason)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SystemName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cufd>(entity =>
        {
            entity.HasKey(e => e.IdCufd).HasName("PK_CufdPROD");

            entity.ToTable("CUFD");

            entity.HasIndex(e => e.IdCuis, "IX_CUFD_IdCuis");

            entity.HasIndex(e => e.IdOffice, "IX_CUFD_IdOffice");

            entity.Property(e => e.CodigoControl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CuisRequest)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaBaja).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Request).IsUnicode(false);
            entity.Property(e => e.Response).IsUnicode(false);
            entity.Property(e => e.ValorCufd)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCuisNavigation).WithMany(p => p.Cufds)
                .HasForeignKey(d => d.IdCuis)
                .HasConstraintName("FK_CUFD_CUIS");

            entity.HasOne(d => d.IdOfficeNavigation).WithMany(p => p.Cufds)
                .HasForeignKey(d => d.IdOffice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CUFD_Office");
        });

        modelBuilder.Entity<Cui>(entity =>
        {
            entity.HasKey(e => e.IdCuis).HasName("PK_CuisPROD");

            entity.ToTable("CUIS");

            entity.HasIndex(e => e.IdOffice, "IX_CUIS_IdOffice");

            entity.Property(e => e.FechaBaja).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Request).IsUnicode(false);
            entity.Property(e => e.Response).IsUnicode(false);
            entity.Property(e => e.ValorCuis).IsUnicode(false);

            entity.HasOne(d => d.IdOfficeNavigation).WithMany(p => p.Cuis)
                .HasForeignKey(d => d.IdOffice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CUIS_Office");
        });

        modelBuilder.Entity<EventoSignificativo>(entity =>
        {
            entity.HasKey(e => e.IdEventoSignificativo);

            entity.ToTable("EventoSignificativo");

            entity.HasIndex(e => e.IdOffice, "IX_EventoSignificativo_IdOffice");

            entity.Property(e => e.CodEvento).HasColumnName("codEvento");
            entity.Property(e => e.CodEventoSignificativo).IsUnicode(false);
            entity.Property(e => e.CodigoControlCufd)
                .IsUnicode(false)
                .HasColumnName("CodigoControlCUFD");
            entity.Property(e => e.CodigoControlCufdevento)
                .IsUnicode(false)
                .HasColumnName("CodigoControlCUFDEvento");
            entity.Property(e => e.Cufd).IsUnicode(false);
            entity.Property(e => e.CufdEvento).IsUnicode(false);
            entity.Property(e => e.DescripcionEvento).IsUnicode(false);
            entity.Property(e => e.FechaHoraFin).HasColumnType("datetime");
            entity.Property(e => e.FechaHoraIni).HasColumnType("datetime");
            entity.Property(e => e.Idcufd).HasColumnName("IDCUFD");
            entity.Property(e => e.Idcufdevento).HasColumnName("IDCUFDEVENTO");
            entity.Property(e => e.Request).IsUnicode(false);
            entity.Property(e => e.Response).IsUnicode(false);

            entity.HasOne(d => d.IdOfficeNavigation).WithMany(p => p.EventoSignificativos)
                .HasForeignKey(d => d.IdOffice)
                .HasConstraintName("FK_EventoSignificativo_Office");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice).HasName("PK_InvoiceImportPROD");

            entity.ToTable("Invoice");

            entity.HasIndex(e => e.IdCufd, "IX_Invoice_IdCufd");

            entity.HasIndex(e => e.IdEventoSignificativo, "IX_Invoice_IdEventoSignificativo");

            entity.HasIndex(e => e.IdOffice, "IX_Invoice_IdOffice");

            entity.Property(e => e.CodeBatch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CodigoSector)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cuf)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DetailProcess).IsUnicode(false);
            entity.Property(e => e.FileNameCompress)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FolderContainer)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceJson)
                .IsUnicode(false)
                .HasColumnName("InvoiceJSON");
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceSha256)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("InvoiceSHA256");
            entity.Property(e => e.InvoiceSign).IsUnicode(false);
            entity.Property(e => e.ModifyDate).HasColumnType("datetime");
            entity.Property(e => e.PathInvoicePdf)
                .IsUnicode(false)
                .HasColumnName("PathInvoicePDF");
            entity.Property(e => e.ProcessDate).HasColumnType("datetime");
            entity.Property(e => e.ProcessObject).IsUnicode(false);
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            entity.Property(e => e.RequestAnulacion).IsUnicode(false);
            entity.Property(e => e.RequestSendOnline).IsUnicode(false);
            entity.Property(e => e.ResponseAnulacion).IsUnicode(false);
            entity.Property(e => e.ResponseSendOnline).IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypeObjectProcessed)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCufdNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdCufd)
                .HasConstraintName("FK_Invoice_CUFD");

            entity.HasOne(d => d.IdEventoSignificativoNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdEventoSignificativo)
                .HasConstraintName("FK_Invoice_EventoSignificativo");

            entity.HasOne(d => d.IdOfficeNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdOffice)
                .HasConstraintName("FK_Invoice_Office");
        });

        modelBuilder.Entity<LogBatchProcess>(entity =>
        {
            entity.HasKey(e => e.IdLogBatchProcess).HasName("PK_LogProcesoMasivoPROD");

            entity.ToTable("LogBatchProcess");

            entity.HasIndex(e => e.IdEventoSignificativo, "IX_LogProcesoMasivo_IdEventoSignificativo");

            entity.Property(e => e.CodeBatch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CodigoDescripcionEnviado)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CodigoDescripcionValidado)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CodigoRecepcionEnviado)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CodigoRecepcionEnviadoValidado)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EstadoEnviado)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EstadoValidado)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RequestEnviado).IsUnicode(false);
            entity.Property(e => e.RequestValidado).IsUnicode(false);
            entity.Property(e => e.ResponseEnviado).IsUnicode(false);
            entity.Property(e => e.ResponseValidado).IsUnicode(false);

            entity.HasOne(d => d.IdEventoSignificativoNavigation).WithMany(p => p.LogBatchProcesses)
                .HasForeignKey(d => d.IdEventoSignificativo)
                .HasConstraintName("FK_LogProcesoMasivo_EventoSignificativo");
        });

        modelBuilder.Entity<LogClassifierSin>(entity =>
        {
            entity.HasKey(e => e.IdClassifierSin).HasName("PK_ClasificadorSIN");

            entity.ToTable("LogClassifierSIN");

            entity.Property(e => e.IdClassifierSin).HasColumnName("IdClassifierSIN");
            entity.Property(e => e.FechaBaja).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Nemotico)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Request).IsUnicode(false);
            entity.Property(e => e.Response).IsUnicode(false);
        });

        modelBuilder.Entity<LogProductSin>(entity =>
        {
            entity.HasKey(e => e.IdProductSin).HasName("PK_ProductoSIN");

            entity.ToTable("LogProductSIN");

            entity.Property(e => e.IdProductSin).HasColumnName("IdProductSIN");
            entity.Property(e => e.CodeActivity)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CodeProduct)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DateRegister).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ResulJson)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("ResulJSON");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(e => e.IdOffice).HasName("PK_OfficeSIN");

            entity.ToTable("Office");

            entity.Property(e => e.IdOffice).ValueGeneratedNever();
            entity.Property(e => e.CuisCreacion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Request)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Response)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.SincodigoSucursal)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SINCodigoSucursal");
            entity.Property(e => e.SincodigoTipoPuntoVenta)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SINCodigoTipoPuntoVenta");
            entity.Property(e => e.SinidPuntoVenta).HasColumnName("SINIdPuntoVenta");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.Offices)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_Office_Comapny");
        });

        modelBuilder.Entity<Parameter>(entity =>
        {
            entity.HasKey(e => e.IdParameter).HasName("PK_ParameterSIN");

            entity.ToTable("Parameter");

            entity.Property(e => e.IdParameter).ValueGeneratedNever();
            entity.Property(e => e.DateRegister).HasColumnType("datetime");
            entity.Property(e => e.DateUpdate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Group)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.KeyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Value)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.Parameters)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parameter_Comapny");
        });

        modelBuilder.Entity<TestInvoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice).HasName("PK__TestInvo__4AFC50A430E8C4CC");

            entity.ToTable("TestInvoice");

            entity.Property(e => e.CodeBatch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CodigoSector)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DetailProcess).IsUnicode(false);
            entity.Property(e => e.FileNameCompress)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FolderContainer)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceSha256)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("InvoiceSHA256");
            entity.Property(e => e.InvoiceSign).IsUnicode(false);
            entity.Property(e => e.ModifyDate).HasColumnType("datetime");
            entity.Property(e => e.ProcessDate).HasColumnType("datetime");
            entity.Property(e => e.ProcessObject).IsUnicode(false);
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
