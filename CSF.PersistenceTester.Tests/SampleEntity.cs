using System;
using System.Collections.Generic;

namespace CSF.PersistenceTester.Tests
{
    public class SampleEntity
    {
        public long Identity { get; set; }
        public string StringProperty { get; set; }
        public ISet<SampleChildEntity> ChildEntities { get; set; }
    }
}
