using System;
using NHibernate;

namespace CSF.PersistenceTester.Impl
{
    public class PersistenceTester<T> : ITestsPersistence<T>where T : class
    {
        readonly PersistenceTestSpec<T> spec;
        object identity;

        public PersistenceTestResult GetTestResult()
        {
            PersistenceTestResult result = null;

            result = TrySetup();
            if (result != null) return result;

            result = TrySave();
            if (result != null) return result;

            return TryCompare();
        }

        PersistenceTestResult TrySetup()
        {
            try
            {
                spec.Setup?.Invoke(spec.SessionProvider.GetSession());
            }
            catch(Exception ex)
            {
                return new PersistenceTestResult(typeof(T))
                {
                    SetupException = ex
                };
            }

            return null;
        }

        PersistenceTestResult TrySave()
        {
            var session = spec.SessionProvider.GetSession();

            try
            {
                using (var tran = session.BeginTransaction())
                {
                    identity = session.Save(spec.Entity);
                    if (identity == null)
                        throw new InvalidOperationException($"The entity identity returned by {nameof(ISession)}.{nameof(ISession.Save)} should not be null.");
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                return new PersistenceTestResult(typeof(T))
                {
                    SaveException = ex
                };
            }

            return null;
        }

        PersistenceTestResult TryCompare()
        {
            var session = spec.SessionProvider.GetSession();

            try
            {
                session.Evict(spec.Entity);

                var retrieved = session.Get<T>(identity);

                var equalityResult = spec.EqualityRule.GetEqualityResult(spec.Entity, retrieved);

                return new PersistenceTestResult(typeof(T))
                {
                    EqualityResult = equalityResult
                };
            }
            catch (Exception ex)
            {
                return new PersistenceTestResult(typeof(T))
                {
                    ComparisonException = ex
                };
            }
        }

        public PersistenceTester(PersistenceTestSpec<T> spec)
        {
            this.spec = spec ?? throw new ArgumentNullException(nameof(spec));
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    spec.SessionProvider.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
