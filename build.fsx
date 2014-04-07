// include Fake lib
#r "tools/FAKE/tools/FakeLib.dll"
open System.IO
open Fake
open Fake.AssemblyInfoFile
open Fake.ReleaseNotesHelper

RestorePackages()

let authors = ["Joey Bratton"]

let projectName = "Moll"
let projectDescription = "A simple .NET object mapper framework, intended to be used along with an IoC container."
let projectSummary = projectDescription

let buildDir = "./build/"
let packagingRoot = "./packaging/"
let packagingDir = packagingRoot @@ "moll"
let testDir = "./test/"

let releaseNotes = parseReleaseNotes (System.IO.File.ReadAllLines "VERSIONS.md")

// Targets
Target "AssemblyInfo" (fun _ ->
    CreateCSharpAssemblyInfo "src/SolutionInfo.cs"
      [ Attribute.Product projectName
        Attribute.Version releaseNotes.AssemblyVersion
        Attribute.FileVersion releaseNotes.AssemblyVersion
        Attribute.ComVisible false ]
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

Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir]
)

Target "CreatePackage" (fun _ ->
    let net45dir = packagingDir @@ "lib/net45/"

    CleanDirs [packagingDir; net45dir]

    CopyFile net45dir (buildDir @@ "Moll.dll")
    CopyFiles packagingDir ["LICENSE.txt"; "README.md"]

    NuGet (fun p ->
        {p with
            Authors = authors
            Project = projectName
            Description = projectDescription
            OutputPath = packagingRoot
            Summary = projectSummary
            WorkingDir = packagingDir
            Version = releaseNotes.AssemblyVersion
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey" })
            "Moll.nuspec"
)

Target "Default" DoNothing

Target "Test" (fun _ ->
    !! (testDir + "/*.Test.dll")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = testDir + "TestResults.xml"})
)

// Dependencies
"Clean"
  ==> "AssemblyInfo"
  ==> "Build"
  ==> "Default"

"Clean"
  ==> "AssemblyInfo"
  ==> "BuildTest"
  ==> "Test"

"Clean"
  ==> "AssemblyInfo"
  ==> "Build"
  ==> "CreatePackage"

// start build
RunTargetOrDefault "Default"