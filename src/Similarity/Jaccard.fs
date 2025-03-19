namespace FNatural.Similarity

open System

module Jaccard =
    let similarity (set1: 'T[]) (set2: 'T[]) =
        if Array.isEmpty set1 && Array.isEmpty set2 then 1.0 else
        if Array.isEmpty set1 || Array.isEmpty set2 then 0.0 else
        
        let set1Set = set1 |> Set.ofArray
        let set2Set = set2 |> Set.ofArray
        
        let intersection = Set.intersect set1Set set2Set |> Set.count |> float
        let union = Set.union set1Set set2Set |> Set.count |> float
        
        intersection / union
    
    let distance (set1: 'T[]) (set2: 'T[]) =
        1.0 - similarity set1 set2