using System;

namespace Enterprise.DA
{

    //EF Core : Microsoft's Cross Platform data access framework for .net
    //ORM : reduce the friction between how data is structured in a relational database and how you define your classes.
    //and that enhances developers productivity and coding consistency.

    //Database Providers for EF Core : docs.microsoft.com/ef
    // and it supports non-relational database : Azur-Cosmos db
    //to see breaking changes /ef/core/what-is-new/ef-core-3.0/breaking-changes

    // Try not to use EF Core 3.0 as it does not support .net framework only .net core

    // add the sql server nugut package of the ef core to the project to start using ef core on sql server

    // you can model view generated from ef core Power tools exstension for VS2019

    // you must specify data provider and connection string

    // you can add some configuration on overrid OnConfiguring(DbContextOptionsBuilder builder) => builder.useSqlServer(connString);

    //https://www.brentozar.com/archive/2017/05/case-entity-framework-cores-odd-sql/

    // the source code on : https://github.com/dotnet/efcore

}
