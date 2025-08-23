using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.InfraStructure.Repositores.Service;

namespace WebApiEcomm.UnitTests.InfraStructure.Services
{
    [TestFixture]
    public class ImageManagementServiceTests
    {
        private ImageManagementService _service;
        private Mock<IFileProvider> _fileProviderMock;

        [SetUp]
        public void Setup()
        {
            _fileProviderMock = new Mock<IFileProvider>();
            _service = new ImageManagementService(_fileProviderMock.Object);

            if (!Directory.Exists(Path.Combine("wwwroot", "Images", "Test")))
            {
                Directory.CreateDirectory(Path.Combine("wwwroot", "Images", "Test"));
            }
        }

        [Test]
        public async Task AddImageAsync_ShouldSaveFileAndReturnPath()
        {
            // Arrange
            var fileName = "testImage.jpg";
            var content = "fake image content";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            var formFile = new FormFile(stream, 0, stream.Length, "file", fileName);

            var files = new FormFileCollection { formFile };

            // Act
            var result = await _service.AddImageAsync(files, "Test");

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo($"/Images/Test/{fileName}"));
            Assert.That(File.Exists(Path.Combine("wwwroot", "Images", "Test", fileName)), Is.True);
        }

        [Test]
        public async Task DeleteImageAsync_ShouldReturnSuccessMessage_WhenFileExists()
        {
            // Arrange
            var filePath = Path.Combine("wwwroot", "Images", "Test", "deleteImage.jpg");
            await File.WriteAllTextAsync(filePath, "dummy image content");

            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.Setup(f => f.Exists).Returns(true);
            fileInfoMock.Setup(f => f.PhysicalPath).Returns(filePath);

            _fileProviderMock.Setup(fp => fp.GetFileInfo("/Images/Test/deleteImage.jpg"))
                             .Returns(fileInfoMock.Object);

            // Act
            var result = await _service.DeleteImageAsync("/Images/Test/deleteImage.jpg");

            // Assert
            Assert.That(result, Is.EqualTo("Image deleted successfully."));
            Assert.That(File.Exists(filePath), Is.False);
        }

        [Test]
        public async Task DeleteImageAsync_ShouldReturnNotFoundMessage_WhenFileDoesNotExist()
        {
            // Arrange
            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.Setup(f => f.Exists).Returns(false);

            _fileProviderMock.Setup(fp => fp.GetFileInfo("/Images/Test/missing.jpg"))
                             .Returns(fileInfoMock.Object);

            // Act
            var result = await _service.DeleteImageAsync("/Images/Test/missing.jpg");

            // Assert
            Assert.That(result, Is.EqualTo("Image not found."));
        }
    }
}
