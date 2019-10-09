using System;
using NHibernate.Mapping.ByCode.Conformist;

namespace CSF.PersistenceTester.Tests.NHibernate
{
    public class SampleEntityMap : ClassMapping<SampleEntity>
    {
        public SampleEntityMap()
        {
            Id(x => x.Identity);
            Property(x => x.StringProperty);
        }
    }
}
