open System
open System.Collections
open System.Diagnostics
open System.IO
open System.Reflection

open Env

let executablePath =
    let assembly = Assembly.GetEntryAssembly()
    assembly.Location

let executableVersion =
    let assembly = Assembly.GetEntryAssembly()
    let versionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
    if not (isNull versionAttribute) then versionAttribute.InformationalVersion else String.Empty

[<EntryPoint>]
let main argv =
    printfn "Hey, I'm Hakudu!"

    // Printing diagnostic information
    printfn "Executable: %s" executablePath
    printfn "Version: %s" executableVersion
    printfn "Command line: %s" Environment.CommandLine
    printfn "Working directory: %s" Environment.CurrentDirectory
    printfn "Arguments: %s" (Environment.GetCommandLineArgs() |> Seq.skip 1 |> String.concat ", ")
    printfn "Environment variables:"
    for (envKey, envValue) in envVars |> Seq.map (fun e -> (e.Key, e.Value)) do
        printfn "* %s = %s" envKey envValue

    // Retrieving the environment paths
    let repositoryDir = envVarOrFail "DEPLOYMENT_SOURCE"
    let webserverDir = envVarOrFail "DEPLOYMENT_TARGET"
    let websiteBuildDir = Path.Combine(repositoryDir, "website")

    // Installing packages
    websiteBuildDir |> npm.install [] |> ignore
    websiteBuildDir |> bower.install [] |> ignore

    // Building the website
    websiteBuildDir |> gulp.gulp "build" [] |> ignore

    // Syncing files to webserver directory
    let deploymentIgnores = [
        ".gitignore";

        // Ignoring package manager configurations
        "node_modules";
        "bower.json";

        // Ignoring NPM modules (used only for build)
        "package.json";

        // Ignoring build script
        "gulpfile.js";

        // Ignoring TypeScript type definitions and sources
        "tsd.json";
        "*.ts";

        // Ignoring SASS files
        "*.scss"
    ]

    kudusync.syncDirs websiteBuildDir webserverDir deploymentIgnores |> ignore

    // Done
    printfn "Finished successfully."
    0
