namespace SolrCore.QueryBuilder
{
    using System;

    public class Path : Query
    {
        private NestPathStyle _nestPathStyle = NestPathStyle.Plus;
        private string[] _paths;

        public Path(params string[] paths)
        {
            _paths = paths;
        }

        public void ChangeStyle(NestPathStyle backslash)
        {
            _nestPathStyle = backslash;

            if (_nestPathStyle == NestPathStyle.Minus)
            {
                Array.Resize(ref _paths, _paths.Length + 1);
                _paths[_paths.Length - 1] = "*";
            }
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            var slash = _nestPathStyle == NestPathStyle.Minus ? "\\\\/" : "\\/";
            if (_nestPathStyle == NestPathStyle.Plus || _paths.Length > 1)
            {
                foreach (var path in _paths)
                {
                    dto.Sb.Append(slash);
                    dto.Sb.Append(path);
                }
            }
            else
            {
                dto.Sb.Append(_paths[0]);
            }
        }
    }
}