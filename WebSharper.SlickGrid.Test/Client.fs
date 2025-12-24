namespace WebSharper.SlickGrid.Test

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.SlickGrid

[<JavaScript>]
module Client =
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let SetupGrid () =
        let columns = [|
            Column<obj>(id = "title", field = "title", Name = "Title", Sortable = true, Width = 200)
            Column(id = "duration", field = "duration", Name = "Duration", Sortable = true)
            Column(id = "percentComplete", field = "percentComplete", Name = "% Complete", Formatter = Formatters.PercentCompleteBar, Width = 250)
            Column(id = "start", field = "start", Name = "Start")
            Column(id = "finish", field = "finish", Name = "Finish")
            Column(
                id = "effortDriven", 
                field = "effortDriven", 
                Name = "Effort Driven",                 
                Formatter = Formatters.Checkmark, 
                CssClass = "text-center"
            )
        |]

        let options = GridOption(
            EnableCellNavigation = true,
            EnableColumnReorder = false,
            RowHeight = 35
        )

        let data = 
            [| for i in 0 .. 499 do
                yield New [
                    "id" => i
                    "title" => "Task " + string i
                    "duration" => "5 days"
                    "percentComplete" => Math.Round(Math.Random() * 100.0)
                    "start" => "01/01/2024"
                    "finish" => "01/05/2024"
                    "effortDriven" => (i % 5 = 0)
                ]
            |]

        Styles.Grid()
        Styles.AlpineTheme()

        SlickGrid("#myGrid", data, columns, options)

    [<SPAEntryPoint>]
    let Main () =
        SetupGrid()