using FluentNHibernate.Mapping;
using LearnDash.Dal.Models;

namespace LearnDash.Dal.NHibernate.Mappings
{
    public class LearningTaskMap : ClassMap<LearningTask>
    {
        public LearningTaskMap()
        {
            Id(x => x.ID);
            Map(x => x.Importance);
            Map(x => x.IsNext);
            Map(x => x.Name);
            Map(x => x.TaskOrder);
            Map(x => x.Reccuring);
            Map(x => x.TimesDone);
        }
    }
}
