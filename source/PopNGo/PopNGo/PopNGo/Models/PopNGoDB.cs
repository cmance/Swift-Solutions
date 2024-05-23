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

    public virtual DbSet<ItineraryNotification> ItineraryNotifications { get; set; }

    public virtual DbSet<PgUser> PgUsers { get; set; }

    public virtual DbSet<RecommendedEvent> RecommendedEvents { get; set; }

    public virtual DbSet<ScheduledNotification> ScheduledNotifications { get; set; }

    public virtual DbSet<SearchRecord> SearchRecords { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TicketLink> TicketLinks { get; set; }

    public virtual DbSet<Weather> Weathers { get; set; }

    public virtual DbSet<WeatherForecast> WeatherForecasts { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlServer("Name=ServerConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountR__3214EC273419E3E1");
        });

        modelBuilder.Entity<BookmarkList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookmark__3214EC271E1FAC4D");
        });

        modelBuilder.Entity<EmailHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmailHis__3214EC277564E4F9");

            entity.HasOne(d => d.User).WithMany(p => p.EmailHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailHistory_UserID");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC27C30782BA");
        });

        modelBuilder.Entity<EventHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventHis__3214EC276731FFCF");

            entity.HasOne(d => d.Event).WithMany(p => p.EventHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventHistory_EventID");

            entity.HasOne(d => d.User).WithMany(p => p.EventHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventHistory_UserID");
        });

        modelBuilder.Entity<EventTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventTag__3214EC277B27B793");

            entity.HasOne(d => d.Event).WithMany(p => p.EventTags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventTag_EventId");

            entity.HasOne(d => d.Tag).WithMany(p => p.EventTags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventTag_TagId");
        });

        modelBuilder.Entity<FavoriteEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Favorite__3214EC27F354E38A");

            entity.HasOne(d => d.BookmarkList).WithMany(p => p.FavoriteEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteEvents_BookmarkListID");

            entity.HasOne(d => d.Event).WithMany(p => p.FavoriteEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteEvents_EventID");
        });

        modelBuilder.Entity<Itinerary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Itinerar__3214EC273D0037EE");

            entity.HasOne(d => d.User).WithMany(p => p.Itineraries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Itinerary_UserID");
        });

        modelBuilder.Entity<ItineraryEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Itinerar__3214EC27D2EEE5E3");

            entity.HasOne(d => d.Event).WithMany(p => p.ItineraryEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItineraryEvents_EventID");

            entity.HasOne(d => d.Itinerary).WithMany(p => p.ItineraryEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItineraryEvents_ItineraryID");
        });

        modelBuilder.Entity<ItineraryNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Itinerar__3214EC27524D1B70");

            entity.HasOne(d => d.Itinerary).WithMany(p => p.ItineraryNotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItineraryNotifications_ItineraryID");
        });

        modelBuilder.Entity<PgUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PG_User__3214EC2746894CDC");
        });

        modelBuilder.Entity<RecommendedEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recommen__3214EC276C010A9B");

            entity.HasOne(d => d.Event).WithMany(p => p.RecommendedEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecommendedEvent_EventID");

            entity.HasOne(d => d.User).WithMany(p => p.RecommendedEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecommendedEvent_UserID");
        });

        modelBuilder.Entity<ScheduledNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC27F3829F7A");

            entity.HasOne(d => d.User).WithMany(p => p.ScheduledNotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduledNotification_UserID");
        });

        modelBuilder.Entity<SearchRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SearchRe__3214EC2711985C4F");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC2721F28A04");
        });

        modelBuilder.Entity<TicketLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketLi__3214EC2735E71EB9");


            entity.HasOne(d => d.Event).WithMany(p => p.TicketLinks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketLink_EventID");
        });

        modelBuilder.Entity<Weather>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Weather__3214EC275DC81B72");

        });

        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WeatherF__3214EC279FCC87CC");

            entity.HasOne(d => d.Weather).WithMany(p => p.WeatherForecasts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeatherForecast_WeatherId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
