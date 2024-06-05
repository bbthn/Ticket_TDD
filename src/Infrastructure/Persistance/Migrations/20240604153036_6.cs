using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ticketing");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "Tickets",
                newSchema: "ticketing");

            migrationBuilder.RenameTable(
                name: "Flights",
                newName: "Flights",
                newSchema: "ticketing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Tickets",
                schema: "ticketing",
                newName: "Tickets");

            migrationBuilder.RenameTable(
                name: "Flights",
                schema: "ticketing",
                newName: "Flights");
        }
    }
}
