﻿namespace Jackfoxy.TestDependentTypes

module DependentTypes =
    let inline mkDependentType (x: ^S) : Option< ^T> = 
        (^T: (static member TryParse: ^S -> Option< ^T>) x)

    let inline extract (x:^S) = 
        (^S: (static member Extract: ^S -> ^T) x)

    let inline convertTo (x: ^S) : Option< ^T> = 
        (^T: (static member ConvertTo: ^S -> Option< ^T>) x)

open DependentTypes
open System

[<Class>]
type Cctor<'Config, 'T, 'T2> (config: 'Config, vfn: 'Config -> 'T -> Option<'T2>) =
    member __.TryCreate(x:'T) : Option<'T2> = vfn config x

type DependentType<'Cctor, 'Config, 'T, 'T2 when 'Cctor :> Cctor<'Config, 'T, 'T2>
                                            and  'Cctor : (new: unit -> 'Cctor)> =
    DependentType of 'T2 
    
    with 
        member __.Value = 
            let (DependentType s) = __
            s
        override __.ToString() = __.Value.ToString()           
        static member Extract (x : DependentType<'Cctor, 'Config, 'T, 'T2> ) = 
            let (DependentType s) = x
            s
        static member TryParse(x:'T) : Option<DependentType<'Cctor, 'Config, 'T, 'T2>> =
            (new 'Cctor()).TryCreate x
            |> Option.map DependentType
        static member TryParse(x:'T option) : Option<DependentType<'Cctor, 'Config, 'T, 'T2>> =
            match x with
            | Some x' -> DependentType.TryParse x'
            | None -> None
        static member Parse (x : 'T) : DependentType<'Cctor, 'Config, 'T, 'T2> =
                (DependentType.TryParse x).Value
        static member Parse (xs : 'T seq) : DependentType<'Cctor, 'Config, 'T, 'T2> seq =
            xs
            |> Seq.choose DependentType.TryParse 
        static member Parse (xs : 'T list) : DependentType<'Cctor, 'Config, 'T, 'T2> list =
            xs
            |> List.choose DependentType.TryParse 
            //static member inline ConvertTo2(x : DependentType<'x, 'y, 'q, 'r> ) : Option<DependentType<'a, 'b, 'r, 's>> = 
            //    let (DependentType v) = x
            //    mkDependentType v   

[<Class>]
type Validator<'Config, 'T> (config: 'Config, vfn: 'Config -> 'T -> Option<'T>) =
    member __.Validate(x:'T) : Option<'T> = vfn config x

type LimitedValue<'Validator, 'Config, 'T when 'Validator :> Validator<'Config, 'T>
                                           and  'Validator : (new: unit -> 'Validator)> =
    DependentType of 'T 
    
    with
        member __.Value = 
            let (DependentType s) = __
            s
        static member Extract (x : LimitedValue<'Validator, 'Config, 'T> ) = 
            let (DependentType s) = x
            s
        static member TryParse(x:'T) : Option<LimitedValue<'Validator, 'Config, 'T>> =
            (new 'Validator()).Validate x
            |> Option.map DependentType

        //static member inline ConvertTo(x : LimitedValue<'x, 'y, 'q> ) : Option<LimitedValue<'a, 'b, 'q>> = 
        //    let (DependentType v) = x
        //    mkDependentType v
