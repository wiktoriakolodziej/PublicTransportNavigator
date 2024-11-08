﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PublicTransportNavigator;

#nullable disable

namespace PublicTransportNavigator.Migrations
{
    [DbContext(typeof(PublicTransportNavigatorContext))]
    [Migration("20240928155238_UserDetails")]
    partial class UserDetails
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PublicTransportNavigator.Models.Bus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("FirstBusStopId")
                        .HasColumnType("bigint")
                        .HasColumnName("first_bus_stop_id");

                    b.Property<long>("LastBusStopId")
                        .HasColumnType("bigint")
                        .HasColumnName("last_bus_stop_id");

                    b.Property<int>("Number")
                        .HasColumnType("integer")
                        .HasColumnName("bus_number");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("FirstBusStopId");

                    b.HasIndex("LastBusStopId");

                    b.ToTable("buses");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.BusSeat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("BusId")
                        .HasColumnType("bigint")
                        .HasColumnName("bus_id");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("seat_position");

                    b.Property<long>("SeatTypeId")
                        .HasColumnType("bigint")
                        .HasColumnName("seat_type_id");

                    b.HasKey("Id");

                    b.HasIndex("BusId");

                    b.HasIndex("SeatTypeId");

                    b.ToTable("bus_seats");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.BusStop", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<bool>("OnRequest")
                        .HasColumnType("boolean")
                        .HasColumnName("on_request");

                    b.HasKey("Id");

                    b.ToTable("bus_stops");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.Discount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("description");

                    b.Property<int>("Percentage")
                        .HasColumnType("integer")
                        .HasColumnName("percentage");

                    b.HasKey("Id");

                    b.ToTable("discounts");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.ReservedSeat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("BusSeatId")
                        .HasColumnType("bigint")
                        .HasColumnName("bus_seat_id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("reservation_date");

                    b.Property<long>("TimeInId")
                        .HasColumnType("bigint")
                        .HasColumnName("timetable_in_id");

                    b.Property<long>("TimeOffId")
                        .HasColumnType("bigint")
                        .HasColumnName("timetable_off_id");

                    b.Property<long>("UserTravelId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_travel_id");

                    b.HasKey("Id");

                    b.HasIndex("BusSeatId");

                    b.HasIndex("TimeInId");

                    b.HasIndex("TimeOffId");

                    b.HasIndex("UserTravelId");

                    b.ToTable("reserved_seat");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.Seat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("SeatType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.ToTable("seat_types");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.TicketType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<float>("Price")
                        .HasColumnType("real")
                        .HasColumnName("price");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("interval")
                        .HasColumnName("time_range");

                    b.HasKey("Id");

                    b.ToTable("ticket_types");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.Timetable", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("BusId")
                        .HasColumnType("bigint")
                        .HasColumnName("bus_id");

                    b.Property<long>("BusStopId")
                        .HasColumnType("bigint")
                        .HasColumnName("bus_stop_id");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("bus_arrival_time");

                    b.HasKey("Id");

                    b.HasIndex("BusId");

                    b.HasIndex("BusStopId");

                    b.ToTable("timetables");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("surname");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.UserFavouriteBusStop", b =>
                {
                    b.Property<long>("BusStopId")
                        .HasColumnType("bigint")
                        .HasColumnName("bus_stop_id");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("BusStopId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("user_favourite_bus_stops");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.UserTravel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("TicketId")
                        .HasColumnType("bigint")
                        .HasColumnName("ticket_id");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.HasIndex("UserId");

                    b.ToTable("user_travels");
                });

            modelBuilder.Entity("user_discounts", b =>
                {
                    b.Property<long>("discount_id")
                        .HasColumnType("bigint");

                    b.Property<long>("user_id")
                        .HasColumnType("bigint");

                    b.HasKey("discount_id", "user_id");

                    b.HasIndex("user_id");

                    b.ToTable("user_discounts");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.Bus", b =>
                {
                    b.HasOne("PublicTransportNavigator.Models.BusStop", "FirstBusStop")
                        .WithMany()
                        .HasForeignKey("FirstBusStopId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PublicTransportNavigator.Models.BusStop", "LastBusStop")
                        .WithMany()
                        .HasForeignKey("LastBusStopId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FirstBusStop");

                    b.Navigation("LastBusStop");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.BusSeat", b =>
                {
                    b.HasOne("PublicTransportNavigator.Models.Bus", "Bus")
                        .WithMany("BusSeats")
                        .HasForeignKey("BusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PublicTransportNavigator.Models.Seat", "SeatType")
                        .WithMany()
                        .HasForeignKey("SeatTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");

                    b.Navigation("SeatType");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.ReservedSeat", b =>
                {
                    b.HasOne("PublicTransportNavigator.Models.BusSeat", "BusSeat")
                        .WithMany()
                        .HasForeignKey("BusSeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PublicTransportNavigator.Models.Timetable", "TimeIn")
                        .WithMany()
                        .HasForeignKey("TimeInId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("PublicTransportNavigator.Models.Timetable", "TimeOff")
                        .WithMany()
                        .HasForeignKey("TimeOffId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("PublicTransportNavigator.Models.UserTravel", "UserTravel")
                        .WithMany("ReservedSeats")
                        .HasForeignKey("UserTravelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusSeat");

                    b.Navigation("TimeIn");

                    b.Navigation("TimeOff");

                    b.Navigation("UserTravel");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.Timetable", b =>
                {
                    b.HasOne("PublicTransportNavigator.Models.Bus", "Bus")
                        .WithMany("Timetables")
                        .HasForeignKey("BusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PublicTransportNavigator.Models.BusStop", "BusStop")
                        .WithMany("Timetables")
                        .HasForeignKey("BusStopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");

                    b.Navigation("BusStop");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.UserFavouriteBusStop", b =>
                {
                    b.HasOne("PublicTransportNavigator.Models.BusStop", "BusStop")
                        .WithMany()
                        .HasForeignKey("BusStopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PublicTransportNavigator.Models.User", "User")
                        .WithMany("Favourites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusStop");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.UserTravel", b =>
                {
                    b.HasOne("PublicTransportNavigator.Models.TicketType", "TicketType")
                        .WithMany()
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PublicTransportNavigator.Models.User", "User")
                        .WithMany("TravelHistory")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TicketType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("user_discounts", b =>
                {
                    b.HasOne("PublicTransportNavigator.Models.Discount", null)
                        .WithMany()
                        .HasForeignKey("discount_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PublicTransportNavigator.Models.User", null)
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.Bus", b =>
                {
                    b.Navigation("BusSeats");

                    b.Navigation("Timetables");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.BusStop", b =>
                {
                    b.Navigation("Timetables");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.User", b =>
                {
                    b.Navigation("Favourites");

                    b.Navigation("TravelHistory");
                });

            modelBuilder.Entity("PublicTransportNavigator.Models.UserTravel", b =>
                {
                    b.Navigation("ReservedSeats");
                });
#pragma warning restore 612, 618
        }
    }
}
