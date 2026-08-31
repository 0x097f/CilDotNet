using System;
using System.Collections;
using System.Collections.Generic;

namespace Semicolon.Cil.Executable
{
    public interface IExecutable
    {
        public BinaryOffset ExecutableOffset { get; set; }
    }
}