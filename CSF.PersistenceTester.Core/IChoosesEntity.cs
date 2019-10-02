using System;
namespace CSF.PersistenceTester
{
    public interface IChoosesEntity<T> where T : class
    {
        IConfiguresComparison<T> WithEntity(T entity);
    }
}
