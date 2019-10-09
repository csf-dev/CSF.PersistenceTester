# NHibernate persistence tester
This small library [and NuGet package] provides a concise, consistent and convenient way to test [NHibernate mappings]. The package facilitates the following process.

1. Save an entity object to a database using an NHibernate **ISession**
2. Re-retrieve a new instance of that entity from the session using its identity/primary key
    * The entity is *evicted* from the session first, to ensure that it is retrieved afresh
3. Compare the retrieved entity with the original, using *a customisable equality comparer*, to verify that they are value-equal
    * If the objects are not equal, a detailed result object indicates how they differ

## Other features
* An *optional setup step* is supported, for example to save dependency entities. This is particularly relevant if you are not using 'cascade on save' in your mappings.
* The comparison step makes use of [CSF.EqualityRules]. This provides a framework for testing **value equality** between object instances, with minimal boilerplate.

[and NuGet package]: https://www.nuget.org/packages/CSF.PersistenceTester
[NHibernate mappings]: https://nhibernate.info/
[CSF.EqualityRules]: https://github.com/csf-dev/CSF.EqualityRules

## Usage example
Let's imagine we are testing the NHibernate mapping for the following sample entity:

```csharp
public class Cat
{
    public virtual long Identity { get;  set; }
    public virtual string Name { get;  set; }
    public virtual DateTime Birthday { get;  set; }
}
```

We will assume that `Identity` is mapped as the primary key/id property and all other properties are mapped as simple property mappings.

```csharp
// Get this NHibernate ISessionFactory in the same way
// your application would.
var sessionFactory = GetSessionFactory();

// The entity instance should have property
// values set. You should consider using
// AutoFixture to automate this.
var myCat = GetSampleCat();

var result = TestPersistence.UsingSessionFactory(sessionFactory)
    .WithEntity(myCat)
    .WithComparison(builder => builder.ForAllOtherProperties());

// If you are using NUnit, you may use the following
Assert.That(result, Persisted.Successfully());
```

Using [the NUnit integration package], this is sufficient to perform a test. Otherwise your test logic should inspect the `result` object to verify that it indicates success. The result includes properties which help diagnose the cause(s) of failures, should they occur.

## How this helps your project
It is accepted that *traditional unit tests* for code/logic *should not use the database*; test data should be injected directly by the test. This can tend to mean that data-access has no explicit test coverage of its own. This means that when problems arise, they are found - at best - in a more expensive testing phase. At worst they are not found until the app is exercised by a human user.

When using NHibernate, in large part, whether or not your data access functions correctly is governed by **your O/R mappings**. Whether you are using XML mappings, mapping by code or by convention, it is very helpful to know:

> Can I save an entity with some data, then retrieve it and be confident that what I get back is the same as I saved?

That is the question that this library helps address.

### Usage suggestion
To get the most from persistence tests, consider using a tool such as [AutoFixture] to generate entity objects for the persistence test. This can generate object instances with unpredictable data filled-into every property.

Additionally it is useful to script either the creation or 'reset' of a testing database. This testing database should have the same schema as your application's database, but should be empty of data, save for any 'system' data which might be considered a part of that schema.

Of course, Sqlite is ideal for this purpose, if your application is able to support it.

### No silver bullet
Of course, just because entities persist and may be retrieved correctly does not *guarantee* that your mappings and data access are perfect. This library should be used as a part of a wider testing strategy.

As an example, non-trivial queries should be isolated into classes of their own, which can be tested against a database which has been scripted to known data-set. The test may then assert that the results are as-expected.

## Credit where due
This project was inspired by [Fluent NHibernate's persistence specification tests].

With the introduction of NHibernate "mapping by code" & "mapping by convention", Fluent NHibernate seems to have fallen somewhat out of favour. The persistence specification tests were a feature which I had always liked, though. This library is an effort to bring the same kind of functionality into a small and independent package.

[the NUnit integration package]: https://www.nuget.org/packages/CSF.PersistenceTester.NUnit
[AutoFixture]: https://github.com/AutoFixture/AutoFixture
[Fluent NHibernate's persistence specification tests]: https://github.com/FluentNHibernate/fluent-nhibernate/wiki/Persistence-specification-testing
