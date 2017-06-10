Contributing
============

- Follow the current coding style which is [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions) with some additions (for example classes fields names begins with `_`) .
- Follow the naming conventions in case of new assembly, new namespace [Microsoft Naming conventions](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-namespaces).
- Follow all rules in the [Framework design principles](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/)
- Follow SOLID and KISS principles when architecturing new classes.
- If a class implements a known design pattern, the pattern name must be in the class name at the end.
- Every external contribution must be done a branch different from master.
- Every new class should be unit tested. Coverage of 100 % is not required but should be a target.
- Do not instiate directly other classes in implementation. Instead use IocConfiguration class to bind implementations to interfaces and use interfaces in implementations classes.
- Use Moq to mock the interfaces in unit tests.
