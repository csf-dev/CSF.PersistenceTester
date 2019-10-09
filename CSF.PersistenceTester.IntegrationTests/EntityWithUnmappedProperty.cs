using System;
namespace CSF.PersistenceTester.Tests
{
    public class EntityWithUnmappedProperty
    {
        public virtual long Identity { get; set; }
        public virtual int UnmappedProperty { get; set; }
    }
}
