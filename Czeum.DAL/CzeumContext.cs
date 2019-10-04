﻿using System;
using System.Reflection;
using Czeum.Domain.Entities;
using Czeum.Domain.Entities.Boards;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Czeum.DAL
{
    public sealed class CzeumContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<SerializedBoard> Boards { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FriendRequest> Requests { get; set; }
        public DbSet<StoredMessage> Messages { get; set; }

        public CzeumContext(DbContextOptions<CzeumContext> options)
            : base(options)
        {
            Matches = Set<Match>();
            Boards = Set<SerializedBoard>();
            Friendships = Set<Friendship>();
            Requests = Set<FriendRequest>();
            Messages = Set<StoredMessage>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            base.OnModelCreating(builder);
        }
    }
}