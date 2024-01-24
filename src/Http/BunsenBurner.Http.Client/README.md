<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Http Client

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Http.Jwt)](https://www.nuget.org/packages/BunsenBurner.Http.Jwt/)

## Getting Started

To use this library, simply include `BunsenBurner.Http.Client.dll` in your
project or grab it
from [NuGet](https://www.nuget.org/packages/BunsenBurner.Http.Client/),
and add this to the top of each test `.cs` file that needs it:

```C#
using BunsenBurner.Http;
```

## What?

This provides a way to test `HttpClient` using a `HttpMessageStore`.

Expected request/response matchers are setup against the store and this allow
for assertions to be made against that know set of responses.

As long as the services is using the `IHttpClientFactory` owned `HttpClient`
instances this will work.

TODO: Example here

For more examples check out the test project, create an issue or start a
discussion.
