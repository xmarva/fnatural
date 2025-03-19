module FNatural.Tests.StemmerTests

open FsUnit
open NUnit.Framework
open FNatural.Core

[<TestFixture>]
type StemmerTests() =
    
    [<Test>]
    member _.``English stemmer should remove common suffixes``() =
        let testCases = [
            ("walking", "walk")
            ("played", "play")
            ("buses", "bus")
            ("cats", "cat")
            ("quickly", "quick")
            ("faster", "fast")
            ("strongest", "strong")
            ("development", "develop")
            ("happiness", "happi")
            ("stability", "stabil")
            ("countries", "countri")
        ]
        
        for (input, expected) in testCases do
            let result = Stemmer.stem Stemmer.Language.English input
            result |> should equal expected
    
    [<Test>]
    member _.``Russian stemmer should remove common suffixes``() =
        let testCases = [
            ("программирование", "программир")
            ("компьютеры", "компьютер")
            ("российский", "российск")
            ("играешь", "игра")
            ("читать", "чит")
            ("открытость", "открыт")
            ("учителями", "учител")
            ("городскому", "городск")
            ("зеленого", "зелен")
        ]
        
        for (input, expected) in testCases do
            let result = Stemmer.stem Stemmer.Language.Russian input
            result |> should equal expected
    
    [<Test>]
    member _.``Stemmer should handle short words``() =
        let shortWord = "и"
        
        let result = Stemmer.stem Stemmer.Language.Russian shortWord
        
        result |> should equal shortWord
    
    [<Test>]
    member _.``Stemmer should handle empty strings``() =
        let emptyString = ""
        
        let result = Stemmer.stem Stemmer.Language.English emptyString
        
        result |> should equal emptyString
    
    [<Test>]
    member _.``StemMany should process multiple words``() =
        let words = [|"running"; "jumps"; "played"; "fastest"|]
        let expected = [|"run"; "jump"; "play"; "fast"|]
        
        let result = Stemmer.stemMany Stemmer.Language.English words
        
        result |> should equal expected