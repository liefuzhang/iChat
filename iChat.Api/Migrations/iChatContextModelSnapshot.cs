﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iChat.Data;

namespace iChat.Api.Migrations
{
    [DbContext(typeof(iChatContext))]
    partial class iChatContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("iChat.Api.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Topic");

                    b.Property<int>("WorkspaceId");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("iChat.Api.Models.ChannelSubscription", b =>
                {
                    b.Property<int>("ChannelId");

                    b.Property<int>("UserId");

                    b.HasKey("ChannelId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ChannelSubscriptions");
                });

            modelBuilder.Entity("iChat.Api.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("SenderId");

                    b.Property<int>("WorkspaceId");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Message");
                });

            modelBuilder.Entity("iChat.Api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email");

                    b.Property<Guid>("IdenticonGuid");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<int>("Status");

                    b.Property<int>("WorkspaceId");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("iChat.Api.Models.Workspace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Name");

                    b.Property<int>("OwnerId");

                    b.HasKey("Id");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("iChat.Api.Models.ChannelMessage", b =>
                {
                    b.HasBaseType("iChat.Api.Models.Message");

                    b.Property<int>("ChannelId");

                    b.HasIndex("ChannelId");

                    b.HasDiscriminator().HasValue("ChannelMessage");
                });

            modelBuilder.Entity("iChat.Api.Models.DirectMessage", b =>
                {
                    b.HasBaseType("iChat.Api.Models.Message");

                    b.Property<int>("ReceiverId");

                    b.HasIndex("ReceiverId");

                    b.HasDiscriminator().HasValue("DirectMessage");
                });

            modelBuilder.Entity("iChat.Api.Models.Channel", b =>
                {
                    b.HasOne("iChat.Api.Models.Workspace", "Workspace")
                        .WithMany("Channels")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.ChannelSubscription", b =>
                {
                    b.HasOne("iChat.Api.Models.Channel", "Channel")
                        .WithMany("ChannelSubscriptions")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iChat.Api.Models.User", "User")
                        .WithMany("ChannelSubscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.Message", b =>
                {
                    b.HasOne("iChat.Api.Models.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("iChat.Api.Models.User", b =>
                {
                    b.HasOne("iChat.Api.Models.Workspace", "Workspace")
                        .WithMany("Users")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("iChat.Api.Models.ChannelMessage", b =>
                {
                    b.HasOne("iChat.Api.Models.Channel", "Channel")
                        .WithMany("ChannelMessages")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.DirectMessage", b =>
                {
                    b.HasOne("iChat.Api.Models.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
