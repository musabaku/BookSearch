using Microsoft.EntityFrameworkCore.Migrations;

public partial class MakeBookIdAutoIncrement : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "BookId",
            table: "Books",
            nullable: false,
            defaultValueSql: "IDENTITY(1,1)",
            oldClrType: typeof(int),
            oldType: "int");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "BookId",
            table: "Books",
            type: "int",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int",
            oldDefaultValueSql: "IDENTITY(1,1)");
    }
}
