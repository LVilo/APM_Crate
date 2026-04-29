//using MySqlConnector;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using Tmds.DBus.Protocol;

//namespace AutoSet_New.Models
//{
//    public static class SQLModel
//    {
//        private static string Ip {  get; } = "172.22.64.138";
//        private static string DBName {  get; } = "TikDatabase";
//        private static string UserName {  get; } = "Tuner";
//        private static string Password {  get; } = "123";
//        private static string Connection { get; set; } = $"Server={Ip};Database={DBName};Uid={UserName};Pwd={Password};Port=3306;";

//        public static string CratePLCTable { get; } = "Devices";
//        public static string SettingsTable { get; } = "Settings";
//        public static string FamilyName { get; } = "PLC";

//        //public static async Task WriteValues()
//        //{
//        //    using MySqlConnection conect = new MySqlConnection(Connection);
//        //    await conect.OpenAsync();

//        //    await conect.CloseAsync();
//        //}
//        public static async Task<ushort> GetSerialNumber()
//        {
//            using MySqlConnection conect = new MySqlConnection(Connection);
//            await conect.OpenAsync();
//            string mes = $"SELECT MAX(SerialNumber) FROM {CratePLCTable} WHERE FamilyName='{FamilyName}'";
//            using MySqlCommand command = new MySqlCommand(mes, conect);
//            object result = command.ExecuteScalar();
//            ushort lastserialnumber = result !=DBNull.Value ?Convert.ToUInt16(result) : (ushort)0;
//            await conect.CloseAsync();
//            return lastserialnumber;
//        }
//        public static async Task WriteNewDevice(int serialNumber, string ordernumber, string plc)
//        {
//            using MySqlConnection conect = new MySqlConnection(Connection);
//            await conect.OpenAsync();
//            string mes = $"INSERT INTO {CratePLCTable} (SerialNumber,OrderNumber,FamilyName) VALUES({serialNumber},'{ordernumber}','{plc}')";
//            using MySqlCommand command = new MySqlCommand(mes, conect);
//            int rows = command.ExecuteNonQuery();
//            await conect.CloseAsync();
//        }
//        public static async Task<ulong> WriteNewParameters(ParametersSettingPLC parameters)
//        {
//            string query = @$"
//            INSERT INTO {SettingsTable}
//            (
//                User,
//                Date,
//                Channel,
//                Setting,
//                StartTime,
//                EndTime,
//                Device,
//                TimeSetting,
//                SerialNumber,
//                OrderNumber,

//                Coef_acc_A,
//                Coef_acc_B,
//                Coef_speed_A,
//                Coef_speed_B,
//                Coef_4_20_A,
//                Coef_4_20_B,
//                Coef_T_A,
//                Coef_T_B,
//                T_Type
//            )
//            VALUES
//            (
//                '{parameters.UserName}',
//                '{parameters.Date}',
//                '{parameters.Channel}',
//                '{parameters.Setting}',
//                '{parameters.StartTime}',
//                '{parameters.EndTime}',
//                '{parameters.DeviceName}',
//                '{parameters.TimeSetting}',
//                 {parameters.SerialNumber},
//                '{parameters.OrderNumber}',

//                '{parameters.Coef_acc_A}',
//                '{parameters.Coef_acc_B}',
//                '{parameters.Coef_speed_A}',
//                '{parameters.Coef_speed_B}',
//                '{parameters.Coef_4_20_A}',
//                '{parameters.Coef_4_20_B}',
//                '{parameters.Coef_T_A}',
//                '{parameters.Coef_T_B}',
//                '{parameters.T_Type}');

//            SELECT LAST_INSERT_ID();";

//            await using var connection = new MySqlConnection(Connection);
//            await using var command = new MySqlCommand(query, connection);

//            await connection.OpenAsync();

//            return (ulong)await command.ExecuteScalarAsync();
//        }
//        public static async Task CreateTableSettings()
//        {
//            using MySqlConnection conect = new MySqlConnection(Connection);
//            await conect.OpenAsync();

//            string mes = $@"CREATE TABLE {SettingsTable}
//            (
//                    Id INT PRIMARY KEY AUTO_INCREMENT,
//                    DeviceId INT NOT NULL,
//                    User TEXT NOT NULL,
//                    Date TEXT NOT NULL,
//                    Channel TEXT NOT NULL,
//                    Setting TEXT NOT NULL,
//                    StartTime TEXT NOT NULL,
//                    EndTime TEXT NOT NULL,
//                    FamilyName VARCHAR(20) NOT NULL,
//                    TypeName VARCHAR(20) NOT NULL,
//                    TimeSetting TEXT NOT NULL,
//                    SerialNumber INT NOT NULL,
//                    OrderNumber VARCHAR(20) NOT NULL,
//                    Coef_acc_A TEXT NOT NULL,
//                    Coef_acc_B TEXT NOT NULL,
//                    Coef_speed_A TEXT NOT NULL,
//                    Coef_speed_B TEXT NOT NULL,
//                    Coef_4_20_A TEXT NOT NULL,
//                    Coef_4_20_B TEXT NOT NULL,
//                    Coef_T_A TEXT NOT NULL,
//                    Coef_T_B TEXT NOT NULL,
//                    T_Type TEXT NOT NULL,

//                    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

//                    KEY idx_plc_id (DeviceId),
//                    KEY idx_channel (Channel),
//                    KEY idx_setting (Setting),
//                    KEY idx_date (Date),

//                    CONSTRAINT fk_settings_plc FOREIGN KEY (DeviceId) REFERENCES {CratePLCTable}(Id) ON DELETE CASCADE ON UPDATE CASCADE
//            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
//            using MySqlCommand command = new MySqlCommand(mes, conect);
//            int rows = command.ExecuteNonQuery();
//            await conect.CloseAsync();
//        }
//        public static async Task CreateTableCratePLC()
//        {
//            using MySqlConnection conect = new MySqlConnection(Connection);
//            await conect.OpenAsync();

//            string mes = $@"CREATE TABLE {CratePLCTable}
//                        (
//                        Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
//                        SerialNumber INT NOT NULL,
//                        OrderNumber VARCHAR(20) NOT NULL,
//                        FamilyName VARCHAR(20) NOT NULL,
//                        TypeName VARCHAR(20) NOT NULL,
//                        UNIQUE KEY unique_family_serial (FamilyName, SerialNumber),
//                        KEY idx_order_number (OrderNumber),
//                        KEY idx_serial_number (SerialNumber),
//                        KEY idx_family_name (FamilyName),
//                        KEY idx_type_name (TypeName)
//                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
//            using MySqlCommand command = new MySqlCommand(mes, conect);
//            int rows = command.ExecuteNonQuery();
//            await conect.CloseAsync();
//        }
//        public static async Task TableExistsSettingsAsync()
//        {
//            //string schema = "dbo";
//            string tableName = SettingsTable;
//            const string query = @"
//            SELECT COUNT(*)
//            FROM INFORMATION_SCHEMA.TABLES
//            WHERE TABLE_NAME = @tableName";

//            using var connection = new MySqlConnection(Connection);
//            using var command = new MySqlCommand(query, connection);

//            //command.Parameters.AddWithValue("@schema", schema);
//            command.Parameters.AddWithValue("@tableName", tableName);

//            await connection.OpenAsync();

//            int count = Convert.ToInt32(await command.ExecuteScalarAsync());
//            if (count == 0)
//            {
//                await CreateTableSettings();
//            }
//            await connection.CloseAsync();
//        }
//        public static async Task TableExistsCratePLCAsync()
//        {
//            //string schema = "dbo";
//            string tableName = CratePLCTable;
//            const string query = @"
//            SELECT COUNT(*)
//            FROM INFORMATION_SCHEMA.TABLES
//            WHERE TABLE_NAME = @tableName";

//            using var connection = new MySqlConnection(Connection);
//            using var command = new MySqlCommand(query, connection);

//            command.Parameters.AddWithValue("@tableName", tableName);

//            await connection.OpenAsync();

//            int count = Convert.ToInt32(await command.ExecuteScalarAsync());
//            if (count == 0)
//            {
//                await CreateTableCratePLC();
//            }
//            await connection.CloseAsync();
//        }
//    }
//}
