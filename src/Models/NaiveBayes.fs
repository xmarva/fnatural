namespace FNatural.Models

open System
open System.Collections.Generic

module NaiveBayes =
    type Document = string[]
    type Label = string
    type LabeledDocument = Document * Label
    
    type NaiveBayesModel = {
        ClassPriors: Map<Label, float>
        WordProbabilities: Map<Label, Map<string, float>>
        Vocabulary: Set<string>
        Smoothing: float
    }
    
    let private countWords (documents: Document[]) =
        documents
        |> Array.collect id
        |> Seq.countBy id
        |> Map.ofSeq
    
    let fit (labeledDocuments: LabeledDocument[]) (smoothing: float) =
        let labels = labeledDocuments |> Array.map snd |> Array.distinct
        let documentCount = float labeledDocuments.Length
        
        let classPriors =
            labeledDocuments
            |> Seq.countBy snd
            |> Seq.map (fun (label, count) -> label, float count / documentCount)
            |> Map.ofSeq
        
        let vocabulary =
            labeledDocuments
            |> Array.collect fst
            |> Seq.distinct
            |> Set.ofSeq
        
        let wordProbabilities =
            labels
            |> Seq.map (fun label ->
                let classDocuments = 
                    labeledDocuments
                    |> Array.filter (fun (_, l) -> l = label)
                    |> Array.map fst
                
                let wordCounts = countWords classDocuments
                let totalWords = classDocuments |> Array.sumBy Array.length |> float
                
                let wordProbs =
                    vocabulary
                    |> Seq.map (fun word ->
                        let wordCount = 
                            if wordCounts.ContainsKey(word) then
                                float wordCounts.[word]
                            else
                                0.0
                        
                        let probability = (wordCount + smoothing) / (totalWords + smoothing * float vocabulary.Count)
                        word, probability)
                    |> Map.ofSeq
                
                label, wordProbs)
            |> Map.ofSeq
        
        { ClassPriors = classPriors
          WordProbabilities = wordProbabilities
          Vocabulary = vocabulary
          Smoothing = smoothing }
    
    let predict (model: NaiveBayesModel) (document: Document) =
        model.ClassPriors
        |> Seq.map (fun kvp ->
            let label = kvp.Key
            let priorProbability = kvp.Value
            
            let wordProbs = model.WordProbabilities.[label]
            
            let logProbability =
                document
                |> Array.fold (fun acc word ->
                    if wordProbs.ContainsKey(word) then
                        acc + Math.Log(wordProbs.[word])
                    else
                        // Handle unknown words with smoothing
                        acc + Math.Log(model.Smoothing / (model.Vocabulary.Count |> float)))
                    Math.Log(priorProbability)
            
            label, logProbability)
        |> Seq.maxBy snd
        |> fst