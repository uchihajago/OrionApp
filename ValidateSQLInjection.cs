class IsSQLInjec
{
    private bool IsSQLInjection(params string[] Collection)
    {
        bool val = false;
        if (Collection.Length > 0)
        {
            for (int i = 0; i <= Information.UBound(Collection, 1); i++)
            {
                if (ValidateString(Collection[i]))
                {
                    val = true;
                    break;
                }
            }
        }
        else
            val = true;
        return val;
    }
    private bool IsSQLInjection(List<string> ls)
    {
        bool val = false;
        if (ls.Count > 0)
        {
            for (var i = 0; i <= (ls.Count - 1); i++)
            {
                if (ValidateString(ls[i]))
                {
                    val = true;
                    break;
                }
            }
        }
        else
            val = true;
        return val;
    }
    private bool IsSQLInjection(string param, bool isPath = false)
    {
        bool re = false;
        if (isPath)
        {
            if (System.IO.File.Exists(param))
                re = true;
        }
        else
            re = ValidateString(param);
        return re;
    }
    private bool ValidateString(string param)
    {
        bool isSQLInjection = false;
        string[] sqlCheckList = new[] { "--", ";--", ";", "/*", "*/", "@@", "@", "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast", "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute", "fetch", "insert", "kill", "select", "sys", "sysobjects", "syscolumns", "table", "update" };
        string CheckString = param.Replace("'", "''");
        int i = 0;
        while (i <= sqlCheckList.Length - 1)
        {
            if ((CheckString.IndexOf(sqlCheckList[i], StringComparison.OrdinalIgnoreCase) >= 0))
                isSQLInjection = true;
            System.Math.Max(System.Threading.Interlocked.Increment(ref i), i - 1);
        }
        return isSQLInjection;
    }
}
