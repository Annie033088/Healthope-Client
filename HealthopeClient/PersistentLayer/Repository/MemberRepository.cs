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

        /// <summary>
        /// 註冊會員
        /// </summary>
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

                object result = cmd.ExecuteScalar();
                int memberId = -1;

                if (result != null) memberId = (int)result;

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

        /// <summary>
        /// 取得會員手機
        /// </summary>
        public (int errorCodeNum, int phone) GetPhoneAtVerifyPhone(int memberId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getPhoneAtVerifyPhone @memberId, @errorCode OUTPUT";

                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = memberId;

                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                object result = cmd.ExecuteScalar();
                int phone = -1;

                if (result != null) phone = (int)result;

                int errorCode = (int)errorCodeOutput.Value;
                return (errorCode, phone);
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
        // TODO: 待驗證
        /// <summary>
        /// 修改會員手機認證狀態 ( 若第三方 OTP 簡訊服務商回傳成功的話，改為驗證通過 )
        /// </summary>
        public bool EditPhoneVerified(int memberId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editPhoneVerified @memberId";

                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = memberId;

                cmd.Connection.Open();

                cmd.ExecuteNonQuery();
                return true;
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
