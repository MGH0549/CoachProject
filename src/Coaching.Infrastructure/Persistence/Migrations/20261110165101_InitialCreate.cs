using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coaching.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ActorId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    RecordId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    At = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReminderForEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Recipient = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Html = table.Column<string>(type: "nvarchar(max)", maxLength: 16000, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    NextAttemptAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MuscleGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    YouTubeVideoId = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Archived = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobState",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastRunDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CairoDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Chest = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Hips = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Energy = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CoachFeedback = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIn", x => x.Id);
                    table.CheckConstraint("CK_CheckIn_Range", "[Weight] > 0 AND [Energy] BETWEEN 1 AND 10");
                    table.ForeignKey(
                        name: "FK_CheckIn_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Package = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ConsentAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ActivatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AccessEndsAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeactivatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReminderSentForEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientProfile_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DietPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsTemplate = table.Column<bool>(type: "bit", nullable: false),
                    ChoicesLocked = table.Column<bool>(type: "bit", nullable: false),
                    Calories = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Protein = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Carbs = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Fat = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietPlan", x => x.Id);
                    table.CheckConstraint("CK_Diet_Owner", "([IsTemplate] = 1 AND [ClientId] IS NULL) OR ([IsTemplate] = 0 AND [ClientId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_DietPlan_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Family = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RevokedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsTemplate = table.Column<bool>(type: "bit", nullable: false),
                    ChoicesLocked = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlan", x => x.Id);
                    table.CheckConstraint("CK_Training_Owner", "([IsTemplate] = 1 AND [ClientId] IS NULL) OR ([IsTemplate] = 0 AND [ClientId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_TrainingPlan_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgressPhoto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckInId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileReference = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressPhoto_CheckIn_CheckInId",
                        column: x => x.CheckInId,
                        principalTable: "CheckIn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DietPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meal_DietPlan_DietPlanId",
                        column: x => x.DietPlanId,
                        principalTable: "DietPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanDay",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanDay_TrainingPlan_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientMealSelection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMealSelection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientMealSelection_Meal_MealId",
                        column: x => x.MealId,
                        principalTable: "Meal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Calories = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Protein = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Carbs = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Fat = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOption", x => x.Id);
                    table.CheckConstraint("CK_Option_Macros", "[Calories] >= 0 AND [Protein] >= 0 AND [Carbs] >= 0 AND [Fat] >= 0");
                    table.ForeignKey(
                        name: "FK_MealOption_Meal_MealId",
                        column: x => x.MealId,
                        principalTable: "Meal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanDayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false),
                    Reps = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    RestSeconds = table.Column<int>(type: "int", nullable: false),
                    Tempo = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ChoiceLocked = table.Column<bool>(type: "bit", nullable: false),
                    SelectedAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanExercise", x => x.Id);
                    table.CheckConstraint("CK_PlanExercise_Sets", "[Sets] > 0 AND [RestSeconds] >= 0");
                    table.ForeignKey(
                        name: "FK_PlanExercise_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanExercise_PlanDay_PlanDayId",
                        column: x => x.PlanDayId,
                        principalTable: "PlanDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealOptionItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Food = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOptionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealOptionItem_MealOption_MealOptionId",
                        column: x => x.MealOptionId,
                        principalTable: "MealOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseAlternative",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: true),
                    Reps = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseAlternative", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseAlternative_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseAlternative_PlanExercise_PlanExerciseId",
                        column: x => x.PlanExerciseId,
                        principalTable: "PlanExercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MainExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PerformedExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CairoDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false),
                    Reps = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutLog_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutLog_Exercise_MainExerciseId",
                        column: x => x.MainExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutLog_Exercise_PerformedExerciseId",
                        column: x => x.PerformedExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutLog_PlanExercise_PlanExerciseId",
                        column: x => x.PlanExerciseId,
                        principalTable: "PlanExercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntry_At",
                table: "AuditEntry",
                column: "At");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntry_SubjectClientId",
                table: "AuditEntry",
                column: "SubjectClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIn_ClientId",
                table: "CheckIn",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIn_ClientId_CairoDate",
                table: "CheckIn",
                columns: new[] { "ClientId", "CairoDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientMealSelection_ClientId",
                table: "ClientMealSelection",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMealSelection_MealId",
                table: "ClientMealSelection",
                column: "MealId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfile_ClientId",
                table: "ClientProfile",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfile_Status_AccessEndsAt",
                table: "ClientProfile",
                columns: new[] { "Status", "AccessEndsAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DietPlan_ClientId",
                table: "DietPlan",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmailMessage_NextAttemptAt",
                table: "EmailMessage",
                column: "NextAttemptAt");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_Archived_Name",
                table: "Exercise",
                columns: new[] { "Archived", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseAlternative_ClientId",
                table: "ExerciseAlternative",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseAlternative_ExerciseId",
                table: "ExerciseAlternative",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseAlternative_PlanExerciseId",
                table: "ExerciseAlternative",
                column: "PlanExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_JobState_Name",
                table: "JobState",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meal_ClientId",
                table: "Meal",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_DietPlanId_Order",
                table: "Meal",
                columns: new[] { "DietPlanId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_MealOption_ClientId",
                table: "MealOption",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOption_MealId",
                table: "MealOption",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOptionItem_ClientId",
                table: "MealOptionItem",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOptionItem_MealOptionId",
                table: "MealOptionItem",
                column: "MealOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDay_ClientId",
                table: "PlanDay",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDay_TrainingPlanId_Order",
                table: "PlanDay",
                columns: new[] { "TrainingPlanId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_PlanExercise_ClientId",
                table: "PlanExercise",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanExercise_ExerciseId",
                table: "PlanExercise",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanExercise_PlanDayId_Order",
                table: "PlanExercise",
                columns: new[] { "PlanDayId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressPhoto_CheckInId",
                table: "ProgressPhoto",
                column: "CheckInId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressPhoto_ClientId",
                table: "ProgressPhoto",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ExpiresAt",
                table: "RefreshToken",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Family",
                table: "RefreshToken",
                column: "Family");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Hash",
                table: "RefreshToken",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlan_ClientId",
                table: "TrainingPlan",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLog_ClientId",
                table: "WorkoutLog",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLog_ClientId_CairoDate",
                table: "WorkoutLog",
                columns: new[] { "ClientId", "CairoDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLog_MainExerciseId",
                table: "WorkoutLog",
                column: "MainExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLog_PerformedExerciseId",
                table: "WorkoutLog",
                column: "PerformedExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLog_PlanExerciseId",
                table: "WorkoutLog",
                column: "PlanExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditEntry");

            migrationBuilder.DropTable(
                name: "ClientMealSelection");

            migrationBuilder.DropTable(
                name: "ClientProfile");

            migrationBuilder.DropTable(
                name: "EmailMessage");

            migrationBuilder.DropTable(
                name: "ExerciseAlternative");

            migrationBuilder.DropTable(
                name: "JobState");

            migrationBuilder.DropTable(
                name: "MealOptionItem");

            migrationBuilder.DropTable(
                name: "ProgressPhoto");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "WorkoutLog");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MealOption");

            migrationBuilder.DropTable(
                name: "CheckIn");

            migrationBuilder.DropTable(
                name: "PlanExercise");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "PlanDay");

            migrationBuilder.DropTable(
                name: "DietPlan");

            migrationBuilder.DropTable(
                name: "TrainingPlan");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
