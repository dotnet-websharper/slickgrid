namespace WebSharper.SlickGrid.Test

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.SlickGrid

[<JavaScript>]
module Client =

    // The template is the whole document, loaded from the DOM, so editing
    // wwwroot/index.html and refreshing the browser is enough.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    // Theme: light is the default; the choice is persisted in localStorage.
    // The storage key is shared with the bootstrap script in index.html.
    let themeStorageKey = "websharper-slickgrid-theme"
    let theme = Var.Create "light"
    let themeLabel = Var.Create "🌙 Dark mode"
    let status = Var.Create "Ready — the first column is pinned to the left."

    // The grid instance is created only after the template is bound.
    let mutable grid : option<SlickGrid<obj>> = None
    let mutable data : obj[] = [||]
    let mutable nextId = 500
    let mutable pinnedFirstColumn = true

    let applyTheme () =
        JS.Document.DocumentElement.SetAttribute("data-theme", theme.Value)
        themeLabel.Value <- if theme.Value = "dark" then "☀️ Light mode" else "🌙 Dark mode"

    let toggleTheme () =
        theme.Value <- (if theme.Value = "dark" then "light" else "dark")
        try JS.Window.LocalStorage.SetItem(themeStorageKey, theme.Value) with _ -> ()
        applyTheme ()

    // ---------------------------------------------------------------
    // Data, columns and options
    // ---------------------------------------------------------------

    let makeData () : obj[] =
        [| for i in 0 .. 499 do
            yield New [
                "id" => i
                "title" => "Task " + string i
                "duration" => string (1 + (i % 7)) + " days"
                "percentComplete" => Math.Round(Math.Random() * 100.0)
                "start" => sprintf "%02d/01/2026" (1 + i % 12)
                "finish" => sprintf "%02d/05/2026" (1 + i % 12)
                "effortDriven" => (i % 5 = 0)
            ]
        |]

    let makeColumns () : Column<obj>[] =
        [|
            Column<obj>(id = "title", field = "title", Name = "Title", Sortable = true, Width = 220)
            Column(id = "duration", field = "duration", Name = "Duration", Sortable = true, Width = 110)
            Column(id = "percentComplete", field = "percentComplete", Name = "% Complete", Formatter = Formatters.PercentCompleteBar, Width = 220)
            Column(id = "start", field = "start", Name = "Start", Width = 140)
            Column(id = "finish", field = "finish", Name = "Finish", Width = 140)
            Column(id = "effortDriven", field = "effortDriven", Name = "Effort Driven", Formatter = Formatters.Checkmark, CssClass = "text-center", Width = 150)
        |]

    // SlickGrid 6 replaced the old `frozenColumn` option with `pinning`.
    let makeOptions () : GridOption<Column<obj>> =
        GridOption(
            EnableCellNavigation = true,
            EnableTextSelectionOnCells = true,
            ShowCellSelection = true,
            RowHeight = 34,
            MultiColumnSort = false,
            Pinning = PinningOption(Columns = PinnedColumns(Left = 0))
        )

    // ---------------------------------------------------------------
    // Interactions
    // ---------------------------------------------------------------

    let togglePin () =
        match grid with
        | Some g when pinnedFirstColumn ->
            g.SetColumnPinning("title", null)
            pinnedFirstColumn <- false
            status.Value <- "First column unpinned."
        | Some g ->
            g.SetColumnPinning("title", "left")
            pinnedFirstColumn <- true
            status.Value <- "First column pinned to the left."
        | None ->
            status.Value <- "The grid is not ready yet."

    let scrollToRow () =
        match grid with
        | Some g ->
            g.ScrollRowIntoView(250)
            status.Value <- "Scrolled to row 250."
        | None -> ()

    let addRow () =
        match grid with
        | Some g ->
            nextId <- nextId + 1
            let item =
                New [
                    "id" => nextId
                    "title" => "Task " + string nextId
                    "duration" => "3 days"
                    "percentComplete" => 0.0
                    "start" => "01/01/2026"
                    "finish" => "01/05/2026"
                    "effortDriven" => false
                ]
            data <- Array.append data [| item |]
            g.SetData(data, true)
            g.Render()
            status.Value <- sprintf "Added row %d — the grid now has %d rows." nextId (g.GetDataLength())
        | None -> ()

    let autosizeColumns () =
        match grid with
        | Some g ->
            g.AutosizeColumns()
            status.Value <- "Columns auto-sized."
        | None -> ()

    // ---------------------------------------------------------------
    // Entry point
    // ---------------------------------------------------------------

    [<SPAEntryPoint>]
    let Main () =
        // Restore a previously chosen theme (light is the default).
        let savedTheme =
            try JS.Window.LocalStorage.GetItem(themeStorageKey) with _ -> null
        if savedTheme = "dark" || savedTheme = "light" then
            theme.Value <- savedTheme
        applyTheme ()

        // `.Bind()` renders the whole document body, so #myGrid exists only now.
        IndexTemplate()
            .status(status.V)
            .themeLabel(themeLabel.V)
            .ToggleTheme(fun _ -> toggleTheme ())
            .TogglePin(fun _ -> togglePin ())
            .ScrollToRow(fun _ -> scrollToRow ())
            .AddRow(fun _ -> addRow ())
            .AutosizeColumns(fun _ -> autosizeColumns ())
            .Bind()

        // SlickGrid's grid + alpine theme stylesheets come from the npm package;
        // these calls register the CSS files with the bundle.
        Styles.Grid()
        Styles.AlpineTheme()
        Styles.Icons()

        data <- makeData()
        let g = SlickGrid("#myGrid", data, makeColumns(), makeOptions())
        grid <- Some g

        g.OnClick.Subscribe(fun (_e: Dom.Event, args: OnClickEventArgs) ->
            status.Value <- sprintf "Clicked row %d · cell %d" args.Row args.Cell
            null)

        status.Value <- sprintf "%d rows · first column pinned to the left." (g.GetDataLength())
