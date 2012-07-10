using System.Collections.Generic;
using FluentNHibernate.Mapping;
using LearnDash.Dal.Models;

namespace LearnDash.Dal.NHibernate.Mappings
{
    public class LearningDashboardMap : ClassMap<LearningDashboard>
    {
        public LearningDashboardMap()
        {
            Id(x => x.ID);
            HasMany(x => x.Flows).Cascade.AllDeleteOrphan().LazyLoad();
        }
    }
}