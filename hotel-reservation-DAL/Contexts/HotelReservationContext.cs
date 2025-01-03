﻿using hotel_reservation_DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace hotel_reservation_DAL.Contexts
{
    public class HotelReservationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=HotelReservation;user=root;password=;AllowZeroDateTime=True",
                new MySqlServerVersion(new Version(8, 0, 21)));
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);
            });
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasIndex(e => e.Number).IsUnique();
                entity.HasOne(e => e.RoomType).WithMany(e => e.Rooms).HasForeignKey(e => e.RoomTypeId);
            });
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasOne(e => e.Client).WithMany(e => e.Reservations).HasForeignKey(e => e.ClientId);
                entity.HasOne(e => e.Room).WithMany(e => e.Reservations).HasForeignKey(e => e.RoomId);
                entity.HasOne(e => e.Payment).WithOne(e => e.Reservation).HasForeignKey<Reservation>(e => e.PaymentId);
                //current date
                entity.Property(e => e.Date).HasDefaultValueSql("CURRENT_TIMESTAMP");
              

            });
            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasMany(e => e.Rooms).WithOne(e => e.RoomType).HasForeignKey(e => e.RoomTypeId);
               

            });
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasMany(e => e.Reservations).WithOne(e => e.Client).HasForeignKey(e => e.ClientId);
                entity.HasIndex(e => e.Cin).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Date).HasDefaultValueSql("CURRENT_TIMESTAMP");




            });
        }
    }
}
