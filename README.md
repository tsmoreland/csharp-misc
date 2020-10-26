# Identity Demo Code

Sample projects for demonstrating login functionality including registration, login, logout, ... using ASP.NET

## MVC

This project is a simple controller based MVC app which uses cookie based authentication as well as extending IdentityUser with a few simple properties.  
In theory the extended properties shoudl be moved to pure claims for the user but it's ok for the moment if the user is specific to the app then it can be
extended as needed which may be faster as we could automatically or even explicitly load related entities for the user where as claims may require seperate
calls to load the specifics of each claim (assuming the claim isn't self descriptive).

In either case the intent is mostly to show all the in's and out's that ASP.NET Identity provides including multi-factor auth, verifying e-mails, locking accounts, ...

### Blazor-Server

Demo of Blazor-Server App, initially implemented using Cookie based authentication with the intent to shift it over to token based with an external identity provider.
The vague plan is to tag the repo at relevant points such as the semi-complete cookie-based approach, then token, then external IDP
