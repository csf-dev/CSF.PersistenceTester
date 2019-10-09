using System;
using NHibernate.Mapping.ByCode.Conformist;

namespace CSF.PersistenceTester.Tests.NHibernate
{
    public class EntityWithBadlyNamedPropertyMap : ClassMapping<EntityWithBadlyNamedProperty>
    {
        public EntityWithBadlyNamedPropertyMap()
        {
            Id(x => x.Identity);

            // Saving or getting this will cause an exception on the db, because
            // in the schema it is named "BadlyNamedProperty"
            Property(x => x.MisnamedProperty);
        }
    }
}
