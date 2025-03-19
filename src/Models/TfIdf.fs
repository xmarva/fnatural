namespace FNatural.Models

open System
open System.Collections.Generic

module TfIdf =
    type Document = string[]
    type Corpus = Document[]
    
    type TfIdfModel = {
        DocumentFrequencies: Map<string, float>
        DocumentCount: int
        Vocabulary: Set<string>
    }
    
    let calculateTermFrequency (document: Document) =
        document
        |> Seq.countBy id
        |> Seq.map (fun (term, count) -> term, float count / float document.Length)
        |> Map.ofSeq
    
    let fit (corpus: Corpus) =
        let documentCount = corpus.Length
        
        let documentFrequencies =
            corpus
            |> Array.collect (fun doc -> doc |> Array.distinct)
            |> Seq.countBy id
            |> Seq.map (fun (term, count) -> term, float count)
            |> Map.ofSeq
        
        let vocabulary =
            corpus
            |> Array.collect id
            |> Seq.distinct
            |> Set.ofSeq
        
        { DocumentFrequencies = documentFrequencies
          DocumentCount = documentCount
          Vocabulary = vocabulary }
    
    let transform (model: TfIdfModel) (document: Document) =
        let termFrequencies = calculateTermFrequency document
        
        model.Vocabulary
        |> Seq.map (fun term ->
            if termFrequencies.ContainsKey(term) then
                let tf = termFrequencies.[term]
                let documentFrequency = 
                    if model.DocumentFrequencies.ContainsKey(term) then
                        model.DocumentFrequencies.[term]
                    else
                        0.0
                let idf = Math.Log(float model.DocumentCount / (1.0 + documentFrequency))
                term, tf * idf
            else
                term, 0.0)
        |> Map.ofSeq
    
    let vectorize (model: TfIdfModel) (document: Document) =
        let tfidfMap = transform model document
        
        model.Vocabulary
        |> Seq.map (fun term ->
            if tfidfMap.ContainsKey(term) then
                tfidfMap.[term]
            else
                0.0)
        |> Seq.toArray