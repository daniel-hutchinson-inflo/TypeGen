﻿using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    public class GenericClass<T> : GenericBaseClass<T>
    {
        public T GenericProperty { get; set; }
        public TestEnum EnumProperty { get; set; }
        public NestedEntity Nested { get; set; }
    }
}
