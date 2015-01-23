Breeze.BusinessTime
===================

Breeze.BusinessTime is a BreezeJs .NET server library for enforcing entity authorization and pipelining of business logic.

#Installation
Breeze.BusinessTime is [hosted on NuGet](https://www.nuget.org/packages/Breeze.BusinessTime/). You can install it by searching for it in the Package Manager UI, or by entering the following in the Package Manager Console.
```
Install-Package Breeze.BusinessTime
```

#Getting Started
BusinessTime is implemented as a subclass of the existing Entity Framework context provider that ships with the BreezeJs.NET libraries.  This means that switching over to it from your existing controller declaration in only 1 line of code.

Original Breeze context provider declaration
```csharp
EFContextProvider<NorthwindDbContext> ContextProvider =
         new EFContextProvider<NorthwindDbContext>();
```
New declaration that uses BusinessTime:
```csharp
AuthorizedDbContextProvider<NorthwindDbContext> ContextProvider =
         new AuthorizedDbContextProvider<NorthwindDbContext>(User);
```
Now, if you were to run your application using this configuration, it will reject all requests to edit every entity in your DbContext.  This is the beginning of a "least privilege" configuration, since by default all save requests that get processed by Breeze are allowed by default (all save requests will get routed to ```SaveChanges()``` most of the time).

#Configuring Authorization Rules
In order to allow changes, you have a couple of options.  You can either decorate your Code First classes wtih ```[AuthorizeEntity]``` attributes to allow roles and/or users, or you can use the included ```RegistryAuthoriztionProvider``` to centralize your entity authorization grants.  

##Using Attributes with Code First
Following is an example which configures the Dealer class to only allow modifications by principals belonging to the Dealer role.

```csharp
[AuthorizeEntity(Roles = "Dealer")]
public class Dealer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public bool Preferred { get; set; }
}
```

And this example allows both the Owner and Dealer roles to submit changes to the Car entity.

```csharp
[AuthorizeEntity(Roles = "Owner, Dealer")]
public class Car
{
    public int Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
}
```

##Using the Registry Provider
The following example sets up the same authorization rules as the attributes above, but uses ```RegistryAuthoriztionProvider```. 

```csharp
private RegistryAuthorizationProvider CreateAuthorizationRegistry() {
	return RegistryAuthorizationProvider.Create()
	    .Register<Car>("Owner, Dealer")
	    .Register<Dealer>("Dealer");
}
```

As you can see above, multiple authorization rules can be chained once you have an instance of the provider. In order to use this provider with the AuthorizedDbContextProvider, just pass it as a constructor argument like the following example.

```csharp
_contextProvider = new AuthorizedDbContextProvider<NorthwindDbContext>(User, 
    roleProvider: CreateAuthorizationRegistry(), 
    allowedRoles: "Admins");
```

##Authorization Whitelisting
In the above example, you may have noticed the allowedRoles named argument. BusinessTime has a whitelist configuration which is typically reserved for system admin roles which should always be allowed to modify entities. 

#Pipelining Business Rules
 
```AuthorizedDbContextProvider<T>``` is a wrapper around another subclass ```PipelinedDbContextProvider<T>``` which places authorization checks as the first item in a pipeline which is processed on every request that is routed to ```SaveChanges()```. This is exposed via 2 pipelines named BeforePipeline and AfterPipeline, actually. The following diagram shows the order of execution of items in a typical pipeline.

![BusinessTime pipeline diagram](https://dl.dropboxusercontent.com/u/101304566/Github/Breeze.BusinessTime/Pipeline.PNG)

The only requirement to add an item to a pipeline is to implement the interface ```IProcessBreezeRequests```.

```csharp
public interface IProcessBreezeRequests
{
    void Process(Dictionary<Type, List<EntityInfo>> saveMap);
}
```

```saveMap``` is the same object that Breeze creates and sends to the default call to ```SaveChanges()```. Following is an example of using it to filter for a particular entity and apply a rule.

```csharp


public class PreferredDealerProtector : IProcessBreezeRequests
{
    private readonly IPrincipal _user;

    public PreferredDealerProtector(IPrincipal user)
    {
        _user = user;
    }

    public void Process(Dictionary<Type, List<EntityInfo>> saveMap)
    {
        if (!_user.IsInRole("Admin")) return;

        saveMap.ToList().ForEach(item =>
        {
            var errors = item.Value
                .Where(entityInfo =>
                    entityInfo.Entity is Dealer &&
                    entityInfo.OriginalValuesMap.ContainsKey("Preferred") &&
                    ((Dealer)entityInfo.Entity).Preferred != (bool)entityInfo.OriginalValuesMap["Preferred"]
                )
                .Select(entityInfo =>
                    new EFEntityError(entityInfo, "Unauthorized", "You are not authorized to make this change.",
                        "Preferred")
                )
                .ToList();

            if (errors.Any())
                throw new EntityErrorsException(errors);
        });
    }
}
```

Since this particular processor should prevent an edit, it should be added to the BeforePipeline.

```csharp
_contextProvider.BeforePipeline.Add(new PreferredDealerProtector(User));
```

Another use case for pipelines is to process additional logic after a successful change, such as auditing or notifications. The following processor is configured to look for changes to a particular entity and send an email.

```csharp
public class CarNotifierProcessor : IProcessBreezeRequests
{
    public void Process(Dictionary<Type, List<EntityInfo>> saveMap)
    {
        foreach (var info in saveMap.Values.SelectMany(l => l))
        {
            var car = info.Entity as Car;
            if (car != null && (info.EntityState == EntityState.Added || info.EntityState == EntityState.Modified))
            {
                EmailService.SendNotificationEmailForCar(car.Id));
            }
        }
    }
}
```

This would be added to the context provider like so:

```csharp
_contextProvider.AfterPipeline.Add(new CarNotifierProcessor());
```
