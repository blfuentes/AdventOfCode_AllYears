module day21_part02

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules

type Food = {
    Ingredients: string list
    Allergens: string list
}

let parseContent(lines: string array) =
    let parseLine line =
        match line with
        | ParseRegex @"([\w| ]+) (?:\(contains ([\w| |,]+)\))" [ingredients; allergens] ->
                {
                    Ingredients = ingredients |> splitBy ' '; 
                    Allergens = allergens |> splitByString ", ";
                }
        | invalid -> failwithf "Couldn't match line %s" invalid

    lines |> Seq.map parseLine

let getAllergenCount foods =
    foods
    |> Seq.collect (fun food -> food.Allergens)
    |> Seq.fold (fun allergenCounts allergen ->
                match allergenCounts |> Map.tryFind allergen with
                | Some allergenCount -> allergenCounts |>Map.add allergen (allergenCount + 1)
                | None -> allergenCounts |> Map.add allergen 1
            ) Map.empty

let identifyAllergens foods =
    let sortedAllergenCounts =
        foods
        |> getAllergenCount
        |> Map.toList
        |> List.sortByDescending snd
    let rec identifyAllergensRecursively identifiedAllergens toSkip =
        let nextAllergen =
            sortedAllergenCounts
            |> List.filter (fun (allergen, _) -> not (identifiedAllergens |> List.map fst |> List.contains allergen))
            |> List.skip toSkip
            |> List.head
            |> fst
        let foodIngredientsContainingAllergen =
            foods
            |> Seq.filter (fun food -> food.Allergens |> List.contains nextAllergen)
            |> Seq.map (
                    (fun food -> food.Ingredients) 
                    >> List.filter (fun ingredient -> not (identifiedAllergens |> List.map snd |> List.contains ingredient))
                    >> Set.ofList
                    )
        match Set.intersectMany foodIngredientsContainingAllergen with
        | commonIngredients when (Set.count commonIngredients) = 1 -> 
                let updatedIdentifiedAllergens = (nextAllergen, commonIngredients |> Set.toList |> List.head) :: identifiedAllergens
                if List.length updatedIdentifiedAllergens = List.length sortedAllergenCounts then
                    updatedIdentifiedAllergens
                else
                    identifyAllergensRecursively updatedIdentifiedAllergens 0
        | commonIngredients when (Set.count commonIngredients) > 1 ->
            identifyAllergensRecursively identifiedAllergens (toSkip + 1)
        | _ -> failwithf "Found no ingredients which match allergen %s" nextAllergen
    identifyAllergensRecursively [] 0

let findAllergens foods =
    let identifiedAllergens =
        identifyAllergens foods

    identifiedAllergens
        |> List.sortBy fst
        |> List.map snd
        |> String.concat ","

let execute =
    let path = "day21/day21_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let foods = parseContent content
    findAllergens foods