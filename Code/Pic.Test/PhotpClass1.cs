using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalPhotos.Controllers;
using PersonalPhotos.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pic.Test
{
    public class PhotoTest
    {
        //Injection overcone on the Constractor if has value instead on new 

        private readonly PhotosController _photoController;

        private readonly Mock<IFileStorage> _fileStorage;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Mock<IKeyGenerator> _keyGenerator;
        private readonly Mock<IPhotoMetaData> _photoMetaData;

        public PhotoTest()
        {
          

           

            _fileStorage =  new Mock<IFileStorage>();
            _keyGenerator = new Mock<IKeyGenerator>();
            _photoMetaData = new Mock<IPhotoMetaData>();

            var session = Mock.Of<ISession>();
            session.Set("user", Encoding.UTF8.GetBytes("a@b.com"));
            var httpContext = Mock.Of<HttpContext>(x => x.Session == session);
            _httpContextAccessor =Mock.Of<IHttpContextAccessor>(x => x.HttpContext == httpContext);


            this._photoController = new PhotosController(_keyGenerator.Object, _httpContextAccessor, _photoMetaData.Object, _fileStorage.Object);
        }
        [Fact]
        public async Task Upload_GivenModel_ReturnLoginView()
        {

            
            var file =Mock.Of<IFormFile>();
            var model = Mock.Of<PhotoUploadViewModel>(x=>x.File ==file);

            var res =await _photoController.Upload(model) as RedirectToActionResult;



            //Assert.IsAssignableFrom<IActionResult>(res);
            //var res2 = res as ViewResult;
            Assert.NotNull(res);
            Assert.IsType<RedirectToActionResult>(res);
            Assert.Equal("Display", res.ActionName, ignoreCase: true);

        }
        [Fact]
        public void Display__ReturnView()
        {
           
            var res =  _photoController.Display() as ViewResult;



            //Assert.IsAssignableFrom<IActionResult>(res);
            //var res2 = res as ViewResult;
            Assert.NotNull(res);
            Assert.IsType<ViewResult>(res);
            Assert.Equal("Display", res.ViewName, ignoreCase: true);

        }
        
    }
}
