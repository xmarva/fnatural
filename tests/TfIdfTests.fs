module FNatural.Tests.TfIdfTests

open FsUnit
open NUnit.Framework
open FNatural.Models

[<TestFixture>]
type TfIdfTests() =
    
    [<Test>]
    member _.``TF-IDF fit should create model with correct properties``() =
        let corpus = [|
            [|"this"; "is"; "a"; "sample"|]
            [|"this"; "is"; "another"; "sample"|]
            [|"yet"; "another"; "example"|]
        |]
        
        let model = TfIdf.fit corpus
        
        model.DocumentCount |> should equal 3
        model.Vocabulary.Count |> should equal 6
        model.Vocabulary |> should contain "sample"
        model.Vocabulary |> should contain "example"
        model.DocumentFrequencies.["sample"] |> should equal 2.0
        model.DocumentFrequencies.["example"] |> should equal 1.0
    
    [<Test>]
    member _.``TF-IDF transform should calculate correct scores``() =
        let corpus = [|
            [|"this"; "is"; "a"; "sample"|]
            [|"this"; "is"; "another"; "sample"|]
            [|"yet"; "another"; "example"|]
        |]
        
        let model = TfIdf.fit corpus
        let document = [|"this"; "is"; "a"; "sample"|]
        
        let tfidfMap = TfIdf.transform model document
        
        // "this" appears in 2 of 3 documents, so IDF should be ln(3/2)
        tfidfMap.["this"] |> should (equalWithin 0.01) (0.25 * System.Math.Log(3.0/2.0))
        
        // "sample" appears in 2 of 3 documents, so IDF should be ln(3/2)
        tfidfMap.["sample"] |> should (equalWithin 0.01) (0.25 * System.Math.Log(3.0/2.0))
        
        // "example" doesn't appear in this document
        tfidfMap.["example"] |> should equal 0.0
    
    [<Test>]
    member _.``TF-IDF vectorize should create vector of correct length``() =
        let corpus = [|
            [|"this"; "is"; "a"; "sample"|]
            [|"this"; "is"; "another"; "sample"|]
            [|"yet"; "another"; "example"|]
        |]
        
        let model = TfIdf.fit corpus
        let document = [|"this"; "is"; "a"; "sample"|]
        
        let vector = TfIdf.vectorize model document
        
        vector.Length |> should equal 6  // Should match vocabulary size