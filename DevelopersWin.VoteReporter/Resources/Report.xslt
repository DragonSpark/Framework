<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:i="http://www.w3.org/2001/XMLSchema-instance"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:local="clr-namespace:DevelopersWin.VoteReporter;assembly=DevelopersWin.VoteReporter" exclude-result-prefixes="msxsl i local"
>
	<xsl:output method="html" indent="no" />

	<xsl:template match="local:VoteReport"><xsl:variable name="Date" select="msxsl:format-date( @Created, 'dddd, MMMM d, yyyy')" /><xsl:variable name="TotalUbiquitous" select="local:VoteReport.Groups/local:VoteGroupView[1]//local:VoteCount/@Count" />TITLE:
======
Weekly Vote Report for <xsl:value-of select="$Date" />

BODY:
=====
| Title 	| Last Week 	| <span class="new">New Votes</span> 	| Total 	|
|-------	|:-----------:|:-----------:|:-------:|
<xsl:apply-templates />
Summary:
========
<xsl:for-each select="local:VoteReport.Groups/local:VoteGroupView">- <xsl:value-of select="@Title" />: *<xsl:value-of select=".//local:VoteCount/@Count" /> Total Votes* (+<xsl:value-of select=".//local:VoteCount/@Delta" /> New)<xsl:text>&#10;</xsl:text></xsl:for-each>
Message:
========
Weekly Vote Report for <xsl:value-of select="$Date" />: <xsl:value-of select="$TotalUbiquitous" /> Cumulative Votes for a Ubiquitious .NET Client Model. 

Dark O' Thirty:
===============
@satyanadella @VisualStudio @windowsdev <xsl:value-of select="$TotalUbiquitous" /> votes for a ubiquitous #dotnet. Will you #empower your #developers?
</xsl:template>

	<xsl:template match="local:VoteGroupView">| **<xsl:value-of select="@Title" />** | **<xsl:value-of select="number(.//local:VoteCount/@Count)-number(.//local:VoteCount/@Delta)" />** | <span class="new">**<xsl:value-of select=".//local:VoteCount/@Delta" />**</span> | **<xsl:value-of select=".//local:VoteCount/@Count" />** |<xsl:text>&#10;</xsl:text><xsl:apply-templates select="local:VoteGroupView.Votes/local:VoteView" /></xsl:template>
	
	<xsl:template match="local:VoteView">| [<xsl:value-of select="@Title" />](<xsl:value-of select="@Location" />) | <xsl:value-of select="number(.//local:VoteCount/@Count)-number(.//local:VoteCount/@Delta)" /> | <span class="new"><xsl:value-of select=".//local:VoteCount/@Delta" /></span> | **<xsl:value-of select=".//local:VoteCount/@Count" />** |<xsl:text>&#10;</xsl:text></xsl:template>
</xsl:stylesheet>