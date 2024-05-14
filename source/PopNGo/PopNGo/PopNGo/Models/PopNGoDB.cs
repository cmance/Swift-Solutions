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

    public virtual DbSet<EventTag> EventTags { get; set; }

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

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ServerConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountR__3214EC275F3E39FF");
        });

        modelBuilder.Entity<BookmarkList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookmark__3214EC277F32DADA");
        });

        modelBuilder.Entity<EmailHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmailHis__3214EC27BABFB2D6");

            entity.HasOne(d => d.User).WithMany(p => p.EmailHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailHistory_UserID");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC27E647B2A7");
        });

        modelBuilder.Entity<EventHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventHis__3214EC27ED51FD31");

            entity.HasOne(d => d.Event).WithMany(p => p.EventHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventHistory_EventID");

            entity.HasOne(d => d.User).WithMany(p => p.EventHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventHistory_UserID");
        });

        modelBuilder.Entity<EventTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventTag__3214EC27B6376181");

            entity.HasOne(d => d.Event).WithMany(p => p.EventTags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventTag_EventId");

            entity.HasOne(d => d.Tag).WithMany(p => p.EventTags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventTag_TagId");
        });

        modelBuilder.Entity<FavoriteEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Favorite__3214EC273A7F2D52");

            entity.HasOne(d => d.BookmarkList).WithMany(p => p.FavoriteEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteEvents_BookmarkListID");

            entity.HasOne(d => d.Event).WithMany(p => p.FavoriteEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteEvents_EventID");
        });

        modelBuilder.Entity<Itinerary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Itinerar__3214EC27349ED671");

            entity.HasOne(d => d.User).WithMany(p => p.Itineraries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Itinerary_UserID");
        });

        modelBuilder.Entity<ItineraryEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Itinerar__3214EC271647E977");

            entity.HasOne(d => d.Event).WithMany(p => p.ItineraryEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItineraryEvents_EventID");

            entity.HasOne(d => d.Itinerary).WithMany(p => p.ItineraryEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItineraryEvents_ItineraryID");
        });

        modelBuilder.Entity<PgUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PG_User__3214EC2718828522");
        });

        modelBuilder.Entity<ScheduledNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC272F626B75");

            entity.HasOne(d => d.User).WithMany(p => p.ScheduledNotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduledNotification_UserID");
        });

        modelBuilder.Entity<SearchRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SearchRe__3214EC272D77BD2D");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC27DAC21423");
        });

        modelBuilder.Entity<TicketLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketLi__3214EC27C3974B19");

            entity.HasOne(d => d.Event).WithMany(p => p.TicketLinks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketLink_EventID");
        });

        modelBuilder.Entity<Weather>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Weather__3214EC27A7336938");
        });

        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WeatherF__3214EC27A643637D");

            entity.HasOne(d => d.Weather).WithMany(p => p.WeatherForecasts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeatherForecast_WeatherId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
