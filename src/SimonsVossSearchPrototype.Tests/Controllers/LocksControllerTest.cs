using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimonsVossSearchPrototype;
using SimonsVossSearchPrototype.Controllers;

namespace SimonsVossSearchPrototype.Tests.Controllers
{
    [TestClass]
    public class LocksControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            LocksController controller = new LocksController();

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            LocksController controller = new LocksController();

            // Act
            var result = controller.Get(Guid.Parse("0cccab2b-bc8d-44c5-b248-8a9ca6d7e899"));

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Search()
        {
            // Arrange
            LocksController controller = new LocksController();

            var term = "Head";
            // Act
            var result = controller.Search(term);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
