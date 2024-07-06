using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlumbingProps.CrossUtil
{
    public class MapHelper
    {
        public TDestination MapObject<TOrigin, TDestination>(TOrigin source) 
            where TOrigin : class             
            where TDestination  : class 
        {
            IMapper mapper = new Mapper();
            return mapper.Map<TDestination>(source);
        }
    }
}
