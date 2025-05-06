using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        public (int errorCodeNum, int memberId) AddMember(AddMemberDto addMemberDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addMember @account, @hash, @email, @phone, @errorCode OUTPUT";

                cmd.Parameters.Add("@account", SqlDbType.VarChar).Value = addMemberDto.Account;
                cmd.Parameters.Add("@hash", SqlDbType.VarChar).Value = addMemberDto.Hash;
                cmd.Parameters.Add("@phone", SqlDbType.Int).Value = addMemberDto.Phone;

                if (addMemberDto.Email == null) cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = String.Empty;
                else cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = addMemberDto.Email;

                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                int memberId = (int)cmd.ExecuteScalar();

                int errorCode = (int)errorCodeOutput.Value;
                return (errorCode, memberId);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }
    }
}
