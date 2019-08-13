namespace CSF.PersistenceTester.Impl
{
    public interface IGetsPropertyValue<in TEntity, out TProperty> : IGetsPropertyValue
    {
        TProperty GetValue(TEntity entity);
    }
}