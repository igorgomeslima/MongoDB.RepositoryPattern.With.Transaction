> 💚💛💙 PT-BR article [here](https://medium.com/@igorgomeslima92/c-repository-pattern-with-support-to-transactions-on-mongodb-b26b7f91f0d1)!

# C# - Repository Pattern With Support To Transactions on MongoDB.

This solution is divided into two approaches, and aims to implement the **Repository** pattern with support for sending transactional commands to an **MongoDB** database.

## 🦾 Environment Setup

  **Attention:** Currently **MongoDB** only supports `Transactions` in ***clusters*** with "[Replica Set](https://docs.mongodb.com/manual/replication/#transactions)" configured. For this reason, for greater agility in the process of setting up the environment, I recommend you to use the [free tier MongoDB Atlas](https://docs.atlas.mongodb.com/tutorial/deploy-free-tier-cluster/) as the even already has this configuration.

- **.NET Core SDK 3.1**;
- **MongoDB.Driver v. (2.11.2)** or higher;
- **MongoDB v. (4.0)** or higher with `Replica Set` configured;

If you have **MongoDB** installed locally, you can follow the [official documentation](https://docs.mongodb.com/manual/tutorial/convert-standalone-to-replica-set/) to perform this configuration, in this link there are several ramifications for completing the configuration, and if I use Docker I recommend this [tutorial here](https://www.youtube.com/watch?v=mlw7vWISaF4).

## 📟 To Run

1. Put your [MongoDB connection string](https://docs.mongodb.com/manual/reference/connection-string/) on `appsettings.Development.json` file;
2. Set the **startup** project client:
     - Client.WorkerService.FirstApproach
     - Client.WorkerService.SecondApproach
2. Study the solution; 🤓


## 🧪 First Approach

This approach shows how to transact commands using only the context implementation(`DbContext`) created for the database, in this case, 
`IMongoDbContextFirstApproach`. In this implementation it's not necessary another logic to manage commands that are inside a created transaction(it replaces something that would act as `Unit Of Work`).

**Approach considerations:**

 - The `IMongoDbContextFirstApproach` interface acts as `Unit Of Work`;
 - The `IMongoDbContextFirstApproach` interface is **responsible** for the creation of a `Transaction`(create the scope), in case the commands need to be involved in a` Transaction`;
 - The `IMongoDbContextFirstApproach` interface is **responsible** to **manage** the current `Transaction` of the scope created;
 - The `IMongoDbContextFirstApproach` interface is **responsible** for sending the commands to Database;
 - The `IMongoDbContextFirstApproach` interface is injected on "services" where the Database commands need to be transacted;


## 🧪 Second Approach

This approach shows how to transact commands using the `Unit Of Work` approach in fact. 
There are some points that we should consider when trying to create a `logic` to manage transactions using `Unit Of Work` in **MongoDB**, and I will talk about this on final considerations.

**Approach considerations:**

- The `IUnitOfWork` interface is responsible for **requesting the creation** of a `Transaction`(creating the scope), case commands need to be involved in one;
- The `IUnitOfWork` interface is responsible to **manage** the current `Transaction` of the created scope;
- The `IUnitOfWork` interface is responsible for sending commands to Database;
- The `IUnitOfWork` interface is injected into "services" where Database commands need to be transacted;
- The implementation of the `IUnitOfWork` interface is directly dependent on the `Driver` used to connect to **MongoDB**;
- The `IMongoDbContextSecondApproach` interface is responsible to receive and resolve **requests** from `Unit Of Work`;

As we can see, the implementation of the `IUnitOfWork` interface is not self-sufficient, as it depends on `IMongoDbContextSecondApproach` to be able create/manage a `Transaction`.
 
## 💭 Final Considerations
First of all, my "favorite" approach is the **"First"**, because the class `MongoDbContextFirstApproach` that manage connections and transactions is completely self-sufficient,
that is, it depends only on your resources to perform yours operations, like: `BeginTransaction`, `Commit`. `Etc...`. Still, this approach is not a "**silver bullet**".

Discussing a little about the "**Second Approach**", we realized that `IUnitOfWork` is not self-sufficient(as already mentioned). 
It is completely dependent of `IMongoDbContextSecondApproach` interface. Because it does not have the "**Driver**" resources, 
in this case the `IMongoClient` resource required to create a `Transaction`, so it needs to "request" these resources for a service that has them, in this case,
`IMongoDbContextSecondApproach`.

Ultimately `IUnitOfWork` acts as "**by pass**" of `Transaction`. Maybe it helps with the fact that you don't need to inject `DbContext` into services that need a `Transaction`,
but in my opinion, injecting a `DbContext` into a service is not a problem.

**Last caveats about this solution...**

*First...*

The implementation carried out to support `Transactions` in this **example project** were made in an **attempt** to meet the need that the 
**official MongoDB `Driver` design has**, which in this case is:

> **It's necessary inform at the moment of the creation
> command (`Insert, Delete, Update`) if it will belong in a
> `Transaction`, and if it belongs, the `Transaction` needs to be informed through a
> parameter(`IClientSessionHandle session`) of the respective `Driver` command**.

*For example:*

 - Collection<TDocument>.InsertOneAsync(session: **transaction**, ...);
 - Collection<TDocument>.InsertManyAsync(session: **transaction**, ...);
 - Collection<TDocument>.UpdateOneAsync(session: **transaction**, ...);

In this scenario, we are not able to "create a magic scope", like `TransactionScope`, and "inside it" execute the commands that will be part of our `Transaction`. 
There is a request to include this feature in the official `Driver` [here](https://jira.mongodb.org/browse/CSHARP-2890).

*Second...*

I still think that implementing the **Repository** & **Unit Of Work** pattern "on top" of the features offered by the official `Driver` connection, 
will imply the same criticisms that are made when we think about doing the same implementation "on top" of the **Entity Framework Core**. 
For example, `IMongoCollection<TDocument>` already acts like a "**repository**" for us, because have `Find, Insert, Update, Delete, etc...` methods. 
This is equivalent to EF Core `DbSet<TEntity>`(which comes under criticism from a large part of the community).

**I recommend some articles for reflection:**

**[Entity Framework Approach]**
- [# Is the repository pattern useful with Entity Framework Core?](https://www.thereformedprogrammer.net/is-the-repository-pattern-useful-with-entity-framework-core/)
- [# Repositories On Top UnitOfWork Are Not a Good Idea](https://rob.conery.io/2014/03/04/repositories-and-unitofwork-are-not-a-good-idea/)
- [# No need for repositories and unit of work with Entity Framework Core](https://gunnarpeipman.com/ef-core-repository-unit-of-work/)
- [# Quando usar Entity Framework com Repository Pattern? [PT-BR]](https://pt.stackoverflow.com/questions/51536/quando-usar-entity-framework-com-repository-pattern)

And of course I recommend the series of articles from [Brian Bu](https://brianbu.com/), where he puts important arguments in this discussion:

- [# The Repository Pattern isn’t an Anti-Pattern; You’re just doing it wrong.](https://brianbu.com/2019/09/25/the-repository-pattern-isnt-an-anti-pattern-youre-just-doing-it-wrong/)
- [# Typical Anti-Repository Arguments](https://brianbu.com/2019/09/27/typical-anti-repository-arguments/)
- [# Repository Pattern: Retrospective and Clarification](https://brianbu.com/2019/10/11/repository-pattern-retrospective-and-clarification/)

**[MongoDB Approach]**
- [# Reddit Discussion - Unit of work repository pattern with mongoDB](https://www.reddit.com/r/dotnet/comments/g9c5ht/unit_of_work_repository_pattern_with_mongodb/fospljn/?utm_source=reddit&utm_medium=web2x&context=3)

*Third(*is not the focus of the solution*)*...

The way the `IUnitOfWork` interface was implemented enables "**switch databases**" only modifying the class **that implements it** in our **dependency injection container**.

*For example:*

- services.AddScoped<IUnitOfWork, **UnitOfWorkPostgreSQL**>
- services.AddScoped<IProductRepository, ProductRepository**PostgreSQL**FirstApproach>();
- services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository**PostgreSQL**FirstApproach<>));

**OR**

- services.AddScoped<IUnitOfWork, **UnitOfWorkMySql**>
- services.AddScoped<IProductRepository, ProductRepository**MySql**FirstApproach>();
- services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository**MySql**FirstApproach<>));

*P.S. Please understand **"switch database"** as an exchange for the `Driver` that their **repositories** use, as each database provider handles a `Transaction` differently, 
thus our **concrete class implementation** of `IUnitOfWork` needs to change according to the specificity of the new assigned database.*

# 

If you find any failure/problems or have knowledge to improve this solution, I kindly ask you to contact me, either by *E-mail, Pull Request or Issue*.

:)

**Resources used**
> [Working with MongoDB Transactions with C# and the .NET Framework](https://developer.mongodb.com/how-to/transactions-c-dotnet)
