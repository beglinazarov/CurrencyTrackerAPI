using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CurrencyTracker.API.Extensions
{
	public static class ExportResponseFormatterExtension
	{
		public static byte[] ExportToXML<T>(IList<T> data, string exportFilePath)
		{
			exportFilePath = !exportFilePath.Equals("") ? exportFilePath : $"C:\\Currency_{DateTime.Today.ToShortDateString()}.xml";

			DataTable dtTest = ToDataTable(data);

			MemoryStream stream = new MemoryStream();

			WriteDataTable(dtTest, stream, XmlWriteMode.IgnoreSchema);
			string result = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);

			var content = stream.ToArray();
			return content;
		}
		
		public static StringBuilder ExportToCsv<T>(this IList<T> data, string filePath = "")
		{
			filePath = !filePath.Equals("") || filePath.Equals("string") ? 
						filePath : @"C:\\Currency.csv";

			DataTable dt = ToDataTable(data);

			StringBuilder sb = new StringBuilder();

			IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
											  Select(column => column.ColumnName);
			sb.AppendLine(string.Join(",", columnNames));

			foreach (DataRow row in dt.Rows)
			{
				IEnumerable<string> fields = row.ItemArray.Select(field =>
				  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
				sb.AppendLine(string.Join(",", fields));
			}

			using (StreamWriter objWriter = new StreamWriter(filePath))
			{
				objWriter.WriteLine(sb);
			}

			return sb;
		}

		public static DataTable ToDataTable<T>(this IList<T> data)
		{
			PropertyDescriptorCollection props =
			TypeDescriptor.GetProperties(typeof(T));
			DataTable table = new DataTable();
			for (int i = 0; i < props.Count; i++)
			{
				PropertyDescriptor prop = props[i];
				table.Columns.Add(prop.Name, prop.PropertyType);
			}
			object[] values = new object[props.Count];
			foreach (T item in data)
			{
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = props[i].GetValue(item);
				}
				table.Rows.Add(values);
			}
			return table;
		}

		public static byte[] ExportToXL<T>(IList<T> data, string exportFilePath)
		{
			exportFilePath = !exportFilePath.Equals("") || 
				!exportFilePath.Equals("string") ? exportFilePath : $"C:\\Currency_{DateTime.Today.ToShortDateString()}.xlsx";


			DataTable dt = ToDataTable(data);
			dt.TableName = $"Currency-{DateTime.Today.ToShortDateString()}";
			
			using XLWorkbook woekBook = new XLWorkbook();
			woekBook.Worksheets.Add(dt);
			
			using MemoryStream stream = new MemoryStream();
			woekBook.SaveAs(stream);

			FileStream file = new FileStream(exportFilePath, FileMode.Create, FileAccess.Write);
			stream.WriteTo(file);
			file.Close();
			
			var content = stream.ToArray();
			return content;
		}

		public static void WriteDataTable(DataTable dt, Stream stm,
									  XmlWriteMode mode)
		{
			DataSet tmp = CreateTempDataSet(dt);
			tmp.WriteXml(stm, mode);
		}

		private static DataSet CreateTempDataSet(DataTable dt)
		{
			// Create a temporary DataSet
			DataSet ds = new DataSet("Currency");
			ds.Tables.Add(dt.Copy());
			return ds;
		}
	}
}
