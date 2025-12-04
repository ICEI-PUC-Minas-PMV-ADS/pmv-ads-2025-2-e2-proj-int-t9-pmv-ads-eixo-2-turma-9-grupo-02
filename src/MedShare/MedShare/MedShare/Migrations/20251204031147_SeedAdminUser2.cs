using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "UsuarioId",
                keyValue: 1,
                column: "UsuarioSenha",
                value: "$2a$11$u1QwQJQwQJQwQJQwQJQwOuQJQwQJQwQJQwQJQwQJQwQJQwQJQwQJQwQJQ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "UsuarioId",
                keyValue: 1,
                column: "UsuarioSenha",
                value: "$2a$11$loGdiwfB4r1N7NPPj2jOjejKZ4rTFrqYdvA3S9FaWcfhe7sy/KuyC");
        }
    }
}
