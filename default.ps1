properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\packages"
  $build_dir = "$base_dir\build"
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
        -version $version
        
    Generate-Assembly-Info `
	-file "$base_dir\LoggingServer.Interface\Properties\AssemblyInfo.cs" `
	-title "LoggingServer Interface $version" `
	-description "LoggingServer user interface" `
	-company "Mitchell Statz" `
	-product "LoggingServer $version" `
        -version $version
    
    Generate-Assembly-Info `
    	-file "$base_dir\LoggingServer.Server\Properties\AssemblyInfo.cs" `
    	-title "LoggingServer Server $version" `
    	-description "LoggingServer main logging classes" `
    	-company "Mitchell Statz" `
    	-product "LoggingServer $version" `
        -version $version
        
    Generate-Assembly-Info `
        -file "$base_dir\LoggingServer.Tests\Properties\AssemblyInfo.cs" `
        -title "LoggingServer Tests $version" `
        -description "Unit tests for LoggingServer" `
        -company "Mitchell Statz" `
        -product "LoggingServer $version" `
        -version $version `
        -clsCompliant "false" `  
        
    Generate-Assembly-Info `
	-file "$base_dir\LoggingServer.WcfService\Properties\AssemblyInfo.cs" `
	-title "LoggingServer WcfService $version" `
	-description "LoggingServer nlog wcf service" `
	-company "Mitchell Statz" `
	-product "LoggingServer $version" `
        -version $version    
        
    new-item $release_dir -itemType directory 
    new-item $build_dir -itemType directory 
    cp $lib_dir\Gallio\*.* $build_dir
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
  exec ".\Gallio.Echo.exe" "$build_dir\LoggingServer.Tests.dll"
  if ($lastExitCode -ne 0) {
          throw "Error: Failed to execute tests"
    }
  cd $old
}

task Release -depends Test {
    
}