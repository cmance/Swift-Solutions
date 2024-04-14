﻿using System;
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

    public virtual DbSet<BookmarkList> BookmarkLists { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventHistory> EventHistories { get; set; }

    public virtual DbSet<FavoriteEvent> FavoriteEvents { get; set; }

    public virtual DbSet<PgUser> PgUsers { get; set; }

    public virtual DbSet<ScheduledNotification> ScheduledNotifications { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TicketLink> TicketLinks { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlServer("Name=ServerConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookmarkList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookmark__3214EC279FC7E8BA");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC27ED3E79DC");
        });

        modelBuilder.Entity<EventHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventHis__3214EC27FF66CD7B");

            entity.HasOne(d => d.Event).WithMany(p => p.EventHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventHistory_EventID");

            entity.HasOne(d => d.User).WithMany(p => p.EventHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventHistory_UserID");
        });

        modelBuilder.Entity<FavoriteEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Favorite__3214EC27747DCC93");

            entity.HasOne(d => d.BookmarkList).WithMany(p => p.FavoriteEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteEvents_BookmarkListID");

            entity.HasOne(d => d.Event).WithMany(p => p.FavoriteEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteEvents_EventID");
        });

        modelBuilder.Entity<PgUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PG_User__3214EC279DED8B11");
        });

        modelBuilder.Entity<ScheduledNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC275E60BD46");

            entity.HasOne(d => d.User).WithMany(p => p.ScheduledNotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduledNotification_UserID");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC27AD0223CA");
        });

        modelBuilder.Entity<TicketLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketLi__3214EC27A59A429E");

            entity.HasOne(d => d.Event).WithMany(p => p.TicketLinks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketLink_EventID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
