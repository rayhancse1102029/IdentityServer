using Microsoft.EntityFrameworkCore.Migrations;

namespace CARAPI.Migrations
{
    public partial class v_Base_Entity_modified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updatedBy",
                table: "Cars",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Cars",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "isDelete",
                table: "Cars",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "Cars",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Cars",
                newName: "CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Cars",
                newName: "updatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Cars",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Cars",
                newName: "isDelete");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Cars",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Cars",
                newName: "createdAt");
        }
    }
}
