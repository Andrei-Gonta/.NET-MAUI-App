using System;
using CarMeetApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarMeetApp.Migrations
{
    [DbContext(typeof(CarMeetDbContext))]
    partial class CarMeetDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("CarMeetApp.Models.EventItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Organizer")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Events", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2026, 4, 18, 14, 50, 29, 239, DateTimeKind.Utc).AddTicks(3063),
                            Date = new DateTime(2026, 4, 21, 18, 0, 0, 0, DateTimeKind.Local),
                            Description = "Casual evening meet for all builds: imports, muscle, and classics.",
                            Location = "Downtown Garage, Austin",
                            Organizer = "Turbo Club",
                            Title = "Sunset Street Meet"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2026, 4, 18, 14, 50, 29, 239, DateTimeKind.Utc).AddTicks(3237),
                            Date = new DateTime(2026, 4, 25, 9, 0, 0, 0, DateTimeKind.Local),
                            Description = "Morning cruise with photo stops and coffee at the summit.",
                            Location = "Blue Ridge Scenic Point",
                            Organizer = "Apex Riders",
                            Title = "Mountain Drive Meetup"
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2026, 4, 18, 14, 50, 29, 239, DateTimeKind.Utc).AddTicks(3240),
                            Date = new DateTime(2026, 5, 2, 20, 0, 0, 0, DateTimeKind.Local),
                            Description = "Night showcase featuring custom lighting and audio builds.",
                            Location = "Riverside Plaza, Seattle",
                            Organizer = "NightShift Garage",
                            Title = "Neon Night Showcase"
                        });
                });

            modelBuilder.Entity("CarMeetApp.Models.EventUser", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CarBrand")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<double?>("CarEngineSizeLiters")
                        .HasColumnType("REAL");

                    b.Property<string>("CarGeneration")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int?>("CarHorsepowerHp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CarModel")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("OtherDetails")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SignedUpAt")
                        .HasColumnType("TEXT");

                    b.HasKey("EventId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("EventUsers", (string)null);
                });

            modelBuilder.Entity("CarMeetApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<double?>("EngineSizeLiters")
                        .HasColumnType("REAL");

                    b.Property<int?>("EventItemId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int?>("HorsepowerHp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OtherDetails")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SelectedBrand")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("SelectedGeneration")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("SelectedModel")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EventItemId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("CarMeetApp.Models.EventUser", b =>
                {
                    b.HasOne("CarMeetApp.Models.EventItem", "Event")
                        .WithMany("EventUsers")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarMeetApp.Models.User", "User")
                        .WithMany("EventUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarMeetApp.Models.User", b =>
                {
                    b.HasOne("CarMeetApp.Models.EventItem", null)
                        .WithMany("SignedUpUsers")
                        .HasForeignKey("EventItemId");
                });

            modelBuilder.Entity("CarMeetApp.Models.EventItem", b =>
                {
                    b.Navigation("EventUsers");

                    b.Navigation("SignedUpUsers");
                });

            modelBuilder.Entity("CarMeetApp.Models.User", b =>
                {
                    b.Navigation("EventUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
