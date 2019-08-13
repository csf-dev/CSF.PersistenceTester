using System;
using System.Reflection;

namespace CSF.PersistenceTester.Impl
{
    public class ReflectionPropertyValueProvider<TEntity,TProperty> : IGetsPropertyValue<TEntity,TProperty>
    {
        readonly PropertyInfo property;

        public TProperty GetValue(TEntity entity) => (TProperty) property.GetValue(entity);

        object IGetsPropertyValue.GetValue(object entity)
        {
            return GetValue((TEntity) entity);
        }

        public ReflectionPropertyValueProvider(PropertyInfo property)
        {
            this.property = property ?? throw new ArgumentNullException(nameof(property));
        }
    }
}