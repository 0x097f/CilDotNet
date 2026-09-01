using System;
using System.Collections.Generic;

namespace CilDotNet.EmbeddedResource
{
    public enum ResourceType
    {
        Win32Resource,
        DotNetEmbedded
    }

    public interface IEmbeddedResource
    {
        void Extract();
        void Using();
        ResourceType Type { get; set; }
    }
}