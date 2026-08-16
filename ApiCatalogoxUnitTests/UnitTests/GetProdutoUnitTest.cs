using ApiCatalago.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;


namespace ApiCatalogoxUnitTests.UnitTests
{
    public class GetProdutoUnitTest : IClassFixture<ProdutosUnitTestsController>
    {
        public ProdutosController _controller;

        public GetProdutoUnitTest(ProdutosUnitTestsController controller)
        {
            _controller = new ProdutosController(NullLogger<ProdutosController>.Instance,controller.repository, controller.mapper); 
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
    }
}
