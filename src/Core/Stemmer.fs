namespace FNatural.Core

open System
open System.Text.RegularExpressions

module Stemmer =
    type Language = 
        | English
        | Russian
    
    let private englishSuffixes = 
        [|
            "ing"; "ed"; "es"; "s"; "ly"; "er"; "est"; "ment"; "ness"; "ity"; "ies"
        |]
    
    let private russianSuffixes = 
        [|
            "ов"; "ин"; "ость"; "ени"; "ами"; "ыми"; "ому"; "его"; "ями"; "ому"; 
            "ему"; "ете"; "ишь"; "ешь"; "ать"; "ять"; "еть"; "ить"
        |]
    
    let stem language (word: string) =
        if String.IsNullOrEmpty(word) then word else
        
        let suffixes = 
            match language with
            | English -> englishSuffixes
            | Russian -> russianSuffixes
        
        let mutable stemmed = word
        
        for suffix in suffixes do
            if stemmed.Length > suffix.Length + 2 && stemmed.EndsWith(suffix) then
                stemmed <- stemmed.Substring(0, stemmed.Length - suffix.Length)
        
        stemmed
    
    let stemMany language words =
        words |> Array.map (stem language)