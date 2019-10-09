using System;
using System.Reflection;
using CSF.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace CSF.PersistenceTester.Tests.NHibernate
{
    public class SessionFactoryProvider
    {
        readonly IDetectsMono monoDetector;

        public ISessionFactory GetSessionFactory()
        {
            var config = GetConfiguration();
            return config.BuildSessionFactory();
        }

        Configuration GetConfiguration()
        {
            var config = new Configuration();

            config.DataBaseIntegration(db => {
                if (monoDetector.IsExecutingWithMono())
                {
                    MonoNHibernateSqlLiteDriver.MonoDataSqliteAssembly = Assembly.Load("Mono.Data.Sqlite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
                    db.Driver<MonoNHibernateSqlLiteDriver>();
                }
                else
                    db.Driver<SQLite20Driver>();

                db.Dialect<SQLiteDialect>();
                db.ConnectionStringName = "IntegrationTest";
                db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
            });

            var mapping = GetMapping();

            config.AddMapping(mapping);

            return config;
        }

        HbmMapping GetMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        public SessionFactoryProvider()
        {
            monoDetector = new MonoRuntimeDetector();
        }
    }
}
