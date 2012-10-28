namespace LearnDash.Dal.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using Models;

    public class LearningFlowMap : ClassMap<LearningFlow>
    {
        public LearningFlowMap()
        {
            Id(x => x.ID);
            Map(x => x.Name);
            Map(x => x.FlowType).CustomType<FlowType>();

            HasMany(x => x.Tasks).KeyColumn("LearningFlowId").Not.Inverse().Cascade.AllDeleteOrphan().Not.LazyLoad();
        }
    }
}
