<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:i="http://www.w3.org/2001/XMLSchema-instance"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:local="clr-namespace:DevelopersWin.VoteReporter;assembly=DevelopersWin.VoteReporter" exclude-result-prefixes="msxsl i local"
>
	<xsl:output method="html" indent="no" />

	<xsl:template match="local:VoteReport">TITLE:
======
Weekly Vote Report for <xsl:value-of select="msxsl:format-date( @Created, 'dddd, MMMM d, yyyy')" />

BODY:
=====
| Title 	| Last Week 	| <span class="new">New Votes</span> 	| Total 	|
|-------	|:-----------:|:-----------:|:-------:|
<xsl:apply-templates select="local:VoteReport.Groups/local:VoteGroupView" /><xsl:text>&#10;</xsl:text>

<xsl:call-template name="Summary" /></xsl:template>

	<xsl:template match="local:VoteGroupView">| **<xsl:value-of select="@Title" />** | **<xsl:value-of select="number(.//local:VoteCount/@Count)-number(.//local:VoteCount/@Delta)" />** | <span class="new">**<xsl:value-of select=".//local:VoteCount/@Delta" />**</span> | **<xsl:value-of select=".//local:VoteCount/@Count" />** |<xsl:text>&#10;</xsl:text><xsl:apply-templates select="local:VoteGroupView.Votes/local:VoteView" /></xsl:template>
	
	<xsl:template match="local:VoteView">| [<xsl:value-of select="@Title" />](<xsl:value-of select="@Location" />) | <xsl:value-of select="number(.//local:VoteCount/@Count)-number(.//local:VoteCount/@Delta)" /> | <span class="new"><xsl:value-of select=".//local:VoteCount/@Delta" /></span> | **<xsl:value-of select=".//local:VoteCount/@Count" />** |<xsl:text>&#10;</xsl:text></xsl:template>

	<xsl:template name="Summary">Summary:<xsl:text>&#10;</xsl:text>========<xsl:text>&#10;</xsl:text><xsl:for-each select="local:VoteReport.Groups/local:VoteGroupView">- <xsl:value-of select="@Title" />: *<xsl:value-of select=".//local:VoteCount/@Count" /> Total Votes* (+<xsl:value-of select=".//local:VoteCount/@Delta" /> New)<xsl:text>&#10;</xsl:text></xsl:for-each></xsl:template>

</xsl:stylesheet>