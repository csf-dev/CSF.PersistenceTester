using System;
using INhTransaction = NHibernate.ITransaction;

namespace CSF.PersistenceTester.NHibernate
{
    public class TransactionAdapter : ITransaction
    {
        readonly INhTransaction nhTran;

        public object GetNativeTransaction() => nhTran;

        void ITransaction.Commit() => nhTran.Commit();

        void IDisposable.Dispose() => nhTran.Dispose();

        void ITransaction.Rollback() => nhTran.Rollback();

        public TransactionAdapter(INhTransaction nhTran)
        {
            this.nhTran = nhTran ?? throw new ArgumentNullException(nameof(nhTran));
        }
    }
}
