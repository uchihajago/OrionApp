#Region "Security"
    Private Function IsSQLInjection(ByVal ParamArray Collection() As String) As Boolean
        Dim val As Boolean = False
        If Collection.Length > 0 Then
            For i As Integer = 0 To UBound(Collection, 1)
                If ValidateString(Collection(i)) Then
                    val = True
                    Exit For
                End If
            Next i
        Else
            val = True
        End If
        Return val
    End Function
    Private Function IsSQLInjection(ByVal ls As List(Of String)) As Boolean
        Dim val As Boolean = False
        If ls.Count > 0 Then
            For i = 0 To (ls.Count - 1)
                If ValidateString(ls(i)) Then
                    val = True
                    Exit For
                End If
            Next
        Else
            val = True
        End If
        Return val
    End Function
    Private Function IsSQLInjection(ByVal param As String, Optional ByVal isPath As Boolean = False) As Boolean
        Dim re As Boolean = False
        If isPath Then
            If System.IO.File.Exists(param) Then
                re = True
            End If
        Else
            re = ValidateString(param)
        End If
        Return re
    End Function
    Private Function ValidateString(ByVal param As String) As Boolean
        Dim isSQLInjection As Boolean = False
        Dim sqlCheckList As String() = {"--", ";--", ";", "/*", "*/", "@@", _
          "@", "char", "nchar", "varchar", "nvarchar", "alter", _
          "begin", "cast", "create", "cursor", "declare", "delete", _
          "drop", "end", "exec", "execute", "fetch", "insert", _
          "kill", "select", "sys", "sysobjects", "syscolumns", "table", _
          "update"}
        Dim CheckString As String = param.Replace("'", "''")
        Dim i As Integer = 0
        While i <= sqlCheckList.Length - 1
            If (CheckString.IndexOf(sqlCheckList(i), StringComparison.OrdinalIgnoreCase) >= 0) Then
                isSQLInjection = True
            End If
            System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        End While
        Return isSQLInjection
    End Function
#End Region
