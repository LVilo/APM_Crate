using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Tmds.DBus.Protocol;

namespace APM_Crate.Models
{
    public static class SQLModel
    {
        private static string Ip {  get; } = "172.22.64.138";
        private static string DBName {  get; } = "TikDatabase";
        private static string UserName {  get; } = "Tuner";
        private static string Password {  get; } = "123";
        private static string Connection { get; set; } = $"Server={Ip};Database={DBName};Uid={UserName};Pwd={Password};Port=3306;";

        private static string CratePLCTable { get; set; } = "CratePLC";
        private static string SettingsTable { get; set; } = "SettingPLC";

        public static async Task WriteValues()
        {
            using MySqlConnection conect = new MySqlConnection(Connection);
            await conect.OpenAsync();

            await conect.CloseAsync();
        }
        public static async Task<ushort> GetSerialNumber()
        {
            using MySqlConnection conect = new MySqlConnection(Connection);
            await conect.OpenAsync();
            string mes = $"SELECT MAX(SerialNumber) FROM {CratePLCTable}";
            using MySqlCommand command = new MySqlCommand(mes, conect);
            object result = command.ExecuteScalar();
            ushort lastserialnumber = result !=DBNull.Value ?Convert.ToUInt16(result) : (ushort)0;
            await conect.CloseAsync();
            return lastserialnumber;
        }
        public static async Task WriteNewDevice(int serialNumber, string ordernumber, string plc)
        {
            using MySqlConnection conect = new MySqlConnection(Connection);
            await conect.OpenAsync();
            string mes = $"INSERT INTO {CratePLCTable} (SerialNumber,OrderNumber,FamilyName) VALUES(@{serialNumber},@{ordernumber},@{plc})";
            using MySqlCommand command = new MySqlCommand(mes, conect);
            int rows = command.ExecuteNonQuery();
            await conect.CloseAsync();
        }
        public static async Task WriteNewParameters(ParametersSettingPLC parameters)
        {
            string query = @$"
            INSERT INTO {SettingsTable}
            (
                UserName,
                Date,
                Setting,
                StartTime,
                EndTime,
                DeviceName,
                TimeSetting,
                SerialNumber,
                OrderNumber,

                Channel1_Coef_acc_A,
                Channel1_Coef_acc_B,
                Channel1_Coef_speed_A,
                Channel1_Coef_speed_B,
                Channel1_Coef_4-20_A,
                Channel1_Coef_4-20_B,
                Channel1_Coef_T_A,
                Channel1_Coef_T_B,
                Channel1_T-Type,

                Channel2_Coef_acc_A,
                Channel2_Coef_acc_B,
                Channel2_Coef_speed_A,
                Channel2_Coef_speed_B,
                Channel2_Coef_4-20_A,
                Channel2_Coef_4-20_B,
                Channel2_Coef_T_A,
                Channel2_Coef_T_B,
                Channel2_T-Type,

                Channel3_Coef_acc_A,
                Channel3_Coef_acc_B,
                Channel3_Coef_speed_A,
                Channel3_Coef_speed_B,
                Channel3_Coef_4-20_A,
                Channel3_Coef_4-20_B,
                Channel3_Coef_T_A,
                Channel3_Coef_T_B,
                Channel3_T-Type,

                Channel4_Coef_acc_A,
                Channel4_Coef_acc_B,
                Channel4_Coef_speed_A,
                Channel4_Coef_speed_B,
                Channel4_Coef_4-20_A,
                Channel4_Coef_4-20_B,
                Channel4_Coef_T_A,
                Channel4_Coef_T_B,
                Channel4_T-Type
            )
            VALUES
            (
                @UserName,
                @Date,
                @Setting,
                @StartTime,
                @EndTime,
                @DeviceName,
                @TimeSetting,
                @SerialNumber,
                @OrderNumber,

                @Channel1_Coef_acc_A,
                @Channel1_Coef_acc_B,
                @Channel1_Coef_speed_A,
                @Channel1_Coef_speed_B,
                @Channel1_Coef_4_20_A,
                @Channel1_Coef_4_20_B,
                @Channel1_Coef_T_A,
                @Channel1_Coef_T_B,
                @Channel1_T_Type,

                @Channel2_Coef_acc_A,
                @Channel2_Coef_acc_B,
                @Channel2_Coef_speed_A,
                @Channel2_Coef_speed_B,
                @Channel2_Coef_4_20_A,
                @Channel2_Coef_4_20_B,
                @Channel2_Coef_T_A,
                @Channel2_Coef_T_B,
                @Channel2_T_Type,

                @Channel3_Coef_acc_A,
                @Channel3_Coef_acc_B,
                @Channel3_Coef_speed_A,
                @Channel3_Coef_speed_B,
                @Channel3_Coef_4_20_A,
                @Channel3_Coef_4_20_B,
                @Channel3_Coef_T_A,
                @Channel3_Coef_T_B,
                @Channel3_T_Type,

                @Channel4_Coef_acc_A,
                @Channel4_Coef_acc_B,
                @Channel4_Coef_speed_A,
                @Channel4_Coef_speed_B,
                @Channel4_Coef_4_20_A,
                @Channel4_Coef_4_20_B,
                @Channel4_Coef_T_A,
                @Channel4_Coef_T_B,
                @Channel4_T_Type
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            await using var connection = new MySqlConnection(Connection);
            await using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", parameters.UserName);
            command.Parameters.AddWithValue("@Date", parameters.Date);
            command.Parameters.AddWithValue("@Setting", parameters.Setting);
            command.Parameters.AddWithValue("@StartTime", parameters.StartTime);
            command.Parameters.AddWithValue("@EndTime", parameters.EndTime);
            command.Parameters.AddWithValue("@DeviceName", parameters.DeviceName);
            command.Parameters.AddWithValue("@TimeSetting", parameters.TimeSetting);
            command.Parameters.AddWithValue("@SerialNumber", parameters.SerialNumber);
            command.Parameters.AddWithValue("@OrderNumber", parameters.OrderNumber);

            command.Parameters.AddWithValue("@Channel1_Coef_acc_A", parameters.Channel1_Coef_acc_A);
            command.Parameters.AddWithValue("@Channel1_Coef_acc_B", parameters.Channel1_Coef_acc_B);
            command.Parameters.AddWithValue("@Channel1_Coef_speed_A", parameters.Channel1_Coef_speed_A);
            command.Parameters.AddWithValue("@Channel1_Coef_speed_B", parameters.Channel1_Coef_speed_B);
            command.Parameters.AddWithValue("@Channel1_Coef_4_20_A", parameters.Channel1_Coef_4_20_A);
            command.Parameters.AddWithValue("@Channel1_Coef_4_20_B", parameters.Channel1_Coef_4_20_B);
            command.Parameters.AddWithValue("@Channel1_Coef_T_A", parameters.Channel1_Coef_T_A);
            command.Parameters.AddWithValue("@Channel1_Coef_T_B", parameters.Channel1_Coef_T_B);
            command.Parameters.AddWithValue("@Channel1_T_Type", parameters.Channel1_T_Type);

            command.Parameters.AddWithValue("@Channel2_Coef_acc_A", parameters.Channel2_Coef_acc_A);
            command.Parameters.AddWithValue("@Channel2_Coef_acc_B", parameters.Channel2_Coef_acc_B);
            command.Parameters.AddWithValue("@Channel2_Coef_speed_A", parameters.Channel2_Coef_speed_A);
            command.Parameters.AddWithValue("@Channel2_Coef_speed_B", parameters.Channel2_Coef_speed_B);
            command.Parameters.AddWithValue("@Channel2_Coef_4_20_A", parameters.Channel2_Coef_4_20_A);
            command.Parameters.AddWithValue("@Channel2_Coef_4_20_B", parameters.Channel2_Coef_4_20_B);
            command.Parameters.AddWithValue("@Channel2_Coef_T_A", parameters.Channel2_Coef_T_A);
            command.Parameters.AddWithValue("@Channel2_Coef_T_B", parameters.Channel2_Coef_T_B);
            command.Parameters.AddWithValue("@Channel2_T_Type", parameters.Channel2_T_Type);

            command.Parameters.AddWithValue("@Channel3_Coef_acc_A", parameters.Channel3_Coef_acc_A);
            command.Parameters.AddWithValue("@Channel3_Coef_acc_B", parameters.Channel3_Coef_acc_B);
            command.Parameters.AddWithValue("@Channel3_Coef_speed_A", parameters.Channel3_Coef_speed_A);
            command.Parameters.AddWithValue("@Channel3_Coef_speed_B", parameters.Channel3_Coef_speed_B);
            command.Parameters.AddWithValue("@Channel3_Coef_4_20_A", parameters.Channel3_Coef_4_20_A);
            command.Parameters.AddWithValue("@Channel3_Coef_4_20_B", parameters.Channel3_Coef_4_20_B);
            command.Parameters.AddWithValue("@Channel3_Coef_T_A", parameters.Channel3_Coef_T_A);
            command.Parameters.AddWithValue("@Channel3_Coef_T_B", parameters.Channel3_Coef_T_B);
            command.Parameters.AddWithValue("@Channel3_T_Type", parameters.Channel3_T_Type);

            command.Parameters.AddWithValue("@Channel4_Coef_acc_A", parameters.Channel4_Coef_acc_A);
            command.Parameters.AddWithValue("@Channel4_Coef_acc_B", parameters.Channel4_Coef_acc_B);
            command.Parameters.AddWithValue("@Channel4_Coef_speed_A", parameters.Channel4_Coef_speed_A);
            command.Parameters.AddWithValue("@Channel4_Coef_speed_B", parameters.Channel4_Coef_speed_B);
            command.Parameters.AddWithValue("@Channel4_Coef_4_20_A", parameters.Channel4_Coef_4_20_A);
            command.Parameters.AddWithValue("@Channel4_Coef_4_20_B", parameters.Channel4_Coef_4_20_B);
            command.Parameters.AddWithValue("@Channel4_Coef_T_A", parameters.Channel4_Coef_T_A);
            command.Parameters.AddWithValue("@Channel4_Coef_T_B", parameters.Channel4_Coef_T_B);
            command.Parameters.AddWithValue("@Channel4_T_Type", parameters.Channel4_T_Type);

            await connection.OpenAsync();

            int newId = (int)await command.ExecuteScalarAsync();
        }
        public static async Task CreateTableSettings()
        {
            using MySqlConnection conect = new MySqlConnection(Connection);
            await conect.OpenAsync();

            string mes = $@"CREATE TABLE SettingsPLC
            (
                    SettingId INT IDENTITY(1,1)  NOT NULL PRIMARY KEY,
                    UserName TEXT NOT NULL,
                    Date TEXT NOT NULL,
                    Setting TEXT NOT NULL,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL,
                    DeviceName TEXT NOT NULL,
                    TimeSetting TEXT NOT NULL,
                    SerialNumber INT NOT NULL,
                    OrderNumber TEXT NOT NULL,
                    Channel1_Coef_acc_A TEXT NOT NULL,
                    Channel1_Coef_acc_B TEXT NOT NULL,
                    Channel1_Coef_speed_A TEXT NOT NULL,
                    Channel1_Coef_speed_B TEXT NOT NULL,
                    Channel1_Coef_4-20_A TEXT NOT NULL,
                    Channel1_Coef_4-20_B TEXT NOT NULL,
                    Channel1_Coef_T_A TEXT NOT NULL,
                    Channel1_Coef_T_B TEXT NOT NULL,
                    Channel1_T-Type TEXT NOT NULL,
                    Channel2_Coef_acc_A TEXT NOT NULL,
                    Channel2_Coef_acc_B TEXT NOT NULL,
                    Channel2_Coef_speed_A TEXT NOT NULL,
                    Channel2_Coef_speed_B TEXT NOT NULL,
                    Channel2_Coef_4-20_A TEXT NOT NULL,
                    Channel2_Coef_4-20_B TEXT NOT NULL,
                    Channel2_Coef_T_A TEXT NOT NULL,
                    Channel2_Coef_T_B TEXT NOT NULL,
                    Channel2_T-Type TEXT NOT NULL,
                    Channel3_Coef_acc_A TEXT NOT NULL,
                    Channel3_Coef_acc_B TEXT NOT NULL,
                    Channel3_Coef_speed_A TEXT NOT NULL,
                    Channel3_Coef_speed_B TEXT NOT NULL,
                    Channel3_Coef_4-20_A TEXT NOT NULL,
                    Channel3_Coef_4-20_B TEXT NOT NULL,
                    Channel3_Coef_T_A TEXT NOT NULL,
                    Channel3_Coef_T_B TEXT NOT NULL,
                    Channel3_T-Type TEXT NOT NULL,
                    Channel4_Coef_acc_A TEXT NOT NULL,
                    Channel4_Coef_acc_B TEXT NOT NULL,
                    Channel4_Coef_speed_A TEXT NOT NULL,
                    Channel4_Coef_speed_B TEXT NOT NULL,
                    Channel4_Coef_4-20_A TEXT NOT NULL,
                    Channel4_Coef_4-20_B TEXT NOT NULL,
                    Channel4_Coef_T_A TEXT NOT NULL,
                    Channel4_Coef_T_B TEXT NOT NULL,
                    Channel4_T-Type TEXT NOT NULL,
                    FOREIGN KEY (SerialNumber) REFERENCES CratePLC (SerialNumber)
            )";
            using MySqlCommand command = new MySqlCommand(mes, conect);
            int rows = command.ExecuteNonQuery();
            await conect.CloseAsync();
        }
        public static async Task CreateTableCratePLC()
        {
            using MySqlConnection conect = new MySqlConnection(Connection);
            await conect.OpenAsync();

            string mes = $"CREATE TABLE {CratePLCTable} (SerialNumber INT NOT NULL PRIMARY KEY,OrderNumber TEXT NOT NULL,FamilyName TEXT NOT NULL)";
            using MySqlCommand command = new MySqlCommand(mes, conect);
            int rows = command.ExecuteNonQuery();
            await conect.CloseAsync();
        }
        public static async Task TableExistsSettingsAsync()
        {
            string schema = "dbo";
            string tableName = SettingsTable;
            const string query = @"
            SELECT COUNT(*)
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = @schema
              AND TABLE_NAME = @tableName";

            using var connection = new MySqlConnection(Connection);
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@schema", schema);
            command.Parameters.AddWithValue("@tableName", tableName);

            await connection.OpenAsync();

            int count = (int)await command.ExecuteScalarAsync();
            if(count == 0)
            {
               await CreateTableSettings();
            }
            await connection.CloseAsync();
        }
        public static async Task TableExistsCratePLCAsync()
        {
            string schema = "dbo";
            string tableName = CratePLCTable;
            const string query = @"
            SELECT COUNT(*)
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = @schema
              AND TABLE_NAME = @tableName";

            using var connection = new MySqlConnection(Connection);
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@schema", schema);
            command.Parameters.AddWithValue("@tableName", tableName);

            await connection.OpenAsync();

            int count = (int)await command.ExecuteScalarAsync();
            if (count == 0)
            {
                await CreateTableCratePLC();
            }
            await connection.CloseAsync();
        }
    }
}
