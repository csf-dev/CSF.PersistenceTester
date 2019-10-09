using System;
using System.Data;
using System.Reflection;
using CSF.Reflection;

namespace CSF.PersistenceTester.Tests.NHibernate
{
    public class SchemaCreator
    {
        public void CreateSchema(IDbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            var script = Assembly.GetExecutingAssembly().GetManifestResourceText("CreateSchema.sql");
            using (var command = connection.CreateCommand())
            {
                command.CommandText = script;
                command.ExecuteNonQuery();
            }
        }
    }
}
