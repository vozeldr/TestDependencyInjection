# Test Dependency Injection

Created this just to play around with some dependency injection in dotnet for a new project.

## Projects

### ClassLibrary1

Just a simple library with some interfaces and implementations to test dependency injection with. A lot of this came from 
[Gunnar Piepman](https://gunnarpeipman.com/dotnet-core-dependency-injection/),
[Microsoft](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1) and 
[Autofac](https://autofac.readthedocs.io/en/latest/getting-started/index.html) sites.

- AsyncMessageWriter: Requires the Microsoft logging to be set up and takes an ILogger in the constructor.
- IOutput: Interface for something that can write content.
- ConsoleOutput: Implementation of IOutput that writes content to the console.
- IDateWriter: Interface for something that can write a date.
- TodayWriter: Implementation of IDateWriter that uses an IOutput provided to the constructor to write the current date.

### ConsoleApp1

Uses the Microsoft CommonServiceLocator, backed by Autofac. Microsoft logging is configured using Microsoft Dependency Injection 
(ServiceCollection) while the IOutput and IDateWriter are configured directly in Autofac.

### ConsoleApp2

Basically the same as ConsoleApp1 except that it also gets part of its Autofac configuration from a json file.

### TestProject1

- Startup: Since (at least from what I found), XUnit doesn't support something like AssemblyInitialize in MSTest, this static
class is used to make sure the ServiceLocator is configured before each test. Sets up mocks for logging and for IOutput.
- MockLogger: A logging implementation for testing... not robust, just enough to test the mocking capability.
- MockLoggerProvider: A logging provider that returns a MockLogger
- MockOutput: An implementation of IOutput for testing... again, not robust.
- UnitTest1: Contains unit tests to exercise the dependency injection / mocking capabilities in 3 ways...
    + UsingAutoWiredDependencyInConstructorTest: The TodayWriter is loaded into Autofac. When Autofac creates the instance for 
    IDateWriter, the dependency for the IOutput is automatically provided via Autofac.
    + UsingManuallyCreatedAndRegisteredDependencyInConstructorTest: Kind of pointless but I wrote it to see how it would look 
    (verbose and ugly!) to use the ServiceLocator to resolve dependencies manually when calling constructors.
    + UsingMoqToInstantiateWithMockedObjectTest: Uses Moq through Autofac to create a mock IOutput implementation that is then
    injected to the constructor of a TodayWriter and used to verify that the Write method of the IOutput is called when the 
    WriteDate method of the TodayWriter is called.
