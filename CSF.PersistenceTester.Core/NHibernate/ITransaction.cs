using System;
namespace CSF.PersistenceTester.NHibernate
{
    /// <summary>
    /// An abstraction around an NHibernate <c>ITransaction</c>. This is used so that the core logic does not
    /// depend directly upon NHibernate (and may support many different versions).
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Gets the native NHibernate ITransaction object.
        /// </summary>
        /// <returns>The transaction.</returns>
        object GetNativeTransaction();

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Roll the transaction back.
        /// </summary>
        void Rollback();
    }
}
