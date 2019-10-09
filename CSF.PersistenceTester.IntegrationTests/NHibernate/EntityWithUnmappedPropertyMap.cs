using System;
using NHibernate.Mapping.ByCode.Conformist;

namespace CSF.PersistenceTester.Tests.NHibernate
{
    public class EntityWithUnmappedPropertyMap : ClassMapping<EntityWithUnmappedProperty>
    {
        public EntityWithUnmappedPropertyMap()
        {
            Id(x => x.Identity);
            // The mapping for UnmappedProperty is intentionally missing
        }
    }
}
