These are the following things we test for in our controllers:

Verify ModelState.IsValid
Return an error response if ModelState is invalid
Retrieve a business entity from persistence
Perform an action on the business entity
Save the business entity to persistence
Return an appropriate IActionResult


NOTE TO NIBSS:
Looking at the AccountController class, its constructor has tons of dependecies so i'll 
go for the JobsController class which has just three (3) dependecies


Observe that the GetDegreess_Should_Return_Degree() will fail if you run it as it is,
you need to copy the wwwroot\content\degree.csv into the bin directory:
e.g C:\Users\Dupree\Documents\Projects\Orc\CodeCoverage\bin\Debug\netcoreapp3.1