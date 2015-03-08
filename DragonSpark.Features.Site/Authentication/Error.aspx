<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="DragonSpark.Features.Site.Authentication.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>An Error Occured During Authentication</title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<div>
				<asp:Label Text="<%# Details.identityProvider %>" runat="server"></asp:Label>
			</div>
		
			<div>
				<asp:Label Text="<%# Message %>" runat="server"></asp:Label>
			</div>
		</div>
	</form>
</body>
</html>
