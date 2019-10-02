using System;
namespace CSF.PersistenceTester.NHibernate
{
    public interface ITransaction : IDisposable
    {
        object GetNativeTransaction();

        void Commit();
        void Rollback();
    }
}
