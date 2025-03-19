module FNatural.Tests.TokenizerTests

open FsUnit
open NUnit.Framework
open FNatural.Core

[<TestFixture>]
type TokenizerTests() =
    
    [<Test>]
    member _.``Basic tokenize should split by whitespace``() =
        let text = "Это простой тест для токенизации"
        let expected = [|"Это"; "простой"; "тест"; "для"; "токенизации"|]
        
        let result = Tokenizer.tokenize text
        
        result |> should equal expected
    
    [<Test>]
    member _.``Tokenize should handle multiple spaces``() =
        let text = "Текст  с   несколькими    пробелами"
        let expected = [|"Текст"; "с"; "несколькими"; "пробелами"|]
        
        let result = Tokenizer.tokenize text
        
        result |> should equal expected
    
    [<Test>]
    member _.``Tokenize should handle tabs and newlines``() =
        let text = "Строка 1\tс табуляцией\nСтрока 2"
        let expected = [|"Строка"; "1"; "с"; "табуляцией"; "Строка"; "2"|]
        
        let result = Tokenizer.tokenize text
        
        result |> should equal expected
    
    [<Test>]
    member _.``Tokenize words should extract only words``() =
        let text = "Привет, мир! Это тест-сообщение."
        let expected = [|"Привет"; "мир"; "Это"; "тест"; "сообщение"|]
        
        let result = Tokenizer.tokenizeWords text
        
        result |> should equal expected
    
    [<Test>]
    member _.``Tokenize sentences should split by punctuation``() =
        let text = "Это первое предложение. А это второе! И вот третье?"
        let expected = [|"Это первое предложение."; "А это второе!"; "И вот третье?"|]
        
        let result = Tokenizer.tokenizeSentences text
        
        result |> should equal expected
    
    [<Test>]
    member _.``Tokenize with Unicode should handle different alphabets``() =
        let text = "English, Русский, 日本語, 123"
        let expected = [|"English"; "Русский"; "日本語"; "123"|]
        
        let result = Tokenizer.tokenizeWithUnicode text
        
        result |> should equal expected
    
    [<Test>]
    member _.``Tokenize should handle empty text``() =
        let text = ""
        let expected = [||]
        
        let result = Tokenizer.tokenize text
        
        result |> should equal expected