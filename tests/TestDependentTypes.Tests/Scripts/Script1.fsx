#load "load-project-debug.fsx"

open Jackfoxy.TestDependentTypes
open TestDependentTypes.Tests
open FsCheck

let (.=.) left right = left = right |@ sprintf "%A = %A" left right

let x' = (FullName.TryParse (None, [], None, NameOrder.Western, Set.empty<Tag>))

Arb.register<DomainGenerators>() |> ignore
//Arb.registerByType typeOf<FullName>


let x = 
    Prop.forAll (Arb.fromGen <| DomainGeneratorsCode.genFullName())
        (fun (fullName : FullName)  -> 
                let first = fullName.First |> Option.map (fun x -> x.Value)
                let middle = fullName.Middle |> List.map (fun x -> x.Value)
                let family = fullName.Family |> Option.map (fun x -> x.Value)

                let t = 
                    FullName.TryParse (first, middle, family, fullName.NameOrder, Set.empty<Tag>)

                t.Value = fullName |@ sprintf "first: %A middle: %A family: %A"  fullName.First middle fullName.Family
            )

//Check.Quick x

Check.One({ Config.Quick with Replay = Some <| Random.StdGen (310936656,296287911) }, x)


