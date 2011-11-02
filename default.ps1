properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\packages"
  $build_dir = "$base_dir\build"
  $build_pub_dir = "$build_dir\_PublishedWebsites"
  $sln_file = "$base_dir\LoggingServer.sln" 
  $version = "1.0.0.0"
  $humanReadableversion = "1.0"
  $tools_dir = "$base_dir\Tools"
  $release_dir = "$base_dir\Release"
} 

include .\psake_ext.ps1

task default -depends Release

task Clean { 
  remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue 
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
} 

task Init -depends Clean { 
    . .\psake_ext.ps1
    Generate-Assembly-Info `
        -file "$base_dir\LoggingServer.Common\Properties\AssemblyInfo.cs" `
        -title "LoggingServer Common $version" `
        -description "Common LoggingServer classes" `
        -company "Mitchell Statz" `
        -product "LoggingServer $version" `
        -version $version `
        -guid "a7b47a02-3d1b-41b6-b8a9-3fc9c40c5984"
        
        
    Generate-Assembly-Info `
	-file "$base_dir\LoggingServer.Interface\Properties\AssemblyInfo.cs" `
	-title "LoggingServer Interface $version" `
	-description "LoggingServer user interface" `
	-company "Mitchell Statz" `
	-product "LoggingServer $version" `
        -version $version `
        -clsCompliant "false" `
        -guid "96562976-6b23-40f5-a44f-f01c3d157461"
    
    Generate-Assembly-Info `
    	-file "$base_dir\LoggingServer.Server\Properties\AssemblyInfo.cs" `
    	-title "LoggingServer Server $version" `
    	-description "LoggingServer main logging classes" `
    	-company "Mitchell Statz" `
    	-product "LoggingServer $version" `
        -version $version `
        -clsCompliant "false" `
        -guid "1957af42-fada-4fea-80b2-a0681d0d02b7" `
        
    Generate-Assembly-Info `
        -file "$base_dir\LoggingServer.Tests\Properties\AssemblyInfo.cs" `
        -title "LoggingServer Tests $version" `
        -description "Unit tests for LoggingServer" `
        -company "Mitchell Statz" `
        -product "LoggingServer $version" `
        -version $version `
        -clsCompliant "false" `
        -guid "bd01c085-3a2c-432b-8ada-d876da6ef4f1"
        
    Generate-Assembly-Info `
	-file "$base_dir\LoggingServer.WcfService\Properties\AssemblyInfo.cs" `
	-title "LoggingServer WcfService $version" `
	-description "LoggingServer nlog wcf service" `
	-company "Mitchell Statz" `
	-product "LoggingServer $version" `
        -version $version `
        -clsCompliant "false" `
        -guid "1e10018f-05ab-466d-b17d-43ab5c0ae7ca"
        
    Generate-Assembly-Info `
    	-file "$base_dir\LoggingServer.LogTruncator\Properties\AssemblyInfo.cs" `
    	-title "LoggingServer LogTruncator $version" `
    	-description "LoggingServer log truncation windows service" `
    	-company "Mitchell Statz" `
    	-product "LoggingServer $version" `
        -version $version `
        -clsCompliant "false" `
        -guid "27875b7d-a0aa-46e3-99d5-22e489954bb7"
        
    new-item $release_dir -itemType directory 
    new-item $build_dir -itemType directory 
    new-item $build_dir\Resources -itemType directory
    cp $lib_dir\Gallio\*.* $build_dir
    cp $lib_dir\Gallio\Resources\*.* $build_dir\Resources
} 

task Compile -depends Init { 
  & msbuild "$sln_file" "/p:OutDir=$build_dir\\" /p:Configuration=Release
    if ($lastExitCode -ne 0) {
          throw "Error: Failed to execute msbuild"
  }
} 

task Test -depends Compile {
  $old = pwd
  cd $build_dir
  .\Gallio.Echo.exe "$build_dir\LoggingServer.Tests.dll"
  if ($lastExitCode -ne 0) {
          throw "Error: Failed to execute tests"
    }
  cd $old
}

task Release -depends Test {
    cp -Recurse $build_pub_dir\*.* $release_dir
}