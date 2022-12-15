using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Migrations;

public partial class InitialCreate : Migration
{
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: "Users",
      columns: table => new
      {
        Id = table.Column<int>(type: "integer", nullable: false)
          .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        Name = table.Column<string>(type: "text", nullable: false),
        IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
      },
      constraints: table =>
      {
        table.PrimaryKey("PK_Users", x => x.Id);
      });
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(
      name: "Users");
  }
}
