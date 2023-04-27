# SolrCore

A lightweight solr adapter for .Net 7+

Use:

In Program.cs add:
builder.Services.AddHttpClient("SolrCore", httpClient => { httpClient.BaseAddress = new Uri("http://localhost:8983/"); });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSolrCore<TKey, TEntity>("YourSolrCoreName");
//You do not have to add anything here. Defaults will be used if you don't want any custom behavior.
builder.Services.AddSolr(typeof(YourCustomSerializer), typeof(YourCustomDefaultSetter));



see:https://www.nuget.org/packages/EbaSoft.SolrCore