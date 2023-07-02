using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Word_Grove_API.Models.DB
{
    public partial class WordGroveDBContext : DbContext
    {
        public WordGroveDBContext()
        {
        }

        public WordGroveDBContext(DbContextOptions<WordGroveDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Likes> Likes { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comments>(entity =>
            {
                entity.ToTable("comments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment");

                entity.Property(e => e.Createdon)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdon")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Editdate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("editdate");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comments_postid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comments_userid_fkey");
            });

            modelBuilder.Entity<Likes>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Postid })
                    .HasName("likes_pkey");

                entity.ToTable("likes");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Hax)
                    .HasColumnType("bit(1)")
                    .HasColumnName("hax");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("likes_postid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("likes_userid_fkey");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.ToTable("posts");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdon)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdon")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Editdate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("editdate");

                entity.Property(e => e.Imageurl)
                    .HasMaxLength(2048)
                    .HasColumnName("imageurl");

                entity.Property(e => e.Post)
                    .IsRequired()
                    .HasColumnName("post");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("posts_userid_fkey");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Username, "users_username_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Access)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("access");

                entity.Property(e => e.Createdon)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdon")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("hash");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
