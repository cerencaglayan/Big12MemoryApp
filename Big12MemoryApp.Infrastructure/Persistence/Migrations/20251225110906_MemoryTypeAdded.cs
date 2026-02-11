using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace yeni.Data.Migrations
{
    /// <inheritdoc />
    public partial class MemoryTypeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemoryTypeId",
                table: "Memories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemoryType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memories_MemoryTypeId",
                table: "Memories",
                column: "MemoryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryType_MemoryTypeId",
                table: "Memories",
                column: "MemoryTypeId",
                principalTable: "MemoryType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryType_MemoryTypeId",
                table: "Memories");

            migrationBuilder.DropTable(
                name: "MemoryType");

            migrationBuilder.DropIndex(
                name: "IX_Memories_MemoryTypeId",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "MemoryTypeId",
                table: "Memories");
        }
    }
}
