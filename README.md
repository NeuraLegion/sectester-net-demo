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
  1.  We recommend using your Github repository secrets feature to store the key, accessible via the `Settings > Security > Secrets > Actions` configuration. We use the ENV variable called `BRIGHT_TOKEN` in our examples
  2.  If you don’t use that option, make sure you save the key in a secure location. You will need to access it later on in the project but will not be able to view it again.
  3.  More info on [**how to use ENV vars in Github actions**](https://docs.github.com/en/actions/learn-github-actions/environment-variables)

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

![Swagger UI](https://user-images.githubusercontent.com/38690835/207823150-de8cd161-a748-4f79-9ae3-951648dee8f5.png)

To explore the Swagger UI:

- Click on the `POST /users` endpoint
- Click on the "Try it out" button
- Click on the blue "Execute" button
- Then you should see a view similar to the following, where you can see the JSON returned from the API:

![Swagger UI](https://user-images.githubusercontent.com/38690835/207823133-670ec680-6875-4c1d-95d9-a67e72dac299.png)

_TBU_

### A full configuration example

_TBU_

### Recommended tests

|                                                                                  |                                                                                                                                              |                              |                                                                                                                                                                                                                                                                                                                                                                                                                              |
| -------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Test name**                                                                    | **Description**                                                                                                                              | **Usage in SecTester**       | **Detectable vulnerabilities**                                                                                                                                                                                                                                                                                                                                                                                               |
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

_TBU_

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
