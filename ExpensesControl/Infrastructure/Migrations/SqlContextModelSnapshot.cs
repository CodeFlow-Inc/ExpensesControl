﻿// <auto-generated />
using System;
using ExpensesControl.Infrastructure.SqlServer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpensesControl.Infrastructure.SqlServer.Migrations
{
    [DbContext(typeof(SqlContext))]
    partial class SqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExpensesControl.Domain.Entities.AggregateRoot.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Category")
                        .HasColumnType("int")
                        .HasColumnName("category");

                    b.Property<string>("CreatedByUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("created_by_user");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime")
                        .HasColumnName("creation_date");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("description");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("end_date");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("last_update_date");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasColumnName("notes");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.Property<string>("UpdatedByUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("updated_by_user");

                    b.Property<int>("UserCode")
                        .HasColumnType("int")
                        .HasColumnName("user_code");

                    b.HasKey("Id");

                    b.ToTable("expenses", (string)null);
                });

            modelBuilder.Entity("ExpensesControl.Domain.Entities.AggregateRoot.Expense", b =>
                {
                    b.OwnsOne("ExpensesControl.Domain.Entities.ValueObjects.Payment", "Payment", b1 =>
                        {
                            b1.Property<int>("ExpenseId")
                                .HasColumnType("int");

                            b1.Property<int?>("InstallmentCount")
                                .HasColumnType("int")
                                .HasColumnName("installment_count");

                            b1.Property<bool>("IsInstallment")
                                .HasColumnType("bit");

                            b1.Property<string>("Notes")
                                .HasMaxLength(500)
                                .HasColumnType("nvarchar(500)")
                                .HasColumnName("payment_notes");

                            b1.Property<decimal>("TotalValue")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("total_value");

                            b1.Property<int>("Type")
                                .HasColumnType("int")
                                .HasColumnName("payment_method_type");

                            b1.HasKey("ExpenseId");

                            b1.ToTable("expenses");

                            b1.WithOwner()
                                .HasForeignKey("ExpenseId");
                        });

                    b.OwnsOne("ExpensesControl.Domain.Entities.ValueObjects.Recurrence", "Recurrence", b1 =>
                        {
                            b1.Property<int>("ExpenseId")
                                .HasColumnType("int");

                            b1.Property<bool>("IsRecurring")
                                .HasColumnType("bit")
                                .HasColumnName("is_recurring");

                            b1.Property<int?>("MaxOccurrences")
                                .HasColumnType("int")
                                .HasColumnName("max_occurrences");

                            b1.Property<int>("Periodicity")
                                .HasColumnType("int")
                                .HasColumnName("recurrence_periodicity");

                            b1.HasKey("ExpenseId");

                            b1.ToTable("expenses");

                            b1.WithOwner()
                                .HasForeignKey("ExpenseId");
                        });

                    b.Navigation("Payment")
                        .IsRequired();

                    b.Navigation("Recurrence")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
