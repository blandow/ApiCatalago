using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Sonic Silver','Sonic Silver Prata com gliter',196.75,'Silver.jpg',20,now(),1)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Ron','Pudim de leite condensado 100g',6.75,'pudim.jpg',10,now(),1)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Kuromi','Pudim de leite condensado 100g',6.75,'pudim.jpg',5,now(),1)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Naruto','Pudim de leite condensado 100g',6.75,'pudim.jpg',58,now(),1)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Bala Lua cheia','balas sortidadas em um pacote',5.75,'BalaLua.jpg',20,now(),2)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Ron','Pudim de leite condensado 100g',6.75,'pudim.jpg',10,now(),2)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Cone','Cone de padaria mor bom',8.53,'cone.jpg',5,now(),2)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Dognout','dognout formato rosquinha escrito certo',7.21,'dognout.jpg',18,now(),2)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Philco','Monitor classico',196.75,'philco.jpg',20,now(),3)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Samsung','Monitor Pica',897.75,'samsung.jpg',10,now(),3)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('LG','monitor duvidoso',444.65,'lg.jpg',5,now(),3)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Asus','isso é mesmo um monitor?',960.90,'Asus.jpg',58,now(),3)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Sonic','Sonic classico',196.75,'sonic.jpg',20,now(),4)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('HogwardsLegacy','jogo HP maneirudo de mundo aberto',560.75,'hl.jpg',10,now(),4)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('MonsterHunterStories','rpg de turno de mh top',196.90,'mhs.jpg',5,now(),4)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('NarutoShippuden','jogo de lutinha do Naruto',200.00,'NarutoShippuden.jpg',28,now(),4)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Sonic Silver','Sonic Silver Prata com gliter',196.75,'Silver.jpg',20,now(),5)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Ron','Pudim de leite condensado 100g',6.75,'pudim.jpg',10,now(),5)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Kuromi','Pudim de leite condensado 100g',6.75,'pudim.jpg',5,now(),5)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Naruto','Pudim de leite condensado 100g',6.75,'pudim.jpg',58,now(),5)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Asus','Pc Gamer sinistro',9196.98,'Asus.jpg',20,now(),6)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('HP','Pc na média',1006.55,'hp.jpg',110,now(),6)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Samsung book','PC maneiro pra estudar e dia a dia',2056.75,'Samsung.jpg',5,now(),6)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Positivo','PC de biblioteca infantil bem ruim',0.75,'Positivo.jpg',158,now(),6)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Mouse Rog','mouse gamer',196.75,'mouserog.jpg',20,now(),7)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Teclado mecanico','teclado barulhento',96.75,'tecladoMec.jpg',10,now(),7)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Mouse pad rgb','mause pad que brilha',5066.75,'mp.jpg',5,now(),7)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Fone capenga','fone velho que cumpre o papel',0.95,'fone.jpg',58,now(),7)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Corsa','Corsinha classico',9196.75,'corsa.jpg',20,now(),8)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('hb20','o bom e pratico cheio de bo',85000.00,'hb20.jpg',10,now(),8)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Celta','Fundido mas rodando',5456.75,'celta.jpg',5,now(),8)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Ferrari','O Toreto sonha em ter pra empinar',958456.75,'ferrari.jpg',58,now(),8)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Sonic Silver','ouriço prata',196.75,'ourico.jpg',20,now(),9)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('Gato','te escrivisa e vc nem nota',546.75,'gato.jpg',10,now(),9)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('cachorro','mor parça',789.75,'cachorro.jpg',5,now(),9)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('tatu','vira bolinha',65654.75,'tatu.jpg',58,now(),9)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('the last of us','Joel ta sinistro aqui',196.75,'tlou.jpg',20,now(),10)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('GodofWar','Deus da guerra regaçando geral',566.75,'gow.jpg',10,now(),10)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('DaysGone','Moto e zombie',466.75,'dg.jpg',5,now(),10)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
            "Values('HorizonZeroDown','Aloy catadora de ferro velho',956.85,'hzd.jpg',58,now(),10)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Produtos");
        }
    }
}
