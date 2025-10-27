using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CrowdsourcingPlatform.Models;

namespace CrowdsourcingPlatform.Data;

public partial class CrowdsourcingPlatformContext : DbContext
{
    public CrowdsourcingPlatformContext()
    {
    }

    public CrowdsourcingPlatformContext(DbContextOptions<CrowdsourcingPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bid> Bids { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<ExecutionTask> ExecutionTasks { get; set; }

    public virtual DbSet<Executor> Executors { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Models.Task> Tasks { get; set; }

    public virtual DbSet<View_ExecutorSummary> View_ExecutorSummaries { get; set; }

    public virtual DbSet<View_TaskDetail> View_TaskDetails { get; set; }

    public virtual DbSet<View_TaskExecutionPayment> View_TaskExecutionPayments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(e => e.BidID).HasName("PK__Bids__4A733DB216762A61");

            entity.HasIndex(e => new { e.TaskID, e.ExecutorID }, "UQ__Bids__7507D7F92AC8C146").IsUnique();

            entity.Property(e => e.BidAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BidDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Executor).WithMany(p => p.Bids)
                .HasForeignKey(d => d.ExecutorID)
                .HasConstraintName("FK_Bids_Executors");

            entity.HasOne(d => d.Task).WithMany(p => p.Bids)
                .HasForeignKey(d => d.TaskID)
                .HasConstraintName("FK_Bids_Tasks");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryID).HasName("PK__Categori__19093A2BFDB2C654");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E012F9BFC5").IsUnique();

            entity.Property(e => e.CategoryDescription).HasMaxLength(500);
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerID).HasName("PK__Customer__A4AE64B8505FD209");

            entity.HasIndex(e => e.ContactEmail, "UQ__Customer__FFA796CDF829E23B").IsUnique();

            entity.Property(e => e.ContactEmail).HasMaxLength(255);
            entity.Property(e => e.ContactPhone).HasMaxLength(50);
            entity.Property(e => e.CustomerName).HasMaxLength(255);
        });

        modelBuilder.Entity<ExecutionTask>(entity =>
        {
            entity.HasKey(e => e.ExecutionTaskID).HasName("PK__Executio__CFC8BDBF308651EB");

            entity.Property(e => e.CompletionDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubmittedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Executor).WithMany(p => p.ExecutionTasks)
                .HasForeignKey(d => d.ExecutorID)
                .HasConstraintName("FK_ExecutionTasks_Executors");

            entity.HasOne(d => d.Task).WithMany(p => p.ExecutionTasks)
                .HasForeignKey(d => d.TaskID)
                .HasConstraintName("FK_ExecutionTasks_Tasks");
        });

        modelBuilder.Entity<Executor>(entity =>
        {
            entity.HasKey(e => e.ExecutorID).HasName("PK__Executor__96E9E2917D2000B9");

            entity.HasIndex(e => e.Nickname, "UQ__Executor__CC6CD17EC81E2B7E").IsUnique();

            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Nickname).HasMaxLength(100);
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.ReviewsCount).HasDefaultValue(0);
            entity.Property(e => e.SkillsDescription).HasMaxLength(1000);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentID).HasName("PK__Payments__9B556A58EC401287");

            entity.HasIndex(e => e.ExecutionTaskID, "UQ__Payments__CFC8BDBEB86F1E14").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.ExecutionTask).WithOne(p => p.Payment)
                .HasForeignKey<Payment>(d => d.ExecutionTaskID)
                .HasConstraintName("FK_Payments_ExecutionTasks");
        });

        modelBuilder.Entity<Models.Task>(entity =>
        {
            entity.HasKey(e => e.TaskID).HasName("PK__Tasks__7C6949D17C13B9EF");

            entity.Property(e => e.Budget).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Complexity).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Open");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CategoryID)
                .HasConstraintName("FK_Tasks_Categories");

            entity.HasOne(d => d.Customer).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CustomerID)
                .HasConstraintName("FK_Tasks_Customers");
        });

        modelBuilder.Entity<View_ExecutorSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ExecutorSummary");

            entity.Property(e => e.AvgPaymentAmount).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Nickname).HasMaxLength(100);
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.SkillsDescription).HasMaxLength(1000);
            entity.Property(e => e.TotalPaidAmount).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<View_TaskDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_TaskDetails");

            entity.Property(e => e.AcceptedBidDate).HasColumnType("datetime");
            entity.Property(e => e.AssignedBidAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AssignedExecutorNickname).HasMaxLength(100);
            entity.Property(e => e.AvgBidAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Budget).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Complexity).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.MaxBidAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MinBidAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TaskStatus).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<View_TaskExecutionPayment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_TaskExecutionPayments");

            entity.Property(e => e.CompletionDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.ExecutionStatus).HasMaxLength(50);
            entity.Property(e => e.ExecutorNickname).HasMaxLength(100);
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.SubmittedDate).HasColumnType("datetime");
            entity.Property(e => e.TaskStatus).HasMaxLength(50);
            entity.Property(e => e.TaskTitle).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
