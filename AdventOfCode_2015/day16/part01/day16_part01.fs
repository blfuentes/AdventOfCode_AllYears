module day16_part01

open System.Collections.Generic
open System.Text.RegularExpressions
open AdventOfCode_2015.Modules

type AuntSue = {
    Id: int
    Children: int option
    Cats: int option
    Samoyeds: int option
    Pomeranians: int option
    Akitas: int option
    Vizslas: int option
    Goldfish: int option
    Trees: int option
    Cars: int option
    Perfumes: int option
}

let getValue(dic: Dictionary<string, int>)(attr: string) =
    if dic.ContainsKey(attr) then
        Some((int)(dic[attr]))
    else
        None

let parseContent(lines: string array) =
    let regexpattern = @"Sue (?<id>\d+):(?:(?:\s(?<attribute>\w+): (?<value>\d+),?))+"

    lines
    |> Array.map(fun l ->
        let matche = Regex.Match(l, regexpattern)
        let attributes = Dictionary<string, int>()
        for attr, value in Seq.zip matche.Groups.["attribute"].Captures matche.Groups.["value"].Captures do
            attributes.Add(attr.Value, int value.Value)

        {
            Id = (int)(matche.Groups["id"].Value);
            Children = getValue attributes "children";
            Cats = getValue attributes "cats";
            Samoyeds = getValue attributes "samoyeds";
            Pomeranians = getValue attributes "pomeranians";
            Akitas = getValue attributes "akitas";
            Vizslas = getValue attributes "vizslas";
            Goldfish = getValue attributes "goldfish";
            Trees = getValue attributes "trees";
            Cars = getValue attributes "cars";
            Perfumes = getValue attributes "perfumes";
        }
    )

let MFCSAM = {
    Id = -1;
    Children = Some(3);
    Cats = Some(7);
    Samoyeds = Some(2);
    Pomeranians = Some(3);
    Akitas = Some(0);
    Vizslas = Some(0);
    Goldfish = Some(5);
    Trees = Some(3);
    Cars = Some(2);
    Perfumes = Some(1)
}

let findCompatible(giftlist: AuntSue) (auntlist: AuntSue array) =
    let fits((left, right): int option*int option) =
        if right.IsSome then
            left.Value = right.Value
        else
            true

    let auntFits((left, right): AuntSue*AuntSue) =
        fits(left.Children, right.Children) &&
        fits(left.Cats, right.Cats) &&
        fits(left.Samoyeds, right.Samoyeds) &&
        fits(left.Pomeranians, right.Pomeranians) &&
        fits(left.Akitas, right.Akitas) &&
        fits(left.Vizslas, right.Vizslas) &&
        fits(left.Goldfish, right.Goldfish) &&
        fits(left.Trees, right.Trees) &&
        fits(left.Cars, right.Cars) &&
        fits(left.Perfumes, right.Perfumes)

    let foundAunt = 
        auntlist
        |> Array.filter(fun a -> auntFits(giftlist, a))
    foundAunt[0].Id

let execute =
    let input = "./day16/day16_input.txt"
    let content = LocalHelper.GetLinesFromFile input
    let sueAunts = parseContent content
    findCompatible MFCSAM sueAunts