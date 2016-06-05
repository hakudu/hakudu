module gulp

open proc

let gulp command args workingDir =
    let gulpBin = workingDir |> npm.resolveLocalBin "gulp"
    workingDir |> exec gulpBin (command :: args)
