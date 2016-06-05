module kudusync

open System

open Env
open proc

let syncDirs sourceDir targetDir (ignores: string list) =
    let kudusyncCmd = envVarOrFail "KUDU_SYNC_CMD"
    let nextManifestPath = envVarOrFail "NEXT_MANIFEST_PATH"
    let previousManifestPath = envVarOrElse "PREVIOUS_MANIFEST_PATH" nextManifestPath

    let kudusyncArgs = [
        "--from"; sourceDir;
        "--to"; targetDir;
        "--previousManifest"; previousManifestPath;
        "--nextManifest"; nextManifestPath;
        "--ignore"; ignores |> String.concat ";"
    ]

    sourceDir |> exec kudusyncCmd kudusyncArgs
