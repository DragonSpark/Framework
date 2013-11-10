var name			= "initializer",
	framework		= "../../DragonSpark.Client",
	application		= "../../DragonSpark.Application.Client",
	directory		= "Client",
	// --
	frameworkPath	= "../" + framework,
	applicationPath = "../" + application;

exports.config = function( weyland )
{
	weyland.build( name )
		.task.rjs( {
			exclude : [ framework + "/Core/**/*.js" ],
			include : [ directory + "/build/require.js", framework + "/**/*.{js,html}", application + "/**/*.{js,html}" ],
			loaderPluginExtensionMaps : {
				".html" : "text"
			},
			rjs : {
				name : name,
				insertRequire : [ name ],
				baseUrl : directory,
				paths : {
					text : frameworkPath + "/Core/text",
					durandal : frameworkPath + "/Durandal",
					dragonspark : frameworkPath,

					plugins : frameworkPath + "/Plugins",
					transitions : frameworkPath + "/Transitions",
				
					application : applicationPath,
					viewmodels : applicationPath + "/ViewModels",
					views : applicationPath + "/Views",

					breeze : "empty:",
					knockout : "empty:",
					bootstrap : "empty:",
					jquery : "empty:"
				},
				inlineText : true,
				optimize : "none",
				pragmas : {
					build : true
				},
				stubModules : [ "text" ],
				keepBuildDir : true,
				out : directory + "/application.packaged.js"
			}
		} );
}