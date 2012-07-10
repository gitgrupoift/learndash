using System.Collections.Generic;
using FluentNHibernate.Mapping;
using LearnDash.Dal.Models;

namespace LearnDash.Dal.NHibernate.Mappings
{
    public class UserProfileMap : ClassMap<UserProfile>
    {
        public UserProfileMap()
        {
            Id(x => x.ID);
            Map(x => x.UserId).Unique();

            HasMany(x => x.Dashboards).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
