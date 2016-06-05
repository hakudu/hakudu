module npm

open System.IO

open Env
open proc

let resolveLocalBin command workingDir =
    let exeFile = Path.Combine(workingDir, "node_modules", ".bin", command + ".cmd")
    if File.Exists(exeFile) then
        exeFile
    else
        command

let npm command args workingDir =
    match envVar "NPM_JS_PATH" with
    | Some npmJsPath -> workingDir |> node.node npmJsPath (command :: args)
    | None -> workingDir |> exec "npm" (command :: args)

let install args workingDir =
    workingDir |> npm "install" args
