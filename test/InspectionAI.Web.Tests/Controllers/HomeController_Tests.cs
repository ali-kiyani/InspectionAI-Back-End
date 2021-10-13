using System.Threading.Tasks;
using InspectionAI.Models.TokenAuth;
using InspectionAI.Web.Controllers;
using Shouldly;
using Xunit;

namespace InspectionAI.Web.Tests.Controllers
{
    public class HomeController_Tests: InspectionAIWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}