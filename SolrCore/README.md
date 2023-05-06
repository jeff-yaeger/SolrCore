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

Changes:
v1.0.9 -

Added Parent Which and Child Of Solr queries.

Added support for typed queries so you can use child field names.

Added filter to ignore default queries.

Added optional quotes around ByField queries.

Moved Entity Setter to EntityDefaults package.

Added commit option to all non GET Repository actions