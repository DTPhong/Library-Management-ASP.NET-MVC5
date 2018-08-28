using Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Lib.Code
{
    public class AccountModel
    {
        private ModelContext context = null;
        public AccountModel()
        {
            context = new ModelContext();
        }
        public bool Login(string username, string password)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@name",username),
                new SqlParameter("@password",password),
            };
            var res = context.Database.SqlQuery<bool>("up_Student_Login @name,@password", sqlParams).SingleOrDefault();
            return res;
        }

        public bool LoginAdmin(string username, string password)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@AdminName",username),
                new SqlParameter("@Password",password),
            };
            var res = context.Database.SqlQuery<bool>("up_Admin_Login @AdminName,@Password", sqlParams).SingleOrDefault();
            return res;
        }
    }
}