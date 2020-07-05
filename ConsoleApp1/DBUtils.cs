using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ConsoleApp1
{
	class DBUtils
	{
		public static SqlConnection GetDBConnection()
		{
			string datasource = @"DESKTOP-UIMHQGP\SQLEXPRESS";
			string database = "TestProjectDataBase";
			/*string username = "maximnn1720@gmail.com";
			string password = "1720Maximnn";*/
			return DBSQLServerUtils.GetDBConnection(datasource, database/*, username, password*/);
		}
	}
}
