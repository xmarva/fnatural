#load "../src/Core/Tokenizer.fs"
#load "../src/Core/Stemmer.fs"
#load "../src/Core/StopWords.fs"
#load "../src/Models/TfIdf.fs"
#load "../src/Similarity/Cosine.fs"
#load "../src/Data/Preprocessing.fs"

open FNatural.Core
open FNatural.Models
open FNatural.Similarity
open FNatural.Data

// Example documents
let documents = [|
    "Машинное обучение - это подраздел искусственного интеллекта"
    "Глубокое обучение использует нейронные сети с множеством слоев"
    "Обработка естественного языка позволяет компьютерам понимать человеческий язык"
    "Компьютерное зрение позволяет машинам видеть и интерпретировать изображения"
    "Футбол - популярный вид спорта во многих странах мира"
    "Баскетбол был изобретен в США в конце 19 века"
    "Теннис - это индивидуальный вид спорта с ракеткой и мячом"
    "Волейбол - командная игра, в которой две команды разделены сеткой"
|]

// Preprocess the documents
let preprocessText (text: string) =
    text
    |> Preprocessing.cleanText
    |> Preprocessing.removePunctuation
    |> Preprocessing.removeExtraWhitespace
    |> Tokenizer.tokenizeWords
    |> StopWords.removeStopWords StopWords.Language.Russian
    |> Stemmer.stemMany Stemmer.Language.Russian

// Tokenize documents
let tokenizedDocuments = 
    documents 
    |> Array.map preprocessText

// Create TF-IDF model
let tfidfModel = TfIdf.fit tokenizedDocuments

// Vectorize documents
let vectorizedDocuments =
    tokenizedDocuments
    |> Array.map (TfIdf.vectorize tfidfModel)

// Simple K-means clustering (K=2)
let kmeans (vectors: float[][]) k maxIterations =
    let random = System.Random()
    
    // Initialize centroids by selecting random documents
    let centroids = 
        [| 0 .. k - 1 |]
        |> Array.map (fun _ -> vectors.[random.Next(vectors.Length)])
    
    let mutable iteration = 0
    let mutable changed = true
    let mutable clusters = Array.create vectors.Length 0
    
    while changed && iteration < maxIterations do
        changed <- false
        
        // Assign each vector to the nearest centroid
        for i = 0 to vectors.Length - 1 do
            let mutable minDistance = System.Double.MaxValue
            let mutable closestCluster = 0
            
            for j = 0 to k - 1 do
                let distance = Cosine.distance vectors.[i] centroids.[j]
                if distance < minDistance then
                    minDistance <- distance
                    closestCluster <- j
            
            if clusters.[i] <> closestCluster then
                changed <- true
                clusters.[i] <- closestCluster
        
        // Recalculate centroids
        for i = 0 to k - 1 do
            let clusterDocuments =
                vectors
                |> Array.mapi (fun idx v -> idx, v)
                |> Array.filter (fun (idx, _) -> clusters.[idx] = i)
                |> Array.map snd
            
            if clusterDocuments.Length > 0 then
                let newCentroid = 
                    Array.init clusterDocuments.[0].Length (fun j ->
                        clusterDocuments
                        |> Array.averageBy (fun doc -> doc.[j]))
                
                centroids.[i] <- newCentroid
        
        iteration <- iteration + 1
    
    clusters

// Perform clustering
let numClusters = 2
let maxIterations = 100
let clusters = kmeans vectorizedDocuments numClusters maxIterations

// Print results
printfn "Document Clustering Results:"
Array.zip documents clusters
|> Array.iter (fun (doc, cluster) ->
    printfn "Cluster %d: %s" cluster doc)