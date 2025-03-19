# FNatural

Minimal F# NLP library for functional text processing.

## Structure

```
src/
├─ Core/
│  ├─ Tokenizer.fs
│  ├─ Stemmer.fs
│  └─ StopWords.fs
├─ Models/
│  ├─ TfIdf.fs
│  └─ NaiveBayes.fs
├─ Similarity/
│  ├─ Jaccard.fs
│  └─ Cosine.fs
└─ Data/
   ├─ Loaders.fs
   └─ Preprocessing.fs
```

## Installation

```bash
dotnet add package FNatural
```

```bash
dotnet test tests/FNatural.Tests
```

## Usage

Text preprocessing pipeline:

```fsharp
"Sample text" 
|> Preprocessing.cleanText
|> Tokenizer.tokenizeWords
|> StopWords.removeStopWords StopWords.Language.English
```

Sentiment classification:

```fsharp
let model = NaiveBayes.fit trainingData 1.0
"Positive experience" |> preprocess |> NaiveBayes.predict model
```

Document similarity:

```fsharp
let vec1 = TfIdf.vectorize tfidfModel doc1
let vec2 = TfIdf.vectorize tfidfModel doc2
Cosine.similarity vec1 vec2
```