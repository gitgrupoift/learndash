namespace LearnDash.Dal.NHibernate.Mappings
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using FluentNHibernate.Mapping;

    using LearnDash.Dal.Models;

    public class UserProfileMap : ClassMap<UserProfile>
    {
        public UserProfileMap()
        {
            Id(x => x.ID);
            Map(x => x.UserId).Unique();

            HasMany(x => x.Dashboards).KeyColumn("UserProfileId").Not.Inverse().Cascade.AllDeleteOrphan().LazyLoad();
        }
    }
}
