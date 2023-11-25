﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PontoLegal.Data;
using PontoLegal.Domain.Enums;

#nullable disable

namespace PontoLegal.Data.Migrations
{
    [DbContext(typeof(PontoLegalContext))]
    [Migration("20231007015009_Refactor ClockTimeStatus Column")]
    partial class RefactorClockTimeStatusColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("PontoLegal.Domain.Entities.Company", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("Id");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("TEXT")
                        .HasColumnName("Cnpj");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATETIME")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Company", (string)null);
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.Department", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATETIME")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Department", (string)null);
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.Employee", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("Id");

                    b.Property<string>("CompanyId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATETIME")
                        .HasColumnName("CreatedAt");

                    b.Property<DateOnly>("HireDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("HireDate");

                    b.Property<string>("JobPositionId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ManagerId")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("ManagerId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .HasColumnName("RegistrationNumber");

                    b.Property<string>("WorkingDayId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("JobPositionId");

                    b.HasIndex("WorkingDayId");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.JobPosition", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATETIME")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("DepartmentId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("JobPosition", (string)null);
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.TimeClock", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("Id");

                    b.Property<int>("ClockTimeStatus")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ClockTimeStatus");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATETIME")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("TEXT");

                    b.Property<RegisterType>("RegisterType")
                        .HasColumnType("INTEGER")
                        .HasColumnName("RegisterType");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("TimeClock", (string)null);
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.WorkingDay", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATETIME")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("EndBreak")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasColumnName("EndBreak");

                    b.Property<string>("EndWork")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasColumnName("EndWork");

                    b.Property<short>("MinutesTolerance")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MinutesTolerance");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.Property<string>("StartBreak")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasColumnName("StartBreak");

                    b.Property<string>("StartWork")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasColumnName("StartWork");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Type");

                    b.HasKey("Id");

                    b.ToTable("WorkingDay", (string)null);
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.Employee", b =>
                {
                    b.HasOne("PontoLegal.Domain.Entities.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PontoLegal.Domain.Entities.JobPosition", "JobPosition")
                        .WithMany("Employees")
                        .HasForeignKey("JobPositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PontoLegal.Domain.Entities.WorkingDay", "WorkingDay")
                        .WithMany("Employees")
                        .HasForeignKey("WorkingDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("PontoLegal.Domain.ValueObjects.Pis", "Pis", b1 =>
                        {
                            b1.Property<string>("EmployeeId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(11)
                                .HasColumnType("TEXT")
                                .HasColumnName("Pis");

                            b1.HasKey("EmployeeId");

                            b1.ToTable("Employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeId");
                        });

                    b.Navigation("Company");

                    b.Navigation("JobPosition");

                    b.Navigation("Pis")
                        .IsRequired();

                    b.Navigation("WorkingDay");
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.JobPosition", b =>
                {
                    b.HasOne("PontoLegal.Domain.Entities.Department", "Department")
                        .WithMany("JobPositions")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.TimeClock", b =>
                {
                    b.HasOne("PontoLegal.Domain.Entities.Employee", "Employee")
                        .WithMany("TimeClocks")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.Company", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.Department", b =>
                {
                    b.Navigation("JobPositions");
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.Employee", b =>
                {
                    b.Navigation("TimeClocks");
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.JobPosition", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("PontoLegal.Domain.Entities.WorkingDay", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
