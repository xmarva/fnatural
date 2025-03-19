#load "../src/Core/Tokenizer.fs"
#load "../src/Core/Stemmer.fs"
#load "../src/Core/StopWords.fs"
#load "../src/Models/NaiveBayes.fs"
#load "../src/Data/Preprocessing.fs"

open FNatural.Core
open FNatural.Models
open FNatural.Data

// Example labeled data
let positiveSamples = [|
    "Отличный продукт, очень доволен покупкой"
    "Прекрасное качество, рекомендую всем"
    "Быстрая доставка и отличный сервис"
    "Полностью удовлетворен, буду заказывать еще"
    "Превзошел все ожидания, очень понравился"
|]

let negativeSamples = [|
    "Плохое качество, не рекомендую"
    "Разочарован покупкой, зря потратил деньги"
    "Долгая доставка и плохой сервис"
    "Ужасный продукт, не работает как описано"
    "Не соответствует описанию, верну обратно"
|]

// Preprocess the samples
let preprocessText (text: string) =
    text
    |> Preprocessing.cleanText
    |> Preprocessing.removePunctuation
    |> Preprocessing.removeExtraWhitespace
    |> Tokenizer.tokenizeWords
    |> StopWords.removeStopWords StopWords.Language.Russian
    |> Stemmer.stemMany Stemmer.Language.Russian

// Prepare labeled data
let labeledData =
    Array.append
        (positiveSamples |> Array.map (fun text -> preprocessText text, "positive"))
        (negativeSamples |> Array.map (fun text -> preprocessText text, "negative"))

// Train the model
let model = NaiveBayes.fit labeledData 1.0

// Test the model
let testSamples = [|
    "Хороший продукт, но цена высоковата"
    "Не понравилось совсем, не рекомендую"
    "Качество на высоте, буду заказывать еще"
|]

let predictions =
    testSamples
    |> Array.map (fun text -> 
        let processed = preprocessText text
        let sentiment = NaiveBayes.predict model processed
        text, sentiment)

// Print results
printfn "Sentiment Analysis Results:"
predictions 
|> Array.iter (fun (text, sentiment) -> 
    printfn "Text: %s\nSentiment: %s\n" text sentiment)