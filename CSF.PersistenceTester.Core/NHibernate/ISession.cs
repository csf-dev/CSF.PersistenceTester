using System;
namespace CSF.PersistenceTester.NHibernate
{
    /// <summary>
    /// An abstraction around an NHibernate <c>ISession</c>.  This is used so that the core logic does not
    /// depend directly upon NHibernate (and may support many different versions).
    /// </summary>
    public interface ISession : IDisposable
    {
        /// <summary>
        /// Gets the native NHibernate ISession object.
        /// </summary>
        /// <returns>The session.</returns>
        object GetNativeSession();

        /// <summary>
        /// Saves an entity object and returns an object representing its identity/primary key.
        /// </summary>
        /// <returns>The identiy.</returns>
        /// <param name="entity">The entity to save.</param>
        object Save(object entity);

        /// <summary>
        /// Gets an entity using its identity.
        /// </summary>
        /// <returns>The retrieved entity, or a <c>null</c> reference if the entity does not exist.</returns>
        /// <param name="identity">The identity.</param>
        /// <typeparam name="T">The entity type.</typeparam>
        T Get<T>(object identity);

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <returns>The transaction.</returns>
        ITransaction BeginTransaction();

        /// <summary>
        /// Evicts/removes an entity instance from the current session.  This means that subsequent retrievals will
        /// return a different object instance, rather than returning a previously-cached instance.
        /// </summary>
        /// <param name="entity">The entity to evict.</param>
        void Evict(object entity);
    }
}
