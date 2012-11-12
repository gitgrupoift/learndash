using System.Web.Mvc;
using LearnDash.JsonHelpers;

namespace LearnDashDal.Tests.Controllers
{
    public static class TestHelpers
    {
        public static bool ExtractIsSuccess(ActionResult result)
        {
            var jsonResult = (JsonResult)result;
            var message = (JsonMessage)jsonResult.Data;

            return message.isSuccess;
        }
    }
}