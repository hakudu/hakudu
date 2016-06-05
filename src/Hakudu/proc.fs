module proc

open System.Diagnostics

let argumentListToString (args: string list): string =
    args |> String.concat " "

let exec command (args: string list) workingDir =
    let argsStr = args |> argumentListToString
    let startInfo = new ProcessStartInfo (WorkingDirectory = workingDir, FileName = command, Arguments = argsStr, UseShellExecute = true)
    let proc = new Process (StartInfo = startInfo)
    proc.Start() |> ignore
    proc.WaitForExit()
    proc.ExitCode
