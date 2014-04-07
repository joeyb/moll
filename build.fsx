// include Fake lib
#r "tools/FAKE/tools/FakeLib.dll"
open Fake

RestorePackages()

// Properties
let buildDir = "./build/"
let testDir  = "./test/"

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir]
)

Target "Build" (fun _ ->
    !! "src/Moll/Moll.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    !! "src/Moll.Test/Moll.Test.csproj"
      |> MSBuildDebug testDir "Build"
      |> Log "TestBuild-Output: "
)

Target "Test" (fun _ ->
    !! (testDir + "/*.Test.dll")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = testDir + "TestResults.xml"})
)

Target "Default" DoNothing

// Dependencies
"Clean"
  ==> "Build"
  ==> "Default"

"Clean"
  ==> "BuildTest"
  ==> "Test"

// start build
RunTargetOrDefault "Default"