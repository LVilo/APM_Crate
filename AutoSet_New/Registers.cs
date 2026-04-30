using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSet_New
{
    public static class Registers
    { //Адреса регистров для работы начинаются с 0 (т.е. обычный адрес -1)
        public static int REGISTER_VERSION_MI { get; } = 1794;
        public static int REGISTER_PASSWORD { get; } = 3495;

        public static int REGISTER_CONTROLLER_TYPE { get; } = 60025;

        public static int REGISTER_COEFF_A_CHANNEL1 { get; } = 59999;
        public static int REGISTER_COEFF_B_CHANNEL1 { get; } = 60001;
        public static int REGISTER_COEFF_A_CHANNEL2 { get; } = 60029;
        public static int REGISTER_COEFF_B_CHANNEL2 { get; } = 60031;
        public static int REGISTER_COEFF_A_CHANNEL3 { get; } = 60059;
        public static int REGISTER_COEFF_B_CHANNEL3 { get; } = 60061;
        public static int REGISTER_COEFF_A_CHANNEL4 { get; } = 61259;
        public static int REGISTER_COEFF_B_CHANNEL4 { get; } = 61261;

        public static int REGISTER_ADDR_ON_CHANNEL1 { get; } = 11055;
        public static int REGISTER_ADDR_ON_CHANNEL2 { get; } = 11255;
        public static int REGISTER_ADDR_ON_CHANNEL3 { get; } = 11455;

        public static int REGISTER_VAL_RMS_CHANNEL1 { get; } = 8021;
        public static int REGISTER_VAL_RMS_CHANNEL2 { get; } = 8046;
        public static int REGISTER_VAL_RMS_CHANNEL3 { get; } = 8071;
        public static int REGISTER_VAL_RMS_CHANNEL4 { get; } = 9409;

        public static int REGISTER_SERIAL_NUM { get; } = 60025;
        public static int REGISTER_SERIAL_NUM_READ { get; } = 8007;
        public static int REGISTER_VERSION_PLC { get; } = 8008;

        public static int REGISTER_COEFFICIENT { get; } = 60055;

        public static int VALUE_PASSWORD_OLD { get; } = -21555; //abcd hex
        public static int VALUE_PASSWORD_NEW { get; } = -9030; //dcba hex

        public static int VALUE_VERSION_NEW { get; } = 1298; //номер версии МИ, с которой меняется пароль

        public static int VALUE_CONTROLLER_TYPE { get; } = 4;
    }
}
