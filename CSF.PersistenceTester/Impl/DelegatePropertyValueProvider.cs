using System;

namespace CSF.PersistenceTester.Impl
{
    public class DelegatePropertyValueProvider<TEntity,TProperty> : IGetsPropertyValue<TEntity,TProperty>
    {
        readonly Func<TEntity, TProperty> getterDelegate;

        public TProperty GetValue(TEntity entity) => getterDelegate(entity);

        object IGetsPropertyValue.GetValue(object entity)
        {
            return GetValue((TEntity) entity);
        }

        public DelegatePropertyValueProvider(Func<TEntity, TProperty> getterDelegate)
        {
            this.getterDelegate = getterDelegate ?? throw new ArgumentNullException(nameof(getterDelegate));
        }
    }
}