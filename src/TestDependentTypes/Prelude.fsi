namespace Jackfoxy.TestDependentTypes

open System
open System.Globalization

module DependentTypes =

    val inline extract : x: ^S ->  ^T when  ^S : (static member Extract :  ^S ->  ^T)

    val inline convertTo : x: ^S -> Option< ^T> when  ^T : (static member ConvertTo :  ^S -> Option< ^T>)

open DependentTypes

[<Class>]
type Cctor<'Config,'T,'T2> =
    new : config:'Config * vfn:('Config -> 'T -> Option<'T2>) -> Cctor<'Config,'T,'T2>
    member TryCreate : x:'T -> Option<'T2>

type DependentType<'Cctor,'Config,'T,'T2 when 'Cctor :> Cctor<'Config,'T,'T2> 
                                            and  'Cctor : (new : unit -> 'Cctor)> =
    | DependentType of 'T2
    with
    member Value : 'T2
    
    static member Extract : x : DependentType<'Cctor,'Config,'T,'T2> -> 'T2
    static member TryParse : x :'T -> Option<DependentType<'Cctor,'Config,'T,'T2>>
    static member TryParse : x :'T option -> Option<DependentType<'Cctor,'Config,'T,'T2>>
    static member Parse : x : 'T -> DependentType<'Cctor,'Config,'T,'T2>
    static member Parse : xs : 'T seq -> DependentType<'Cctor,'Config,'T,'T2> seq
    static member Parse : xs : 'T list -> DependentType<'Cctor,'Config,'T,'T2> list
    //static member inline ConvertTo : x : DependentType<'x,'y,'q, 'r> ->
    //                Option<DependentType<'a,'b, 'r,'s>>
    //                    when 'x :> Cctor<'y,'q, 'r> and 'x : (new : unit -> 'x) 
    //                    and  'a :> Cctor<'b, 'r,'s> and 'a : (new : unit -> 'a)
 // end

[<Class>]
type Validator<'Config,'T> =
    new : config:'Config * vfn:('Config -> 'T -> Option<'T>) -> Validator<'Config,'T>
    member Validate : x:'T -> Option<'T>

type LimitedValue<'Validator,'Config,'T
                    when 'Validator :> Validator<'Config,'T> and
                         'Validator : (new : unit -> 'Validator)> =
  | DependentType of 'T
  with
    member Value : 'T
    static member Extract : x:LimitedValue<'Validator,'Config,'T> -> 'T
    static member TryParse : x:'T -> Option<LimitedValue<'Validator,'Config,'T>>
    //static member ConvertTo : x:LimitedValue<'x,'y, ^q> -> Option<LimitedValue<'a,'b, ^q>>
    //                when 'x :> Validator<'y, ^q> and 'x : (new : unit -> 'x) 
    //                and  'a :> Validator<'b, ^q> and 'a : (new : unit -> 'a)
  end
