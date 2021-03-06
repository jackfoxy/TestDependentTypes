﻿namespace Jackfoxy.TestDependentTypes

open System
open System.Text.RegularExpressions

module TrimNonEmptyStringDef =
    let verifyTrimNonEmptyString config (value : string) =
        if String.IsNullOrWhiteSpace value then
            None        
        else 
            Some <| value.Trim()

    type NonEmptyValidator(config) = 
        inherit Cctor<unit, string, string>(config, verifyTrimNonEmptyString)
        new() = NonEmptyValidator(())

    type NonEmpty () = inherit NonEmptyValidator()

type TrimNonEmptyString = DependentType<TrimNonEmptyStringDef.NonEmpty, unit, string, string> 

module UtcDateTimeDef =
    let verifyUtcDateTime config (value : DateTime) =
        Some <| value.ToUniversalTime()     

    type UtcDateTimeValidator(config) = 
        inherit Cctor<unit, DateTime, DateTime>(config, verifyUtcDateTime)
        new() = UtcDateTimeValidator(())

    type ValidUtcDateTime () = inherit UtcDateTimeValidator()
    
type UtcDateTime = DependentType<UtcDateTimeDef.ValidUtcDateTime, unit, DateTime, DateTime> 

module NonEmptySetDef =
    let verifyNonEmptySet f (value : Set<int>) =
        if value.Count > 0 then
            Some value  
        else
            None

    type NonEmptySetValidator(config) = 
        inherit Validator<unit, Set<int>>(config, verifyNonEmptySet)
        new() = NonEmptySetValidator(())

    type ValidNonEmptySet() = inherit NonEmptySetValidator()
    
type NonEmptyIntSet = LimitedValue<NonEmptySetDef.ValidNonEmptySet, unit, Set<int>>   

module RegExStringVerify =
    let regExStringVerify (regex : Regex) config (value : string) =
        if String.IsNullOrWhiteSpace value then
            None
        else
            let s' = value.Trim()
            if regex.IsMatch s' then 
                if config > 0 then
                    if String.length(s') = config then 
                            Some s'
                        else 
                            None
                else
                    Some s'
            else
                None

module UpperLatinDef =
    let regex = new Regex("^[A-Z]+$")
    let verifyUpperLatin config value =
        RegExStringVerify.regExStringVerify regex config value

    type UpperLatinValidator(config) = 
        inherit Cctor<int, string, string>(config, verifyUpperLatin)
        new() = UpperLatinValidator(0)

    type ValidUpperLatin2 () = inherit UpperLatinValidator(2)
    type ValidUpperLatin3 () = inherit UpperLatinValidator(3)
    
type UpperLatin2 = DependentType<UpperLatinDef.ValidUpperLatin2, int, string, string>
type UpperLatin3 = DependentType<UpperLatinDef.ValidUpperLatin3, int, string, string>

module DigitsDef =
    let regex = new Regex("^[0-9]+$")
    let verifyDigits config value =
        RegExStringVerify.regExStringVerify regex config value

    type DigitsValidator(config) = 
        inherit Cctor<int, string, string>(config, verifyDigits)
        new() = DigitsValidator(0)

    type ValidDigits () = inherit DigitsValidator(0)
    type ValidDigits2 () = inherit DigitsValidator(2)
    type ValidDigits3 () = inherit DigitsValidator(3)
    type ValidDigits4 () = inherit DigitsValidator(4)
    
type Digits = DependentType<DigitsDef.ValidDigits, int, string, string>
type Digits2 = DependentType<DigitsDef.ValidDigits2, int, string, string>
type Digits3 = DependentType<DigitsDef.ValidDigits3, int, string, string>
type Digits4 = DependentType<DigitsDef.ValidDigits4, int, string, string>

