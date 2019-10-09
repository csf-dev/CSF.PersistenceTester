
DROP TABLE IF EXISTS SampleEntity;
DROP TABLE IF EXISTS EntityWithUnmappedProperty;
DROP TABLE IF EXISTS EntityWithBadlyNamedProperty;

CREATE TABLE SampleEntity (
    Identity BIGINT NOT NULL PRIMARY KEY,
    StringProperty VARCHAR NULL
);

CREATE TABLE EntityWithUnmappedProperty (
    Identity BIGINT NOT NULL PRIMARY KEY,
    UnmappedProperty INT NULL
);

CREATE TABLE EntityWithBadlyNamedProperty (
    Identity BIGINT NOT NULL PRIMARY KEY,
    BadlyNamedProperty INT NULL
);