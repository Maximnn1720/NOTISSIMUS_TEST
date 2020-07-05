using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ConsoleApp1
{
	class DBSQLServerUtils
	{
		public static SqlConnection
		GetDBConnection(string datasource, string database/*, string username, string password*/)
		{
			//
			// Data Source=DESKTOP-UIMHQGP\SQLEXPRESS;Initial Catalog=TestDataBase;Integrated Security=True
			//
			//string connString = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;
			string connString = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=true";
			SqlConnection conn = new SqlConnection(connString);
			return conn;
		}
	}
}
