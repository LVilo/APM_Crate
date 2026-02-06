using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSet_New
{
    public static class Registers
    { //Адреса регистров для работы начинаются с 0 (т.е. обычный адрес -1)
        public const int REGISTER_VERSION_MI = 1794;
        public const int REGISTER_PASSWORD = 3495;

        public const int REGISTER_CONTROLLER_TYPE = 60025;

        public const int REGISTER_COEFF_A_CHANNEL1 = 59999;
        public const int REGISTER_COEFF_B_CHANNEL1 = 60001;
        public const int REGISTER_COEFF_A_CHANNEL2 = 60029;
        public const int REGISTER_COEFF_B_CHANNEL2 = 60031;
        public const int REGISTER_COEFF_A_CHANNEL3 = 60059;
        public const int REGISTER_COEFF_B_CHANNEL3 = 60061;
        public const int REGISTER_COEFF_A_CHANNEL4 = 61259;
        public const int REGISTER_COEFF_B_CHANNEL4 = 61261;

        public const int REGISTER_ADDR_ON_CHANNEL1 = 11055;
        public const int REGISTER_ADDR_ON_CHANNEL2 = 11255;
        public const int REGISTER_ADDR_ON_CHANNEL3 = 11455;

        public const int REGISTER_VAL_RMS_CHANNEL1 = 8021;
        public const int REGISTER_VAL_RMS_CHANNEL2 = 8046;
        public const int REGISTER_VAL_RMS_CHANNEL3 = 8071;
        public const int REGISTER_VAL_RMS_CHANNEL4 = 9409;

        public const int REGISTER_SERIAL_NUM = 60025;
        public const int REGISTER_SERIAL_NUM_READ = 8007;
        public const int REGISTER_VERSION_PLC = 8008;

        public const int REGISTER_COEFFICIENT = 60055;

        public const int VALUE_PASSWORD_OLD = -21555; //abcd hex
        public const int VALUE_PASSWORD_NEW = -9030; //dcba hex

        public const int VALUE_VERSION_NEW = 1298; //номер версии МИ, с которой меняется пароль

        public const int VALUE_CONTROLLER_TYPE = 4;
    }
}
