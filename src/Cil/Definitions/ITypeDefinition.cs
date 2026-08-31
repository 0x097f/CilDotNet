using System;
using System.Collections.Generic;
using System.Reflection;

namespace Semicolon.Cil.Definitions
{
    public interface ITypeDefinition
    {
        uint MetadataToken { get; }
        string? Namespace { get; }
        string Name { get; }
        string FullName { get; }
        string AssemblyQualifiedName { get; }

        public ITypeDefinition? BaseType { get; }
        public TypeAttributes TypeAttributes { get; set; }
        IEnumerable<ITypeDefinition> Interfaces { get; }

        TypeAttributes Attributes { get; }
        bool IsValueType { get; }
        bool IsEnum { get; }
        bool IsInterface { get; }
        bool IsAbstract { get; }
        bool IsSealed { get; }
        bool IsGenericType { get; }
        bool IsGenericInstance { get; }
        IReadOnlyList<IGenericParameter> GenericParameters { get; }

        IEnumerable<IFieldDefinition> Fields { get; }
        IEnumerable<IMethodDefinition> Methods { get; }
        IEnumerable<IPropertyDefinition> Properties { get; }
        IEnumerable<IEventDefinition> Events { get; }
        IEnumerable<ITypeDefinition> NestedTypes { get; }

        IModuleDefinition Module { get; }
        IAssemblyDefinition Assembly { get; }
        ITypeDefinition? DeclaringType { get; }

        IEnumerable<ICustomAttribute> CustomAttributes { get; }

        bool IsAssignableTo(ITypeDefinition other);
        IFieldDefinition? GetField(string name);
        IMethodDefinition? GetMethod(string name);
        ITypeDefinition? GetNestedType(string name);
        int InstanceSize { get; }
    }


    public interface IModuleDefinition { }
    public interface IAssemblyDefinition { }
    public interface IFieldDefinition { }
    public interface IMethodDefinition { }
    public interface IPropertyDefinition { }
    public interface IEventDefinition { }
    public interface IGenericParameter { }
    public interface ICustomAttribute { }
}