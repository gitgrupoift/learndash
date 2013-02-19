namespace LearnDash.Controllers
{
    using System.Web.Http;

    using LearnDash.Dal.Models;
    using LearnDash.Services;

    public class FlowsController : ApiController
    {
        private readonly ILearningFlowService service;
         
        public FlowsController(ILearningFlowService service)
        {
            this.service = service;
        }

        public LearningFlow GetFlowById(int id)
        {
            return this.service.Get(id);
        }
    }
}
