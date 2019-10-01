using System;
namespace CSF.PersistenceTester
{
    public interface ISavesEntity<T> where T : class
    {
        IConfiguresComparison<T> WithEntity(T entity);
    }
}
