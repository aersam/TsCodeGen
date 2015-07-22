using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATestProject
{
    class AClass
    {
        public string Name
        {
            get; set;
        }

        public AClass[] AnArray
        {
            get; set;
        }

        public IEnumerable<GenericTestClass<TestEnum, string>> AnIEnumerable
        {
            get; set;
        }

        public int AnInt
        {
            get; set;
        }
    }
}
