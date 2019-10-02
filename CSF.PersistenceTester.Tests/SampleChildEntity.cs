using System;
namespace CSF.PersistenceTester.Tests
{
    public class SampleChildEntity
    {
        public long Identity { get; set; }
        public string StringProperty { get; set; }
        public SampleEntity Parent { get; set; }
    }
}
