using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

public partial class PopNGoDB : DbContext
{
    public PopNGoDB()
    {
    }

    public PopNGoDB(DbContextOptions<PopNGoDB> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountRecord> AccountRecords { get; set; }

    public virtual DbSet<BookmarkList> BookmarkLists { get; set; }

    public virtual DbSet<EmailHistory> EmailHistories { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventHistory> EventHistories { get; set; }

    public virtual DbSet<FavoriteEvent> FavoriteEvents { get; set; }

    public virtual DbSet<Itinerary> Itineraries { get; set; }

    public virtual DbSet<ItineraryEvent> ItineraryEvents { get; set; }

    public virtual DbSet<PgUser> PgUsers { get; set; }

    public virtual DbSet<ScheduledNotification> ScheduledNotifications { get; set; }

    public virtual DbSet<SearchRecord> SearchRecords { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TicketLink> TicketLinks { get; set; }

    public virtual DbSet<Weather> Weathers { get; set; }

    public virtual DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ServerConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountR__3214EC2736376E4C");

            entity.ToTable("AccountRecord");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Day).HasColumnType("datetime");
        });

        modelBuilder.Entity<BookmarkList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookmark__3214EC27F09E82A8");

            entity.ToTable("BookmarkList");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(128);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<EmailHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmailHis__3214EC2725E92579");

            entity.ToTable("EmailHistory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TimeSent).HasColumnType("datetime");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.EmailHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailHistory_UserID");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC27AFF3F013");

            entity.ToTable("Event");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApiEventId)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("ApiEventID");
            entity.Property(e => e.EventDate).HasColumnType("datetime");
            entity.Property(e => e.EventLocation).HasMaxLength(255);
            entity.Property(e => e.EventName).HasMaxLength(255);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.VenueName).HasMaxLength(255);
            entity.Property(e => e.VenuePhoneNumber).HasMaxLength(255);
            entity.Property(e => e.VenueRating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.VenueWebsite).HasMaxLength(255);
        });

        modelBuilder.Entity<EventHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventHis__3214EC2702179907");

            entity.ToTable("EventHistory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ViewedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Event).WithMany(p => p.EventHistories)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventHistory_EventID");

            entity.HasOne(d => d.User).WithMany(p => p.EventHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventHistory_UserID");
        });

        modelBuilder.Entity<FavoriteEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Favorite__3214EC2719CE2A1A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookmarkListId).HasColumnName("BookmarkListID");
            entity.Property(e => e.EventId).HasColumnName("EventID");

            entity.HasOne(d => d.BookmarkList).WithMany(p => p.FavoriteEvents)
                .HasForeignKey(d => d.BookmarkListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteEvents_BookmarkListID");

            entity.HasOne(d => d.Event).WithMany(p => p.FavoriteEvents)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteEvents_EventID");
        });

        modelBuilder.Entity<Itinerary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Itinerar__3214EC271C0B7234");

            entity.ToTable("Itinerary");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItineraryTitle)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Itineraries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Itinerary_UserID");
        });

        modelBuilder.Entity<ItineraryEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Itinerar__3214EC271D3BA638");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");

            entity.HasOne(d => d.Event).WithMany(p => p.ItineraryEvents)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItineraryEvents_EventID");

            entity.HasOne(d => d.Itinerary).WithMany(p => p.ItineraryEvents)
                .HasForeignKey(d => d.ItineraryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItineraryEvents_ItineraryID");
        });

        modelBuilder.Entity<PgUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PG_User__3214EC270B41BF75");

            entity.ToTable("PG_User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AspnetuserId)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("ASPNETUserID");
        });

        modelBuilder.Entity<ScheduledNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC27DB1ACF27");

            entity.ToTable("ScheduledNotification");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ScheduledNotifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduledNotification_UserID");
        });

        modelBuilder.Entity<SearchRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SearchRe__3214EC2781992411");

            entity.ToTable("SearchRecord");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SearchQuery)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC27880F5087");

            entity.ToTable("Tag");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BackgroundColor)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.TextColor)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<TicketLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketLi__3214EC27EE84785D");

            entity.ToTable("TicketLink");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.Link).HasMaxLength(255);
            entity.Property(e => e.Source).HasMaxLength(255);

            entity.HasOne(d => d.Event).WithMany(p => p.TicketLinks)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketLink_EventID");
        });

        modelBuilder.Entity<Weather>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Weather__3214EC27B681ED6D");

            entity.ToTable("Weather");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateCached).HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
        });

        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WeatherF__3214EC27B9D94B51");

            entity.ToTable("WeatherForecast");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Condition)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.PrecipitationType)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Weather).WithMany(p => p.WeatherForecasts)
                .HasForeignKey(d => d.WeatherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeatherForecast_WeatherId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
