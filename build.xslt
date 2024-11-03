<?xml version="1.0" encoding="UTF-8"?> 
<xsl:stylesheet
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
    exclude-result-prefixes="wix">

  <!-- Template to match the root node and output <Include> as the root element -->
  <xsl:template match="wix:Wix">
    <Include xmlns="http://schemas.microsoft.com/wix/2006/wi">
      <xsl:apply-templates select="node() | @*"/>
    </Include>
  </xsl:template>

  <!-- Identity template to copy all nodes by default -->
  <xsl:template match="node() | @*">
    <xsl:copy>
      <xsl:apply-templates select="node() | @*" />
    </xsl:copy>
  </xsl:template>

  <!-- Template to exclude Components where any File under them has Source containing 'runtimes', 'Config', or 'piper' -->
  <xsl:template match="wix:Component[wix:File[contains(@Source, '\runtimes\') or contains(@Source, '\Config\') or contains(@Source, '\piper\')]]">
    <!-- Do nothing, effectively removing these components -->
  </xsl:template>

  <!-- Template to exclude ComponentRefs that reference the excluded Components -->
  <xsl:template match="wix:ComponentRef[@Id = //wix:Component[wix:File[contains(@Source, '\runtimes\') or contains(@Source, '\Config\') or contains(@Source, '\piper\')]]/@Id]">
    <!-- Do nothing -->
  </xsl:template>

  <!-- Optionally, exclude empty directories -->
  <xsl:template match="wix:Directory[not(node())]">
    <!-- Do nothing, removing empty directories -->
  </xsl:template>

  <xsl:template match="wix:Fragment[wix:DirectoryRef]">
    <!-- Do nothing, effectively removing these Fragments -->
  </xsl:template>
</xsl:stylesheet>