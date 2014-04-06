using System.Linq;
using System.Reflection;

namespace Moll
{
    public class AutomaticMapper<TSrc, TDest> : IMapper<TSrc, TDest>
        where TSrc : class
        where TDest : class, new()
    {
        public TDest Map(TSrc src)
        {
            if (src == null) return null;

            var srcProps = typeof (TSrc)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            var destProps = typeof (TDest)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            var dest = new TDest();

            foreach (var srcProp in srcProps)
            {
                var destProp =
                    destProps.FirstOrDefault(x => x.Name == srcProp.Name && x.PropertyType == srcProp.PropertyType);

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
