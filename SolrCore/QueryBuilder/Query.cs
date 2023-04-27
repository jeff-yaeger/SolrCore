namespace SolrCore.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using SolrCore.Query;

    public abstract class Query : IQuery
    {
        public abstract string Render(Builder dto);

        public virtual void Build(Builder dto)
        {
            dto.Sb.Append(Render(dto));
        }

        protected static string Render(Action<Builder> build, Builder dto)
        {
            build(dto);
            return dto.Sb.ToString();
        }

        protected static string GetFieldName(string name, IDictionary<string, string> translationDictionary)
        {
            if (translationDictionary == null)
            {
                return name;
            }

            var succeed = translationDictionary.TryGetValue(name, out var translatedName);
            return succeed ? translatedName : name;
        }

        public Query Or(Query queryBuilder)
        {
            return new OrOperator(this, queryBuilder);
        }

        public Query And(Query queryBuilder)
        {
            return new AndOperator(this, queryBuilder);
        }

        public Query Or(params Query[] queryBuilder)
        {
            return new OrOperator(this, new OrOperator(queryBuilder));
        }

        public Query And(params Query[] queryBuilder)
        {
            return new AndOperator(this, new AndOperator(queryBuilder));
        }

        public Query Not()
        {
            return new NotOperator(this);
        }

        public Query Boost(double boost)
        {
            return new Boost(this, boost);
        }

        public static Query operator |(Query query1, Query query2)
        {
            return new OrOperator(query1, query2);
        }

        public static Query operator &(Query query1, Query query2)
        {
            return new AndOperator(query1, query2);
        }

        public static Query operator +(Query query1, Query query2)
        {
            return new AndOperator(query1, query2);
        }

        public static Query operator !(Query query)
        {
            return new NotOperator(query);
        }
    }
}