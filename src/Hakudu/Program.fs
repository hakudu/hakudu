open System
open System.Collections
open System.Diagnostics
open System.Reflection

let executablePath =
    let assembly = Assembly.GetEntryAssembly()
    assembly.Location

let executableVersion =
    let assembly = Assembly.GetEntryAssembly()
    let fileVersionAttribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()
    if not (isNull fileVersionAttribute) then fileVersionAttribute.Version else String.Empty

let environVars =
    Environment.GetEnvironmentVariables() |> Seq.cast<DictionaryEntry> |> Seq.map (fun e -> e.Key :?> string, e.Value :?> string)

[<EntryPoint>]
let main argv =
    printfn "Hey, I'm Hakudu!"
    printfn "Executable: %s" executablePath
    printfn "Version: %s" executableVersion
    printfn "Command line: %s" Environment.CommandLine
    printfn "Working directory: %s" Environment.CurrentDirectory
    printfn "Arguments: %s" (Environment.GetCommandLineArgs() |> Seq.skip 1 |> String.concat ", ")
    printfn "Environment variables:"
    for (envKey, envValue) in environVars do
        printfn "* %s = %s" envKey envValue
    0
