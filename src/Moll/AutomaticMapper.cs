using System.Linq;
using System.Reflection;

namespace Moll
{
    public class AutomaticMapper<TSrc, TDest> : IMapper<TSrc, TDest>
        where TSrc : class
        where TDest : class, new()
    {
        private readonly PropertyInfo[] _srcPropertyInfos;
        private readonly PropertyInfo[] _destPropertyInfos;

        public AutomaticMapper()
        {
            _srcPropertyInfos = typeof (TSrc)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            _destPropertyInfos = typeof (TDest)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
        }

        public TDest Map(TSrc src)
        {
            if (src == null) return null;

            var dest = new TDest();

            foreach (var srcProp in _srcPropertyInfos)
            {
                var destProp = _destPropertyInfos
                    .FirstOrDefault(x => x.Name == srcProp.Name && x.PropertyType == srcProp.PropertyType);

                if (destProp == null) continue;

                destProp.SetValue(dest, srcProp.GetValue(src));
            }

            AdditionalCustomMapping(src, dest);

            return dest;
        }

        public virtual void AdditionalCustomMapping(TSrc src, TDest dest)
        {
            
        }
    }
}
