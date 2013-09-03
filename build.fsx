#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("IntelliFactory.WebSharper.SlickGrid", "2.5")

let resourceFiles =
    [
        "SlickGrid/lib/jquery.event.drag-2.2.js"
        "SlickGrid/slick.grid.css"
        "SlickGrid/slick.grid.js"
        "SlickGrid/slick.core.js"
        "SlickGrid/slick.dataview.js"
        "SlickGrid/slick.editors.js"
        "SlickGrid/slick.formatters.js"
        "SlickGrid/plugins/slick.autotooltips.js"
        "SlickGrid/plugins/slick.cellcopymanager.js"
        "SlickGrid/plugins/slick.cellrangedecorator.js"
        "SlickGrid/plugins/slick.cellrangeselector.js"
        "SlickGrid/plugins/slick.cellselectionmodel.js"
        "SlickGrid/plugins/slick.checkboxselectcolumn.js"
        "SlickGrid/plugins/slick.rowmovemanager.js"
        "SlickGrid/plugins/slick.rowselectionmodel.js"
        "SlickGrid/controls/slick.columnpicker.css"
        "SlickGrid/controls/slick.columnpicker.js"
        "SlickGrid/controls/slick.pager.css"
        "SlickGrid/controls/slick.pager.js"
    ]

let main =
    bt.WebSharper.Extension("IntelliFactory.WebSharper.SlickGrid")
        .SourcesFromProject()
        .Embed(resourceFiles)

// let website =
//     bt.WebSharper.Library("Website")
//         .SourcesFromProject()
//         .References(fun r ->
//             [
//                 r.Assembly("System.Web")
//                 r.Project main
//             ])

// let web =
//     bt.WebSharper.HostWebsite("Web")
//         .References(fun r ->
//             [
//                 r.Project main
//                 r.Project website
//                 r.NuGet("WebSharper").At(["/tools/net45/IntelliFactory.Xml.dll"]).Reference()
//             ])

bt.Solution [

    main
    // website
    // web

    bt.NuGet.CreatePackage()
        .Add(main)
        .Description("SlickGrid bindings for WebSharper")

]
|> bt.Dispatch