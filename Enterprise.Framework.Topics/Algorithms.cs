using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = System.ValueTuple;

namespace Enterprise.Framework.Topics
{
    
    public class Counter
    {
        private readonly string counterName;
        private int _currentValue;
        public Counter(string counterName)
        {
            this.counterName = counterName;
        }

        public void Increment() { _currentValue++; }
        public int Tally() { return _currentValue; }

        public override string ToString()
        {
            return $"{counterName} : {_currentValue}";
        }
    }
    




}
