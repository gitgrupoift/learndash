namespace LearnDash.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using LearnDash.Dal.Models;
    using LearnDash.Services;

    public class LearningFlowDTO
    {
        public virtual int ID { get; set; }

        public virtual string Name { get; set; }

        public virtual FlowType FlowType { get; set; }
    }

    public class FlowsController : ApiController
    {
        private readonly ILearningFlowService service;
         
        public FlowsController(ILearningFlowService service)
        {
            this.service = service;
        }

        public LearningFlowDTO[] GetAll()
        {
            return this.service.GetAll()
                .Select(x => new LearningFlowDTO
                                {
                                    ID = x.ID,
                                    Name = x.Name,
                                    FlowType = x.FlowType
                                }).ToArray();;
        }

        public LearningFlow Get(int id)
        {
            return this.service.Get(id);
        }
    }
}
