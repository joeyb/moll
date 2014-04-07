# Moll
A simple .NET object mapper framework, intended to be used along with an IoC
container.

## Why?
Moll was built after spending many years using AutoMapper and eventually
running into issues maintaing large numbers of object mapping definitions
as a project grows.

Moll takes a completely differently approach to declaring and executing
mappings. The core of Moll is the `IMapper<TSrc, TDest>` interface.
Instead of having a static method that executes the mapping, the intention
is to use dependency injection to get the correct instance of that
interface. This means that your mapper is now just a normal, test-able
class like the rest of your business logic instead of being some custom
DSL.

For convenience there is a default implementation of the `IMapper<TSrc, TDest>`
interface, `AutomaticMapper<TSrc, TDest>` that uses reflection to
automatically map properties whose name and type match. It also provides
an extension point for doing additional custom mapping if needed.

The `AutomaticMapper<TSrc, TDest>` is intended to be used for simple objects
in sections of code that are not sensitive to huge performance concerns.
If you need to map a huge number of objects or have very custom needs then
you'll probably just want to make your own custom implementation of the
`IMapper<TSrc, TDest>` interface.

## Usage

```
PM> Install-Package Moll
```

Then start making your own mappers, either by using `AutomaticMapper<TSrc, TDest>`
as a base or building your own custom implementation of the `IMapper<TSrc, TDest>`
interface.

You can then just directly new up instances of those mappers and use them, or
configure your IoC container so that it knows about the mappers and let it handle
injecting the proper implementation of any mappers that you need. This is definitely
the suggested route, especially because it makes nested mappers trivial since the
mappers can have any dependencies injected that they need (including other mappers).

## What does the name Moll mean?
[Herman Moll](http://en.wikipedia.org/wiki/Herman_Moll) was an early cartographer
who published one of the most famous early maps of North America, unofficially
known as the "Beaver Map".
