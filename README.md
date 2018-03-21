# Ministry.ReflectionHelper
This project is set up to make the process of Reflection in .net much easier to code than it is using the standard methods provided by the framework by applying core features through a simple API. I always found the .net Reflection methods quite unwieldy and unintuitive.

## Fields
The following methods exist for fields...

### Ministry.ReflectionHelper.Field.Exists()
Traditional and generic variants of this method can be called either passing in a type or a value of a given type along with a field name to look for on that type.

### Ministry.ReflectionHelper.Field.Get()
Traditional and generic variants of this method can be called by passing in an object to query along with a field name. For example...
```
Field.Get(myObj, "name");
```
Overloads are also provided for static types that take a type parameter instead of an object instance.

### Ministry.ReflectionHelper.Field.Set()
Traditional and generic variants of this method can be called by passing in an object to query along with a field name to set. For example...
```
Field.Set(myObj, "name", "new value");
```
Overloads are also provided for static types that take a type parameter instead of an object instance.

### Ministry.ReflectionHelper.Field.GetInfo()
Sometimes you do need the Info objects but in traditional .net code this can become quite awkward if the object you are looking for is further down the inheritance tree than the layer that you are interrogating. Ministry.ReflectionHelper wraps up all of the code required to recurse down the inheritance tree to find the type that you are looking for. This method also provides the same service to the rest of the Field class, enabling the helper to provide Reflection against the whole inheritance tree.

## Properties
Reflection on Fields & Properties are similar. The following methods exist for properties...

### Ministry.ReflectionHelper.Property.Exists()
Traditional and generic variants of this method can be called either passing in a type or a value of a given type along with a property name to look for on that type.

### Ministry.ReflectionHelper.Property.IsReadOnly()
Traditional and generic variants of this method can be called either passing in a type or a value of a given type along with a property name to look for on that type.

### Ministry.ReflectionHelper.Property.Get()
Traditional and generic variants of this method can be called by passing in an object to query along with a property name. For example...
```
Property.Get(myObj, "Name");
```
Overloads are also provided for static types that take a type parameter instead of an object instance.

### Ministry.ReflectionHelper.Property.Set()
Traditional and generic variants of this method can be called by passing in an object to query along with a property name to set. For example...
```
Property.Set(myObj, "Name", "new value");
```
Overloads are also provided for static types that take a type parameter instead of an object instance.

### Ministry.ReflectionHelper.Property.GetInfo()
Sometimes you do need the Info objects but in traditional .net code this can become quite awkward if the object you are looking for is further down the inheritance tree than the layer that you are interrogating. Ministry.ReflectionHelper wraps up all of the code required to recurse down the inheritance tree to find the type that you are looking for. This method also provides the same service to the rest of the Properties class, enabling the helper to provide Reflection against the whole inheritance tree.

### Ministry.ReflectionHelper.Property.Indexer.GetItem()
Traditional and generic variants of this method can be called by passing in an object to query along with a property name and an index. For example...
```
Property.Indexer.GetIItem(myObj, "Index", 4);
```

### Ministry.ReflectionHelper.Property.Indexer.SetItem()
Traditional and generic variants of this method can be called by passing in an object to query along with a property name and an index to set. For example...
```
Property.Indexer.SetItem(myObj, "Index", 4, "new value");
```

## Methods
The following methods exist for methods. Method access through Reflection is very problematic. The Methods class within Ministry.ReflectionHelper, as with Fields and Properties, benefits from inheritance tree recursion.

### Ministry.ReflectionHelper.Method.Execute()
Many overloaded variants of this exist to call a method with or without parameters. The method execution code will search for the method to execute using the method below.

### Ministry.ReflectionHelper.Method.GetInfo()
Sometimes you do need the Info objects but in traditional .net code this can become quite awkward if the object you are looking for is further down the inheritance tree than the layer that you are interrogating. Ministry.ReflectionHelper wraps up all of the code required to recurse down the inheritance tree to find the type that you are looking for.

### Ministry.ReflectionHelper.Method.GetParameters()
Provides you with the info about the parameters for a given method.

## Summary
That's the key functionality of the Ministry.ReflectionHelper classes. If you want to do any kind of deep reflection and you don't have the joy of the Dynamic Language Runtime to help you out then I hope you'll find some of the methods in these classes really useful. If you want to find out any more, simply check out the source code.

## Upgrading v3.x to v4.x
If you are upgrading from v2 or v3 to v4 you may find breaking changes. In v4 we have added some helpful extension methods and moved support away from the stanadrd .net framework to NETSTandard 1.6 to support dotnet core.

## The Ministry of Technology Open Source Products
Welcome to The Ministry of Technology open source products. All open source Ministry of Technology products are distributed under the MIT License for maximum re-usability. Details on more of our products and services can be found on our website at http://www.ministryotech.co.uk

Our other open source repositories can be found here...

* [https://bitbucket.org/ministryotech](https://bitbucket.org/ministryotech)
* [https://github.com/ministryotech](https://github.com/ministryotech)
* [https://github.com/tiefling](https://github.com/tiefling)

Newer content prefers Github. Bitbucket is no longer actively used.

### Where can I get it?
You can download the package for this project from any of the following package managers...

- **NUGET** - [https://www.nuget.org/packages/Ministry.ReflectionHelper](https://www.nuget.org/packages/Ministry.ReflectionHelper)

### Contribution guidelines
If you would like to contribute to the project, please contact me.

### Who do I talk to?
* Keith Jackson - keith@ministryotech.co.uk
