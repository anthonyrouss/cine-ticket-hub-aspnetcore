using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTicketHub.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "reservations_ibfk_1",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "screenings_ibfk_1",
                table: "screenings");

            migrationBuilder.DropForeignKey(
                name: "screenings_ibfk_2",
                table: "screenings");

            migrationBuilder.AddForeignKey(
                name: "reservations_ibfk_1",
                table: "reservations",
                column: "screening_id",
                principalTable: "screenings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "screenings_ibfk_1",
                table: "screenings",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "screenings_ibfk_2",
                table: "screenings",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "reservations_ibfk_1",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "screenings_ibfk_1",
                table: "screenings");

            migrationBuilder.DropForeignKey(
                name: "screenings_ibfk_2",
                table: "screenings");

            migrationBuilder.AddForeignKey(
                name: "reservations_ibfk_1",
                table: "reservations",
                column: "screening_id",
                principalTable: "screenings",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "screenings_ibfk_1",
                table: "screenings",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "screenings_ibfk_2",
                table: "screenings",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id");
        }
    }
}
