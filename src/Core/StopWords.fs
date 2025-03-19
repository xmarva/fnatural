namespace FNatural.Core

open System
open System.Collections.Generic

module StopWords =
    type Language = 
        | English
        | Russian
    
    let private englishStopWords = lazy(
        Set.ofArray [|
            "a"; "an"; "the"; "and"; "or"; "but"; "if"; "because"; "as"; "until"
            "while"; "of"; "at"; "by"; "for"; "with"; "about"; "against"; "between"
            "into"; "through"; "during"; "before"; "after"; "above"; "below"; "to"
            "from"; "up"; "down"; "in"; "out"; "on"; "off"; "over"; "under"; "again"
            "further"; "then"; "once"; "here"; "there"; "when"; "where"; "why"; "how"
            "all"; "any"; "both"; "each"; "few"; "more"; "most"; "other"; "some"; "such"
            "no"; "nor"; "not"; "only"; "own"; "same"; "so"; "than"; "too"; "very"
            "s"; "t"; "can"; "will"; "just"; "don"; "should"; "now"
        |]
    )
    
    let private russianStopWords = lazy(
        Set.ofArray [|
            "и"; "в"; "во"; "не"; "что"; "он"; "на"; "я"; "с"; "со"; "как"; "а"; "то"
            "все"; "она"; "так"; "его"; "но"; "да"; "ты"; "к"; "у"; "же"; "вы"; "за"
            "бы"; "по"; "только"; "ее"; "мне"; "было"; "вот"; "от"; "меня"; "еще"
            "нет"; "о"; "из"; "ему"; "теперь"; "когда"; "даже"; "ну"; "вдруг"; "ли"
            "если"; "уже"; "или"; "ни"; "быть"; "был"; "него"; "до"; "вас"; "нибудь"
            "опять"; "уж"; "вам"; "сказал"; "ведь"; "там"; "потом"; "себя"; "ничего"
            "ей"; "может"; "они"; "тут"; "где"; "есть"; "надо"; "ней"; "для"; "мы"
            "тебя"; "их"; "чем"; "была"; "сам"; "чтоб"; "без"; "будто"; "человек"
            "чего"; "раз"; "тоже"; "себе"; "под"; "жизнь"; "будет"; "ж"; "тогда"
            "кто"; "этот"; "говорил"; "того"; "потому"; "этого"; "какой"; "совсем"
            "ним"; "здесь"; "этом"; "один"; "почти"; "мой"; "тем"; "чтобы"; "нее"
            "кажется"; "сейчас"; "были"; "куда"; "зачем"; "сказать"; "всех"; "никогда"
            "можно"; "при"; "наконец"; "два"; "об"; "другой"; "хоть"; "после"; "над"
            "больше"; "тот"; "через"; "эти"; "нас"; "про"; "всего"; "них"; "какая"
            "много"; "разве"; "сказала"; "три"; "эту"; "моя"; "впрочем"; "хорошо"
            "свою"; "этой"; "перед"; "иногда"; "лучше"; "чуть"; "том"; "нельзя"
        |]
    )
    
    let getStopWords language =
        match language with
        | English -> englishStopWords.Value
        | Russian -> russianStopWords.Value
    
    let isStopWord language word =
        (getStopWords language).Contains(word.ToLower())
    
    let removeStopWords language (words: string[]) =
        let stopWords = getStopWords language
        words |> Array.filter (fun word -> not (stopWords.Contains(word.ToLower())))