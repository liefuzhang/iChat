﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iChat.Api.Data;

namespace iChat.Api.Migrations
{
    [DbContext(typeof(iChatContext))]
    [Migration("20190627090636_AddedResetPasswordRequest")]
    partial class AddedResetPasswordRequest
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("iChat.Api.Models.Conversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("WorkspaceId");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("iChat.Api.Models.ConversationUser", b =>
                {
                    b.Property<int>("ConversationId");

                    b.Property<int>("UserId");

                    b.HasKey("ConversationId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ConversationUsers");
                });

            modelBuilder.Entity("iChat.Api.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentType");

                    b.Property<string>("Name");

                    b.Property<string>("SavedName");

                    b.Property<int>("UploadedByUserId");

                    b.Property<DateTime>("UploadedDate");

                    b.Property<int>("WorkspaceId");

                    b.HasKey("Id");

                    b.HasIndex("UploadedByUserId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("iChat.Api.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<bool>("ContentEdited");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<bool>("HasFileAttachments");

                    b.Property<int>("SenderId");

                    b.Property<int>("WorkspaceId");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Messages");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Message");
                });

            modelBuilder.Entity("iChat.Api.Models.MessageFileAttachment", b =>
                {
                    b.Property<int>("FileId");

                    b.Property<int>("MessageId");

                    b.HasKey("FileId", "MessageId");

                    b.HasIndex("MessageId");

                    b.ToTable("MessageFileAttachments");
                });

            modelBuilder.Entity("iChat.Api.Models.MessageReaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmojiColons");

                    b.Property<int>("MessageId");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("MessageReactions");
                });

            modelBuilder.Entity("iChat.Api.Models.MessageReactionUser", b =>
                {
                    b.Property<int>("MessageReactionId");

                    b.Property<int>("UserId");

                    b.HasKey("MessageReactionId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("MessageReactionUsers");
                });

            modelBuilder.Entity("iChat.Api.Models.ResetPasswordRequset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Cancelled");

                    b.Property<string>("Email");

                    b.Property<Guid>("ResetCode");

                    b.Property<bool>("Resetted");

                    b.HasKey("Id");

                    b.ToTable("ResetPasswordRequsets");
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

                    b.Property<string>("PhoneNumber");

                    b.Property<int>("Status");

                    b.Property<int>("WorkspaceId");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("iChat.Api.Models.UserInvitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Acceptted");

                    b.Property<bool>("Cancelled");

                    b.Property<Guid>("InvitationCode");

                    b.Property<DateTime>("InviteDate");

                    b.Property<int>("InvitedByUserId");

                    b.Property<string>("UserEmail");

                    b.Property<int>("WorkspaceId");

                    b.HasKey("Id");

                    b.HasIndex("InvitedByUserId");

                    b.ToTable("UserInvitations");
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

            modelBuilder.Entity("iChat.Api.Models.ConversationMessage", b =>
                {
                    b.HasBaseType("iChat.Api.Models.Message");

                    b.Property<int>("ConversationId");

                    b.HasIndex("ConversationId");

                    b.HasDiscriminator().HasValue("ConversationMessage");
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

            modelBuilder.Entity("iChat.Api.Models.Conversation", b =>
                {
                    b.HasOne("iChat.Api.Models.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.ConversationUser", b =>
                {
                    b.HasOne("iChat.Api.Models.Conversation", "Conversation")
                        .WithMany("ConversationUsers")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iChat.Api.Models.User", "User")
                        .WithMany("ConversationUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.File", b =>
                {
                    b.HasOne("iChat.Api.Models.User", "UploadedByUser")
                        .WithMany()
                        .HasForeignKey("UploadedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("iChat.Api.Models.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("iChat.Api.Models.Message", b =>
                {
                    b.HasOne("iChat.Api.Models.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("iChat.Api.Models.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("iChat.Api.Models.MessageFileAttachment", b =>
                {
                    b.HasOne("iChat.Api.Models.File", "File")
                        .WithMany("MessageFileAttachments")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iChat.Api.Models.Message", "Message")
                        .WithMany("MessageFileAttachments")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.MessageReaction", b =>
                {
                    b.HasOne("iChat.Api.Models.Message", "Message")
                        .WithMany("MessageReactions")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.MessageReactionUser", b =>
                {
                    b.HasOne("iChat.Api.Models.MessageReaction", "MessageReaction")
                        .WithMany("MessageReactionUsers")
                        .HasForeignKey("MessageReactionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iChat.Api.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.User", b =>
                {
                    b.HasOne("iChat.Api.Models.Workspace", "Workspace")
                        .WithMany("Users")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("iChat.Api.Models.UserInvitation", b =>
                {
                    b.HasOne("iChat.Api.Models.User", "InvitedByUser")
                        .WithMany()
                        .HasForeignKey("InvitedByUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.ChannelMessage", b =>
                {
                    b.HasOne("iChat.Api.Models.Channel", "Channel")
                        .WithMany("ChannelMessages")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iChat.Api.Models.ConversationMessage", b =>
                {
                    b.HasOne("iChat.Api.Models.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
