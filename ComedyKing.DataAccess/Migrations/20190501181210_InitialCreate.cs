using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComedyKing.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jokes",
                columns: table => new
                {
                    JokeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    Rate = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    CelebrityMentioned = table.Column<string>(nullable: true),
                    StoryBehind = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jokes", x => x.JokeID);
                });

            migrationBuilder.CreateTable(
                name: "Pepole",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    NickName = table.Column<string>(nullable: true),
                    Profession = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pepole", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CelebrityInCelebrityJokes",
                columns: table => new
                {
                    CelebrityID = table.Column<int>(nullable: false),
                    CelebrityJokeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebrityInCelebrityJokes", x => new { x.CelebrityID, x.CelebrityJokeID });
                    table.ForeignKey(
                        name: "FK_CelebrityInCelebrityJokes_Pepole_CelebrityID",
                        column: x => x.CelebrityID,
                        principalTable: "Pepole",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CelebrityInCelebrityJokes_Jokes_CelebrityJokeID",
                        column: x => x.CelebrityJokeID,
                        principalTable: "Jokes",
                        principalColumn: "JokeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityInCelebrityJokes_CelebrityJokeID",
                table: "CelebrityInCelebrityJokes",
                column: "CelebrityJokeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CelebrityInCelebrityJokes");

            migrationBuilder.DropTable(
                name: "Pepole");

            migrationBuilder.DropTable(
                name: "Jokes");
        }
    }
}
