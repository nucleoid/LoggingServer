function Get-Git-Commit
{
	$gitLog = git log --oneline -1
	return $gitLog.Split(' ')[0]
}

function Generate-Assembly-Info
{
param(
	[string]$clsCompliant = "true",
	[string]$title, 
	[string]$description, 
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$guid,
	[string]$version,
	[string]$internals,
	[string]$file = $(throw "file is a required parameter.")
)
  $commit = Get-Git-Commit
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: CLSCompliantAttribute($clsCompliant )]
[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyTitleAttribute(""$title"")]
[assembly: AssemblyDescriptionAttribute(""$description"")]
[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: Guid(""$guid"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyInformationalVersionAttribute(""$version / $commit"")]
[assembly: AssemblyDelaySignAttribute(false)]
"
if($internals)
{
	$asmInfo = $asmInfo + "
[assembly: InternalsVisibleTo(""$internals"")]
";
}


	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $file"
	out-file -filePath $file -encoding UTF8 -inputObject $asmInfo
}