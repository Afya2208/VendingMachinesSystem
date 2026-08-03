using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Models.Entities;

public partial class VendingDbContext : DbContext
{
    public VendingDbContext()
    {
    }

    public VendingDbContext(DbContextOptions<VendingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalMachineStatus> AdditionalMachineStatuses { get; set; }

    public virtual DbSet<CashCollection> CashCollections { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<ConnectionType> ConnectionTypes { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<MachineAdditionalInformation> MachineAdditionalInformations { get; set; }

    public virtual DbSet<MachineEquipment> MachineEquipments { get; set; }

    public virtual DbSet<MachineProductStorage> MachineProductStorages { get; set; }

    public virtual DbSet<MachineProvider> MachineProviders { get; set; }

    public virtual DbSet<MachineRentingInformation> MachineRentingInformations { get; set; }

    public virtual DbSet<MachineTechnicalInformation> MachineTechnicalInformations { get; set; }

    public virtual DbSet<MachineWorkStatus> MachineWorkStatuses { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductMatrix> ProductMatrices { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportStatus> ReportStatuses { get; set; }

    public virtual DbSet<ReportType> ReportTypes { get; set; }

    public virtual DbSet<Reserving> Reservings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sell> Sells { get; set; }

    public virtual DbSet<SellProduct> SellProducts { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VendingMachine> VendingMachines { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalMachineStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("additional_machine_status");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('status_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CashCollection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cash_collection_pkey");

            entity.ToTable("cash_collection");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time");
            entity.Property(e => e.InputSum)
                .HasPrecision(10, 2)
                .HasColumnName("input_sum");
            entity.Property(e => e.MachineId).HasColumnName("machine_id");
            entity.Property(e => e.TakenSum)
                .HasPrecision(10, 2)
                .HasColumnName("taken_sum");

            entity.HasOne(d => d.Machine).WithMany(p => p.CashCollections)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cash_collection_machine_id_fkey");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_pkey");

            entity.ToTable("company");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Contacts)
                .HasMaxLength(200)
                .HasColumnName("contacts");
            entity.Property(e => e.DateWorkStarted)
                .HasDefaultValueSql("now()")
                .HasColumnName("date_work_started");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpperCompanyId).HasColumnName("upper_company_id");

            entity.HasOne(d => d.UpperCompany).WithMany(p => p.InverseUpperCompany)
                .HasForeignKey(d => d.UpperCompanyId)
                .HasConstraintName("company_upper_company_id_fkey");
        });

        modelBuilder.Entity<ConnectionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("connection_type_pkey");

            entity.ToTable("connection_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contract_pkey");

            entity.ToTable("contract");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateExpired).HasColumnName("date_expired");
            entity.Property(e => e.DateSigned)
                .HasDefaultValueSql("now()")
                .HasColumnName("date_signed");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(100)
                .HasColumnName("document_number");
            entity.Property(e => e.FranchiseeId).HasColumnName("franchisee_id");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");

            entity.HasOne(d => d.Franchisee).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.FranchiseeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contract_user_id_fkey");
        });

        modelBuilder.Entity<MachineAdditionalInformation>(entity =>
        {
            entity.HasKey(e => e.MachineId).HasName("machine_additional_information_pkey");

            entity.ToTable("machine_additional_information");

            entity.Property(e => e.MachineId)
                .ValueGeneratedNever()
                .HasColumnName("machine_id");
            entity.Property(e => e.Coordinates)
                .HasMaxLength(100)
                .HasColumnName("coordinates");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.ProductMatrixId).HasColumnName("product_matrix_id");
            entity.Property(e => e.RfidCashCollection)
                .HasMaxLength(100)
                .HasColumnName("rfid_cash_collection");
            entity.Property(e => e.RfidLoad)
                .HasMaxLength(100)
                .HasColumnName("rfid_load");
            entity.Property(e => e.RfidService)
                .HasMaxLength(100)
                .HasColumnName("rfid_service");
            entity.Property(e => e.ServicePriority)
                .HasMaxLength(40)
                .HasColumnName("service_priority");
            entity.Property(e => e.TemplateCriticalValues)
                .HasMaxLength(50)
                .HasColumnName("template_critical_values");
            entity.Property(e => e.TemplateNotifications)
                .HasMaxLength(50)
                .HasColumnName("template_notifications");
            entity.Property(e => e.Timezone)
                .HasMaxLength(10)
                .HasColumnName("timezone");
            entity.Property(e => e.WorkRegime)
                .HasMaxLength(50)
                .HasColumnName("work_regime");
            entity.Property(e => e.WorkTime)
                .HasMaxLength(100)
                .HasColumnName("work_time");

            entity.HasOne(d => d.Machine).WithOne(p => p.MachineAdditionalInformation)
                .HasForeignKey<MachineAdditionalInformation>(d => d.MachineId)
                .HasConstraintName("machine_additional_information_machine_id_fkey");

            entity.HasOne(d => d.ProductMatrix).WithMany(p => p.MachineAdditionalInformations)
                .HasForeignKey(d => d.ProductMatrixId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("machine_additional_information_product_matrix_fkey");
        });

        modelBuilder.Entity<MachineEquipment>(entity =>
        {
            entity.HasKey(e => e.MachineId).HasName("machine_equipment_pkey");

            entity.ToTable("machine_equipment");

            entity.Property(e => e.MachineId)
                .ValueGeneratedNever()
                .HasColumnName("machine_id");
            entity.Property(e => e.CardsStatus)
                .HasDefaultValue(0)
                .HasColumnName("cards_status");
            entity.Property(e => e.ChangeStatus)
                .HasDefaultValue(0)
                .HasColumnName("change_status");
            entity.Property(e => e.ChequesStatus)
                .HasDefaultValue(0)
                .HasColumnName("cheques_status");
            entity.Property(e => e.ContactlessPaymentStatus)
                .HasDefaultValue(0)
                .HasColumnName("contactless_payment_status");
            entity.Property(e => e.ElectricityStatus)
                .HasDefaultValue(0)
                .HasColumnName("electricity_status");
            entity.Property(e => e.ModemStatus)
                .HasDefaultValue(0)
                .HasColumnName("modem_status");
            entity.Property(e => e.MonitorStatus)
                .HasDefaultValue(0)
                .HasColumnName("monitor_status");

            entity.HasOne(d => d.Machine).WithOne(p => p.MachineEquipment)
                .HasForeignKey<MachineEquipment>(d => d.MachineId)
                .HasConstraintName("machine_equipment_machine_id_fkey");
        });

        modelBuilder.Entity<MachineProductStorage>(entity =>
        {
            entity.HasKey(e => new { e.MachineId, e.ProductId }).HasName("machine_product_pkey");

            entity.ToTable("machine_product_storage");

            entity.Property(e => e.MachineId).HasColumnName("machine_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineProductStorages)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("machine_product_machine_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.MachineProductStorages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("machine_product_product_id_fkey");
        });

        modelBuilder.Entity<MachineProvider>(entity =>
        {
            entity.HasKey(e => e.MachineId).HasName("machine_provider_pkey");

            entity.ToTable("machine_provider");

            entity.Property(e => e.MachineId)
                .ValueGeneratedNever()
                .HasColumnName("machine_id");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .HasColumnName("company_name");
            entity.Property(e => e.Ping)
                .HasDefaultValue(0)
                .HasColumnName("ping");
            entity.Property(e => e.Sum)
                .HasPrecision(10, 2)
                .HasColumnName("sum");

            entity.HasOne(d => d.Machine).WithOne(p => p.MachineProvider)
                .HasForeignKey<MachineProvider>(d => d.MachineId)
                .HasConstraintName("machine_provider_machine_id_fkey");
        });

        modelBuilder.Entity<MachineRentingInformation>(entity =>
        {
            entity.HasKey(e => e.MachineId).HasName("machine_renting_information_pkey");

            entity.ToTable("machine_renting_information");

            entity.Property(e => e.MachineId)
                .ValueGeneratedNever()
                .HasColumnName("machine_id");
            entity.Property(e => e.MonthsAmountToGetSuccess)
                .HasDefaultValue(1)
                .HasColumnName("months_amount_to_get_success");
            entity.Property(e => e.RentPriceMonthly)
                .HasPrecision(10, 2)
                .HasColumnName("rent_price_monthly");
            entity.Property(e => e.RentPriceYearly)
                .HasPrecision(10, 2)
                .HasColumnName("rent_price_yearly");
            entity.Property(e => e.ReservingStatus)
                .HasMaxLength(100)
                .HasColumnName("reserving_status");

            entity.HasOne(d => d.Machine).WithOne(p => p.MachineRentingInformation)
                .HasForeignKey<MachineRentingInformation>(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("machine_renting_information_machine_id_fkey");
        });

        modelBuilder.Entity<MachineTechnicalInformation>(entity =>
        {
            entity.HasKey(e => e.MachineId).HasName("machine_technical_information_pkey");

            entity.ToTable("machine_technical_information");

            entity.Property(e => e.MachineId)
                .ValueGeneratedNever()
                .HasColumnName("machine_id");
            entity.Property(e => e.CommandsAmount).HasColumnName("commands_amount");
            entity.Property(e => e.DetailsAmount).HasColumnName("details_amount");
            entity.Property(e => e.ProductLoad)
                .HasPrecision(5, 2)
                .HasColumnName("product_load");

            entity.HasOne(d => d.Machine).WithOne(p => p.MachineTechnicalInformation)
                .HasForeignKey<MachineTechnicalInformation>(d => d.MachineId)
                .HasConstraintName("machine_technical_information_machine_id_fkey");
        });

        modelBuilder.Entity<MachineWorkStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("machine_work_status_pkey");

            entity.ToTable("machine_work_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manufacturer_pkey");

            entity.ToTable("manufacturer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("model_pkey");

            entity.ToTable("model");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("news_pkey");

            entity.ToTable("news");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("now()")
                .HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.News)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("news_author_id_fkey");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("note_pkey");

            entity.ToTable("note");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("note_user_id_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notification_pkey");

            entity.ToTable("notification");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_type_pkey");

            entity.ToTable("payment_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.MinimalAmountInStorage)
                .HasDefaultValue(1)
                .HasColumnName("minimal_amount_in_storage");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Popularity)
                .HasPrecision(8, 2)
                .HasColumnName("popularity");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
        });

        modelBuilder.Entity<ProductMatrix>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_matrix_pkey");

            entity.ToTable("product_matrix");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("report_pkey");

            entity.ToTable("report");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateTimePublished)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time_published");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.MeterReadings).HasColumnName("meter_readings");
            entity.Property(e => e.ReportStatusId).HasColumnName("report_status_id");
            entity.Property(e => e.ReportTypeId).HasColumnName("report_type_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ReportStatus).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ReportStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("report_report_status_id_fkey");

            entity.HasOne(d => d.ReportType).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ReportTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("report_report_type_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("report_user_id_fkey");
        });

        modelBuilder.Entity<ReportStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("report_status_pkey");

            entity.ToTable("report_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ReportType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("report_type_pkey");

            entity.ToTable("report_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Reserving>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reserving_pkey");

            entity.ToTable("reserving");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateEnd).HasColumnName("date_end");
            entity.Property(e => e.DateStart).HasColumnName("date_start");
            entity.Property(e => e.Insurance)
                .HasDefaultValue(false)
                .HasColumnName("insurance");
            entity.Property(e => e.IsConfirmed)
                .HasDefaultValue(false)
                .HasColumnName("is_confirmed");
            entity.Property(e => e.MachineId).HasColumnName("machine_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Way)
                .HasMaxLength(100)
                .HasColumnName("way");

            entity.HasOne(d => d.Machine).WithMany(p => p.Reservings)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reserving_machine_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Reservings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reserving_user_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Sell>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sell_pkey");

            entity.ToTable("sell");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Change)
                .HasPrecision(10, 2)
                .HasColumnName("change");
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time");
            entity.Property(e => e.Income)
                .HasPrecision(10, 2)
                .HasColumnName("income");
            entity.Property(e => e.MachineId).HasColumnName("machine_id");
            entity.Property(e => e.PaymentTypeId).HasColumnName("payment_type_id");

            entity.HasOne(d => d.Machine).WithMany(p => p.Sells)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sell_machine_id_fkey");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Sells)
                .HasForeignKey(d => d.PaymentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sell_payment_type_id_fkey");
        });

        modelBuilder.Entity<SellProduct>(entity =>
        {
            entity.HasKey(e => new { e.SellId, e.ProductId }).HasName("sell_product_pkey");

            entity.ToTable("sell_product");

            entity.Property(e => e.SellId).HasColumnName("sell_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Amount)
                .HasDefaultValue(1)
                .HasColumnName("amount");

            entity.HasOne(d => d.Product).WithMany(p => p.SellProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sell_product_product_id_fkey");

            entity.HasOne(d => d.Sell).WithMany(p => p.SellProducts)
                .HasForeignKey(d => d.SellId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sell_product_sell_id_fkey");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("service_pkey");

            entity.ToTable("service");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.MachineId).HasColumnName("machine_id");
            entity.Property(e => e.MasterId).HasColumnName("master_id");
            entity.Property(e => e.Problems)
                .HasMaxLength(1000)
                .HasColumnName("problems");

            entity.HasOne(d => d.Machine).WithMany(p => p.Services)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service_machine_id_fkey");

            entity.HasOne(d => d.Master).WithMany(p => p.Services)
                .HasForeignKey(d => d.MasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service_user_id_fkey");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.ToTable("task");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsCompleted)
                .HasDefaultValue(false)
                .HasColumnName("is_completed");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "user_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.HasBeenOnWebsite)
                .HasDefaultValue(false)
                .HasColumnName("has_been_on_website");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.RoleId)
                .HasDefaultValue(1)
                .HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_role_id_fkey");
        });

        modelBuilder.Entity<VendingMachine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vending_machine_pkey");

            entity.ToTable("vending_machine");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.ChangeCash)
                .HasPrecision(10, 2)
                .HasColumnName("change_cash");
            entity.Property(e => e.DateInstalled)
                .HasDefaultValueSql("now()")
                .HasColumnName("date_installed");
            entity.Property(e => e.IncomeCash)
                .HasPrecision(10, 2)
                .HasColumnName("income_cash");
            entity.Property(e => e.KitOnlineId)
                .HasMaxLength(100)
                .HasColumnName("kit_online_id");
            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.ModemNumber)
                .HasMaxLength(40)
                .HasColumnName("modem_number");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Number)
                .HasMaxLength(40)
                .HasColumnName("number");
            entity.Property(e => e.OwnerCompanyId).HasColumnName("owner_company_id");
            entity.Property(e => e.Place)
                .HasMaxLength(50)
                .HasColumnName("place");
            entity.Property(e => e.RenterCompanyId).HasColumnName("renter_company_id");
            entity.Property(e => e.WorkStatusId).HasColumnName("work_status_id");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vending_machine_manufacturer_id_fkey");

            entity.HasOne(d => d.Model).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vending_machine_model_id_fkey");

            entity.HasOne(d => d.OwnerCompany).WithMany(p => p.VendingMachineOwnerCompanies)
                .HasForeignKey(d => d.OwnerCompanyId)
                .HasConstraintName("vending_machine_owner_company_id_fkey");

            entity.HasOne(d => d.RenterCompany).WithMany(p => p.VendingMachineRenterCompanies)
                .HasForeignKey(d => d.RenterCompanyId)
                .HasConstraintName("vending_machine_renter_company_id_fkey");

            entity.HasOne(d => d.WorkStatus).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.WorkStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vending_machine_work_status_id_fkey");

            entity.HasMany(d => d.ConnectionTypes).WithMany(p => p.Machines)
                .UsingEntity<Dictionary<string, object>>(
                    "MachineConnectionType",
                    r => r.HasOne<ConnectionType>().WithMany()
                        .HasForeignKey("ConnectionTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("machine_connection_type_connection_type_id_fkey"),
                    l => l.HasOne<VendingMachine>().WithMany()
                        .HasForeignKey("MachineId")
                        .HasConstraintName("machine_connection_type_machine_id_fkey"),
                    j =>
                    {
                        j.HasKey("MachineId", "ConnectionTypeId").HasName("machine_connection_type_pkey");
                        j.ToTable("machine_connection_type");
                        j.IndexerProperty<int>("MachineId").HasColumnName("machine_id");
                        j.IndexerProperty<int>("ConnectionTypeId").HasColumnName("connection_type_id");
                    });

            entity.HasMany(d => d.PaymentTypes).WithMany(p => p.Machines)
                .UsingEntity<Dictionary<string, object>>(
                    "MachinePaymentType",
                    r => r.HasOne<PaymentType>().WithMany()
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("machine_payment_type_payment_type_id_fkey"),
                    l => l.HasOne<VendingMachine>().WithMany()
                        .HasForeignKey("MachineId")
                        .HasConstraintName("machine_payment_type_machine_id_fkey"),
                    j =>
                    {
                        j.HasKey("MachineId", "PaymentTypeId").HasName("machine_payment_type_pkey");
                        j.ToTable("machine_payment_type");
                        j.IndexerProperty<int>("MachineId").HasColumnName("machine_id");
                        j.IndexerProperty<int>("PaymentTypeId").HasColumnName("payment_type_id");
                    });

            entity.HasMany(d => d.Statuses).WithMany(p => p.Machines)
                .UsingEntity<Dictionary<string, object>>(
                    "MachineStatus",
                    r => r.HasOne<AdditionalMachineStatus>().WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("machine_status_status_id_fkey"),
                    l => l.HasOne<VendingMachine>().WithMany()
                        .HasForeignKey("MachineId")
                        .HasConstraintName("machine_status_machine_id_fkey"),
                    j =>
                    {
                        j.HasKey("MachineId", "StatusId").HasName("machine_status_pkey");
                        j.ToTable("machine_status");
                        j.IndexerProperty<int>("MachineId").HasColumnName("machine_id");
                        j.IndexerProperty<int>("StatusId").HasColumnName("status_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
