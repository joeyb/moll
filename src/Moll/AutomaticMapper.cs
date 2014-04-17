using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Moll
{
    public class AutomaticMapper<TSrc, TDest> : IMapper<TSrc, TDest>
        where TSrc : class
        where TDest : class, new()
    {
        private readonly IReadOnlyCollection<Tuple<PropertyInfo, PropertyInfo>> _propertyInfoMappings; 

        public AutomaticMapper()
        {
            var srcPropertyInfos = typeof (TSrc)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            var destPropertyInfos = typeof (TDest)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            _propertyInfoMappings = (from srcProp in srcPropertyInfos
                                     let destProp =
                                         destPropertyInfos.FirstOrDefault(
                                             x => x.Name == srcProp.Name && x.PropertyType == srcProp.PropertyType)
                                     where destProp != null
                                     select new Tuple<PropertyInfo, PropertyInfo>(srcProp, destProp)).ToList();
        }

        public TDest Map(TSrc src)
        {
            if (src == null) return null;

            var dest = new TDest();

            foreach (var mapping in _propertyInfoMappings)
            {
                mapping.Item2.SetValue(dest, mapping.Item1.GetValue(src));
            }

            AdditionalCustomMapping(src, dest);

            return dest;
        }

        public virtual void AdditionalCustomMapping(TSrc src, TDest dest)
        {
            
        }
    }
}
