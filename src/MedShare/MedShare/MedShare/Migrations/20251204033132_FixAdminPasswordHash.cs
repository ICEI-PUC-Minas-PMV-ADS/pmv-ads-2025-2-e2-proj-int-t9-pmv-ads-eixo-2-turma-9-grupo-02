using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class FixAdminPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "UsuarioId",
                keyValue: 1,
                column: "UsuarioSenha",
                value: "$2a$11$KIXQJQJQwQJQwQJQwQJQwOQJQwQJQwQJQwQJQwQJQwQJQwQJQwQJQwQJQ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "UsuarioId",
                keyValue: 1,
                column: "UsuarioSenha",
                value: "$2a$11$u1QwQJQwQJQwQJQwQJQwOuQJQwQJQwQJQwQJQwQJQwQJQwQJQwQJQwQJQ");
        }
    }
}
