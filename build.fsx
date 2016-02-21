#r "packages/build/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake
open Fake.ProcessHelper

let solutionFile = "Hakudu.sln"

let executableDir buildConfiguration = __SOURCE_DIRECTORY__ </> "src/Hakudu/bin" </> buildConfiguration
let executableFile buildConfiguration = (executableDir buildConfiguration) </> "hakudueng.exe"

Target "Clean" (fun _ ->
    !! solutionFile
    |> MSBuildDebug "" "Clean"
    |> ignore
)

Target "Build" (fun _ ->
    !! solutionFile
    |> MSBuildDebug "" "Build"
    |> ignore
)

Target "Run" (fun _ ->
    execProcess (fun p ->
        let buildConfiguration = "Debug"
        p.FileName <- executableFile buildConfiguration
        p.WorkingDirectory <- executableDir buildConfiguration
        p |> platformInfoAction
    ) TimeSpan.MaxValue |> ignore
)

"Build"
    ==> "Run"

RunTargetOrDefault "Build"
