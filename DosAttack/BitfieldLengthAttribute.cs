using System;

namespace DosAttack
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	sealed class BitfieldLengthAttribute : Attribute
	{
        public uint Length { get; }

        public BitfieldLengthAttribute(uint length)
		{
			Length = length;
		}
    }
}
