# SEM5640 Restful Service

## About this project
This project is an example of building a Restful Service with .NET. It also illustrates some issues with testing an MVC web application. The code was written for students taking the SEM5640 module.

The repository contains a Visual Studio solution with three projects. This code was written in Visual Studio 2017 with C# using .NET Core 2.0.

**Note** the code is currently incomplete and has a few issues. However, it does illustrate the main points for creating a RESTful service in .Net and for consuming that service using HttpClient.

# Projects

The three C# projects are:

* **DotNetRestful** - an example service providing information about employees.
* **DotNetRestfulTests** - an example of tests for a controller and the model classes in DotNetRestfulWebClient MVC application.
* **DotNetRestfulWebClient** - an MVC project that accesses data from the DotNetRestful service.

## DotNetRestful
This class includes a service, which is defined in Controllers/EmployeesController. This contains the REST service with the following operations:

* **Get** - at /api/employees - This will return a list of three Employees.
* **Get with Id** - at /api/employees/{id} - This will return a single Employee that matches the specified Id. If there isn't a match for the Id, a BadRequest is returned.
* **Post** - at /api/employees - This will create a new Employee record. The operation will specify an Id for the new record. A path to Get the Employee details can be accessed.
* **Put** - at /api/employees/{id} - This will either create a new employee or update an existing employee. If there is an employee with the specified Id, this will update the record. If there is no matching employee, a new employee will be created.
* **Delete** - at /api/employees/{id} - This will attempt to delete the Employee record for the specified Id.

The service uses an static list on the class. Whilst that is not appropriate for a real application, it is sufficient for this example. That means that the data will be reset to the initial data when it is restarted. The data is created by the constructor. This might be better created by a static initialiser, but that would be a future change to this code.

### Data Classes
This project uses two data classes, which are defined in the `Models` folder. These are:

* **WebApiEmployee** - This contains a number, representing the employee number, a name and an address.
* **WebApiAddress** - This represents the address infomration, including a house number, a street name, a city and a postcode. It is used within the `WebApiEmployee`.

The `WebApi` prefix is only used to differentiate the classes from equivalent classes in the `DotNetRestfulWebClient` project.

### Port Number
The client code, in `DotNetRestfulWebClient` assumes that the service is running at localhost:50727. However, when loading this on to a new machine and running it, Visual Studio did choose a different port number. You can either update the code in `DotNetRestfulWebClient` to show a changed port number, or you can set this in the Build section of the Properties for the `DotNetRestful` project.

### Running the project
To run the project, you could use Ctrl-F5. This will start the project without linking the debugger. That means that it can run whilst you start and stop the the `DotNetRestfulWebClient`.

When it is running, you should be able to navigate to `http://localhost:50727/api/employees`, which should return a JSON array with employee data.

## DotNetRestfulWebClient
This is an MVC project. The default controller, HomeController, is left in the project, but it is not used. There are two controllers that are notable for the project:

* **EmployeeController** - This is an example of using the HttpClient class to make requests to a remote service. Instead of loading data from a local store, each of the operations interacts with the operations in `DotNetRestful`.

* **EmployeeWithInjectionController** - This is an equivalent implementation of the `EmployeeController`, but it uses Dependency Injection to provide a layer of separation between the logic in the controller and the code that interacts with the Restful service. Within this project, it should behave the same as if running the EmployeeController. The benefit of this approach is that we can switch in alternative implementations of the service for testing purposes. More is said about this below and in the discussion of the `DotNetRestfulTests` project.

### HttpClient
The project uses enhancements to the basic HttpClient so that JSON can be processed. The project includes `Microsoft.AspNet.WebApi.Client`  package, loaded by the NuGet package manager, to make this enhancement.

### Dependency Injection
Dependency Injection is where we allow an external piece of code to inject resources into the code; for a discussion related to .Net, you can read [Introduction to Dependency Injection in ASP.NET Core](https://docs.microsoft.com/en-gb/aspnet/core/fundamentals/dependency-injection).

In this example, we are using constructor injection. This is where the constructor specifies arguments that state services that it needs to work. The .Net runtime identifies possible matches for these services and provides objects of the required type at runtime. Rather than the `EmployeeController` creating its own Logger class, it can request that it needs one via its constructor. The Runtime can provide a logger object that can then be used by the `EmployeeController`.

In the `EmployeeWithInjectionController` we see that the constructor requests a logger and an `IRestfulClientService`. An implementation of the `IRestfulClientService` will provide the way to interact with the remote service. An implementation is provided in `RestfulClientService`. Both of these classes are found in the `Services` folder.

To register the `IRestfulClientService`, we add the following line into `Startup.cs` in the `ConfigureServices` method.

    services.AddTransient<IRestfulClientService, RestfulClientService>();

The ASP.NET Core code already knows how to create a logger service. However, there is still a small amount of configuration to do, as mentioned in the following section.

### Logging
This project also makes limited use of a logger. There are various logging solutions available on the .Net platform. This example is using Microsoft's default Logging facilities; these are described in [Introduction to Logging in ASP.NET Core](https://docs.microsoft.com/en-gb/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x).

Request an instance of a Logger for the class, by adding code like the following for the constructor.

    public EmployeeController(ILogger<EmployeeController> logger) { ... }
    
Within the constructor, store a reference to the logger for use in the class.

The logger can then be used to generate messages, e.g. `logger.LogInformation()`.

To configure where the log messages are output, you can add .ConfigureLogging when buliding the WebHost in `Program.cs`.  The following code sets the logger to output to the Console and to the Debug stream. You can view the Debug stream in Visual Studio when the application is running in debug mode.

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .ConfigureLogging((hostingContext, logging) =>
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
            logging.AddDebug();
        })
        .UseStartup<Startup>()
        .Build();


## DotNetRestfulTests
This project illustrates the use of [MSTest](https://docs.microsoft.com/en-gb/dotnet/core/testing/unit-testing-with-mstest). To add tests, create a class for your test methods.

Each test class needs to have a `[TestClass]` attribute, from the `Microsoft.VisualStudio.TestTools.UnitTesting` namespace. Note that the class needs to be public. If it isn't, the MSTest framework won't find it.

Each method that contains the tests nees to have a `[TestMethod]` attribute.  Assertions within the test methods come from the `Assert` class, e.g. `Assert.AreSame()` and `Assert.IsNull()`.

The project contains the following test classes. Note that the tests aren't complete for any of the classes. The examples have been chosen to show some examples of testing. These tests are checking the code in the `DotNetRestfulWebClient` project.

* **ClientAddressTests** - Example tests for a data class in the application.
* **ClientEmployeeTests** - Example tests for a data class in the application.
* **EmployeeControllerTests** - Example tests for the `EmployeeController`.  This is the controller that directly calls the remote service. This means that the tests require that the `DotNetRestful` project is running.
* **EmployeeWithInjectionTests** - Example tests for the `EmployeeWithInjectionController`. This makes use of alternative implementations for the `IRestfulClientService`. This means that the controller can be setup for testing purposes to call a Test Stub, which can allow tests that do not require the remote service to be running.

To support the `EmployeeWithInjectionTests`, two test stubs are created. Both of these conform to the `IRestfulClientService` interface.

`RestfulClientServiceStub`, the first stub, will return 'good' data. It currently only provides code for the `Employees()` operation, but could be extended to add the rest of the normal implementations.

`RestfulClientServiceErrorStub` is used to enable testing of the error conditions. The example provided is that when the `Employees()` operation is called, the value null is returned. This makes it possible to test what the controller does if it receives null when trying to contact the remote service.

We pass these stubs into the constructors for `EmployeeWithInjectionController` in the tests in `EmployeeWithInjectionTests` . An example is show below.

    [TestMethod]
    public async Task ShouldShowListOfEmployees()
    {
        RestfulClientServiceStub clientService = new RestfulClientServiceStub();
        EmployeeWithInjectionController controller = new EmployeeWithInjectionController(logger, clientService);
        var result = await controller.Index() as ViewResult;
        Assert.AreEqual("Index", result.ViewName);

        List<ClientEmployee> employees = result.Model as List<ClientEmployee>;
        Assert.AreEqual(2, employees.Count);
    }

### Tasks
Notice that the methods in the stubs include code that we have not covered in the practicals.


The `Employee()` method has the following signature, which is specified in the `IRestfulClientService`.

    public Task<List<ClientEmployee>> Employees() { ... }

This states that there is a return type of `List<ClientEmployee>`. This is wrapped up in a `Task`. The use of the Task is necessary because the code in the `DotNetRestfulWebClient` project is calling the remote service asynchronously.  However, we don't make asynchronous calls to the remote service in these test stubs.

We can't just return a list of Employees, because that is not the required return type. Therefore, we need to wrap the `List<ClientEmployee>` in a new Task. We then need to start that task so that it runs to completion. The result of that task is then returned to the calling code. In this project, that means we are able to return the list of employees to the test.

The mechanism to wrap up the data and return it from a Task is shown below.

    Task<List<ClientEmployee>> task = new Task<List<ClientEmployee>>(() => employees);
    task.Start();
    return task;

Whilst the use of stubs does complicate the tests, it does make it easier for use to pass different implementations of a service to the `EmployeeWithInjectionController` class.  This allows us to focus tests on the logic in the controller. We will still want some tests to confirm that we are accessing the remote service correctly, but we also want ways to check that the logic of the controller handles things like error situations. Simulating those error situations directly from the remote service can be harder.





