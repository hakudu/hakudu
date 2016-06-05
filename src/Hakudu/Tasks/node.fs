module node

open proc

let node app args workingDir =
    workingDir |> exec "node" (app :: args)
