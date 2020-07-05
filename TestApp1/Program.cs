using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.XPath;

namespace ConsoleApp1
{
	class Program
	{
		
		static void Main(string[] args)
		{

			string StrCreateData;
			string connString = @"Data Source=DESKTOP-UIMHQGP\SQLEXPRESS;Initial Catalog=TestProjectDataBase;Integrated Security=True";
			//SqlConnection myConn = new SqlConnection(@"Data Source=" + @"DESKTOP-UIMHQGP\SQLEXPRESS" + ";Initial Catalog=" + "TestProjectDataBase" + ";Integrated Security=true;Enlist=False");
			SqlConnection myConn = new SqlConnection(connString);

			/*Считывание данных из xml файла*/
			var doc = new XmlDocument();                                                               // Создаем экземпляр Xml документа.			
			doc.Load("YML.xml");                                                                       // Загружаем данные из файла.			

			var root = doc.DocumentElement;                                                            // Получаем корневой элемент документа.			
			XmlNodeList childnodes = root.SelectNodes("shop/offers/offer");
			var ColumnData = new List<string>();

			/*Получаем столбцы таблицы базы данных*/
			foreach (XmlNode m in childnodes)
			{
				foreach (XmlNode n in m)
				{
					if (!ColumnData.Contains(n.Name))
					{ ColumnData.Add(n.Name); }
				}
			}

			//foreach (String s in ColumnData)
			//{
			//	Console.WriteLine(s);
			//}			

			/*Подготовка запроса создания столбцов для таблицы*/
			StrCreateData = "CREATE TABLE TestTable (";
			foreach (string sss in ColumnData)
			{
				StrCreateData = StrCreateData + sss + " TEXT, ";
			}
			StrCreateData = StrCreateData.Remove(StrCreateData.Length - 2) + ")";


			/*Создание таблицы*/
			try
			{
				Console.WriteLine("Getting Connection ...");
				myConn.Open();
				SqlCommand myCommand = new SqlCommand(StrCreateData, myConn);   //Создание новой таблицы 
				myCommand.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine("Error: " + e.Message);
				
			}


			/*Подключение к таблице*/
			try
			{
				Console.WriteLine("Openning Connection ...");

								
				SqlCommand cmd = new SqlCommand(" ", myConn);

				string column = "";
				string text = "'";

				/*Записываем url всех offer*/
				foreach (XmlNode m in childnodes)
				{
					column = column + m.FirstChild.Name + ", ";
					text = text + m.FirstChild.InnerXml + "', '";

					column = column.Remove(column.Length - 2);
					text = text.Remove(text.Length - 3);

					cmd.CommandText = "INSERT INTO TestTable (" + column + ") VALUES (" + text + ") "; 
					cmd.ExecuteNonQuery();

					column = "";
					text = "'";
				}

				/*Заполняем данные всех offer*/

				cmd.Parameters.Add("@n_column", SqlDbType.Text);
				cmd.Parameters.Add("@n_string", SqlDbType.VarChar);

				foreach (XmlNode m in childnodes)
				{
					foreach (XmlNode n in m)
					{
						if (n.Name != "url")
						{
							cmd.Parameters["@n_column"].Value = n.InnerXml;
							cmd.Parameters["@n_string"].Value = m.FirstChild.InnerXml;
							cmd.CommandText = "UPDATE TestTable SET " + n.Name + " = @n_column WHERE CONVERT(VARCHAR (MAX), url) = @n_string;";
							cmd.ExecuteNonQuery();
						}
							
					}
				}

				Console.WriteLine("Connection successful!");

			}
			catch (Exception e)
			{
				Console.WriteLine("Error: " + e.Message);
			}
			/*Закрытие таблицы*/
			finally
			{
				if (myConn.State == ConnectionState.Open)
				{
					myConn.Close();
				}
			}

			Console.Read();
		}
	}
}
