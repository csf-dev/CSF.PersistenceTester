using System;
namespace CSF.PersistenceTester
{
    public interface IChoosesEntityWithOptionalSetup<T> : IChoosesEntity<T>, IConfiguresTestSetup<T> where T : class
    {
    }
}
