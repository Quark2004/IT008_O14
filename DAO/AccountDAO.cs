using System.Data;

namespace QLSV.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get
            {
                if (instance == null) instance = new AccountDAO();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
        private AccountDAO() { }

        bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }

        public DataTable GetAccount(string userName, string password)
        {
            string query = "SELECT * FROM \"Login\"( :userName )";

            DataTable res = DataProvider.Instance.ExcuteQuery(query, new object[] { userName });

            if (res.Rows.Count > 0)
            {
                string hashedPassword = res.Rows[0]["password"].ToString();

                if (!VerifyPassword(password, hashedPassword))
                {
                    res.Rows.Clear();
                }

            }

            return res;
        }

        public bool Login(string userName, string password)
        {
            return GetAccount(userName, password).Rows.Count > 0;
        }
        public string GetRole(string userName, string password)
        {
            string res = "";

            DataTable account = GetAccount(userName, password);

            if (account.Rows.Count > 0)
            {
                res = account.Rows[0]["role"].ToString();
            }

            return res;
        }
        public string GetId(string username, string password)
        {
            string res = "";

            DataTable account = GetAccount(username, password);

            if (account.Rows.Count > 0)
            {
                res = account.Rows[0]["id"].ToString();
            }

            return res;
        }
    }
}
