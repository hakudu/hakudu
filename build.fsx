#r "packages/build/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake
open Fake.ProcessHelper

let solutionFile = "Hakudu.sln"

let executableDir configuration = __SOURCE_DIRECTORY__ </> "src/Hakudu/bin" </> configuration
let executableFile configuration = (executableDir configuration) </> "hakudueng.exe"

let BuildTarget name configuration target =
    TargetTemplate (fun _ ->
        !! solutionFile
        |> MSBuild null target [ "Configuration", configuration ]
        |> ignore
    ) name ()

let BuildDebugTarget name = BuildTarget name "Debug"
let BuildReleaseTarget name = BuildTarget name "Release"

BuildDebugTarget "clean-debug" "Clean"
BuildReleaseTarget "clean" "Clean"

BuildDebugTarget "build-debug" "Build"
BuildReleaseTarget "build" "Build"

BuildDebugTarget "rebuild-debug" "Rebuild"
BuildReleaseTarget "rebuild" "Rebuild"

Target "run" (fun _ ->
    let buildConfiguration = "Debug"
    execProcess (fun p ->
        p.FileName <- executableFile buildConfiguration
        p.WorkingDirectory <- executableDir buildConfiguration
        p |> platformInfoAction
    ) TimeSpan.MaxValue |> ignore
)

"build-debug"
    ==> "run"

RunTargetOrDefault "build"
