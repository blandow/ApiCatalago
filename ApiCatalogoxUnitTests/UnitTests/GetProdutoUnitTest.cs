using ApiCatalago.Controllers;
using ApiCatalago.DTO;
using ApiCatalago.Pagination;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Org.BouncyCastle.Asn1.Crmf;


namespace ApiCatalogoxUnitTests.UnitTests
{
    public class GetProdutoUnitTest : IClassFixture<ProdutosUnitTestsController>
    {
        public ProdutosController _controller;

        public GetProdutoUnitTest(ProdutosUnitTestsController controller)
        {
            _controller = new ProdutosController(NullLogger<ProdutosController>.Instance, controller.repository, controller.mapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

        }

        [Fact]
        public async Task GetProdutoByID_OkResult()
        {
            //Arrange
            var prodId = 2;

            //Act
            var data = await _controller.Get(prodId);

            //Assert
            /*Assert com xunit
             * var ok = Assert.IsType<OkObjectResult>(data.Result);
             * Assert.Equal(200, ok.StatusCode);
             */
            //Assert com fluentassertions
            data.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAllProdutos_OK()
        {
            //Arrange

            //Act
            var data = await _controller.Get();

            //Assert
            data.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>().And.NotBeNull();
        }

        [Fact]
        public async Task GetProdutosPreco_Ok()
        {
            //Arrange
            ProdutoFiltroPreco maior = new ProdutoFiltroPreco
            {
                Preco = 5,
                PrecoCriterio = "maior"
            };
            ProdutoFiltroPreco menor = new ProdutoFiltroPreco
            {
                Preco = 50,
                PrecoCriterio = "menor"
            };
            ProdutoFiltroPreco igual = new ProdutoFiltroPreco
            {
                Preco = 200,
                PrecoCriterio = "igual"
            };

            //Act
            var dataMaior = await _controller.GetProdutosPreco(maior);

            var dataMenor = await _controller.GetProdutosPreco(menor);

            var dataIgual = await _controller.GetProdutosPreco(igual);

            //Assert

            dataMaior.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>().And.NotBeNull();
            dataMenor.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>().And.NotBeNull();
            dataIgual.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>().And.NotBeNull();
        
        }
    }
}
