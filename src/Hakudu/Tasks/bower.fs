module bower

open proc

let bower command args workingDir =
    let bowerExe = workingDir |> npm.resolveLocalBin "bower"
    workingDir |> exec bowerExe (command :: args)

let install args workingDir =
    workingDir |> bower "install" args
