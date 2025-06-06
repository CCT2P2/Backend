﻿// <auto-generated />
using Gnuf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace gnufv2.Migrations
{
    [DbContext(typeof(GnufContext))]
    [Migration("20250421142528_fuckingwork")]
    partial class fuckingwork
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("Gnuf.Models.CommunityStructure", b =>
                {
                    b.Property<int>("CommunityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("COMMUNITY_ID");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("DESCRIPTION");

                    b.Property<string>("Img_path")
                        .HasColumnType("TEXT")
                        .HasColumnName("IMG_PATH");

                    b.Property<int>("MemberCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MEMBER_COUNT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.Property<string>("PostID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("POST_ID");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("TAGS");

                    b.HasKey("CommunityID");

                    b.ToTable("COMMUNITIES");
                });

            modelBuilder.Entity("Gnuf.Models.FeedbackStructure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ID");

                    b.Property<string>("Didnt")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("DIDNT");

                    b.Property<string>("Feedback")
                        .HasColumnType("TEXT")
                        .HasColumnName("FEEDBACK");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER")
                        .HasColumnName("RATING");

                    b.Property<int>("Timestamp")
                        .HasColumnType("INTEGER")
                        .HasColumnName("TIMESTAMP");

                    b.Property<string>("Worked")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("WORKED");

                    b.HasKey("Id");

                    b.ToTable("FEEDBACK");
                });

            modelBuilder.Entity("Gnuf.Models.PostStructure", b =>
                {
                    b.Property<int>("PostID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("POST_ID");

                    b.Property<string>("Img")
                        .HasColumnType("TEXT")
                        .HasColumnName("IMG_PATH");

                    b.Property<string>("MainText")
                        .IsRequired()
                        .HasMaxLength(100000)
                        .HasColumnType("TEXT")
                        .HasColumnName("MAIN");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT")
                        .HasColumnName("TAGS");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT")
                        .HasColumnName("TITLE");

                    b.Property<int>("auth_id")
                        .HasColumnType("INTEGER")
                        .HasColumnName("AUTHOR_ID");

                    b.Property<string>("author_name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("AUTHOR_NAME");

                    b.Property<int>("com_id")
                        .HasColumnType("INTEGER")
                        .HasColumnName("COMMUNITY_ID");

                    b.Property<int>("comment_Count")
                        .HasColumnType("INTEGER")
                        .HasColumnName("COMMENT_CNT");

                    b.Property<bool>("comment_flag")
                        .HasColumnType("INTEGER")
                        .HasColumnName("COMMENT_FLAG");

                    b.Property<string>("comments")
                        .HasColumnType("TEXT")
                        .HasColumnName("COMMENTS");

                    b.Property<int>("dislikes")
                        .HasColumnType("INTEGER")
                        .HasColumnName("DISLIKES");

                    b.Property<int>("likes")
                        .HasColumnType("INTEGER")
                        .HasColumnName("LIKES");

                    b.Property<int?>("post_id_ref")
                        .HasColumnType("INTEGER")
                        .HasColumnName("POST_ID_REF");

                    b.Property<long>("timestamp")
                        .HasColumnType("INTEGER")
                        .HasColumnName("TIMESTAMP");

                    b.HasKey("PostID");

                    b.ToTable("POSTS");
                });

            modelBuilder.Entity("Gnuf.Models.UserStructure", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("USER_ID");

                    b.Property<string>("CommunityIds")
                        .HasColumnType("TEXT")
                        .HasColumnName("COMMUNITY_IDs");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasColumnName("DESCRIPTION");

                    b.Property<string>("DislikeId")
                        .HasColumnType("TEXT")
                        .HasColumnName("DISLIKE_I");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT")
                        .HasColumnName("DISPLAY_NAME");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("EMAIL");

                    b.Property<string>("ImagePath")
                        .HasColumnType("TEXT")
                        .HasColumnName("IMG_PATH");

                    b.Property<int>("IsAdmin")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ADMIN");

                    b.Property<string>("LikeId")
                        .HasColumnType("TEXT")
                        .HasColumnName("LIKE_ID");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT")
                        .HasColumnName("PASSWORD");

                    b.Property<string>("PostIds")
                        .HasColumnType("TEXT")
                        .HasColumnName("POST_IDs");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("SALT");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT")
                        .HasColumnName("TAGS");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("USERNAME");

                    b.HasKey("UserId");

                    b.ToTable("USER");
                });
#pragma warning restore 612, 618
        }
    }
}
