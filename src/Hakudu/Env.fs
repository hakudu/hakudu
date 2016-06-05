module Env

open System
open System.Collections

let private _envVars = lazy (
    Environment.GetEnvironmentVariables()
    |> Seq.cast<DictionaryEntry>
    |> Seq.map (fun d -> d.Key :?> string, d.Value :?> string)
    |> Map.ofSeq
    )

let envVars = _envVars.Force()

let envVar = envVars.TryFind

let envVarOrElse name b =
    match envVar name with
    | Some a -> a
    | None -> b

let envVarOrFail name =
    match envVar name with
    | Some a -> a
    | None -> failwithf "Envirionment variable %s not set." name
