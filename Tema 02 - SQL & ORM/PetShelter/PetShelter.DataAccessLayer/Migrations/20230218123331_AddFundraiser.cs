using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShelter.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddFundraiser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FundraiserId",
                table: "Donations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Fundraiser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Target = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Total = table.Column<float>(type: "real", nullable: false, defaultValue: 0f)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fundraiser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donations_FundraiserId",
                table: "Donations",
                column: "FundraiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Fundraiser_FundraiserId",
                table: "Donations",
                column: "FundraiserId",
                principalTable: "Fundraiser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Fundraiser_FundraiserId",
                table: "Donations");

            migrationBuilder.DropTable(
                name: "Fundraiser");

            migrationBuilder.DropIndex(
                name: "IX_Donations_FundraiserId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "FundraiserId",
                table: "Donations");
        }
    }
}
