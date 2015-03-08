<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DragonSpark.Features.Site.Authentication.Default" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Authentication</title>
	<script type="text/javascript">
		try 
		{
			opener.window.document.Application.content.AuthenticationStatusMonitor.Refresh();
			window.close();
		}
		catch (e) 
		{
			alert("There was a problem refreshing authentication status in the application.");
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
