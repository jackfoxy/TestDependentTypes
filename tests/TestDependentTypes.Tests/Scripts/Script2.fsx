#load "load-project-debug.fsx"

open TestDependentTypes.Tests
open Jackfoxy.TestDependentTypes

module List = 
    // http://fssnip.net/4u/title/Very-Fast-Permutations
    // From: http://stackoverflow.com/questions/286427/calculating-permutations-in-f
    // Much faster than anything else I've tested

    let rec insertions x = function
    //let rec private insertions x = function
        | []             -> [[x]]
        | (y :: ys) as l -> (x::l)::(List.map (fun x -> y::x) (insertions x ys))

    let rec permutations = function
        | []      -> seq [ [] ]
        | x :: xs -> Seq.concat (Seq.map (insertions x) (permutations xs))

let x = List.permutations [0;1;2;3;4]

Seq.length 

x
|> Seq.iter (fun x -> printfn "%A" x)

let names = Contacts.simpleNameElim6
                

let namesPerumations = List.permutations names

let z =
    namesPerumations
    |> Array.ofSeq

z.[26]

ContactName.elimination z.[26]


