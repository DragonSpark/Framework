namespace DragonSpark.Application.AspNet.Middleware.Agents;

sealed class ChallengeContent : Text.Text
{
	public static ChallengeContent Default { get; } = new();

	ChallengeContent()
		: base("""
		       <!DOCTYPE html>
		       <html>
		       <head>
		       	<meta charset="utf-8" />
		       	<meta http-equiv="refresh" content="0" />
		       	<title>Loading...</title>
		       </head>
		       <body></body>
		       </html>
		       """) {}
}