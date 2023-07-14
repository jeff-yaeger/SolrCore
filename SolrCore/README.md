# SolrCore

A lightweight Apache Solr adapter for .Net 7+

Use:

In Program.cs add:
builder.Services.AddHttpClient("SolrCore", httpClient => { httpClient.BaseAddress = new
Uri("http://localhost:8983/"); });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSolrCore<TKey, TEntity>("YourSolrCoreName");

//You do not have to add anything here. Defaults will be used if you don't want any custom behavior.
builder.Services.AddSolr(typeof(YourCustomSerializer), typeof(YourCustomDefaultSetter));

Check out https://github.com/jeff-yaeger/SolrCore to see how the web project is set up and for usage.

see:https://www.nuget.org/packages/EbaSoft.SolrCore

[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/27173723-bc9258dd-27eb-4c39-9b2b-c33cae354883?action=collection%2Ffork&collection-url=entityId%3D27173723-bc9258dd-27eb-4c39-9b2b-c33cae354883%26entityType%3Dcollection%26workspaceId%3Ddceebdce-605e-4c97-82d4-4415b2d0d34b#?env%5BDevelopment%5D=W3sia2V5IjoidXJsIiwidmFsdWUiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJlbmFibGVkIjp0cnVlLCJ0eXBlIjoiZGVmYXVsdCIsInNlc3Npb25WYWx1ZSI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsInNlc3Npb25JbmRleCI6MH0seyJrZXkiOiJ0b2tlbiIsInZhbHVlIjoiYXNvYXNqZGZpODkwNzIzNHEwOTh1aXdxZWZyIiwiZW5hYmxlZCI6dHJ1ZSwidHlwZSI6InNlY3JldCIsInNlc3Npb25WYWx1ZSI6ImFzb2FzamRmaTg5MDcyMzRxMDk4dWl3cWVmciIsInNlc3Npb25JbmRleCI6MX1d)

Changes:

v1.0.10 -

Added optional checking for duplicate ids.

Changed Translations dictionary to concurrent.

Added test controllers for IOnAdd<> types.

Made repository more generic for expanded use.

Fixed delete to work with items that have children. 

v1.0.9 -

Added Parent Which and Child Of Solr queries.

Added support for typed queries so you can use child field names.

Added filter to ignore default queries.

Added optional quotes around ByField queries.

Moved Entity Setter to EntityDefaults package.

Added commit option to all non GET Repository actions