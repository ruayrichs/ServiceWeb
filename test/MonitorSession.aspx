<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitorSession.aspx.cs" Inherits="ServiceWeb.test.MonitorSession" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <!-- Bootstrap Core CSS -->
    <link href="/vendor/bootstrap/css/bootstrap.min.css?vs=20180618" rel="stylesheet" type="text/css">

    <!-- DataTables CSS -->
    <link href="/vendor/datatables/css/dataTables.bootstrap4.min.css?vs=20180625" rel="stylesheet" type="text/css" />

    <!-- jQuery -->
    <script src="/vendor/jquery/jquery.min.js?vs=20190113" type="text/javascript"></script>

    <!-- DataTables JavaScript -->
    <script src="/vendor/datatables/js/jquery.dataTables.min.js?vs=20190113" type="text/javascript"></script>

    <!-- Bootstrap JavaScript -->
    <script src="/vendor/datatables/js/dataTables.bootstrap4.min.js?vs=20190113" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="container-x" style="padding: 10px 50px;">
            <div class="table-responsive-x">
                <table class="table table-hover small">
                    <tr>
                        <td style="width: 100px;">no</td>
                        <td>key</td>
                        <td style="width: 100px;">size(kb)</td>
                    </tr>

                    <%
                        long totalSessionBytes = 0;
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter b =
                                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        System.IO.MemoryStream m;
                        for (int i = 0; i < HttpContext.Current.Session.Keys.Count; i++)
                        {
                            String key = HttpContext.Current.Session.Keys[i];
                            var obj = Session[key];
                            m = new System.IO.MemoryStream();
                            if (obj != null)
                            {
                                b.Serialize(m, obj);
                                totalSessionBytes += m.Length;
                            }
                

                    %>
                    <tr>
                        <td><%=i%></td>
                        <td><%=key%></td>
                        <td style="text-align: right; width: 200px;"><%=m.Length / 1024%>  KB </td>

                    </tr>
                    <%   } %>
                </table>
            </div>
            Total size <%=totalSessionBytes/1024 %> KB
        </div>
    </form>
</body>
</html>
