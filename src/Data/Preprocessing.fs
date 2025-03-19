namespace FNatural.Data

open System
open System.Text.RegularExpressions

module Preprocessing =
    let cleanText (text: string) =
        if String.IsNullOrEmpty(text) then "" else
        
        let lowercased = text.ToLower()
        
        let cleaned = Regex.Replace(lowercased, @"[^\w\s]", "")
        
        Regex.Replace(cleaned, @"\s+", " ").Trim()
    
    let normalizeText (text: string) =
        if String.IsNullOrEmpty(text) then "" else
        
        let lowercased = text.ToLower()
        
        let normalized = 
            lowercased
                .Replace("don't", "do not")
                .Replace("can't", "cannot")
                .Replace("won't", "will not")
                .Replace("i'm", "i am")
                .Replace("i've", "i have")
                .Replace("it's", "it is")
                .Replace("they're", "they are")
        
        normalized
    
    let removePunctuation (text: string) =
        if String.IsNullOrEmpty(text) then "" else
        Regex.Replace(text, @"[^\w\s]", "")
    
    let removeNumbers (text: string) =
        if String.IsNullOrEmpty(text) then "" else
        Regex.Replace(text, @"\d+", "")
    
    let removeExtraWhitespace (text: string) =
        if String.IsNullOrEmpty(text) then "" else
        Regex.Replace(text, @"\s+", " ").Trim()
    
    let applyPreprocessingPipeline (text: string) (pipeline: (string -> string) list) =
        pipeline
        |> List.fold (fun currentText preprocessStep -> preprocessStep currentText) text