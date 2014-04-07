@echo off

cls

if not exist tools\FAKE\tools\Fake.exe (
  "tools\NuGet\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
)

if not exist tools\NUnit.Runners\tools\nunit-console.exe (
  "tools\nuget\nuget.exe" "install" "NUnit.Runners" "-OutputDirectory" "tools" "-ExcludeVersion"
)

"tools\FAKE\tools\Fake.exe" "build.fsx" %*