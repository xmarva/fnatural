namespace FNatural.Similarity

open System

module Cosine =
    let similarity (vector1: float[]) (vector2: float[]) =
        if vector1.Length <> vector2.Length then
            invalidArg "vector2" "Vectors must have the same length"
        
        if Array.isEmpty vector1 then 0.0 else
        
        let dotProduct = 
            Array.zip vector1 vector2
            |> Array.sumBy (fun (x, y) -> x * y)
        
        let magnitude1 = 
            vector1
            |> Array.sumBy (fun x -> x * x)
            |> sqrt
        
        let magnitude2 = 
            vector2
            |> Array.sumBy (fun x -> x * x)
            |> sqrt
        
        if magnitude1 = 0.0 || magnitude2 = 0.0 then 0.0 else
        dotProduct / (magnitude1 * magnitude2)
    
    let distance (vector1: float[]) (vector2: float[]) =
        1.0 - similarity vector1 vector2