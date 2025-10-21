\# MiniDeepThought



This project is a console application given as an exercise for the Book Club Quiz #42, and it is meant to represent an "oracle" that responds to every question with "42". As an exercise it showcases xUnit testing, JSON file reading/writing, asynchronous programming and the use of the Strategy design pattern.



\## Folder Structure



The solution is organized as follows:



MiniDeepThought/

│

├─ MiniDeepThought.sln# MiniDeepThought



\## Folder Structure



The solution is organized as follows:



MiniDeepThought/

│

├─ MiniDeepThought.sln

├─ src/

│ └─ MiniDeepThought/

│ ├─ MiniDeepThought.csproj

│ ├─ Program.cs

│ ├─ Domain/

│ │ ├─ Job.cs

│ │ └─ JobResult.cs

│ ├─ Strategies/

│ │ ├─ IAnswerStrategy.cs

│ │ ├─ TrivialStrategy.cs

│ │ ├─ SlowCountStrategy.cs

│ │ └─ RandomGuessStrategy.cs

│ ├─ Services/

│ │ ├─ JobRunner.cs

│ │ └─ JobStore.cs

│ └─ Util/

│ └─ ConsoleHelpers.cs

│ └─ JobStatus.cs

└─ tests/

└─ MiniDeepThought.Tests/

├─ MiniDeepThought.Tests.csproj

├─ TrivialStrategyTests.cs

├─ SlowCountStrategyTests.cs

├─ RandomGuessStrategyTests.cs

└─ JobStoreTests.cs



---



\## Project Setup



To set up the MiniDeepThought solution from scratch, follow these steps:



1\. Open a terminal in your workspace folder.

2\. Create a new solution:



dotnet new sln -n MiniDeepThought

Create the console application project:



dotnet new console -o src/MiniDeepThought

Create the xUnit test project:



dotnet new xunit -o tests/MiniDeepThought.Tests

Add both projects to the solution:


dotnet sln add src/MiniDeepThought/MiniDeepThought.csproj

dotnet sln add tests/MiniDeepThought.Tests/MiniDeepThought.Tests.csproj

If Visual Studio prompts you with “The solution was modified outside the environment. Reload or Ignore?”, click Ignore. The projects are already added to the solution file.



Make the test project reference the console app:



dotnet add tests/MiniDeepThought.Tests/MiniDeepThought.Tests.csproj reference src/MiniDeepThought/MiniDeepThought.csproj

Create the folder structure inside the console app project:



src/MiniDeepThought/

│

├─ Domain/

├─ Strategies/

├─ Services/

└─ Util/

Create corresponding empty .cs files in each folder for your classes and strategies.



Build, Run, and Test

Once the solution is set up:



Build the solution from the root folder:



dotnet clean

dotnet restore

dotnet build

Run the console application:



dotnet run --project src/MiniDeepThought/MiniDeepThought.csproj

Run all tests:



dotnet test

Run tests for a specific project:



dotnet test tests/MiniDeepThought.Tests/MiniDeepThought.Tests.csproj

Run a single test (optional) using the fully qualified test name:



dotnet test --filter FullyQualifiedName~Namespace.ClassName.MethodName

Ensure all test methods are public and decorated with \[Fact] or \[Theory] for xUnit discovery.





