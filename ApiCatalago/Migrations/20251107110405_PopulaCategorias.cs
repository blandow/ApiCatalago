using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('Funko','Funko.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('Doce','Doce.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('Monitor','Monitor.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('SwitchGames','SwitchGames.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('Notebook','Notebook.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('Perifericos','Perifericos.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('Carros','Carros.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('Motos','Motos.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('Animal','Animal.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias (Nome, ImagemUrl) Values ('PlayStationGames','PlayStationGames.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categorias");
        }
    }
}
