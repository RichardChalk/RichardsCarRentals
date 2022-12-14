// <auto-generated />
using System;
using EFCoreCodeFirstTogether.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace RichardsCarRentals.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221128174819_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EFCoreCodeFirstTogether.Data.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CarBookingLicensePlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CarBookingLicensePlate");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("EFCoreCodeFirstTogether.Data.Car", b =>
                {
                    b.Property<string>("LicensePlate")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ManufacturingYear")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("LicensePlate");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("EFCoreCodeFirstTogether.Data.Booking", b =>
                {
                    b.HasOne("EFCoreCodeFirstTogether.Data.Car", "CarBooking")
                        .WithMany()
                        .HasForeignKey("CarBookingLicensePlate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarBooking");
                });
#pragma warning restore 612, 618
        }
    }
}
