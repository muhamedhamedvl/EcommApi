using NUnit.Framework;
using WebApiEcomm.Core.Sharing;

namespace WebApiEcomm.UnitTests.Core
{
    [TestFixture]
    public class ProductParamsTests
    {
        [Test]
        public void DefaultValues_ShouldBeSetCorrectly()
        {
            var productParams = new ProductParams();

            Assert.That(productParams.PageSize, Is.EqualTo(3));
            Assert.That(productParams.PageNumber, Is.EqualTo(1));
            Assert.That(productParams.MaxPageSize, Is.EqualTo(6));
            Assert.That(productParams.CategoryId, Is.Null);
            Assert.That(productParams.Search, Is.Null);
            Assert.That(productParams.sort, Is.Null);
        }

        [Test]
        public void PageSize_ShouldNotExceed_MaxPageSize()
        {
            var productParams = new ProductParams
            {
                PageSize = 20 
            };

            Assert.That(productParams.PageSize, Is.EqualTo(productParams.MaxPageSize));
        }

        [Test]
        public void PageSize_ShouldSet_WhenWithinLimit()
        {
            var productParams = new ProductParams
            {
                PageSize = 4
            };

            Assert.That(productParams.PageSize, Is.EqualTo(4));
        }

        [Test]
        public void CategoryId_ShouldBeSetCorrectly()
        {
            var productParams = new ProductParams
            {
                CategoryId = 10
            };

            Assert.That(productParams.CategoryId, Is.EqualTo(10));
        }

        [Test]
        public void SearchAndSort_ShouldBeSetCorrectly()
        {
            var productParams = new ProductParams
            {
                Search = "Laptop",
                sort = "priceAsc"
            };

            Assert.That(productParams.Search, Is.EqualTo("Laptop"));
            Assert.That(productParams.sort, Is.EqualTo("priceAsc"));
        }
    }
}
