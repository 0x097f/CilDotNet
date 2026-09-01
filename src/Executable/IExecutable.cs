using System;
using System.Collections;
using System.Collections.Generic;

namespace CilDotNet.Executable
{
    public interface IExecutable
    {
        public BinaryOffset ExecutableOffset { get; set; }
    }
}