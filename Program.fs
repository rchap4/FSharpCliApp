// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open CommandLine
open CommandLine.Text

type options = {
    [<Option(HelpText = "Provide a CSV Path", Default = "~/data.csv")>] csvPath: string;
    [<Option('d', Min=1, Max = 7, HelpText = "Enter a range of days" )>] days: int seq;
    [<Value(0, MetaName = "scalar", HelpText = "A optional long scalar here.")>] longValue : int64 option;
}
with
    [<Usage(ApplicationAlias = "FSharpCliApp")>]
    static member examples
        with get() = seq {
           yield  Example("Arguments", {csvPath = "~/file.csv"; days = seq {1..2}; longValue = Some 10L; })
        }

let formatLong o =
  match o with
    | Some(v) -> string v
    | _ -> "{None}"

let formatInput (o : options)  =
    sprintf "--csvPath: %s\n-d: %A\n-scalar: %s\n" o.csvPath o.days (formatLong o.longValue)


let inline (|Success|Help|Version|Fail|) (result : ParserResult<'a>) =
    match result with
    | :? Parsed<'a> as parsed -> Success(parsed.Value)
    | :? NotParsed<'a> as notParsed when notParsed.Errors.IsHelp() -> Help
    | :? NotParsed<'a> as notParsed when notParsed.Errors.IsVersion() -> Version
    | :? NotParsed<'a> as notParsed -> Fail(notParsed.Errors)
    | _ -> failwith "invalid parser result"

[<EntryPoint>]
let main argv =
    let argsResult = Parser.Default.ParseArguments<options>(argv)

    match argsResult with
    | Success(o) -> printf  "%s" (formatInput  o)
    | Fail(errs) -> printf "Invalid: %A, Errors: %d\n" argv (Seq.length errs)
    | Help | Version -> ()
     // return an integer exit codeß
    0