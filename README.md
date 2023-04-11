# SecTester SDK Demo

## Table of contents

- [About this project](#about-this-project)
- [About SecTester](#about-sectester)
- [Setup](#setup)
  - [Fork and clone this repo](#fork-and-clone-this-repo)
  - [Get a Bright API key](#get-a-bright-api-key)
  - [Explore the demo application](#explore-the-demo-application)
  - [A full configuration example](#a-full-configuration-example)
  - [Recommended tests](#recommended-tests)
  - [Example of a CI configuration](#example-of-a-ci-configuration)
- [Documentation & Help](#documentation--help)
- [Contributing](#contributing)
- [License](#license)

## About this project

This is a demo project for the [SecTester NET SDK framework](https://github.com/NeuraLegion/sectester-net), with some installation and usage examples. We recommend forking it and playing around, that’s what it’s for!

## About SecTester

Bright is a developer-first Dynamic Application Security Testing (DAST) scanner.

SecTester is a new tool that integrates our enterprise-grade scan engine directly into your unit tests.

With SecTester you can:

- Test every function and component directly
- Run security scans at the speed of unit tests
- Find vulnerabilities with no false positives, before you finalize your Pull Request

Trying out Bright’s SecTester is _**free**_ 💸, so let’s get started!

> ⚠️ **Disclaimer**
>
> The SecTester project is currently in beta as an early-access tool. We are looking for your feedback to make it the best possible solution for developers, aimed to be used as part of your team’s SDLC. We apologize if not everything will work smoothly from the start, and hope a few bugs or missing features will be no match for you!
>
> Thank you! We appreciate your help and feedback!

## Setup

### Fork and clone this repo

1.  Press the ‘fork’ button to make a copy of this repo in your own GH account
2.  In your forked repo, clone this project down to your local machine using either SSH or HTTP

### Get a Bright API key

1.  Register for a free account at Bright’s [**signup**](https://app.neuralegion.com/signup) page
2.  Optional: Skip the quickstart wizard and go directly to [**User API key creation**](https://app.neuralegion.com/profile)
3.  Create a Bright API key ([**check out our doc on how to create a user key**](https://docs.brightsec.com/docs/manage-your-personal-account#manage-your-personal-api-keys-authentication-tokens))
4.  Save the Bright API key
5.  We recommend using your Github repository secrets feature to store the key, accessible via the `Settings > Security > Secrets > Actions` configuration. We use the ENV variable called `BRIGHT_TOKEN` in our examples
6.  If you don’t use that option, make sure you save the key in a secure location. You will need to access it later on in the project but will not be able to view it again.
7.  More info on [**how to use ENV vars in Github actions**](https://docs.github.com/en/actions/learn-github-actions/environment-variables)

> ⚠️ Make sure your API key is saved in a location where you can retrieve it later! You will need it in these next steps!

### Explore the demo application

Navigate to your local version of this project. Then, in your command line, install the dependencies:

```bash
$ dotnet restore --use-lock-file
```

The whole list of required variables to start the demo application is described in `.env.example` file. The template for this .env file is available in the root folder.

After that, you can easily create a `.env` file from the template by issuing the following command:

```bash
$ cp .env.example .env
```

Once this template is done, copying over (should be instantaneous), navigate to your `.env` file, and paste your Bright API key as the value of the `BRIGHT_TOKEN` variable.

```text
BRIGHT_TOKEN = <your_API_key_here>
```

Then you have to build and run services with Docker. Start Docker, and issue the command as follows:

```bash
$ docker compose up -d
```

To initialize DB schema, you should execute a migration, as shown here:

```bash
$ dotnet ef database update --project src/App
```

Finally, perform this command in terminal to run the application:

```bash
$ dotnet run --project src/App
```

While having the application running, open a browser and type `http://localhost:3000/swagger`, and hit enter.
You should see the Swagger UI page for that application that allows you to test the RESTFul CRUD API, like in the following screenshot:

![Swagger UI](https://user-images.githubusercontent.com/38690835/207978865-12fe2292-b7e5-461c-a3b9-9a7a03bc7f12.png)

To explore the Swagger UI:

- Click on the `POST /users` endpoint
- Click on the "Try it out" button
- Click on the blue "Execute" button
- Then you should see a view similar to the following, where you can see the JSON returned from the API:

![Swagger UI](https://user-images.githubusercontent.com/38690835/207978869-4581e1e4-e0d7-4b08-bf23-a49b58d50688.png)

Then you can start tests with SecTester against these endpoints as follows (make sure you use a new terminal window, as the original is still running the API for us!)

```bash
$ dotnet test -c Debug --nologo --filter FullyQualifiedName~SecurityTests
```

> You will find tests written with SecTester in the `./test/sec` folder.

This can take a few minutes, and then you should see the result, like in the following screenshot:

```text
[xUnit.net 00:06:47.46]     App.SecurityTests.AppTests.Get_Users_ShouldNotHaveSqli [FAIL]
  Failed App.SecurityTests.AppTests.Get_Users_ShouldNotHaveSqli [3 m 35 s]
  Error Message:
   SecTester.Runner.IssueFound : Target is vulnerable

Issue in Bright UI:   https://development.playground.neuralegion.com/scans/kPzCzYEEVqtqqi8K13x9zY/issues/5YwVwZpcKQNgPpX2YP6XEb
Name:                 SQL Injection
Severity:             High
Remediation:
If available, use structured mechanisms that automatically enforce the separation between data and code. These mechanisms may be able to provide the relevant quoting, encoding, and validation automatically, instead of relying on the developer to provide this capability at every point where output is generated. Process SQL queries using prepared statements, parameterized queries, or stored procedures. These features should accept parameters or variables and support strong typing. Do not dynamically construct and execute query strings within these features using 'exec' or similar functionality, since this may re-introduce the possibility of SQL injection
Details:
A SQL Injection attack consists of inserting or injecting a SQL query via the input data received by the application from the client.
A successful SQL injection exploit can read sensitive data from the database, modify database data (Insert/Update/Delete), execute administration operations on the database (such as shutdown the DBMS), recover the content of a given file present on the DBMS file system and in some cases issue commands to the operating system.
SQL injection attacks are a type of injection attack, in which SQL commands are injected into data-plane input in order to affect the execution of predefined SQL commands.
Attack Vector Information:
Attacked Parameter:
Attacked Parameter Type: MultiParse::DataType::String
Attacked Parameter Location: Query
Triggered Using Token: '
Parameter Encoding: [:none]
Extra Details:
● Injection Points
        Parameter: #1* (URI)
            Type: boolean-based blind
            Title: AND boolean-based blind - WHERE or HAVING clause (subquery - comment)
            Payload: http://127.0.0.1:56581/Users?name=%' AND 4552=(SELECT (CASE WHEN (4552=4552) THEN 4552 ELSE (SELECT 7744 UNION SELECT 3614) END))-- LRbn

            Type: error-based
            Title: PostgreSQL AND error-based - WHERE or HAVING clause
            Payload: http://127.0.0.1:56581/Users?name=%' AND 3650=CAST((CHR(113)||CHR(122)||CHR(118)||CHR(107)||CHR(113))||(SELECT (CASE WHEN (3650=3650) THEN 1 ELSE 0 END))::text||(CHR(113)||CHR(98)||CHR(122)||CHR(106)||CHR(113)) AS NUMERIC) AND 'Mlpq%'='Mlpq

            Type: stacked queries
            Title: PostgreSQL > 8.1 stacked queries (comment)
            Payload: http://127.0.0.1:56581/Users?name=%';SELECT PG_SLEEP(5)--

            Type: time-based blind
            Title: PostgreSQL > 8.1 AND time-based blind
            Payload: http://127.0.0.1:56581/Users?name=%' AND 1123=(SELECT 1123 FROM PG_SLEEP(5)) AND 'aZdM%'='aZdM
        Database Banner: 'postgresql 14.6 on x86_64-pc-linux-musl, compiled by gcc (alpine 11.2.1_git20220219) 11.2.1 20220219, 64-bit'

References:
● https://cwe.mitre.org/data/definitions/89.html
● https://www.owasp.org/index.php/SQL_Injection
● https://www.neuralegion.com/blog/sql-injection-sqli/
● https://kb.neuralegion.com/#/guide/vulnerabilities/3-sql-injection.md
  Stack Trace:
     at SecTester.Runner.SecScan.Assert(IScan scan) in /home/runner/work/sectester-net/sectester-net/src/SecTester.Runner/SecScan.cs:line 64
   at SecTester.Runner.SecScan.Run(Target target, CancellationToken cancellationToken) in /home/runner/work/sectester-net/sectester-net/src/SecTester.Runner/SecScan.cs:line 36
   at SecTester.Runner.SecScan.Run(Target target, CancellationToken cancellationToken) in /home/runner/work/sectester-net/sectester-net/src/SecTester.Runner/SecScan.cs:line 40
   at SecTester.Runner.SecScan.Run(Target target, CancellationToken cancellationToken) in /home/runner/work/sectester-net/sectester-net/src/SecTester.Runner/SecScan.cs:line 40
   at App.SecurityTests.AppTests.Get_Users_ShouldNotHaveSqli() in /Users/Projects/sectester-net-demo/test/App.SecurityTests/AppTests.cs:line 65
--- End of stack trace from previous location ---

Failed!  - Failed:     1, Passed:     1, Skipped:     0, Total:     2, Duration: 4 m 5 s - /Users/artemderevnjuk/Projects/sectester-net-demo/test/App.SecurityTests/bin/Debug/net6.0/App.SecurityTests.dll (net6.0)
```

### A full configuration example

Now you will look under the hood to see how this all works. In the following example, we will test the app we just set up for any instances of SQL injection. [xUnit](https://xunit.net/) is provided as the testing framework, that provides assert functions and excellent extensibility of test classes and test methods.

To start the webserver within the same process with tests, not in a remote environment or container, we use ASP.NET [testing utilities](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0). You don’t have to use ASP.NET, but it is what we chose for this project. The code is as follows:

[`test/App.SecurityTests/AppTests.cs`](./test/App.SecurityTests/AppTests.cs)

```csharp
public class AppFixture : WebApplicationFactory<Program>
{
  private IHost _host;

  protected override void ConfigureWebHost(IWebHostBuilder builder) =>
    builder.ConfigureAppConfiguration(cb => cb.AddJsonFile("appsettings.json", true)
        .AddEnvironmentVariables())
      .ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
      .UseSolutionRelativeContentRoot("")
      .UseKestrel()
      .UseUrls("http://127.0.0.1:0");

  protected override IHost CreateHost(IHostBuilder builder)
  {
    var testHost = builder.Build();

    builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

    // See https://github.com/dotnet/aspnetcore/issues/33846.
    _host = builder.Build();

    _host.Start();

    var server = _host.Services.GetRequiredService<IServer>();
    var addresses = server.Features.Get<IServerAddressesFeature>();

    ClientOptions.BaseAddress = addresses!.Addresses
      .Select(x => new Uri(x))
      .Last();

    // See https://github.com/dotnet/aspnetcore/pull/34702.
    testHost.Start();
    return testHost;
  }
}
```

The [`SecTester.Runner`](https://github.com/NeuraLegion/sectester-net/tree/master/src/SecTester.Runner) project provides a set of utilities that allows scanning the demo application for vulnerabilities. Let's expand the previous example using the built-in `SecRunner` class:

```csharp
public class AppTests : IClassFixture<AppFixture>, IAsyncLifetime
{
  private readonly Configuration _config = new("app.brightsec.com");
  private readonly AppFixture _fixture;
  private SecRunner _runner;

  public AppTests(AppFixture fixture)
  {
    _fixture = fixture;
  }

  public async Task InitializeAsync()
  {
    _runner = await SecRunner.Create(_config);
    await _runner.Init();
  }

  public async Task DisposeAsync()
  {
    await _runner.DisposeAsync();
    GC.SuppressFinalize(this);
  }
}
```

To set up a runner, create a `SecRunner` instance on the top of the file, passing a configuration as follows:

```csharp
using SecTester.Runner;

await using var runner = await SecRunner.Create(_config);
```

After that, you have to initialize a `SecRunner` instance:

```csharp
await runner.Init();
```

The runner is now ready to perform your tests. To start scanning your endpoint, first, you have to create a `SecScan` instance. We do this with `runner.CreateScan` as shown in the example below.

Now, you will write and run your first unit test!

Let's verify the `GET /Users` endpoint for SQLi:

```csharp
public class AppTests : IClassFixture<AppFixture>, IAsyncLifetime
{
  // ...
  [Fact]
  public async Task Get_Users_ShouldNotHaveSqli()
  {
    var target = new Target($"{_fixture.Url}/Users")
      .WithQuery(new Dictionary<string, string> { { "name", "Test" } });

    var builder = new ScanSettingsBuilder()
      .WithTests(new List<TestType> { TestType.Sqli });

    await _runner
      .CreateScan(builder)
      .Run(target);
  }
}
```

This will raise an exception when the test fails, with remediation information and a deeper explanation of SQLi, right in your command line!

Let's look at another test for the `POST /users` endpoint, this time for XSS.

```csharp
public class AppTests : IClassFixture<AppFixture>, IAsyncLifetime
{
  // ...
  [Fact]
  public async Task Post_Users_ShouldNotHaveXss()
  {
    var content = JsonContent.Create(new { Name = "Test" },
      options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

    var target = new Target($"{_fixture.Url}/Users")
      .WithMethod(HttpMethod.Post)
      .WithBody(content);

    var builder = new ScanSettingsBuilder()
      .WithTests(new List<TestType> { TestType.Xss });

    await _runner
      .CreateScan(builder)
      .Run(target);
  }
}
```

As you can see, writing a new test for XSS follows the same pattern as SQLi.

Finally, to run a scan against the endpoint, you have to obtain a port to which the server listens. For that, we should adjust the example above just a bit:

```csharp
public class AppFixture : WebApplicationFactory<Program>
{
  private readonly Lazy<Uri> _urlInitializer;

  // ...

  public AppFixture()
  {
    _urlInitializer = new Lazy<Uri>(GetUrl);
  }

  public Uri Url => _urlInitializer.Value;

  // ...

  private Uri GetUrl()
  {
    EnsureServer();
    return ClientOptions.BaseAddress;
  }

  private void EnsureServer()
  {
    if (_host is null)
    {
      // This forces WebApplicationFactory to bootstrap the server
      using var _ = CreateDefaultClient();
    }
  }
}
```

Now, you can use the `Url` to set up a target:

```csharp
var target = new Target($"{_fixture.Url}/Users")
  .WithMethod(HttpMethod.Post)
  .WithBody(content);
```

By default, each found issue will cause the scan to stop. To control this behavior you can set a severity threshold using the `Threshold` method. Since SQLi (SQL injection) is considered to be high severity issue, we can pass `Severity.High` for stricter checks:

```ts
scan.Threshold(Severity.High);
```

To avoid long-running test, you can specify a timeout, to say how long to wait before aborting it:

```ts
scan.Timeout(TimeSpan.FromMinutes(5));
```

To clarify an attack surface and speed up the test, we suggest making clear where to discover parameters according to the source code.

[`src/App/Controllers/UsersController.cs`](./src/App/Controllers/UsersController.cs)

```csharp
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
  // ...

  [HttpGet]
  [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
  public Task<List<User>> FindByName([FromQuery] string name) => _users.FindByName(name);
}
```

For the example above, it should look like this:

```csharp
var builder = new ScanSettingsBuilder()
  .WithAttackParamLocations(new List<AttackParamLocation>
  {
    AttackParamLocation.Query
  })
  .WithTests(new List<TestType> { TestType.Sqli });
```

Finally, the test should look like this:

```csharp
var target = new Target($"{_fixture.Url}/Users")
  .WithMethod(HttpMethod.Get)
  .WithQuery(new Dictionary<string, string> { { "name", "Test" } });

var builder = new ScanSettingsBuilder()
  .WithAttackParamLocations(new List<AttackParamLocation>
  {
    AttackParamLocation.Query
  })
  .WithTests(new List<TestType> { TestType.Sqli });

await _runner
  .CreateScan(builder)
  .Threshold(Severity.High)
  .Run(target);
```

Here is a completed `test/App.SecurityTests/AppTests.cs` file with all the tests and configuration set up.

```csharp
namespace App.SecurityTests;

public class AppTests : IClassFixture<AppFixture>, IAsyncLifetime
{
  private readonly Configuration _config = new("app.brightsec.com");
  private readonly AppFixture _fixture;
  private SecRunner _runner;

  public AppTests(AppFixture fixture)
  {
    _fixture = fixture;
  }

  public async Task InitializeAsync()
  {
    _runner = await SecRunner.Create(_config);
    await _runner.Init();
  }

  public async Task DisposeAsync()
  {
    await _runner.DisposeAsync();
    GC.SuppressFinalize(this);
  }

  [Fact]
  public async Task Post_Users_ShouldNotHaveXss()
  {
    var content = JsonContent.Create(new { Name = "Test" },
      options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

    var target = new Target($"{_fixture.Url}/Users")
      .WithMethod(HttpMethod.Post)
      .WithBody(content);

    var builder = new ScanSettingsBuilder()
      .WithAttackParamLocations(new List<AttackParamLocation>
      {
        AttackParamLocation.Body
      })
      .WithTests(new List<TestType> { TestType.Xss });

    await _runner
      .CreateScan(builder)
      .Threshold(Severity.Medium)
      .Run(target);
  }

  [Fact]
  public async Task Get_Users_ShouldNotHaveSqli()
  {
    var target = new Target($"{_fixture.Url}/Users")
      .WithMethod(HttpMethod.Get)
      .WithQuery(new Dictionary<string, string> { { "name", "Test" } });

    var builder = new ScanSettingsBuilder()
      .WithAttackParamLocations(new List<AttackParamLocation>
      {
        AttackParamLocation.Query
      })
      .WithTests(new List<TestType> { TestType.Sqli });

    await _runner
      .CreateScan(builder)
      .Threshold(Severity.High)
      .Run(target);
  }
}
```

Full documentation can be found in the [`SecTester.Runner`](https://github.com/NeuraLegion/sectester-net/tree/master/src/SecTester.Runner) README.

### Recommended tests

|                                                                                  |                                                                                                                                              |                              |                                                                                                                                                                                                                                                                                                                                                                                                                              |
| -------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Test name**                                                                    | **Description**                                                                                                                              | **Usage in SecTester**       | **Detectable vulnerabilities**                                                                                                                                                                                                                                                                                                                                                                                               |
| **Amazon S3 Bucket Takeover**                                                    | Tests for S3 buckets that no longer exist to prevent data breaches and malware distribution                                                  | `amazon_s3_takeover`         | - [Amazon S3 Bucket Takeover](https://docs.brightsec.com/docs/amazon-s3-bucket-take-over)                                                                                                                                                                                                                                                                                                                                    |
| **Broken JWT Authentication**                                                    | Tests for secure implementation of JSON Web Token (JWT) in the application                                                                   | `jwt`                        | - [Broken JWT Authentication](https://docs.brightsec.com/docs/broken-jwt-authentication)                                                                                                                                                                                                                                                                                                                                     |
| **Broken SAML Authentication**                                                   | Tests for secure implementation of SAML authentication in the application                                                                    | `broken_saml_auth`           | - [Broken SAML Authentication](https://docs.brightsec.com/docs/broken-saml-authentication)                                                                                                                                                                                                                                                                                                                                   |
| **Brute Force Login**                                                            | Tests for availability of commonly used credentials                                                                                          | `brute_force_login`          | - [Brute Force Login](https://docs.brightsec.com/docs/brute-force-login)                                                                                                                                                                                                                                                                                                                                                     |
| **Business Constraint Bypass**                                                   | Tests if the limitation of number of retrievable items via an API call is configured properly                                                | `business_constraint_bypass` | - [Business Constraint Bypass](https://docs.brightsec.com/docs/business-constraint-bypass)                                                                                                                                                                                                                                                                                                                                   |
| **Client-Side XSS** <br>_(DOM Cross-Site Scripting)_                             | Tests if various application DOM parameters are vulnerable to JavaScript injections                                                          | `dom_xss`                    | - [Reflective Cross-site scripting (rXSS)](https://docs.brightsec.com/docs/reflective-cross-site-scripting-rxss)<br> <br> - [Persistent Cross-site scripting (pXSS)](https://docs.brightsec.com/docs/persistent-cross-site-scripting-pxss)                                                                                                                                                                                   |
| **Common Files Exposure**                                                        | Tests if common files that should not be accessible are accessible                                                                           | `common_files`               | - [Exposed Common File](https://docs.brightsec.com/docs/exposed-common-file)                                                                                                                                                                                                                                                                                                                                                 |
| **Cookie Security Check**                                                        | Tests if the application uses and implements cookies with secure attributes                                                                  | `cookie_security`            | - [Sensitive Cookie in HTTPS Session Without Secure Attribute](https://docs.brightsec.com/docs/sensitive-cookie-in-https-session-without-secure-attribute)<br> <br> - [Sensitive Cookie Without HttpOnly Flag](https://docs.brightsec.com/docs/sensitive-cookie-without-httponly-flag)<br> <br>- [Sensitive Cookie Weak Session ID](https://docs.brightsec.com/docs/sensitive-cookie-weak-session-id)                        |
| **Cross-Site Request Forgery (CSRF)**                                            | Tests application forms for vulnerable cross-site filling and submitting                                                                     | `csrf`                       | - [Unauthorized Cross-Site Request Forgery (CSRF)](https://docs.brightsec.com/docs/unauthorized-cross-site-request-forgery-csrf)<br> <br> - [Authorized Cross-Site Request Forgery (CSRF)](https://docs.brightsec.com/docs/authorized-cross-site-request-forgery-csrf)                                                                                                                                                       |
| **Cross-Site Scripting (XSS)**                                                   | Tests if various application parameters are vulnerable to JavaScript injections                                                              | `xss`                        | - [Reflective Cross-Site Scripting (rXSS)](https://docs.brightsec.com/docs/reflective-cross-site-scripting-rxss)<br> <br> - [Persistent Cross-Site Scripting (pXSS)](https://docs.brightsec.com/docs/persistent-cross-site-scripting-pxss)                                                                                                                                                                                   |
| **Default Login Location**                                                       | Tests if login form location in the target application is easy to guess and accessible                                                       | `default_login_location`     | - [Default Login Location](https://docs.brightsec.com/docs/default-login-location)                                                                                                                                                                                                                                                                                                                                           |
| **Directory Listing**                                                            | Tests if server-side directory listing is possible                                                                                           | `directory_listing`          | - [Directory Listing](https://docs.brightsec.com/docs/directory-listing)                                                                                                                                                                                                                                                                                                                                                     |
| **Email Header Injection**                                                       | Tests if it is possible to send emails to other addresses through the target application mailing server, which can lead to spam and phishing | `email_injection`            | - [Email Header Injection](https://docs.brightsec.com/docs/email-header-injection)                                                                                                                                                                                                                                                                                                                                           |
| **Exposed AWS S3 Buckets Details** <br>_(Open Buckets)_                          | Tests if exposed AWS S3 links lead to anonymous read access to the bucket                                                                    | `open_buckets`               | - [Exposed AWS S3 Buckets Details](https://docs.brightsec.com/docs/open-bucket)                                                                                                                                                                                                                                                                                                                                              |
| **Exposed Database Details** <br>_(Open Database)_                               | Tests if exposed database connection strings are open to public connections                                                                  | `open_buckets`               | - [Exposed Database Details](https://docs.brightsec.com/docs/open-database)<br> <br> - [Exposed Database Connection String](https://docs.brightsec.com/docs/exposed-database-connection-string)                                                                                                                                                                                                                              |
| **Full Path Disclosure (FPD)**                                                   | Tests if various application parameters are vulnerable to exposure of errors that include full webroot path                                  | `full_path_disclosure`       | - [Full Path Disclosure](https://docs.brightsec.com/docs/full-path-disclosure)                                                                                                                                                                                                                                                                                                                                               |
| **Headers Security Check**                                                       | Tests for proper Security Headers configuration                                                                                              | `header_security`            | - [Misconfigured Security Headers](https://docs.brightsec.com/docs/misconfigured-security-headers)<br> <br> - [Missing Security Headers](https://docs.brightsec.com/docs/missing-security-headers)<br> <br>- [Insecure Content Secure Policy Configuration](https://docs.brightsec.com/docs/insecure-content-secure-policy-configuration)                                                                                    |
| **HTML Injection**                                                               | Tests if various application parameters are vulnerable to HTML injection                                                                     | `html_injection`             | - [HTML Injection](https://docs.brightsec.com/docs/html-injection)                                                                                                                                                                                                                                                                                                                                                           |
| **Improper Assets Management**                                                   | Tests if older or development versions of API endpoints are exposed and can be used to get unauthorized access to data and privileges        | `improper_asset_management`  | - [Improper Assets Management](https://docs.brightsec.com/docs/improper-assets-management)                                                                                                                                                                                                                                                                                                                                   |
| **Insecure HTTP Method** <br>_(HTTP Method Fuzzer)_                              | Tests enumeration of possible HTTP methods for vulnerabilities                                                                               | `http_method_fuzzing`        | - [Insecure HTTP Method](https://docs.brightsec.com/docs/insecure-http-method)                                                                                                                                                                                                                                                                                                                                               |
| **Insecure TLS Configuration**                                                   | Tests SSL/TLS ciphers and configurations for vulnerabilities                                                                                 | `insecure_tls_configuration` | - [Insecure TLS Configuration](https://docs.brightsec.com/docs/insecure-tls-configuration)                                                                                                                                                                                                                                                                                                                                   |
| **Known JavaScript Vulnerabilities** <br>_(JavaScript Vulnerabilities Scanning)_ | Tests for known JavaScript component vulnerabilities                                                                                         | `retire_js`                  | - [JavaScript Component with Known Vulnerabilities](https://docs.brightsec.com/docs/javascript-component-with-known-vulnerabilities)                                                                                                                                                                                                                                                                                         |
| **Known WordPress Vulnerabilities** <br>_(WordPress Scan)_                       | Tests for known WordPress vulnerabilities and tries to enumerate a list of users                                                             | `wordpress`                  | - [WordPress Component with Known Vulnerabilities](https://docs.brightsec.com/docs/wordpress-component-with-known-vulnerabilities)                                                                                                                                                                                                                                                                                           |
| **LDAP Injection**                                                               | Tests if various application parameters are vulnerable to unauthorized LDAP access                                                           | `ldapi`                      | - [LDAP Injection](https://docs.brightsec.com/docs/ldap-injection)<br> <br> - [LDAP Error](https://docs.brightsec.com/docs/ldap-error)                                                                                                                                                                                                                                                                                       |
| **Local File Inclusion (LFI)**                                                   | Tests if various application parameters are vulnerable to loading of unauthorized local system resources                                     | `lfi`                        | - [Local File Inclusion (LFI)](https://docs.brightsec.com/docs/local-file-inclusion-lfi)                                                                                                                                                                                                                                                                                                                                     |
| **Mass Assignment**                                                              | Tests if it is possible to create requests with additional parameters to gain privilege escalation                                           | `mass_assignment`            | - [Mass Assignment](https://docs.brightsec.com/docs/mass-assignment)                                                                                                                                                                                                                                                                                                                                                         |
| **OS Command Injection**                                                         | Tests if various application parameters are vulnerable to Operation System (OS) commands injection                                           | `osi`                        | - [OS Command Injection](https://docs.brightsec.com/docs/os-command-injection)                                                                                                                                                                                                                                                                                                                                               |
| **Prototype Pollution**                                                          | Tests if it is possible to inject properties into existing JavaScript objects                                                                | `proto_pollution`            | - [Prototype Pollution](https://docs.brightsec.com/docs/prototype-pollution)                                                                                                                                                                                                                                                                                                                                                 |
| **Remote File Inclusion (RFI)**                                                  | Tests if various application parameters are vulnerable to loading of unauthorized remote system resources                                    | `rfi`                        | - [Remote File Inclusion (RFI)](https://docs.brightsec.com/docs/remote-file-inclusion-rfi)                                                                                                                                                                                                                                                                                                                                   |
| **Secret Tokens Leak**                                                           | Tests for exposure of secret API tokens or keys in the target application                                                                    | `secret_tokens`              | - [Secret Tokens Leak](https://docs.brightsec.com/docs/secret-tokens-leak)                                                                                                                                                                                                                                                                                                                                                   |
| **Server Side Template Injection (SSTI)**                                        | Tests if various application parameters are vulnerable to server-side code execution                                                         | `ssti`                       | - [Server Side Template Injection (SSTI)](https://docs.brightsec.com/docs/server-side-template-injection-ssti)                                                                                                                                                                                                                                                                                                               |
| **Server Side Request Forgery (SSRF)**                                           | Tests if various application parameters are vulnerable to internal resources access                                                          | `ssrf`                       | - [Server Side Request Forgery (SSRF)](https://docs.brightsec.com/docs/server-side-request-forgery-ssrf)                                                                                                                                                                                                                                                                                                                     |
| **SQL Injection (SQLI)**                                                         | SQL Injection tests vulnerable parameters for SQL database access                                                                            | `sqli`                       | - [SQL Injection: Blind Boolean Based](https://docs.brightsec.com/docs/sql-injection-blind-boolean-based)<br> <br> - [SQL Injection: Blind Time Based](https://docs.brightsec.com/docs/sql-injection-blind-time-based)<br> <br> - [SQL Injection](https://docs.brightsec.com/docs/sql-injection)<br> <br> - [SQL Database Error Message in Response](https://docs.brightsec.com/docs/sql-database-error-message-in-response) |
| **Unrestricted File Upload**                                                     | Tests if file upload mechanisms are validated properly and denies upload of malicious content                                                | `file_upload`                | - [Unrestricted File Upload](https://docs.brightsec.com/docs/unrestricted-file-upload)                                                                                                                                                                                                                                                                                                                                       |
| **Unsafe Date Range** <br>_(Date Manipulation)_                                  | Tests if date ranges are set and validated properly                                                                                          | `date_manipulation`          | - [Unsafe Date Range](https://docs.brightsec.com/docs/unsafe-date-range)                                                                                                                                                                                                                                                                                                                                                     |
| **Unsafe Redirect** <br>_(Unvalidated Redirect)_                                 | Tests if various application parameters are vulnerable to injection of a malicious link which can redirect a user without validation         | `unvalidated_redirect`       | - [Unsafe Redirect](https://docs.brightsec.com/docs/unsafe-redirect)                                                                                                                                                                                                                                                                                                                                                         |
| **User ID Enumeration**                                                          | Tests if it is possible to collect valid user ID data by interacting with the target application                                             | `id_enumeration`             | - [Enumerable Integer-Based ID](https://docs.brightsec.com/docs/enumerable-integer-based-id)                                                                                                                                                                                                                                                                                                                                 |
| **Version Control System Data Leak**                                             | Tests if it is possible to access Version Control System (VCS) resources                                                                     | `version_control_systems`    | - [Version Control System Data Leak](https://docs.brightsec.com/docs/version-control-system-data-leak)                                                                                                                                                                                                                                                                                                                       |
| **XML External Entity Injection**                                                | Tests if various XML parameters are vulnerable to XML parsing of unauthorized external entities                                              | `xxe`                        | - [XML External Entity Injection](https://docs.brightsec.com/docs/xml-external-entity-injection)                                                                                                                                                                                                                                                                                                                             |

### Example of a CI configuration

You can integrate this library into any CI you use, for that you will need to add the `BRIGHT_TOKEN` ENV vars to your CI. Then add the following to your `github actions` configuration:

```yaml
steps:
  - name: Run sec tests
    run: dotnet test -c Release --no-build --nologo --filter FullyQualifiedName~SecurityTests
    env:
      POSTGRES_PASSWORD: ${{ secrets.POSTGRES_PASSWORD }}
      POSTGRES_USER: ${{ secrets.POSTGRES_USER }}
      BRIGHT_TOKEN: ${{ secrets.BRIGHT_TOKEN }}
      BRIGHT_HOSTNAME: app.brightsec.com
```

For a full list of CI configuration examples, check out the docs below.

## Documentation & Help

- Full documentation available at: [https://docs.brightsec.com/](https://docs.brightsec.com/)
- Join our [Discord channel](https://discord.gg/jy9BB7twtG) and ask anything!

## Contributing

Please read [contributing guidelines here](./CONTRIBUTING.md).

<a href="https://github.com/NeuraLegion/sectester-net-demo/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=NeuraLegion/sectester-net-demo"/>
</a>

## License

Copyright © 2022 [Bright Security](https://brightsec.com/).

This project is licensed under the MIT License - see the [LICENSE file](LICENSE) for details.
