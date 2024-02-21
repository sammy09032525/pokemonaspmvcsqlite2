# Project name: Favorite Pokemons using ASP.net Core MVC 6.0

## Main features:
Display List of pokemons available in the remote service, allow authenticated user to FAVOR/UNFAVOR each pokemon into local database


Some points to be highlighted
* This application was developed based on a separation of concern principles: Model View Controller + DataAccess (DBContext), RestApi Client (HttpClient).
* Good development practices were prioritized in this project, following S.O.L.I.D. Principles and Clean Code.
* Generic and reusable methods were used in several parts of application


## Authentication
use Nuget package Microsoft.AspNetCore.Identity library 
with UserManager SignInManager
and Cookie-based authentication

## Database
Sqlite file pokemonfav.db  and SQL script db.sql*

## RestApiClient
PokeApiClient connecting to https://pokeapi.co/

## Data Access
Using Entity Framework Core . The use of migrations was made to keep the system update simplified and also used the services for validating methods that make changes to the database, making data writing safer

## Exception Handling
Standard exception handling using error page app.UseExceptionHandler("/Home/Error") obtaining a complete log of each failure obtained in the application through generic and reusable methods, optimizing our time in the search for bugs and any failures that may occur.
