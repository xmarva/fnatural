namespace FNatural.Core

open System
open System.Text.RegularExpressions

module Tokenizer =
    let tokenize (text: string) =
        if String.IsNullOrEmpty(text) then [||] else
        text.Split([|' '; '\t'; '\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries)
    
    let tokenizeWithRegex pattern (text: string) =
        if String.IsNullOrEmpty(text) then [||] else
        Regex.Matches(text, pattern)
        |> Seq.cast<Match>
        |> Seq.map (fun m -> m.Value)
        |> Seq.toArray
    
    let tokenizeWords (text: string) =
        tokenizeWithRegex @"\w+" text
    
    let tokenizeSentences (text: string) =
        tokenizeWithRegex @"[^.!?]+[.!?]" text
        |> Array.map (fun s -> s.Trim())
    
    let tokenizeWithUnicode (text: string) =
        tokenizeWithRegex @"[\p{L}\p{N}]+" text