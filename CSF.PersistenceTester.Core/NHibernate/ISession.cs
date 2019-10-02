using System;
namespace CSF.PersistenceTester.NHibernate
{
    public interface ISession : IDisposable
    {
        object GetNativeSession();

        object Save(object entity);
        T Get<T>(object identity);
        ITransaction BeginTransaction();
        void Evict(object entity);
    }
}
