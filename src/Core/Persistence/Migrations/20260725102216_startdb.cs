using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class startdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachmentSubjects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodeDisplay = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentSubjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CapcCaptchaHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Code = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapcCaptchaHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HasNeedToCheckFields = table.Column<bool>(type: "bit", nullable: false),
                    HasNeedToDefaultFields = table.Column<bool>(type: "bit", nullable: false),
                    HasAccessToChild = table.Column<bool>(type: "bit", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeleteReasons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    HasDescription = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeleteReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryCheckers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryCheckers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JsonConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentConfigVersion = table.Column<short>(type: "smallint", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageCodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRtl = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListContactUs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListContactUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token_JWT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HttpReferrer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemoteIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PortIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateChangeWorker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JsonField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSpace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebugView = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogServers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Namespace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MethodName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemoteIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PortIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HttpReferrer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exceptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NeedToEditReasons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    HasDescription = table.Column<bool>(type: "bit", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeedToEditReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhoneOperators",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneOperators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlateCodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    TypeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ArOm = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EnUs = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlateCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlateStatusList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlateStatusList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Regions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportReasons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    HasDescription = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubSystems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ServerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CategoryTypeId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PinInHome = table.Column<int>(type: "int", nullable: true),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_CategoryTypes_CategoryTypeId",
                        column: x => x.CategoryTypeId,
                        principalTable: "CategoryTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FullPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowProfileInAnnouncement = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCodeId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_LanguageCodes_LanguageCodeId",
                        column: x => x.LanguageCodeId,
                        principalTable: "LanguageCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    FolderName = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false, defaultValue: "uploads"),
                    SubSystemId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RelationId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ServerId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AttachmentSubjectId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileThumbnailName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileOriginalName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AltText = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ImageTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_AttachmentSubjects_AttachmentSubjectId",
                        column: x => x.AttachmentSubjectId,
                        principalTable: "AttachmentSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attachments_SubSystems_SubSystemId",
                        column: x => x.SubSystemId,
                        principalTable: "SubSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageLocalizers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    LanguageCodeId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelationId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    SubSystemId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageLocalizers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageLocalizers_LanguageCodes_LanguageCodeId",
                        column: x => x.LanguageCodeId,
                        principalTable: "LanguageCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageLocalizers_SubSystems_SubSystemId",
                        column: x => x.SubSystemId,
                        principalTable: "SubSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    JsonConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigVersion = table.Column<short>(type: "smallint", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    FieldTypeId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fields_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fields_FieldTypes_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalTable: "FieldTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    IsShowProfileInAnnouncement = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    HasWarningDictionaryChecker = table.Column<bool>(type: "bit", nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    DictionaryCheckerId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CategoryId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    StatusId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    DeleteReasonId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    DeleteReasonDescription = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    LanguageCodeId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    BlurPlateLetters = table.Column<bool>(type: "bit", nullable: true),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Announcements_DeleteReasons_DeleteReasonId",
                        column: x => x.DeleteReasonId,
                        principalTable: "DeleteReasons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Announcements_DictionaryCheckers_DictionaryCheckerId",
                        column: x => x.DictionaryCheckerId,
                        principalTable: "DictionaryCheckers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Announcements_LanguageCodes_LanguageCodeId",
                        column: x => x.LanguageCodeId,
                        principalTable: "LanguageCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Announcements_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Announcements_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FieldMultiValue",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FieldId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldMultiValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldMultiValue_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementViews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AnnouncementId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementViews_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnnouncementViews_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AnnouncementId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FieldValueAnnouncements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AnnouncementId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    FieldId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldValueAnnouncements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldValueAnnouncements_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FieldValueAnnouncements_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NeedToEditLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    NeedToEditReasonId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AnnouncementId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeedToEditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NeedToEditLogs_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NeedToEditLogs_NeedToEditReasons_NeedToEditReasonId",
                        column: x => x.NeedToEditReasonId,
                        principalTable: "NeedToEditReasons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NeedToEditLogs_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AnnouncementId = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notes_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ReportReasonId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AnnouncementId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportLogs_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportLogs_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportLogs_ReportReasons_ReportReasonId",
                        column: x => x.ReportReasonId,
                        principalTable: "ReportReasons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CategoryId",
                table: "Announcements",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_DeleteReasonId",
                table: "Announcements",
                column: "DeleteReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_DictionaryCheckerId",
                table: "Announcements",
                column: "DictionaryCheckerId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_LanguageCodeId",
                table: "Announcements",
                column: "LanguageCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ProfileId",
                table: "Announcements",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_StatusId",
                table: "Announcements",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementViews_AnnouncementId",
                table: "AnnouncementViews",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementViews_CreateDateTime",
                table: "AnnouncementViews",
                column: "CreateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementViews_ProfileId",
                table: "AnnouncementViews",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_AttachmentSubjectId",
                table: "Attachments",
                column: "AttachmentSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_SubSystemId",
                table: "Attachments",
                column: "SubSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryTypeId",
                table: "Categories",
                column: "CategoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AnnouncementId",
                table: "Favorites",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ProfileId",
                table: "Favorites",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldMultiValue_FieldId",
                table: "FieldMultiValue",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_CategoryId",
                table: "Fields",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_FieldTypeId",
                table: "Fields",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValueAnnouncements_AnnouncementId",
                table: "FieldValueAnnouncements",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValueAnnouncements_FieldId",
                table: "FieldValueAnnouncements",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageLocalizers_LanguageCodeId",
                table: "LanguageLocalizers",
                column: "LanguageCodeId");

            migrationBuilder.CreateIndex(
                name: "UX_LanguageLocalizer",
                table: "LanguageLocalizers",
                columns: new[] { "SubSystemId", "RelationId", "PropertyName", "LanguageCodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NeedToEditLogs_AnnouncementId",
                table: "NeedToEditLogs",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_NeedToEditLogs_NeedToEditReasonId",
                table: "NeedToEditLogs",
                column: "NeedToEditReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_NeedToEditLogs_ProfileId",
                table: "NeedToEditLogs",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AnnouncementId",
                table: "Notes",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ProfileId",
                table: "Notes",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_LanguageCodeId",
                table: "Profiles",
                column: "LanguageCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ParentId",
                table: "Regions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportLogs_AnnouncementId",
                table: "ReportLogs",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportLogs_ProfileId",
                table: "ReportLogs",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportLogs_ReportReasonId",
                table: "ReportLogs",
                column: "ReportReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SubSystems_Name",
                table: "SubSystems",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementViews");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "CapcCaptchaHistories");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "FieldMultiValue");

            migrationBuilder.DropTable(
                name: "FieldValueAnnouncements");

            migrationBuilder.DropTable(
                name: "LanguageLocalizers");

            migrationBuilder.DropTable(
                name: "ListContactUs");

            migrationBuilder.DropTable(
                name: "LogDetails");

            migrationBuilder.DropTable(
                name: "LogServers");

            migrationBuilder.DropTable(
                name: "NeedToEditLogs");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PhoneOperators");

            migrationBuilder.DropTable(
                name: "PlateCodes");

            migrationBuilder.DropTable(
                name: "PlateStatusList");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "ReportLogs");

            migrationBuilder.DropTable(
                name: "AttachmentSubjects");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "SubSystems");

            migrationBuilder.DropTable(
                name: "NeedToEditReasons");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "ReportReasons");

            migrationBuilder.DropTable(
                name: "FieldTypes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DeleteReasons");

            migrationBuilder.DropTable(
                name: "DictionaryCheckers");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "CategoryTypes");

            migrationBuilder.DropTable(
                name: "LanguageCodes");
        }
    }
}
