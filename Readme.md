# Task 2 - Profile Sample

Identified issues

- The ProfileSampleEntities class extends DbContext and implements IDisposable, but instances aren't being disposed.

- Introduced AsNoTracking() for queries and used LINQ to simplify the code.

- Made the Index action in HomeController asynchronous.

- Instead of incurring large memory allocations from Base64 conversion in the Razor view, we created a separate action that returns images by ID directly from the database. Since an MVC action can only return one ActionResult per request, you can't combine this with Index().