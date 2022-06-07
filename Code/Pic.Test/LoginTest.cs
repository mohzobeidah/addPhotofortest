using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalPhotos.Controllers;
using PersonalPhotos.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pic.Test
{
    public class LoginTest
    {
        //Injection overcone on the Constractor if has value instead on new 

        private readonly LoginsController _loginsController;

        private readonly Mock<ILogins> _logins;

       
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        public LoginTest()
        {
            _logins = new Mock<ILogins>();

            var session = Mock.Of<ISession>();
            var httpContext = Mock.Of<HttpContext>(x=>x.Session ==session);
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            this._loginsController = new LoginsController(_logins.Object, _httpContextAccessor.Object);
        }
        [Fact]
        public void Index_returnNoreturnUrl_ReturnLoginView()
        {

            var res = _loginsController.Index();



            Assert.IsAssignableFrom<IActionResult>(res);
            var res2 = res as ViewResult;
            Assert.NotNull(res);
            Assert.IsType<ViewResult>(res);
            Assert.Equal("login", res2.ViewName, ignoreCase: true);

        }
        [Fact]
        public async Task login_returnInvalidModelsatate_ReturnLoginView()
        {
            _loginsController.ModelState.AddModelError("test", "test");

            //var model = new Mock<LoginViewModel>();
            //var res = _loginsController.Login(model.Object) ;
            var res = await _loginsController.Login(Mock.Of<LoginViewModel>());



            Assert.IsAssignableFrom<IActionResult>(res);
            var res2 = res as ViewResult;
            Assert.NotNull(res);
            Assert.IsType<ViewResult>(res);
            Assert.Equal("login", res2.ViewName, ignoreCase: true);

        }
        [Fact]
        public async Task login_GivenCorrectPassword_RedirectTo()
        {

            var viewModel = Mock.Of<LoginViewModel>(x=>x.Email=="s@s.com" && x.Password =="ssss");
            var model = Mock.Of<User>(x => x.Email == "s@s.com" && x.Password == "ssss");
            _logins.Setup(x => x.GetUser(viewModel.Email)).ReturnsAsync(model);
            var res = await _loginsController.Login(viewModel) as RedirectToActionResult;

                Assert.IsType<RedirectToActionResult>(res) ;
            Assert.Equal("Display" ,res.ActionName, ignoreCase:true);


        }

        public async Task login_GivenWrongPassword_RedirectTo()
        {

            var viewModel = Mock.Of<LoginViewModel>(x => x.Email == "s@s.com" && x.Password == "ssss");
            var model = Mock.Of<User>(x => x.Password == "232");
            _logins.Setup(x => x.GetUser(viewModel.Email)).ReturnsAsync(model);
            var res = await _loginsController.Login(viewModel) as ViewResult;

            Assert.IsType<ViewResult>(res);
            Assert.Equal("login", res.ViewName, ignoreCase: true);


        }
    }
}
