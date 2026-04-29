using System;
using System.IO.Ports;

namespace PortsWork
{
	public interface Modbus
	{
        
        protected byte[] Exchange(byte[] frame, int expectedLength, uint attempt = 0);
        /// <summary>
        /// запись значения в регистр. Номер регистра задается +1 от настоящего(для удобства)
        /// </summary>
        /// <param name="reg">номре регистра(необходимо записать номер регистра в модскане)</param>
        /// <param name="value"></param>
        /// <param name="func">номер функции</param>
        /// <returns></returns>
        protected bool Write(ushort reg, byte[] value, byte func);
        protected byte[] Read(ushort reg, byte func, ushort len);
        protected void WriteMultiple(ushort reg, byte[] values, byte func = 0x10);

        
    }
}
