# In order to run this script, you might have run the following from an elevated prompt: Set-ExecutionPolicy Unrestricted -Force

Function RegisterEventSource ([String]$sourceName, [string]$eventLogName = "Application") {
  if ([System.Diagnostics.EventLog]::SourceExists($sourceName) -eq $False) {
    [System.Diagnostics.EventLog]::CreateEventSource($sourceName, $eventLogName)
    Write-Host "The event source '$sourceName' has been added to the '$eventLogName' event log."
  }
  else {
    Write-Host "The event source '$sourceName' already existed."
  }
    Write-Host "Press any key to continue."
  Read-Host
}


$CurrentPrincipal = New-Object Security.Principal.WindowsPrincipal( [Security.Principal.WindowsIdentity]::GetCurrent( ) )
if ( -not ($currentPrincipal.IsInRole( [Security.Principal.WindowsBuiltInRole]::Administrator ) ) )
{
    $path = [System.IO.Path]::GetFullPath('.\EventSource.ps1');
    $directory = [System.Environment]::CurrentDirectory;

    Start-Process powershell -Verb runAs -WorkingDirectory $directory -ArgumentList $path
} 
else
{
    RegisterEventSource( "DragonSpark Application" );
}