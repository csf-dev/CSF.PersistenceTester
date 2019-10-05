using System;
namespace CSF.PersistenceTester
{
    /// <summary>
    /// A service which is the union of <see cref="IChoosesEntity"/> and <see cref="IConfiguresTestSetup"/>.
    /// </summary>
    public interface IChoosesEntityWithOptionalSetup : IChoosesEntity, IConfiguresTestSetup
    {
    }
}
