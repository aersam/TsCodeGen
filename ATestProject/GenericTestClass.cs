using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATestProject
{
    class GenericTestClass<K, V> where V : IDictionary<string, string>
    {
        public KeyValuePair<K, V> this[int index]
        {
            get
            {
                return new KeyValuePair<K, V>(default(K),default(V));
            }
            set
            {

            }
        }

        

        public K Key
        { get; set; }

        public V Value
        { get; set; }
    }
}
