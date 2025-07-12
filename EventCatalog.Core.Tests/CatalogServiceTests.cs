using System.Linq;
using Xunit;
using System;

namespace EventCatalog.Core.Tests
{
    public class CatalogServiceTests
    {
        [Fact]
        public void GetDomains_Returns_Correct_Number_Of_Domains()
        {
            // Arrange
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            var catalogService = new CatalogService("examples/default");

            // Act
            var domains = catalogService.GetDomains();

            // Assert
            Assert.Single(domains);
        }
    }
}
