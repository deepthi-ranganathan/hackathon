using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.MultiTranTeller.Base
{
    public static class DataRowExtension
    {
        public static string GetStringValue( this DataRow a, string columnName)
        {
            int index = a.Table.Columns.IndexOf(columnName);
            if (index < 0 || a.IsNull(index))
                return null;
            return Convert.ToString(a[index]);
        }

        public static decimal GetDecimalValue(this DataRow a, string columnName)
        {
            int index = a.Table.Columns.IndexOf(columnName);
            if (index < 0 || a.IsNull(index))
                return decimal.MinValue;
            return Convert.ToDecimal(a[index]);
        }

        public static int GetIntValue(this DataRow a, string columnName)
        {
            int index = a.Table.Columns.IndexOf(columnName);
            if (index < 0 || a.IsNull(index))
                return int.MinValue;
            return Convert.ToInt32(a[index]);
        }
    }
}
