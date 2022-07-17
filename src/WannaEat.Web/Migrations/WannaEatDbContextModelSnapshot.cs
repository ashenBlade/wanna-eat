﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WannaEat.Web.Services;

#nullable disable

namespace WannaEat.Web.Migrations
{
    [DbContext(typeof(WannaEatDbContext))]
    partial class WannaEatDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CookingApplianceDish", b =>
                {
                    b.Property<int>("RequiredToCookId")
                        .HasColumnType("integer");

                    b.Property<int>("UsedInCookingId")
                        .HasColumnType("integer");

                    b.HasKey("RequiredToCookId", "UsedInCookingId");

                    b.HasIndex("UsedInCookingId");

                    b.ToTable("CookingApplianceDish");
                });

            modelBuilder.Entity("WannaEat.Web.Models.CookingAppliance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CookingAppliance");
                });

            modelBuilder.Entity("WannaEat.Web.Models.Food", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Foods");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Food");
                });

            modelBuilder.Entity("WannaEat.Web.Models.Dish", b =>
                {
                    b.HasBaseType("WannaEat.Web.Models.Food");

                    b.Property<string>("Recipe")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Dish");
                });

            modelBuilder.Entity("WannaEat.Web.Models.Product", b =>
                {
                    b.HasBaseType("WannaEat.Web.Models.Food");

                    b.Property<bool>("IsFoundational")
                        .HasColumnType("boolean");

                    b.HasDiscriminator().HasValue("Product");
                });

            modelBuilder.Entity("CookingApplianceDish", b =>
                {
                    b.HasOne("WannaEat.Web.Models.CookingAppliance", null)
                        .WithMany()
                        .HasForeignKey("RequiredToCookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WannaEat.Web.Models.Dish", null)
                        .WithMany()
                        .HasForeignKey("UsedInCookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WannaEat.Web.Models.Food", b =>
                {
                    b.OwnsOne("WannaEat.Web.Models.Minerals", "Minerals", b1 =>
                        {
                            b1.Property<int>("FoodId")
                                .HasColumnType("integer");

                            b1.Property<double?>("Calcium")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Chromium")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Copper")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Fluorine")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Iodine")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Iron")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Magnesium")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Manganese")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Phosphorus")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Potassium")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Selenium")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Silicon")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Sodium")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Sulfur")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Zinc")
                                .HasColumnType("double precision");

                            b1.HasKey("FoodId");

                            b1.ToTable("Foods");

                            b1.WithOwner()
                                .HasForeignKey("FoodId");
                        });

                    b.OwnsOne("WannaEat.Web.Models.NutritionalValue", "NutritionalValue", b1 =>
                        {
                            b1.Property<int>("FoodId")
                                .HasColumnType("integer");

                            b1.Property<double?>("Carbohydrates")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Cellulose")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Cholesterol")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Fat")
                                .HasColumnType("double precision");

                            b1.Property<double?>("GlycemicIndex")
                                .HasColumnType("double precision");

                            b1.Property<double?>("KiloCalories")
                                .HasColumnType("double precision");

                            b1.Property<double?>("OrganicAcids")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Protein")
                                .HasColumnType("double precision");

                            b1.Property<double?>("SaturatedFats")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Water")
                                .HasColumnType("double precision");

                            b1.HasKey("FoodId");

                            b1.ToTable("Foods");

                            b1.WithOwner()
                                .HasForeignKey("FoodId");
                        });

                    b.OwnsOne("WannaEat.Web.Models.Vitamins", "Vitamins", b1 =>
                        {
                            b1.Property<int>("FoodId")
                                .HasColumnType("integer");

                            b1.Property<double?>("A")
                                .HasColumnType("double precision");

                            b1.Property<double?>("B1")
                                .HasColumnType("double precision");

                            b1.Property<double?>("B2")
                                .HasColumnType("double precision");

                            b1.Property<double?>("B3PP")
                                .HasColumnType("double precision");

                            b1.Property<double?>("B4")
                                .HasColumnType("double precision");

                            b1.Property<double?>("B5")
                                .HasColumnType("double precision");

                            b1.Property<double?>("B6")
                                .HasColumnType("double precision");

                            b1.Property<double?>("B9")
                                .HasColumnType("double precision");

                            b1.Property<double?>("C")
                                .HasColumnType("double precision");

                            b1.Property<double?>("D")
                                .HasColumnType("double precision");

                            b1.Property<double?>("E")
                                .HasColumnType("double precision");

                            b1.Property<double?>("H")
                                .HasColumnType("double precision");

                            b1.Property<double?>("K")
                                .HasColumnType("double precision");

                            b1.HasKey("FoodId");

                            b1.ToTable("Foods");

                            b1.WithOwner()
                                .HasForeignKey("FoodId");
                        });

                    b.Navigation("Minerals")
                        .IsRequired();

                    b.Navigation("NutritionalValue")
                        .IsRequired();

                    b.Navigation("Vitamins")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
