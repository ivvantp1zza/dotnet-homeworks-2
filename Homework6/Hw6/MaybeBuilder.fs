﻿namespace Hw6

type MaybeBuilder() =
    member builder.Bind(a, f): Result<'e,'d> =
        match a with
        | Ok s -> f s
        | Error e -> Error e
    member builder.Return x: Result<'a,'b> =
        Ok x