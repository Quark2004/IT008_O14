using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
    internal class managercourseid
    {
        private string id;
        private string name;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }

        public managercourseid(DataRow row)
        {
            this.Id = row["MSSV/MGV"].ToString();
            this.Name = row["Họ tên"].ToString();

        }

        public managercourseid(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}