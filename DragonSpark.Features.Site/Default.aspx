<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DragonSpark.Features.Site.Default" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
	<%--<meta id="FacebookApplicationId" content="<%= FacebookContext.Current.AppId %>" />
	<meta id="FacebookPermissions" content="<%= ServiceLocator.Current.GetInstance<string>( "Application.Permissions" ) %>" />--%>
	<title>The DragonSpark Application Framework: Framework Featureset Application</title>
	
	<style type="text/css">
		html, body {
		height: 100%;
		overflow:hidden;
	}
	body {
		padding: 0;
		margin: 0;
	}
	#silverlightControlHost {
		height: 100%;
		text-align:center;
	}
	</style>
	<script type="text/javascript" src="<%= ResolveUrl( "~/Scripts/Silverlight.js" ) %>"></script>
</head>
<body>
	<form id="form1" runat="server" style="height: 100%">
		<div id="silverlightControlHost">
			<object id="Application" data="data:application/x-silverlight-2," type="application/x-silverlight-2"
				width="100%" height="100%">
				<param name="source" value="ClientBin/DragonSpark.Features.Application.xap" />
				<param name="onError" value="onSilverlightError" />
				<param name="background" value="white" />
				<param name="minRuntimeVersion" value="5.0.61118.0" />
				<%--<param name="windowless" value="true" />--%>
				<param name="initParams" value="DragonSpark.Application.ClientExceptionDetail.IpAddress=<%= Request.ServerVariables["REMOTE_ADDR"] %>, DragonSpark.Application.Presentation.Entity.Security.AuthenticationStatusMonitor.LoginUri=<%= ConfigurationManager.AppSettings[ "DragonSpark.Application.Presentation.Entity.Security.AuthenticationStatusMonitor.LoginUri" ] %>" />
				<param name="uiculture" value="<%= System.Threading.Thread.CurrentThread.CurrentUICulture %>" />
				<param name="culture" value="<%= System.Threading.Thread.CurrentThread.CurrentCulture %>" />
				<param name="autoUpgrade" value="true" />
				<a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration: none">
					<img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style: none" />
				</a>
			</object>
			<iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px; border: 0px"></iframe>
		</div>
		<asp:ScriptManager ID="ScriptManager" runat="server" EnableHistory="true" EnableSecureHistoryState="false"></asp:ScriptManager>
	</form>
	<%--
	<div id="fb-root"></div>
	<script type="text/javascript" src="<%= ResolveUrl( "~/Scripts/Facebook.js" ) %>"></script>
	<div id="FacebookConnectAuthenticationFrame" style="position:absolute; height:31px;width:176px;border:0px">
		<a href="#" onclick="Connect();return false;"><img src="Images/Connect.png" width="176" height="31" alt="Connect with Facebook, Dawg." border="0" /></a>
	</div>--%>
</body>
</html>