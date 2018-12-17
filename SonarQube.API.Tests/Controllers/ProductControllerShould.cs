using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SonarQube.API.Controllers;
using SonarQube.API.Models;
using Xunit;

namespace SonarQube.API.Tests.Controllers
{
    public class ProductControllerShould
    {
        private readonly ProductController _productController = new ProductController();

        [Theory]
        [InlineData("Notebook")]
        public void GetAll_Ok(string productName)
        {
            var createdProduct = (CreatedResult) _productController.Register(new Product {Name = productName});
            Assert.IsType<CreatedResult>(createdProduct);
            var productCollection = (OkObjectResult) _productController.GetAll();
            Assert.True(((List<Product>) productCollection.Value).Count > 0);
        }

        [Theory]
        [InlineData("some-invalid-token-id")]
        public void GetById_With_NotFound(string invalidTokenId)
        {
            var product = (NotFoundResult) _productController.GetById(invalidTokenId);
            Assert.IsType<NotFoundResult>(product);
        }

        [Theory]
        [InlineData(null)]
        public void GetById_With_BadRequest(string invalidTokenId)
        {
            var product = (BadRequestResult) _productController.GetById(invalidTokenId);
            Assert.IsType<BadRequestResult>(product);
        }

        [Theory]
        [InlineData("Book")]
        public void GetById_With_Ok(string productName)
        {
            var createdProduct = (CreatedResult) _productController.Register(new Product {Name = productName});
            Assert.IsType<CreatedResult>(createdProduct);
            var product = (OkObjectResult) _productController.GetById(((Product) createdProduct.Value).Id);
            Assert.Equal(((Product) createdProduct.Value).Id, ((Product) product.Value).Id);
            Assert.IsType<OkObjectResult>(product);
        }

        [Theory]
        [InlineData(null, null)]
        public void Update_With_BadRequest(string invalidTokenId, string productName)
        {
            var notCreatedProduct =
                (BadRequestResult) _productController.Update(invalidTokenId, new Product {Name = productName});
            Assert.IsType<BadRequestResult>(notCreatedProduct);
        }

        [Theory]
        [InlineData("some-invalid-token-id", null)]
        public void Update_With_NotFound(string invalidTokenId, string productName)
        {
            var notFoundProduct =
                (NotFoundResult) _productController.Update(invalidTokenId, new Product {Name = productName});
            Assert.IsType<NotFoundResult>(notFoundProduct);
        }

        [Theory]
        [InlineData("Book", "New-Book")]
        public void Update_With_Accepted(string oldProductName, string newProductName)
        {
            var createdProduct = (CreatedResult) _productController.Register(new Product {Name = oldProductName});
            Assert.IsType<CreatedResult>(createdProduct);
            var updatedProduct = (AcceptedResult) _productController.Update(((Product) createdProduct.Value).Id,
                new Product {Name = newProductName});
            Assert.Equal(((Product) createdProduct.Value).Id, ((Product) updatedProduct.Value).Id);
            Assert.NotEqual(((Product) createdProduct.Value).Name, ((Product) updatedProduct.Value).Name);
            Assert.Equal(((Product) updatedProduct.Value).Name, newProductName);
            Assert.IsType<AcceptedResult>(updatedProduct);
        }

        [Theory]
        [InlineData(null)]
        public void Delete_With_BadRequest(string invalidTokenId)
        {
            var createdProduct = (BadRequestResult) _productController.Delete(invalidTokenId);
            Assert.IsType<BadRequestResult>(createdProduct);
        }

        [Theory]
        [InlineData("some-invalid-token-id")]
        public void Delete_With_NotFound(string invalidTokenId)
        {
            var notFoundProduct = (NotFoundResult) _productController.Delete(invalidTokenId);
            Assert.IsType<NotFoundResult>(notFoundProduct);
        }

        [Theory]
        [InlineData("Cellphone")]
        public void Delete_With_Ok(string productName)
        {
            var createdProduct = (CreatedResult) _productController.Register(new Product {Name = productName});
            Assert.IsType<CreatedResult>(createdProduct);
            var deleted = (OkResult) _productController.Delete(((Product) createdProduct.Value).Id);
            Assert.IsType<OkResult>(deleted);
        }
    }
}