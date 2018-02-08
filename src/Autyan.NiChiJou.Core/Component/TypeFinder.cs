using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autyan.NiChiJou.Core.Component
{
    public class TypeFinder
    {
        private Assembly[] _assemblies;

        private TypeFinder()
        {

        }

        public static TypeFinder Scope(Assembly[] assemblies)
        {
            return new TypeFinder
            {
                _assemblies = assemblies
            };
        }

        public IEnumerable<Type> Find(Func<Type, bool> expression)
        {
            var types = new List<Type>();
            foreach (var assembly in _assemblies)
            {
                types.AddRange(assembly.GetTypes().Where(expression));
            }

            return types;
        }
    }
}
