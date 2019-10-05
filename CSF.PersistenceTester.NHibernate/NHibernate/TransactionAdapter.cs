using System;
using INhTransaction = NHibernate.ITransaction;

namespace CSF.PersistenceTester.NHibernate
{
    /// <summary>
    /// An adapter around the native NHibernate <c>ITransaction</c> object, allowing it to be used as
    /// an <see cref="ITransaction"/>.
    /// </summary>
    public class TransactionAdapter : ITransaction
    {
        readonly INhTransaction nhTran;

        /// <summary>
        /// Gets the native NHibernate ITransaction object.
        /// </summary>
        /// <returns>The transaction.</returns>
        public object GetNativeTransaction() => nhTran;

        void ITransaction.Commit() => nhTran.Commit();

        void IDisposable.Dispose() => nhTran.Dispose();

        void ITransaction.Rollback() => nhTran.Rollback();

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionAdapter"/> class.
        /// </summary>
        /// <param name="nhTran">The native NHibernate transaction.</param>
        public TransactionAdapter(INhTransaction nhTran)
        {
            this.nhTran = nhTran ?? throw new ArgumentNullException(nameof(nhTran));
        }
    }
}
