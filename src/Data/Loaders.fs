namespace FNatural.Data

open System
open System.IO
open System.Text

module Loaders = 
    let loadText (filePath: string) =
        if not (File.Exists filePath) then
            failwithf "File not found: %s" filePath
        
        File.ReadAllText(filePath)
    
    let loadLines (filePath: string) =
        if not (File.Exists filePath) then
            failwithf "File not found: %s" filePath
        
        File.ReadAllLines(filePath)
    
    let loadCsv (filePath: string) (delimiter: char) (hasHeader: bool) =
        if not (File.Exists filePath) then
            failwithf "File not found: %s" filePath
        
        let lines = File.ReadAllLines(filePath)
        
        if Array.isEmpty lines then
            [||]
        else
            let headerRow = if hasHeader then Some(lines.[0].Split(delimiter)) else None
            
            let dataRows = 
                if hasHeader then
                    lines.[1..]
                else
                    lines
                |> Array.map (fun line -> line.Split(delimiter))
            
            headerRow, dataRows
    
    let loadLabeledDocuments (filePath: string) (delimiter: char) (textColumnIndex: int) (labelColumnIndex: int) =
        let _, dataRows = loadCsv filePath delimiter true
        
        dataRows
        |> Array.map (fun row -> 
            let text = row.[textColumnIndex]
            let label = row.[labelColumnIndex]
            text, label)