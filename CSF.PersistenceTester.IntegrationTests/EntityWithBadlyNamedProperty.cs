using System;
namespace CSF.PersistenceTester.Tests
{
    public class EntityWithBadlyNamedProperty
    {
        public virtual long Identity { get; set; }
        public virtual int MisnamedProperty { get; set; }
    }
}
