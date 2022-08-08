using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPhongKham.Migrations
{
    public partial class SeedDataAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Email", "FullName", "Password", "PhoneNumber", "Role", "Status", "UserName" },
                values: new object[] { "46eb7528-7347-4952-8b8e-41144276eae5", "Hà Nội", "admin@gmail.com", "Admin", "$2a$10$SXnrntO1/dUB/ysxj80vG.wMv6E6X8fNfPfHSNu8w2ZsBtiGINd0y", "0123456789", "admin", true, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "46eb7528-7347-4952-8b8e-41144276eae5");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Patients");
        }
    }
}
