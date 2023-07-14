namespace SolrCore
{
    using System;
    using Connection;
    using EntityDefaults;
    using EntityModels;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Repository;
    using Serializer;

    public static class Extensions
    {
        public static void AddSolrCore<TKey, T>(this IServiceCollection serviceCollection, string coreName, bool useOverwriteProtection = true) where T : class where TKey : IEquatable<TKey>
        {
            serviceCollection.AddSingleton<ISolrRepository<TKey, T>, SolrRepository<TKey, T>>();
            SolrRepository<TKey, T>.SetCoreName(coreName);
            SolrRepository<TKey, T>.SetOverwriteProtection(useOverwriteProtection);
        }

        public static void AddSolr(this IServiceCollection serviceCollection, Type serializer = null, Type setter = null)
        {
            serviceCollection.AddSingleton<ISolrConnection, SolrConnection>();

            if (serializer != null && typeof(ISolrSerializer).IsAssignableFrom(serializer))
            {
                serviceCollection.AddSingleton(typeof(ISolrSerializer), serializer);
            }
            else
            {
                serviceCollection.AddSingleton<ISolrSerializer, SolrNewtonsoftJsonSerializer>();
            }

            if (setter != null && typeof(IEntitySetter).IsAssignableFrom(setter))
            {
                serviceCollection.AddSingleton(typeof(IEntitySetter), setter);
            }
            else
            {
                serviceCollection.AddSingleton<IEntitySetter, DoNothingEntitySetter>();
            }
        }

        public static string ToSolrValue(this bool value)
        {
            return value ? "true" : "false";
        }

        public static string ToSolrValue(this DateTime value)
        {
            return value.ToString(Constants.SolrDateFormat);
        }

        public static string ToSolrValue(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToString(Constants.SolrDateFormat) : string.Empty;
        }
    }
}