using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WorkingWithVisualStudio.Controllers;
using WorkingWithVisualStudio.Models;
using Xunit;
using System;
using Moq;

namespace WorkingWithVisualStudio.Tests {
    public class HomeControllerTests {
        
        class ModelCompleteFakeRepository : IRepository
        {
            /*
            public IEnumerable<Product> Products { get;  } = new Product[]
            {
                new Product { Name = "P1", Price = 275M },
                new Product { Name = "P2", Price = 48.95M },
                new Product { Name = "P3", Price = 19.50M },
                new Product { Name = "P4", Price = 34.95M }
            };
            */
            // this is for the parameterized test
            public IEnumerable<Product> Products { get; set; }
            public void AddProduct(Product p)
            {
                //
            }
        }
        // parameterized test
        [Theory]
        [InlineData(275, 48.95, 19.50, 24.95)]
        [InlineData(5, 48.95, 19.50, 24.95)]
        public void IndexActionModelIsComplete(decimal price1, decimal price2,
            decimal price3, decimal price4)
        {
            // Arrange
            var controller = new HomeController();
            controller.Repository = new ModelCompleteFakeRepository
            {
                Products = new Product[]
                {
                    new Product {Name = "P1", Price = price1},
                    new Product {Name = "P2", Price = price2},
                    new Product {Name = "P3", Price = price3},
                    new Product {Name = "P4", Price = price4},
                }
            };
            // Act
            var model = (controller.Index() as ViewResult)?.ViewData.Model
                as IEnumerable<Product>;
            // Assert
            Assert.Equal(controller.Repository.Products, model, Comparer.Get<Product>((p1, p2) => 
                p1.Name == p2.Name && p1.Price == p2.Price));
        }
        /*
        [Fact]
        public void IndexActionModelIsComplete()
        {
            // Arrange
            var controller = new HomeController();
            controller.Repository = new ModelCompleteFakeRepository();

            // Act
            var model = (controller.Index() as ViewResult)?.ViewData.Model
                as IEnumerable<Product>;
            // Assert
            Assert.Equal(controller.Repository.Products, model, Comparer.Get<Product>((p1, p2) =>
                p1.Name == p2.Name && p1.Price == p2.Price));
        }*/
        class ModelCompleteFakeRepositoryPricesUnder50 : IRepository
        {
            public IEnumerable<Product> Products { get; } = new Product[]
            {
                // notice this first product is different from the original
                new Product { Name = "P1", Price = 5M },
                new Product { Name = "P2", Price = 48.95M },
                new Product { Name = "P3", Price = 19.50M },
                new Product { Name = "P4", Price = 34.95M }
            };

            public void AddProduct(Product p)
            {
                //
            }
        }
        [Fact]
        public void IndexActionModelIsCompltePricesUnder50()
        {
            // Arrange
            var controller = new HomeController();
            controller.Repository = new ModelCompleteFakeRepositoryPricesUnder50();
            // Act
            var model = (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;
            // Assert
            Assert.Equal(controller.Repository.Products, model, Comparer.Get<Product>((p1, p2) => 
                p1.Name == p2.Name && p1.Price == p2.Price));
        }
        // using test data class
        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void IndexActionModelIsComplete(Product[] products) {

            // Arrange
            var controller = new HomeController();
            controller.Repository = new ModelCompleteFakeRepository { Products = products };

            // Act
            var model = (controller.Index() as ViewResult)?.ViewData.Model
                as IEnumerable<Product>;

            // Assert
            Assert.Equal(controller.Repository.Products, model,
                Comparer.Get<Product>((p1, p2) => p1.Name == p2.Name
                    && p1.Price == p2.Price));
        }

        [Fact]
        public void RepositoryPropertyCalledOnce() {

            // Arrange
            var mock = new Mock<IRepository>();
            mock.SetupGet(m => m.Products)
                .Returns(new[] { new Product { Name = "P1", Price = 100 } });
            var controller = new HomeController { Repository = mock.Object };

            // Act
            var result = controller.Index();

            // Assert
            mock.VerifyGet(m => m.Products, Times.Once);
        }
    }
}
