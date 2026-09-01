using System;
using System.Collections.Generic;
using System.Text;

namespace CilDotNet
{
    public class Annotations
    {
        public DateTime Timestamp;
        void Annotation()
        {
            Timestamp = DateTime.Now;
        }
    }
}
