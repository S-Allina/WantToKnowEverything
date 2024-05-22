//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace Kyrsach.Migrations
//{
//    public partial class addGroup : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "AspNetRoles",
//                columns: table => new
//                {
//                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Category",
//                columns: table => new
//                {
//                    IdCategory = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    NameCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    WhoAnsweredCategory = table.Column<int>(type: "int", nullable: true),
//                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Сategory", x => x.IdCategory);
//                });

//            migrationBuilder.CreateTable(
//                name: "Group",
//                columns: table => new
//                {
//                    IdGroup = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    NameGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Group", x => x.IdGroup);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetRoleClaims",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
//                        column: x => x.RoleId,
//                        principalTable: "AspNetRoles",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Pair",
//                columns: table => new
//                {
//                    IdPair = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    IdCategory = table.Column<int>(type: "int", nullable: false),
//                    Card1Text = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
//                    Card2Text = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
//                    Card1Img = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    Card2Img = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Pair", x => x.IdPair);
//                    table.ForeignKey(
//                        name: "FK_Pair_Category",
//                        column: x => x.IdCategory,
//                        principalTable: "Category",
//                        principalColumn: "IdCategory");
//                });

//            migrationBuilder.CreateTable(
//                name: "Test",
//                columns: table => new
//                {
//                    IdTest = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    IdCategory = table.Column<int>(type: "int", nullable: false),
//                    NameTest = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    WhoAnsweredTest = table.Column<int>(type: "int", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Test", x => x.IdTest);
//                    table.ForeignKey(
//                        name: "FK_Test_Сategory",
//                        column: x => x.IdCategory,
//                        principalTable: "Category",
//                        principalColumn: "IdCategory",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUsers",
//                columns: table => new
//                {
//                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    IdGroup = table.Column<int>(type: "int", nullable: true),
//                    IdGroupNavigationIdGroup = table.Column<int>(type: "int", nullable: true),
//                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
//                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
//                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
//                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
//                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
//                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AspNetUsers_Group_IdGroupNavigationIdGroup",
//                        column: x => x.IdGroupNavigationIdGroup,
//                        principalTable: "Group",
//                        principalColumn: "IdGroup");
//                });

//            migrationBuilder.CreateTable(
//                name: "Question",
//                columns: table => new
//                {
//                    IdQuestion = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    IdTest = table.Column<int>(type: "int", nullable: false),
//                    TextQuetion = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Option1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
//                    Option2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
//                    Option3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
//                    Option4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
//                    Option5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
//                    CorrectAnswer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    WhoAnsweredQuestion = table.Column<int>(type: "int", nullable: true),
//                    PictureTest = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Question", x => x.IdQuestion);
//                    table.ForeignKey(
//                        name: "FK_Question_Test",
//                        column: x => x.IdTest,
//                        principalTable: "Test",
//                        principalColumn: "IdTest",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Quez",
//                columns: table => new
//                {
//                    IdQuez = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    IdTest = table.Column<int>(type: "int", nullable: false),
//                    Q1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    Q2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    Q3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    Q4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    Q5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    pic1 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    pic2 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    pic3 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    pic4 = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    pic5 = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Quez", x => x.IdQuez);
//                    table.ForeignKey(
//                        name: "FK_Quez_Test",
//                        column: x => x.IdTest,
//                        principalTable: "Test",
//                        principalColumn: "IdTest");
//                });

//            migrationBuilder.CreateTable(
//                name: "AnswersUser",
//                columns: table => new
//                {
//                    IdAnswersUser = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    IdUser = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
//                    CountCurrent = table.Column<int>(type: "int", nullable: false),
//                    Time = table.Column<DateTime>(type: "datetime", nullable: true),
//                    UserModelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AnswersUser", x => x.IdAnswersUser);
//                    table.ForeignKey(
//                        name: "FK_AnswersUser_AspNetUsers_UserModelId",
//                        column: x => x.UserModelId,
//                        principalTable: "AspNetUsers",
//                        principalColumn: "Id");
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserClaims",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
//                        column: x => x.UserId,
//                        principalTable: "AspNetUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserLogins",
//                columns: table => new
//                {
//                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
//                        column: x => x.UserId,
//                        principalTable: "AspNetUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserRoles",
//                columns: table => new
//                {
//                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
//                        column: x => x.RoleId,
//                        principalTable: "AspNetRoles",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
//                        column: x => x.UserId,
//                        principalTable: "AspNetUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserTokens",
//                columns: table => new
//                {
//                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
//                        column: x => x.UserId,
//                        principalTable: "AspNetUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Answer",
//                columns: table => new
//                {
//                    IdAnswer = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    IdAnswersUser = table.Column<int>(type: "int", nullable: false),
//                    IdQuestion = table.Column<int>(type: "int", nullable: false),
//                    Answer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    loyal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Answer", x => x.IdAnswer);
//                    table.ForeignKey(
//                        name: "FK_Answer_AnswersUser",
//                        column: x => x.IdAnswersUser,
//                        principalTable: "AnswersUser",
//                        principalColumn: "IdAnswersUser",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_Answer_Question",
//                        column: x => x.IdQuestion,
//                        principalTable: "Question",
//                        principalColumn: "IdQuestion",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_Answer_IdAnswersUser",
//                table: "Answer",
//                column: "IdAnswersUser");

//            migrationBuilder.CreateIndex(
//                name: "IX_Answer_IdQuestion",
//                table: "Answer",
//                column: "IdQuestion");

//            migrationBuilder.CreateIndex(
//                name: "IX_AnswersUser_UserModelId",
//                table: "AnswersUser",
//                column: "UserModelId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetRoleClaims_RoleId",
//                table: "AspNetRoleClaims",
//                column: "RoleId");

//            migrationBuilder.CreateIndex(
//                name: "RoleNameIndex",
//                table: "AspNetRoles",
//                column: "NormalizedName",
//                unique: true,
//                filter: "[NormalizedName] IS NOT NULL");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserClaims_UserId",
//                table: "AspNetUserClaims",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserLogins_UserId",
//                table: "AspNetUserLogins",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserRoles_RoleId",
//                table: "AspNetUserRoles",
//                column: "RoleId");

//            migrationBuilder.CreateIndex(
//                name: "EmailIndex",
//                table: "AspNetUsers",
//                column: "NormalizedEmail");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUsers_IdGroupNavigationIdGroup",
//                table: "AspNetUsers",
//                column: "IdGroupNavigationIdGroup");

//            migrationBuilder.CreateIndex(
//                name: "UserNameIndex",
//                table: "AspNetUsers",
//                column: "NormalizedUserName",
//                unique: true,
//                filter: "[NormalizedUserName] IS NOT NULL");

//            migrationBuilder.CreateIndex(
//                name: "IX_Pair_IdCategory",
//                table: "Pair",
//                column: "IdCategory");

//            migrationBuilder.CreateIndex(
//                name: "IX_Question_IdTest",
//                table: "Question",
//                column: "IdTest");

//            migrationBuilder.CreateIndex(
//                name: "IX_Quez_IdTest",
//                table: "Quez",
//                column: "IdTest");

//            migrationBuilder.CreateIndex(
//                name: "IX_Test_IdCategory",
//                table: "Test",
//                column: "IdCategory");
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "Answer");

//            migrationBuilder.DropTable(
//                name: "AspNetRoleClaims");

//            migrationBuilder.DropTable(
//                name: "AspNetUserClaims");

//            migrationBuilder.DropTable(
//                name: "AspNetUserLogins");

//            migrationBuilder.DropTable(
//                name: "AspNetUserRoles");

//            migrationBuilder.DropTable(
//                name: "AspNetUserTokens");

//            migrationBuilder.DropTable(
//                name: "Pair");

//            migrationBuilder.DropTable(
//                name: "Quez");

//            migrationBuilder.DropTable(
//                name: "AnswersUser");

//            migrationBuilder.DropTable(
//                name: "Question");

//            migrationBuilder.DropTable(
//                name: "AspNetRoles");

//            migrationBuilder.DropTable(
//                name: "AspNetUsers");

//            migrationBuilder.DropTable(
//                name: "Test");

//            migrationBuilder.DropTable(
//                name: "Group");

//            migrationBuilder.DropTable(
//                name: "Category");
//        }
//    }
//}
