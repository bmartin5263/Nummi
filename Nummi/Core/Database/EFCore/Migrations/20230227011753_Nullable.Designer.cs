// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Nummi.Core.Database.EFCore;

#nullable disable

namespace Nummi.Core.Database.EFCore.Migrations
{
    [DbContext(typeof(EFCoreContext))]
    [Migration("20230227011753_Nullable")]
    partial class Nullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("character varying(50000)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("UserCode");

                    b.HasIndex("DeviceCode")
                        .IsUnique();

                    b.HasIndex("Expiration");

                    b.ToTable("DeviceCodes", (string)null);
                });

            modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Key", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Algorithm")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("DataProtected")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsX509Certificate")
                        .HasColumnType("boolean");

                    b.Property<string>("Use")
                        .HasColumnType("text");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Use");

                    b.ToTable("Keys", (string)null);
                });

            modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("ConsumedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("character varying(50000)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Key");

                    b.HasIndex("ConsumedTime");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.HasIndex("SubjectId", "SessionId", "Type");

                    b.ToTable("PersistedGrants", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Bot.Bot", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("BotId");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CurrentBotActivationId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Funds")
                        .HasColumnType("numeric");

                    b.Property<bool>("InErrorState")
                        .HasColumnType("boolean");

                    b.Property<string>("Mode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId", "Name");

                    b.HasIndex("CurrentBotActivationId")
                        .IsUnique();

                    b.ToTable("Bot", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Bot.BotActivation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("BotActivationId");

                    b.Property<Guid>("BotId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("BotId");

                    b.ToTable("BotActivation", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Bot.BotLog", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("BotLogId");

                    b.Property<Guid>("BotActivationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan>("TotalTime")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("BotActivationId");

                    b.ToTable("BotLog", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Crypto.Bar", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("OpenTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan>("Period")
                        .HasColumnType("interval");

                    b.Property<decimal>("Close")
                        .HasColumnType("numeric");

                    b.Property<decimal>("High")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Low")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Open")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Volume")
                        .HasColumnType("numeric");

                    b.HasKey("Symbol", "OpenTime", "Period");

                    b.ToTable("HistoricalBar", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Crypto.OrderLog", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("OrderLogId");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<decimal?>("FundsAfter")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("FundsBefore")
                        .HasColumnType("numeric");

                    b.Property<string>("Quantity")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Side")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("StrategyLogId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("StrategyLogId");

                    b.ToTable("OrderLog", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Crypto.Price", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("text");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Symbol", "Time");

                    b.ToTable("HistoricalPrice", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Simulations.Simulation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("SimulationId");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("FinishedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("SimulationEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("SimulationStartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("StartedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeSpan?>("TotalExecutionTime")
                        .HasColumnType("interval")
                        .HasColumnName("TotalExecutionTime");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Simulation", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.Strategy", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("StrategyId");

                    b.Property<Guid?>("BotActivationId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ParametersJson")
                        .HasColumnType("jsonb");

                    b.Property<Guid?>("SimulationId")
                        .HasColumnType("uuid");

                    b.Property<string>("StateJson")
                        .HasColumnType("jsonb");

                    b.Property<Guid>("StrategyTemplateVersionId")
                        .HasColumnType("uuid");

                    b.Property<string>("StrategyType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("BotActivationId")
                        .IsUnique();

                    b.HasIndex("SimulationId")
                        .IsUnique();

                    b.HasIndex("StrategyTemplateVersionId");

                    b.ToTable("Strategy", (string)null);

                    b.HasDiscriminator<string>("StrategyType").HasValue("Strategy");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyLog", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("StrategyLogId");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ApiCalls")
                        .HasColumnType("integer");

                    b.Property<Guid?>("BotLogId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<string>("Mode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("StrategyId")
                        .HasColumnType("uuid");

                    b.Property<TimeSpan>("TotalApiCallTime")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("TotalTime")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("BotLogId");

                    b.HasIndex("StrategyId");

                    b.ToTable("StrategyLog", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("StrategyTemplateId");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId", "Name");

                    b.ToTable("StrategyTemplate", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyTemplateVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("StrategyTemplateVersionId");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan>("Frequency")
                        .HasColumnType("interval");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SourceCode")
                        .HasColumnType("text");

                    b.Property<Guid>("StrategyTemplateId")
                        .HasColumnType("uuid");

                    b.Property<string>("StrategyTemplateType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("VersionNumber")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasAlternateKey("StrategyTemplateId", "VersionNumber");

                    b.ToTable("StrategyTemplateVersion", (string)null);

                    b.HasDiscriminator<string>("StrategyTemplateType").HasValue("StrategyTemplateVersion");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Test.Blog", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("Nummi.Core.Domain.Test.Post", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("BlogId")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Meta")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("BlogId")
                        .IsUnique();

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiRole", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("AlpacaLiveId")
                        .HasColumnType("text");

                    b.Property<string>("AlpacaLiveKey")
                        .HasColumnType("text");

                    b.Property<string>("AlpacaPaperId")
                        .HasColumnType("text");

                    b.Property<string>("AlpacaPaperKey")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken", (string)null);
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyBuiltin", b =>
                {
                    b.HasBaseType("Nummi.Core.Domain.Strategies.Strategy");

                    b.HasDiscriminator().HasValue("builtin");
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyTemplateVersionBuiltin", b =>
                {
                    b.HasBaseType("Nummi.Core.Domain.Strategies.StrategyTemplateVersion");

                    b.Property<string>("LogicTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ParameterTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StateTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("builtin");
                });

            modelBuilder.Entity("Nummi.Core.Domain.Bot.Bot", b =>
                {
                    b.HasOne("Nummi.Core.Domain.Bot.BotActivation", "CurrentActivation")
                        .WithOne()
                        .HasForeignKey("Nummi.Core.Domain.Bot.Bot", "CurrentBotActivationId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("Nummi.Core.Domain.User.NummiUser", null)
                        .WithMany("Bots")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentActivation");
                });

            modelBuilder.Entity("Nummi.Core.Domain.Bot.BotActivation", b =>
                {
                    b.HasOne("Nummi.Core.Domain.Bot.Bot", null)
                        .WithMany("ActivationHistory")
                        .HasForeignKey("BotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Bot.BotLog", b =>
                {
                    b.HasOne("Nummi.Core.Domain.Bot.BotActivation", null)
                        .WithMany("Logs")
                        .HasForeignKey("BotActivationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Crypto.OrderLog", b =>
                {
                    b.HasOne("Nummi.Core.Domain.Strategies.StrategyLog", null)
                        .WithMany("Orders")
                        .HasForeignKey("StrategyLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Simulations.Simulation", b =>
                {
                    b.HasOne("Nummi.Core.Domain.User.NummiUser", null)
                        .WithMany("Simulations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.Strategy", b =>
                {
                    b.HasOne("Nummi.Core.Domain.Bot.BotActivation", null)
                        .WithOne("Strategy")
                        .HasForeignKey("Nummi.Core.Domain.Strategies.Strategy", "BotActivationId");

                    b.HasOne("Nummi.Core.Domain.Simulations.Simulation", null)
                        .WithOne("Strategy")
                        .HasForeignKey("Nummi.Core.Domain.Strategies.Strategy", "SimulationId");

                    b.HasOne("Nummi.Core.Domain.Strategies.StrategyTemplateVersion", "ParentTemplateVersion")
                        .WithMany()
                        .HasForeignKey("StrategyTemplateVersionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ParentTemplateVersion");
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyLog", b =>
                {
                    b.HasOne("Nummi.Core.Domain.Bot.BotLog", null)
                        .WithMany()
                        .HasForeignKey("BotLogId");

                    b.HasOne("Nummi.Core.Domain.Strategies.Strategy", null)
                        .WithMany("Logs")
                        .HasForeignKey("StrategyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyTemplate", b =>
                {
                    b.HasOne("Nummi.Core.Domain.User.NummiUser", null)
                        .WithMany("StrategyTemplates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyTemplateVersion", b =>
                {
                    b.HasOne("Nummi.Core.Domain.Strategies.StrategyTemplate", null)
                        .WithMany("Versions")
                        .HasForeignKey("StrategyTemplateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Test.Post", b =>
                {
                    b.HasOne("Nummi.Core.Domain.Test.Blog", null)
                        .WithOne("Post")
                        .HasForeignKey("Nummi.Core.Domain.Test.Post", "BlogId");
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiRoleClaim", b =>
                {
                    b.HasOne("Nummi.Core.Domain.User.NummiRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUserClaim", b =>
                {
                    b.HasOne("Nummi.Core.Domain.User.NummiUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUserLogin", b =>
                {
                    b.HasOne("Nummi.Core.Domain.User.NummiUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUserRole", b =>
                {
                    b.HasOne("Nummi.Core.Domain.User.NummiRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nummi.Core.Domain.User.NummiUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUserToken", b =>
                {
                    b.HasOne("Nummi.Core.Domain.User.NummiUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Bot.Bot", b =>
                {
                    b.Navigation("ActivationHistory");
                });

            modelBuilder.Entity("Nummi.Core.Domain.Bot.BotActivation", b =>
                {
                    b.Navigation("Logs");

                    b.Navigation("Strategy")
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Simulations.Simulation", b =>
                {
                    b.Navigation("Strategy")
                        .IsRequired();
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.Strategy", b =>
                {
                    b.Navigation("Logs");
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyLog", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Nummi.Core.Domain.Strategies.StrategyTemplate", b =>
                {
                    b.Navigation("Versions");
                });

            modelBuilder.Entity("Nummi.Core.Domain.Test.Blog", b =>
                {
                    b.Navigation("Post");
                });

            modelBuilder.Entity("Nummi.Core.Domain.User.NummiUser", b =>
                {
                    b.Navigation("Bots");

                    b.Navigation("Simulations");

                    b.Navigation("StrategyTemplates");
                });
#pragma warning restore 612, 618
        }
    }
}
