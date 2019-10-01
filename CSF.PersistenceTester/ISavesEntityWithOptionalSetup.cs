using System;
namespace CSF.PersistenceTester
{
    public interface ISavesEntityWithOptionalSetup<T> : ISavesEntity<T>, IConfiguresTestSetup<T> where T : class
    {
    }
}
