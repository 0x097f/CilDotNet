using Semicolon.Cil.Executable;
using System;
using System.Collections.Generic;

namespace Semicolon.Cil
{
    public readonly struct BinaryOffset : IEquatable<BinaryOffset>
    {
        private static List<SectionInfo>? _sections;

        public long Value { get; }

        public BinaryOffset(long value)
        {
            Value = value;
        }

        public static void SetSections(List<SectionInfo> sections)
        {
            _sections = sections;
        }

        public Rva ToRva()
        {
            if (_sections == null)
                throw new InvalidOperationException("Sections not set. Call BinaryOffset.SetSections() first.");

            foreach (var section in _sections)
            {
                if (Value >= section.PointerToRawData &&
                    Value < section.PointerToRawData + section.SizeOfRawData)
                {
                    var rva = Value - section.PointerToRawData + section.VirtualAddress;
                    return new Rva((uint)rva);
                }
            }
            return new Rva(0);
        }

        public bool TryToRva(out Rva rva)
        {
            if (_sections == null)
            {
                rva = default;
                return false;
            }

            foreach (var section in _sections)
            {
                if (Value >= section.PointerToRawData &&
                    Value < section.PointerToRawData + section.SizeOfRawData)
                {
                    var rvaValue = Value - section.PointerToRawData + section.VirtualAddress;
                    rva = new Rva((uint)rvaValue);
                    return true;
                }
            }

            rva = default;
            return false;
        }

        public SectionInfo? GetSection()
        {
            if (_sections == null)
                return null;

            foreach (var section in _sections)
            {
                if (Value >= section.PointerToRawData &&
                    Value < section.PointerToRawData + section.SizeOfRawData)
                {
                    return section;
                }
            }
            return null;
        }

        public bool IsInSection(string sectionName)
        {
            var section = GetSection();
            return section != null && section.Name.TrimEnd('\0') == sectionName;
        }

        public static implicit operator long(BinaryOffset offset) => offset.Value;
        public static implicit operator BinaryOffset(long value) => new BinaryOffset(value);
        public static implicit operator BinaryOffset(int value) => new BinaryOffset(value);

        public static BinaryOffset operator +(BinaryOffset offset, long delta) => new BinaryOffset(offset.Value + delta);
        public static BinaryOffset operator -(BinaryOffset offset, long delta) => new BinaryOffset(offset.Value - delta);
        public static BinaryOffset operator +(BinaryOffset offset, int delta) => new BinaryOffset(offset.Value + delta);
        public static BinaryOffset operator -(BinaryOffset offset, int delta) => new BinaryOffset(offset.Value - delta);

        public static bool operator ==(BinaryOffset left, BinaryOffset right) => left.Value == right.Value;
        public static bool operator !=(BinaryOffset left, BinaryOffset right) => left.Value != right.Value;
        public static bool operator <(BinaryOffset left, BinaryOffset right) => left.Value < right.Value;
        public static bool operator >(BinaryOffset left, BinaryOffset right) => left.Value > right.Value;
        public static bool operator <=(BinaryOffset left, BinaryOffset right) => left.Value <= right.Value;
        public static bool operator >=(BinaryOffset left, BinaryOffset right) => left.Value >= right.Value;

        public bool Equals(BinaryOffset other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is BinaryOffset other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => $"0x{Value:X8}";

        public bool IsValid => Value >= 0;
        public bool IsNull => Value == 0;
    }
}