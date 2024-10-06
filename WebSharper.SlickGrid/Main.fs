namespace WebSharper.SlickGrid

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator
open WebSharper.JavaScript.Dom

module Definition =

    let ImportFrom g (c: CodeModel.Class) =
        c
        |> Import c.Name ("SlickGrid" + "/" + g ) 

    module Res =

    

    (* //Need to use Sortable.js (to-do)
        let JQueryEventDrag =
            (Resource "JQueryEventDrag" "jquery.event.drag-2.2.js").AssemblyWide()

        let JQueryEventDrop =
            (Resource "JQueryEventDrop" "jquery.event.drop-2.2.js").AssemblyWide()
            *)

        let Css =
            (Resource "Css" "slick.grid.scss").AssemblyWide()
            //need to remove jquery dep (to-do):
            //|> RequiresExternal [T<JQueryUI.Dependencies.JQueryUICss>]

        let Core =
            (Resource "Core" "slick.core.ts").AssemblyWide()
            //need to remove jquery dep (to-do):
            //|> Requires [JQueryEventDrag; JQueryEventDrop; Css]
            //|> RequiresExternal [T<JQuery.Resources.JQuery>]

        let DefaultTheme =
            (Resource "DefaultTheme" "slick-default-theme.scss").AssemblyWide()
            |> Requires [Css]

        let Js =
            (Resource "Js" "slick.grid.ts").AssemblyWide()
            |> Requires [Core]
            //need to remove jquery dep (to-do):
            //|> RequiresExternal [T<JQueryUI.Dependencies.JQueryUIJs>]
        
        //added
        let CompositeEditor = 
            Resource "CompositeEditor" "slick.compositeeditor.ts"
            |> Requires [Js]

        let Dataview =
            Resource "Dataview" "slick.dataview.ts"
            |> Requires [Js]

        let Editors =
            Resource "Editors" "slick.editors.ts"
            |> Requires [Js]

        let Formatters =
            Resource "Formatters" "slick.formatters.ts"
            |> Requires [Js]

        let GroupItemMetadataProvider =
            Resource "GroupItemsMetadataProvider" "slick.groupitemmetadataprovider.ts"
            |> Requires [Js]

        //added
        let Interactions =
            Resource "Interactions" "slick.interactions.ts"
            |> Requires [Js]

        //added
        let RemoteModel =
            Resource "RemoteModel" "slick.remotemodel.ts"
            |> Requires [Js]

        module Plugins =

            let Autotooltips =
                Resource "Autotooltips" "plugins/slick.autotooltips.ts"

            let Cellcopymanager =
                Resource "Cellcopymanager" "plugins/slick.cellcopymanager.ts"

            //added
            let Cellexternalcopymanager =
                Resource "Cellexternalcopymanager" "plugins/slick.cellexternalcopymanager.ts"

            //added
            let Cellmenu =
                Resource "Cellmenu" "plugins/slick.cellmenu.ts"

            let Cellrangedecorator =
                Resource "Cellrangedecorator" "plugins/slick.cellrangedecorator.ts"

            let Cellrangeselector =
                Resource "Cellrangeselector" "plugins/slick.cellrangeselector.ts"
                |> Requires [Cellrangedecorator]

            let Cellselectionmodel =
                Resource "Cellselectionmodel" "plugins/slick.cellselectionmodel.ts"
                |> Requires [Cellrangeselector]

            let Checkboxselectcolumn =
                Resource "Checkboxselectcolumn" "plugins/slick.checkboxselectcolumn.ts"

            //added
            let Contextmenu =
                Resource "Contextmenu" "plugins/slick.contextmenu.ts"

            //added
            let Crossgridrowmovemanager =
                Resource "Crossgridrowmovemanager" "plugins/slick.crossgridrowmovemanager.ts"

            //added
            let Customtooltip =
                Resource "Customtooltip" "plugins/slick.customtooltip.ts"

            //added
            let Draggablegrouping =
                Resource "Draggablegrouping" "plugins/slick.draggablegrouping.ts"
            
            let HeaderButtonsCss =
                Resource "HeaderbuttonsCss" "styles/slick.headerbuttons.scss"

            let HeaderButtons =
                Resource "Headerbuttons" "plugins/slick.headerbuttons.ts"
                |> Requires [HeaderButtonsCss]

            let HeaderMenuCss =
                Resource "HeadermneuCss" "plugins/slick.headermenu.scss"

            let HeaderMenu =
                Resource "Headermenu" "plugins/slick.headermenu.ts"
                |> Requires [HeaderMenuCss]

            //added
            let Resizer =
                Resource "Resizer" "plugins/slick.resizer.ts"

            //added
            let Rowdetailview =
                Resource "Rowdetailview" "plugins/slick.rowdetailview.ts"

            let Rowmovemanager =
                Resource "Rowmovemanager" "plugins/slick.rowmovemanager.ts"

            let Rowselectionmodel =
                Resource "Rowselectionmodel" "plugins/slick.rowselectionmodel.ts"

            //added
            let State =
                Resource "State" "plugins/slick.state.ts"

        module Controls =

            //ADDED
            let Columnmenu =
                Resource "Columnmenu" "controls/slick.columnmenu.ts"

            let ColumnpickerCss =
                Resource "ColumnpickerCss" "styles/slick.columnpicker.scss"

            let Columnpicker =
                Resource "Columnpicker" "controls/slick.columnpicker.ts"
                |> Requires [ColumnpickerCss]

            //ADDED
            let Gridmenu =
                Resource "Gridmenu" "controls/slick.gridmenu.ts"

            let PagerCss =
                Resource "PagerCss" "styles/slick.pager.scss"

            let Pager =
                Resource "Pager" "controls/slick.pager.ts"
                |> Requires [PagerCss]

   
    
    
    //****NOT Found
    let Box =
        Class "Slick.Box"
        |+> Instance [
                "height" =? T<int>
                "width" =? T<int>
                "visible" =? T<bool>
            ]
    // ?? Not sure of this one; there is a slickgrid function absBox(elem)
    let AbsBox =
        Class "Slick.AbsBox"
        |=> Inherits Box
        |+> Instance [
                "bottom" =? T<int>
                "left" =? T<int>
                "right" =? T<int>
                "top" =? T<int>
            ]

    let Grid_t = Type.New()

    //renamed from AutoTooltipsOptions
    let AutoTooltipOption =
        Pattern.Config "AutoTooltipOption" {   //was slick.auto..
            Required = []
            Optional =
                [
                    "maxToolTipLength", T<int>
                    //added three options:
                    "enableForCells", T<bool>
                    "enableForHeaderCells", T<bool>
                    "replaceExisting", T<bool>
                ]
        }        
        |> ImportFrom "models/AutoTooltipOption.interface.ts"

    let AutoTooltips =
        Generic - fun t ->   //I notice all the 'Generic - fun 's are commented out in WSThreeJS......
            Class "AutoTooltips"  //was Slick.Auto..
            |> ImportFrom "plugins/slick.autotooltips.ts"
            |+> Static [
                    Constructor (!?AutoTooltipOption)
                ]
            |+> Instance [
                    "destroy" => T<unit -> unit>
                    "init" => Grid_t.[t] ^-> T<unit>
                ]

    //****updated name
    let EditorConstructor =
        Generic - fun t ->
            Class "EditorConstructor"      // was Slick.Editor..
        |> ImportFrom "models/editor.interface.ts"

    //****updated name from editorvalidatorresult
    let EditorValidationResult =
        Pattern.Config "Slick.EditorValidationResult" {
            Optional =
                [
                    "msg", T<string>
                    "errors", T<obj[]>  //added ** type of errors is 'any' array, is it correct & Type-safe ?
                ]
            Required =
                [
                    "valid", T<bool>
                ]
        }
        |> ImportFrom "models/EditorValidationResult.interface.ts"

    let Column_t = Type.New()

    let Formatter (t: Type.IType) = (T<int> * T<int> * T<string> * Column_t.[t] * t) ^-> T<string>

    //****not found yet - now SlickGroupTotals??
    let Total =
        Class "Slick.Total"
        |+> Instance [
            "sum" =? T<obj>
            "avg" =? T<obj>
        ]

    //added CellMenuOption
    let CellMenuOption =  
        Pattern.Config "CellMenuOption" {   //was slick.Cell..
            Required = []
            Optional =
                [
                    "hideMenuOnScroll", T<bool>
                    "hideOptionSection", T<bool>
                    "maxHeight", T<int>
                    "maxWidth", T<int>
                    "width", T<int>
                    // "optionItems", T<MenuOptionItem | divider [||]>   //**how to implement???
                    "optionTitle", T<string>
                    "subItemChevronClass", T<string>
                    //***how to implement the following two???????
                    //subMenuOpenByEvent?: 'mouseover' | 'click'
                    ///menuUsabilityOverride?: (args: MenuCallbackArgs) => boolean
                ]

        }
        |> ImportFrom "models/CellMenuOption.interface.ts"

    let Column =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Pattern.Config "Column" {   //was Slick.Column
                Required =
                    [
                        "id", T<string>
                        "name", T<string>  //** name can also be HTML element or doc fragment?
                    ]
                Optional =
                    [
                        //added many props
                        "alwaysRenderColumn", T<bool>
                        "asyncPostRender", T<bool>
                        "asyncPostRenderCleanup", T<bool>
                        "autoSize", T<bool>
                        "behavior", T<string>
                        "cannotTriggerInsert", T<bool>
                        //**** not sure how to implement cellAttrs
                        //"cellAttrs", T<t>
                        "cellMenu", CellMenuOption.[t]   //**type cellmenuoption, how to implement???
                        "columnGroup", T<string>
                        "colspan", T<int>
                        "cssClass", T<string> 
                        //"customTooltip", T<CustomTooltipOption>  //**type customtooltipoption, how to implement???
                        "defaultSortAsc", T<bool>
                        "denyPaste", T<bool>
                        "disableTooltip", T<bool>
                        "editor", EditorConstructor.[t]
                        "editorFixedDecimalPlaces", T<int>
                        "excludeFromColumnPicker", T<bool>
                        "excludeFromExport", T<bool>
                        "excludeFromGridMenu", T<bool>
                        "excludeFromQuery", T<bool>
                        "excludeFromHeaderMenu", T<bool>
                        //**not sure about field as it has type Join<PathsToStringProps<TData>, '.'>
                        "field", T<string>
                        "focusable", T<bool>
                        "formatter", Formatter t
                        //"formatterOverride",  //*** don't know how to implement
                        //"grouping", //*** don't know how to implement
                        "groupTotalsFormatter", Total * T<string> ^-> T<string>  //** is this right - should take GroupTotalsFormatter?
                        //"header"  //*** don't know how to implement
                        //"headerCellAttrs", T<t>   ////****how to implement?
                        "headerCssClass", T<string>
                        "hidden", T<bool>
                        "maxWidth", T<int>
                        "minWidth", T<int>
                        "offsetWidth", T<int>
                        "previousWidth", T<int>
                        "reorderable", T<bool>
                        "rerenderOnResize", T<bool>
                        "resizable", T<bool>
                        "selectable", T<bool>
                        "sortable", T<bool>
                        "toolTip", T<string>
                        "unselectable", T<bool>
                        "validator", T<string> ^-> EditorValidationResult //** should take EditorValidator??
                        "width", T<int>
                        "widthRequest", T<int>
                    ]
            }
        |=> Column_t
        |> ImportFrom "models/column.interface.ts"
        
    //gridEvents.interface.ts interfaces have added 'On' before names; many more of the interfaces not included here, most/
    //are handled further down under let Grid =.. 
    let OnBeforeEditCellEventArgs =
        Generic - fun t ->
            Class "Slick.OnBeforeEditCellEventArgs"
            |> ImportFrom "models/gridEvents.interface.ts"
            |+> Instance [
                    "cell" =? T<int>
                    "column" =? Column.[t]
                    "item" =? t
                    "row" =? T<int>
                    //** also overload to instead specify 'composite' and give CompositeEditorOptions- how to implement?
                ]
    
    //provides input to SlickRowMoveManager
    let BeforeMoveRowsArgs =
        Class "Slick.BeforeMoveRowsArgs"
        |+> Instance [
                "insertBefore" =? T<int>
                "rows" =? T<int[]>
                //"grid" =? Grid_t // Grid definition is much later in file....
            ]
    

    let OnCellChangeEventArgs =   
        Generic - fun t ->
            Class "Slick.OnCellChangeEventArgs"
            |> ImportFrom "models/gridEvents.interface.ts"
            |+> Instance [
                    "cell" =? T<int>
                    "item" =? t
                    "row" =? T<int>
                    //added:
                    "column" =? Column.[t]
                ]
        
    //** used to supply values for cell and row to events from gridEvents.interface
    let CellCoords =
        Pattern.Config "Slick.CellCoords" {
            Required =
                [
                    "cell", T<int>
                    "row", T<int>
                ]
            Optional = []
        }

    let Event =
        Generic - fun t ->
            Class "SlickEvent"  //was slick.Event
            |> Requires [Res.Core]
            //from slick.core.ts available assembly wide
            |+> Instance [
                    "notify" => t * !?T<Event> * !?T<obj> ^-> T<unit>
                    "subscribe" => (T<Event> * t ^-> T<unit>) ^-> T<unit>
                    "unsubscribe" => (T<Event> * t ^-> T<unit>) ^-> T<unit>
                    "subscriberCount" => T<unit> ^-> T<int> //added
                    "setPubSubService" => (T<obj> ^-> T<unit>)  //added
                ]
        
    //** not found - 
    let RangesArgs =  
        Class "Slick.RangesArgs"  
        |+> Instance [
                "ranges" =? T<obj>
            ]
            

    let CellCssEventArgs =
        Class "Slick.CellCssEventArgs"
        |+> Instance [
                "hash" =? T<obj>
                "key" =? T<string>
            ]

    let Range =
        Class "SlickRange"  
        //from slick.core.ts; was Slick.Range
        |+> Static [
                Constructor (T<int> * T<int> * T<int> * T<int>)
            ]
        |+> Instance [
                "fromCell" =? T<int>
                "fromRow" =? T<int>
                "toCell" =? T<int>
                "toRow" =? T<int>
                "isSingleCell" => T<unit -> bool>
                "isSingleRow" => T<unit -> bool>
                //added
                "contains" => T<int> * T<int> ^-> T<bool>
                "toString" => T<unit -> string>
            ]

    //used to use 'RangesArgs' now uses 'Range'
    let CellCopyManager =
        Generic - fun t ->
            Class "SlickCellCopyManager"   // was Slick.CellCopyManager
            |> ImportFrom "plugins/slick.cellcopymanager.ts"
            |+> Instance [
                    "onCopyCancelled" =? Event.[Range]
                    "onCopyCells" =? Event.[Range]
                    //"clearCopySelection" => T<unit> ^-> T<unit>  //not found
                    "onPasteCells" =? Event.[Range * Range]  //added
                    "destroy" => T<unit -> unit>
                    "init" => Grid_t.[t] ^-> T<unit>
                ]
        

    let CellRangeDecoratorOption =  //was ..Options
        Pattern.Config "CellRangeDecoratorOption" {    // was ..Slick.CellRangeDecoratorOptions
            Required = []
            Optional =
                [
                    "selectionCssClass", T<string>  //added
                    "selectionCss", T<obj>
                    "offset", T<int> * T<int> * T<int> * T<int>  //added
                ]
        }
        |> ImportFrom "models/cellRange.interface.ts"

    let CellRangeDecorator =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "SlickCellRangeDecorator"  //was Slick.CellRangeDecorator
            |> ImportFrom "plugins/slick.cellrangedecorator.ts"
            |+> Static [Constructor (Grid_t.[t] * !?CellRangeDecoratorOption)]
            |+> Instance [
                    "hide" => T<unit> ^-> T<unit>
                    "show" => Range ^-> T<Element>     //??was T<JQuery>
                ]
        
    let CellRangeSelectorOption =  //was ..Options        
        Pattern.Config "CellRangeSelectorOption" {   //was Slick.CellrangeselectorOptions
            Required = []
            Optional =
                [
                    "autoScroll", T<bool>  //added
                    "minIntervalToShowNextCell", T<int>  //added
                    "maxIntervalToShowNextCell", T<int>  //added
                    "accelerateInterval", T<int>  //added
                    "cellDecorator", T<unit> ^-> CellRangeDecorator  //added
                    "selectionCss", T<obj>
                ]
        }
        |> ImportFrom "models/cellRange.interface.ts"

    let CellRangeSelector =
        Generic - fun t ->
            Class "SlickCellRangeSelector"  //was Slick.CellRangeSelector
            |> ImportFrom "plugins/slick.cellrangeselector.ts"
            |+> Static [Constructor (!?CellRangeSelectorOption)]   //was ..Options
            |+> Instance [
                    "onBeforeCellRangeSelected" =? Event.[CellCoords]
                    "onCellRangeSelected" =? Event.[Range]
                    "onCellRangeSelecting" =? Event.[Range]  //added
                    "destroy" => T<unit -> unit>
                    "init" => Grid_t.[t] ^-> T<unit>
                    //"getCellDecorator" => T<unit> ^-> CellRangeDecorator  //added
                    "getCurrentRange" => T<unit> ^-> Range  //added
                ]
    
    let SelectionModel =
        Generic - fun t ->
            Class "SelectionModel"   //was Slick.Sel..
            |> ImportFrom "models/selectionModel.type.ts"
            |+> Instance [
                    //"destroy" => T<unit -> unit>
                    "getSelectedRanges" => T<unit> ^-> Type.ArrayOf Range
                    //"init" => Grid_t.[t] ^-> T<unit>
                    "setSelectedRanges" => Type.ArrayOf Range ^-> T<unit>
                    "refreshSelections" => T<unit -> unit>  //added
                    "onSelectedRangesChanged" => Event[Type.ArrayOf Range]  //added
                ]

    let CellSelectionModelOption =  //was ..Options
        Generic - fun t ->
            Pattern.Config "CellSelectionModelOption" {  //was slick.CellSel..
                Required = 
                    [
                        "selectActiveCell", T<bool>
                    ]
                Optional =
                    [
                        "cellRangeSelector", CellRangeSelector.[t]  //added
                        //"selectionCss", T<obj>  //not found
                    ]
            }
        |> ImportFrom "plugins/slick.cellselectionmodel.ts"

    let CellSelectionModel =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "SlickCellSelectionModel"  //was Slick.CellSel..
            |=> Inherits SelectionModel.[t]
            |+> Static [Constructor (!?CellSelectionModelOption)]
            //looks like some functions could be added such as getSelectedRanges
        |> ImportFrom "plugins/slick.cellselectionmodel.ts" 


    //provides input to OnRowCountChanged
    let Change =
        Generic - fun t ->
            Pattern.Config "Slick.Change" {
                Required =
                    [
                        "current", t.Type
                        "previous", t.Type
                        "itemCount", t.Type  //added (int)
                        "callingOnRowsChanged", t.Type  //added; bool but later use calls up Int for params?
                    ]
                Optional = []
            }

    let CheckboxSelectorOption =
        Pattern.Config "Slick.CheckboxSelectorOption" {   //was Slick.CheckboxSelectColumnOptions; 
            Required = []
            Optional =
                [
                    "columnId", T<string>
                    "cssClass", T<string>
                    "toolTip", T<string>
                    "width", T<int>
                    //added multiple:
                    "applySelectOnAllPages", T<bool>
                    "field", T<string>
                    "columnIndexPosition", T<int>
                    "excludeFromColumnPicker", T<bool>
                    "excludeFromHeaderMenu", T<bool>
                    "hideSelectAllCheckbox", T<bool>
                    "hideInColumnTitleRow", T<bool>
                    "hideInFilterHeaderRow", T<bool>
                    "name", T<string>
                    "reorderable", T<bool>
                    //"selectableOverride", T<int> * T<???> * Grid_t ^-> T<bool>  //how to implement 'dataContext' type??
                ]
        }
        |> ImportFrom "models/checkboxSelectorOption.interface.ts"

    let CheckboxSelectColumn =
        Generic - fun t ->
            Class "SlickCheckboxSelectColumn"  //was Slick.Check..
            |> ImportFrom "plugins/slick.checkboxselectcolumn.ts"
            |+> Static [Constructor (!?CheckboxSelectorOption)]
            |+> Instance [
                    "destroy" => T<unit -> unit>
                    "getColumnDefinition" => T<unit> ^-> Column.[t]
                    "init" => Grid_t.[t] ^-> T<unit>
                    "createCheckBoxElement" => T<string> ^-> T<DocumentFragment>  //**added, check!
                    "selectRows" => T<int[] -> unit>  //added
                    "deselectRows" => T<int[] -> unit>  //added
                    "getOptions" => T<unit> ^-> CheckboxSelectorOption   //added
                    "setOptions" => CheckboxSelectorOption ^-> T<unit> //**added, check!
                ]

    let EditController =
        Interface "EditController" //was Slick.Edit..
        |> Import "EditController" "models/core.interface.ts"
        |+> [
                "cancelCurrentEdit" => T<unit -> bool>
                "commitCurrentEdit" => T<unit -> bool>
            ]

    //not found:
    let Position =
        Pattern.Config "Slick.Position" {
            Required =
                [
                    "left", T<int>
                    "top", T<int>
                ]
            Optional = []
        }


    let EditorArgs =
        Generic - fun t ->
            Class "EditorArguments"  //was Slick.EditorArgs
            |> ImportFrom "models/editorArguments.interface.ts"
            |+> Instance [
                    "column" =? Column.[t]
                    "columnMetaData" =? T<obj>  //added
                    "compositeEditorOptions" =? T<obj>  //added
                    "container" =? T<Element>
                    "dataView" =? T<DataView>  //added
                    "event" =? Event  //added
                    "grid" =? Grid_t  //added
                    "gridPosition" =? Position  //added
                    "position" =? Position
                    "cancelChanges" => T<unit -> unit>
                    "commitChanges" => T<unit -> unit>
                    "item" =? T<obj>  //added
                ]

    let Editor =
        let Editor_t = Type.New()
        Generic - fun t ->
            Pattern.Config "Slick.Editor" {
                Required =
                    [
                        "applyValue", T<obj> * T<obj> ^-> T<unit>
                        "cancel", T<unit -> unit>  //added
                        "datacontext", T<obj>  //added
                        "disabled", T<bool>  //added
                        "destroy", T<unit -> unit>
                        "focus", T<unit -> unit>
                        "hide", T<unit -> unit>  //added
                        "keyCaptureList", Type.ArrayOf T<int>  //added
                        "loadValue", t ^-> T<unit>
                        "position", T<obj -> unit>  //added
                        "preClick", T<unit -> unit>  //added
                        "serializeValue", T<unit -> string>
                        "save", T<unit -> unit>  //added
                        "show", T<unit -> unit>  //added
                        //"setValue" ?  //how to implement?
                        "applyValue", t * T<string> ^-> T<unit>
                        "isValueChanged", T<unit -> bool>
                        "validate", T<unit> ^-> EditorValidationResult
                    ]
                Optional = []
            }
            |=> Editor_t
            |+> Instance [
                    "create" => (EditorArgs.[t] ^-> Editor_t.[t]) ^-> EditorConstructor.[t]
                ]
        |> ImportFrom "models/editor.interface.ts"


    let EditorEventArgs =
        Generic - fun t ->
            Class "Slick.EditorEventArgs"
            |+> Instance [
                    "editor" =? Editor.[t]
                ]

    let EditorLock =   
        let EditorLock_t = Type.New()
        Class "SlickEditorLock"  //from slick.core.ts; was Slick.EditorLock
        |=> EditorLock_t
        |+> Static [
                Constructor T<unit>
                "global" =? EditorLock_t
            ]
        |+> Instance [
                "activate" => EditController ^-> T<unit>
                "cancelCurrentEdit" => T<unit -> bool>
                "commitCurrentEdit" => T<unit -> bool>
                "deactivate" => EditController ^-> T<unit>
                "isActive" => EditController ^-> T<bool>
            ]

    let Editors =
        Generic - fun t ->
            Class "Slick.Editors"
            |+> Static [
                    "Checkbox" =? EditorConstructor.[t]
                    "Date" =? EditorConstructor.[t]   //Not found??
                    "Float" =? EditorConstructor.[t]  //added
                    "Flatpickr" =? EditorConstructor.[t]  //added
                    "Integer" =? EditorConstructor.[t]
                    "LongText" =? EditorConstructor.[t]
                    "PercentComplete" =? EditorConstructor.[t]
                    "Text" =? EditorConstructor.[t]
                    "YesNoSelect" =? EditorConstructor.[t]
                ]

    let Formatters =
        Generic - fun t ->
            Class "Slick.Formatters"
            |> ImportFrom "slick.formatters.ts"  //added
            |+> Static [
                    "Checkmark" =? Formatter t
                    "PercentComplete" =? Formatter t
                    "PercentCompleteBar" =? Formatter t
                    "YesNo" =? Formatter t
                ]

    let FromToRangesArgs =
        Class "Slick.FromToRangesArgs"
        |+> Instance [
                "from" =? T<obj>
                "to" =? T<obj>
            ]

    let Group_t = Type.New()

    let NonDataItem =
        Interface "Slick.NonDataItem"

    let GroupTotals =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Pattern.Config "SlickGroupTotals" {  //was Slick.Group..
                Required = []
                Optional =
                    [
                        "group", Group_t.[t]
                    ]
            }
            |=> Implements [NonDataItem]
        |> ImportFrom "slick.core.ts"  //added

    let Group =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Pattern.Config "SlickGroup" {  //was Slick.Group
                Required = []
                Optional =
                    [
                        "collapsed", T<bool>
                        "count", T<int>
                        "title", T<string>
                        "totals", GroupTotals.[t]
                        "value", T<string>
                        "level", T<int>  //added
                        "selectChecked", T<bool>  //added
                        "rows", Type.ArrayOf T<int>  //added
                        "groups", Type.ArrayOf T<string>  //added
                        "groupingKey", T<string>  //added
                    ]
            }
            |=> Group_t
            |=> Implements [NonDataItem]
        |> ImportFrom "slick.core.ts"  //added

    let HeaderEventArgs =   //not found
        Generic - fun t ->
            Class "Slick.HeaderEventArgs"
            |+> Instance [
                    "column" =? Column.[t]
                ]

    let IEditorFactory =  //not found
        Generic - fun t ->
            Interface "Slick.IEditorFactory"
            |+> [
                    "getEditor" => Column.[t] ^-> Editor.[t]
                ]

    let IFormatterFactory =  //not found
        Generic - fun t ->
            Interface "Slick.IFormatterFactory"
            |+> [
                    "getFormatter" => Column.[t] ^-> Formatter t   //there is a protected 'getformatter' in slick.grid.ts
                ]

    let NewRowEventArgs =
        Generic - fun t ->
            Class "OnAddNewRowEventArgs"  //was Slick.NewRowEventArgs
            |> ImportFrom "models/gridEvents.interface.ts"
            |+> Instance [
                    "column" =? Column.[t]
                    "item" =? t
                ]
        

    let RowMoveManager =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "SlickRowMoveManager"   //plugins/slick.rowmovemanager.ts; was Slick.Row..
            |> ImportFrom "plugins/slick.rowmovemanager.ts"
            |+> Static [Constructor T<unit>]
            |+> Instance [
                    //"onBeforeMoveRows" =? Event.[BeforeMoveRowsArgs]
                    //"onMoveRows" =? Event.[BeforeMoveRowsArgs]
                    "destroy" => T<unit -> unit>
                    "init" => Grid_t.[t] ^-> T<unit>
                    "getColumnDefinition" => T<unit> ^-> Column  //added
                    // "setOptions" => ??
                ]     

    let RowSelectionModelOptions =
        Pattern.Config "Slick.RowSelectionModelOptions" {
            Required = []
            Optional =
                [
                    "selectActiveRow", T<bool>
                ]
        }

    let RowSelectionModel =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "SlickRowSelectionModel"  //was Slick.Row
            |> ImportFrom "plugins/slick.rowselectionmodel.ts"
            |=> Inherits SelectionModel.[t]
            |+> Static [Constructor (!?RowSelectionModelOptions)]
            |+> Instance [
                    "getSelectedRows" => T<unit -> int[]>
                    "setSelectedRows" => T<int[] -> unit>
                    "destroy" => T<unit -> unit>  //added
                    "init" => Grid_t.[t] ^-> T<unit>  //added
                    "setSelectedRanges" => Type.ArrayOf Range ^-> T<unit>  //added
                    "getSelectedRanges" => T<unit> ^-> Type.ArrayOf Range   //added
                    "refreshSelections" => T<unit -> unit>  //added

                ]      

    let RowsEventArgs =
        Class "Slick.RowsEventArgs"  //not found
        |+> Instance [
                "rows" =? T<int[]>
            ]

    let ScrollEventArgs =
        Class "OnScrollEventArgs"  //was Slick.ScrollEventArgs
        |> ImportFrom "models/gridEvents.interface.ts"
        |+> Instance [
                "scrollLeft" => T<int>  //changed optional to =>
                "scrollTop" => T<int>  //changed optional to =>
                "cell" => T<int>  //added
                "row" => T<int>  //added
            ]   

    let SortColumn =  //used as input to methods getSortColumns & setSortColumns 
        Pattern.Config "Slick.SortColumn" {
            Required =
                [
                    "columnId", T<string>
                    "sortAsc", T<bool>
                ]
            Optional = []
        }

    let SortEventArgs =  // input to onsort (method not found)?
        Generic - fun t ->
            Class "Slick.SortEventArgs"
            |+> Instance [
                    "multiColumnSort" =? T<bool>
                    "sortAsc" =? T<bool>
                    "sortCol" =? Column.[t]
                    "sortCols" =? Type.ArrayOf SortColumn
                ]

    let ValidationError =
        Generic - fun t ->
            Class "OnValidationErrorEventArgs"  //was Slick.ValidationError
            |> ImportFrom "models/gridEvents.interface.ts"
            |+> Instance [
                    "cell" =? T<int>
                    "cellNode" =? T<Element>
                    "column" =? Column.[t]
                    "editor" =? Editor.[t]
                    "row" =? T<int>
                    "EditorValidationResult" =? EditorValidationResult
                ]

    let VerticalRange =
        Class "Slick.VerticalRange"
        |+> Instance [
                "bottom" =? T<int>
                "top" =? T<int>
            ]

    let Options =  //not sure about all these
        Generic - fun t ->
            Pattern.Config "Slick.Options" {  //not found
                Required = []
                Optional =
                    [
                        "asyncEditorLoadDelay", T<int>
                        "asyncEditorLoading", T<bool>
                        "asyncPostRenderDelay", T<int>
                        "autoEdit", T<bool>
                        "autoHeight", T<bool>
                        "cellFlashingCssClass", T<string>
                        "cellHighlightCssClass", T<string>
                        "defaultColumnWidth", T<int>
                        "editable", T<bool>
                        "editCommandHandler", (t * Column.[t] * T<obj>) ^-> T<unit>
                        "editorFactory", IEditorFactory.[t]
                        "editorLock", EditorLock.Type
                        "enableAddRow", T<bool>
                        "enableAsyncPostRender", T<bool>
                        "enableCellNavigation", T<bool>
                        "enableCellRangeSelection", T<bool>
                        "enableColumnReorder", T<bool>
                        "enableRowReordering", T<bool>
                        "enableTextSelectionOnCells", T<bool>
                        "explicitInitialization", T<bool>
                        "forceFitColumns", T<bool>
                        "formatterFactory", IFormatterFactory.[t]
                        "headerRowHeight", T<int>
                        "leaveSpaceForNewRows", T<bool>
                        "multiColumnSort", T<bool>
                        "multiSelect", T<bool>
                        "rowHeight", T<int>
                        "showHeaderRow", T<bool>
                        "topPanelHeight", T<int>
                    ]
            }

    module Data =

        let Aggregator =
            Generic - fun t ->
                Class "Aggregator"  //was Slick.Data.Aggregator
                |> ImportFrom "models/aggregator.interface.ts"
                |+> Instance [
                        "accumulate" => t ^-> T<unit>
                        "init" => T<unit -> unit>
                        "storeResult" => T<obj -> unit>
                    ]

        let ColumnMetadata =
            Generic - fun t ->
                Pattern.Config "Slick.Data.ColumnMetadata" {
                    Required = []
                    Optional =
                        [
                            "colspan", T<string>
                            "editor", Editor.[t]
                            "focusable", T<bool>  //added
                            "formatter", Formatter t
                            "selectable", T<bool> //added
                        ]
                }

        let Metadata =
            Generic - fun t ->
                Pattern.Config "ItemMetadata" {  //was Slick.Data.Metadata                
                    Required = []
                    Optional =
                        [
                            "columns", Type.ArrayOf ColumnMetadata.[t]
                            "cssClasses", T<string>
                            "editor", Editor.[t]
                            "focusable", T<bool>
                            "formatter", Formatter t
                            "selectable", T<bool>
                        ]
                }
            |> ImportFrom "models/itemMetadata.interface.ts"

        let GroupItemMetadataProviderOptions =
            Generic - fun t ->
                Pattern.Config "GroupItemMetadataProviderOption" {  //was Slick.Data.Group..'s
                    Required = []
                    Optional =
                        [
                            "checkboxSelect", T<bool> //added
                            "checkboxSelectCssClass", T<string> //added
                            "checkboxSelectPlugin", T<obj> //added
                            "enableExpandCollapse", T<bool>
                            "groupCssClass", T<string>
                            "groupTitleCssClass", T<string>  //added
                            "indentation", T<int>  //added
                            "groupFocusable", T<bool>
                            "toggleCollapsedCssClass", T<string>
                            "toggleCssClass", T<string>
                            "toggleExpandedCssClass", T<string>
                            "totalsCssClass", T<string>
                            "totalsFocusable", T<bool>
                            "groupFormatter", Formatter t  //added
                            "totalsFormatter", Formatter t  //added
                            "includeHeaderTotals", T<bool>  //added
                        ]
                }
            |> ImportFrom "models/groupItemMetadataProviderOption.interface.ts"

        let GroupItemMetadataProvider =
            Generic - fun t ->
                Class "SlickGroupItemMetadataProvider"   //was Slick.Data.Group..
                |+> Static [Constructor (!?GroupItemMetadataProviderOptions)]
                |+> Instance [
                        "destroy" => T<unit -> unit>
                        "getGroupRowMetadata" => t ^-> Metadata.[t]
                        "getTotalsRowMetadata" => t ^-> Metadata.[t]
                        "init" => Grid_t.[t] ^-> T<unit>
                        //getOptions, setOptions ...?
                    ]
            |> ImportFrom "slick.groupitemmetadataprovider.ts"

        let PagingInfo =
            Class "PagingInfo"  //was Slick.Data.PagingInfo
            |+> Instance [
                    "pageNum" =? T<int>
                    "pageSize" =? T<int>
                    "totalPages" =? T<int>
                    "totalRows" =? T<int>
                    "dataView" =? T<obj>  //added
                ]
            |> ImportFrom "models/pagingInfo.interface.ts"

        let PagingOptions =
        //corresponds to method setPagingOptions
            Pattern.Config "SetPagingOptions" {   //was Slick.Data.P..
                Required = []
                Optional =
                    [
                        "pageNum", T<int>
                        "pageSize", T<int>
                    ]
            }
            |> ImportFrom "slick.dataview.ts"

        let RefreshHints =
            Pattern.Config "DataViewHints" {  //was Slick.Data.RefreshHints
                Required = []
                Optional =
                    [
                        "ignoreDiffsAfter", T<int>
                        "ignoreDiffsBefore", T<int>
                        "isFilterExpanding", T<bool>
                        "isFisterNarrowing", T<bool>
                        "isFilterUnchanged", T<bool>
                    ]
            }
            |> ImportFrom "models/dataViewHints.interface.ts"

        let DataViewOptions =
            Generic - fun t ->
                Pattern.Config "DataViewOption" {  //was Slick.Data.DataViewOptions
                    Required = []
                    Optional =
                        [
                            "groupItemMetadataProvider", GroupItemMetadataProvider.[t]
                            "inlineFilters", T<bool>
                            "useCSPSafeFilter", T<bool>  //added
                        ]
                }
                |> ImportFrom "slick.dataview.ts"
                
        let GroupingOptions =
            Generic - fun t ->
                Pattern.Config "Grouping" {  //was Slick.Data.GroupingOptions
                    Required = 
                        [
                            "getter", T<string>
                            "formatter", Group.[t] ^-> T<string>
                        ]
                    Optional = 
                        [
                            "aggregators", Type.ArrayOf Aggregator.[t]
                            "aggregateCollapsed", T<bool>
                            "aggregateEmpty", T<bool>
                            "aggregateChildGroups", T<bool>
                            "collapsed", T<bool>
                            "displayTotalsRow", T<bool>
                            "lazyTotalsCalculation", T<bool>
                            "comparer", Group.[t] * Group.[t] ^-> T<int>
                            "predefinedValues", Type.ArrayOf t  //added
                            "sortAsc", T<bool>  //added
                        ]
                }
                |> ImportFrom "models/grouping.interface.ts"

        let DataView =
            Generic - fun (t: CodeModel.TypeParameter) ->
                Class "SlickDataView"  //was Slick.Data.DataView
                |+> Static [Constructor !?DataViewOptions.[t]]
                |+> Instance [
                        "onPagingInfoChanged" =? Event.[PagingInfo]
                        "onRowCountChanged" =? Event.[Change.[T<int>]]
                        "onRowsChanged" =? Event.[RowsEventArgs]
                        "addItem" => t ^-> T<unit>
                        "addItems" => Type.ArrayOf t ^-> T<unit>  //added
                        "beginUpdate" => T<bool -> unit>  //was unit->unit
                        "collapseGroup" => T<string -> unit>
                        "collapseAllGroups" => T<int -> unit>  //added
                        "deleteItem" => T<string -> unit>
                        "deleteItems" => T<string[] -> unit>  //added
                        "destroy" => T<unit -> unit>  //added
                        "endUpdate" => T<unit -> unit>
                        "expandGroup" => T<string -> unit>
                        "expandAllGroups" => T<string -> unit>  //added
                        "expandCollapseGroup" => T<int * string> * !?T<bool> ^-> T<unit>  //added
                        "fastSort" => (T<string> + T<unit -> obj>) * T<bool> ^-> T<unit>
                        "getAllSelectedIds" => T<unit -> string[]>  //added
                        "getAllSelectedItems" => T<unit> ^-> Type.ArrayOf t  //added
                        "getAllSelectedFilteredIds" => T<unit -> string[]>  //added
                        "getAllSelectedFilteredItems" => T<unit -> string[]>  //added
                        "getFilterArgs" => T<unit -> obj>   //added
                        "getFilteredItems" => T<unit> ^-> Type.ArrayOf t  //added
                        "getFilteredItemCount" => T<unit -> int>  //added
                        "getFilter" => T<unit> ^-> t  //added
                        "getGrouping"  => T<unit> ^-> Type.ArrayOf GroupingOptions  //added
                        "getGroups" => T<unit> ^-> Type.ArrayOf Group.[t]
                        "getIdPropertyName" => T<unit -> string>  //added
                        "getIdxById" => T<string -> int>
                        "getItem" => T<int> ^-> t
                        "getItemById" => T<string> ^-> t
                        "getItemByIdx" => T<int> ^-> t
                        "getItemCount" => T<unit -> int>  //added
                        "getItemMetadata" => T<int> ^-> Metadata.[t]
                        "getItems" => T<unit> ^-> Type.ArrayOf t  //did not have the "T<unit> ^->"
                        "getLength" => T<unit -> int>
                        "getPagingInfo" => T<unit> ^-> PagingInfo
                        "getRowById" => T<string -> int>
                        "getRowByItem" => t ^-> T<int>  //added
                        "groupBy" => (T<string> + (t ^-> T<obj>)) * (Group.[t] ^-> T<string>) * (Group.[t] * Group.[t] ^-> T<int>) ^-> T<unit>
                        "insertItem" => T<int> * t ^-> T<unit>
                        "insertItems" => T<int> * Type.ArrayOf t ^-> T<unit>  //added
                        "mapIdsToRows" => T<string[] -> int[]>
                        "mapItemsToRows" => Type.ArrayOf t ^-> T<int[]>  //added
                        "mapRowsToIds" => T<int[] -> string[]>
                        "refresh" => T<unit -> unit>
                        "reSort" => T<unit -> unit>
                        "setAggregators" => Type.ArrayOf Aggregator.[t] * T<bool> ^-> T<unit>
                        Generic - fun t' -> "setFilter" => (t * t' ^-> T<bool>) ^-> T<unit>
                        Generic - fun t' -> "setFilterArgs" => t' ^-> T<unit>
                        "setItems" => Type.ArrayOf t * !?T<string> ^-> T<unit>
                        "setPagingOptions" => PagingOptions ^-> T<unit>
                        "setRefreshHints" => RefreshHints ^-> T<unit>
                        "setSelectedIds" => T<int[]> * !?T<bool> * !?T<bool> * !?T<bool> ^-> T<unit>  //added
                        "sort" => (t * t ^-> T<int>) * T<bool> ^-> T<unit>
                        "sortedAddItem" => t ^-> T<unit>  //added
                        "sortedUpdateItem" => T<int> * t ^-> T<unit>  //added
                        "syncGridCellCssStyles" => Grid_t.[t] * T<string> ^-> T<unit>
                        "syncGridSelection" => Grid_t.[t] * T<bool> ^-> T<unit>
                        "updateItem" => T<string> * t ^-> T<unit>
                        "updateSingleItem" => T<string> * t ^-> T<unit>  //added
                        "updateItems" => T<string[]> * Type.ArrayOf t ^-> T<unit> //added
                        "setGrouping" => GroupingOptions.[t] ^-> T<unit>
                        "setGrouping" => (Type.ArrayOf GroupingOptions.[t]) ^-> T<unit>
                    ]
            |> ImportFrom "slick.dataview.ts"

        module Aggregators =
            
            let AvgAggregator =
                Generic - fun (t: CodeModel.TypeParameter) ->
                    Class "AvgAggregator"  //was Slick.Data.Aggregators.Avg
                    |=> Inherits Aggregator.[t]
                    |+> Static [Constructor T<string>]
                    |> ImportFrom "slick.dataview.ts"

            let MaxAggregator =
                Generic - fun (t: CodeModel.TypeParameter) ->
                    Class "MaxAggregator"  //was Slick.Data.Aggregators.Max
                    |=> Inherits Aggregator.[t]
                    |+> Static [Constructor T<string>]
                    |> ImportFrom "slick.dataview.ts"

            let MinAggregator =
                Generic - fun (t: CodeModel.TypeParameter) ->
                    Class "MinAggregator"  //was Slick.Data.Aggregators.Min
                    |=> Inherits Aggregator.[t]
                    |+> Static [Constructor T<string>]
                    |> ImportFrom "slick.dataview.ts"

            let SumAggregator =
                Generic - fun (t: CodeModel.TypeParameter) ->
                    Class "SumAggregator"  //was Slick.Data.Aggregators.Sum
                    |=> Inherits Aggregator.[t]
                    |+> Static [Constructor T<string>]
                    |> ImportFrom "slick.dataview.ts"

    let Grid =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "SlickGrid"  //from slick.grid.ts;  was Slick.Grid
            |> ImportFrom "slick.grid.ts"
            |=> Grid_t
            |+> Static [Constructor ((T<Element> + T<string>)?container *
                              (Data.DataView.[t] + Type.ArrayOf t)?data *
                              (Type.ArrayOf Column.[t])?columns *
                              !?Options.[t])]
            |+> Instance [
                    "onActiveCellChanged" =? Event.[CellCoords]  
                    "onActiveCellPositionChanged" =? Event.[T<unit>]
                    "onAddNewRow" =? Event.[NewRowEventArgs.[t]]
                    "onAutosizeColumns" =? Event.[EditorEventArgs.[t]] //added
                    "onBeforeAppendCell" =? Event.[EditorEventArgs.[t]] //added
                    "onBeforeCellEditorDestroy" =? Event.[EditorEventArgs.[t]]
                    "onBeforeColumnsResize" =? Event.[EditorEventArgs.[t]] //added
                    "onBeforeDestroy" =? Event.[T<unit>] 
                    "onBeforeEditCell" =? Event.[OnBeforeEditCellEventArgs.[t]]
                    "onBeforeFooterRowCellDestroy" =? Event.[EditorEventArgs.[t]] //added
                    "onBeforeHeaderCellDestroy" =? Event.[EditorEventArgs.[t]] //added
                    "onBeforeHeaderRowCellDestroy" =? Event.[EditorEventArgs.[t]] //added
                    "nBeforeSetColumns" =? Event.[EditorEventArgs.[t]] //added
                    //"onBeforeSort" =? Event.[??] //added
                    "onBeforeUpdateColumns" =? Event.[EditorEventArgs.[t]] //added
                    "onCellChange" =? Event.[OnCellChangeEventArgs.[t]]
                    "onCellCssStylesChanged" =? Event.[CellCssEventArgs]
                    "onClick" =? Event.[CellCoords]
                    "onColumnsReordered" =? Event.[T<unit>]
                    "onColumnsDrag" =? Event.[EditorEventArgs.[t]] //added
                    "onColumnsResized" =? Event.[T<unit>]  //? is <unit> correct?
                    "onColumnsResizeDblClick" =? Event.[EditorEventArgs.[t]] //added
                    "onCompositeEditorChange" =? Event.[EditorEventArgs.[t]] //added
                    "onContextMenu" =? Event.[T<unit>]  
                    "onDblClick" =? Event.[CellCoords]
                    "onDrag" =? Event.[T<obj>]
                    "onDragEnd" =? Event.[T<obj>]  
                    "onDragInit" =? Event.[T<obj>]  
                    "onDragStart" =? Event.[T<obj>]  
                    "onFooterClick" =? Event.[EditorEventArgs.[t]]   //added; use FooterEventArgs?
                    "onFooterContextMenu" =? Event.[EditorEventArgs.[t]]
                    "onHeaderClick" =? Event.[HeaderEventArgs.[t]]
                    "onHeaderContextMenu" =? Event.[HeaderEventArgs.[t]]
                    "onHeaderCellRendered" =? Event.[HeaderEventArgs.[t]]  //added
                    "onHeaderContextMenu" =? Event.[HeaderEventArgs.[t]]
                    "onHeaderMouseEnter" =? Event.[HeaderEventArgs.[t]]  //added
                    "onHeaderMouseLeave" =? Event.[HeaderEventArgs.[t]]  //added
                    "onHeaderRowCellRendered" =? Event.[HeaderEventArgs.[t]]  //added
                    "onHeaderRowMouseEnter" =? Event.[HeaderEventArgs.[t]]  //added
                    "onHeaderRowMouseLeave" =? Event.[HeaderEventArgs.[t]]  //added
                    "onPreHeaderContextMenu" =? Event.[HeaderEventArgs.[t]]  //added
                    "onPreHeaderClick" =? Event.[HeaderEventArgs.[t]]  //added
                    "onKeyDown" =? Event.[CellCoords]
                    "onMouseEnter" =? Event.[T<unit>] 
                    "onMouseLeave" =? Event.[T<unit>]  
                    "onRendered"  =? Event.[EditorEventArgs.[t]]  //added
                    "onScroll" =? Event.[ScrollEventArgs] 
                    "onSelectedRangesChanged" =? Event.[RowsEventArgs]
                    "onSetOptions" =? Event.[EditorEventArgs.[t]]  //added
                    "onActivateChangedOptions" =? Event.[EditorEventArgs.[t]]  //added
                    "onSort" =? Event.[SortEventArgs.[t]]  
                    "onValidationError" =? Event.[ValidationError.[t]]
                    "onViewportChanged" =? Event.[T<unit>] 
                    "slickGridVersion" =? T<string>  

                    // methods from slick.grid.ts
                    "addCellCssStyles" => T<string> * T<obj> ^-> T<unit>
                    "autosizeColumn" => T<string> * T<bool> ^-> T<unit>  //added
                    "autosizeColumns" => T<string> * T<bool> ^-> T<unit> //was T<unit -> unit>
                    "canCellBeActive" => T<int> * T<int> ^-> T<bool>
                    "canCellBeSelected" => T<int> * T<int> ^-> T<bool>
                    "destroy" => T<unit -> unit>
                    "editActiveCell" => !?Editor.[t] ^-> T<unit>
                    "finishInitialization" => T<unit -> unit>
                    "flashCell" => T<int>?x * T<int>?y * !?T<int> ^-> T<unit>
                    "focus" => T<unit -> unit>
                    "getActiveCell" => T<unit> ^-> CellCoords
                    "getActiveCellNode" => T<unit -> Element>
                    "getActiveCellPosition" => T<unit> ^-> AbsBox
                    //** need to replace with sortable.js:
                    //"getCanvasNode" => T<unit -> JQuery>  //need to fix jquery
                    "getCellCssStyles" => T<string -> obj>
                    "getCellEditor" => Editor.[t]
                    "getCellFromEvent" => T<Event> ^-> CellCoords
                    "getCellFromPoint" => T<int> * T<int> ^-> CellCoords
                    "getCellNode" => T<int> * T<int> ^-> T<Element>
                    "getCellNodeBox" => T<int> * T<int> ^-> Box
                    "getColumnIndex" => T<string -> int>
                    "getColumns" => T<unit> ^-> Type.ArrayOf Column.[t]
                    "getData" => Type.ArrayOf t
                    "getDataItem" => T<int> ^-> t
                    "getDataLength" => T<unit -> int>
                    "getEditController" => T<unit> ^-> EditController
                    "getEditorLock" => T<unit> ^-> EditorLock
                    "getGridPosition" => T<unit> ^-> AbsBox
                    //"getHeaderRow" => T<unit -> JQuery>  //need to fix jquery
                    "getHeaderRowColumn" => T<int -> Element>
                    "getOptions" => T<unit> ^-> Options.[t]
                    "getRenderedRange" => !?T<int> ^-> VerticalRange
                    "getSelectedRows" => T<unit -> int[]>
                    "getSelectionModel" => T<unit> ^-> SelectionModel.[t]
                    "getSortColumns" => Type.ArrayOf SortColumn
                    //"getTopPanel" => T<unit -> JQuery>    //need to fix jquery
                    "getViewport" => !?T<int> ^-> VerticalRange
                    "gotoCell" => T<int>?x * T<int>?y * !?T<bool> ^-> T<unit>
                    "hideHeaderRowColumns" => T<unit -> unit>
                    "hideTopPanel" => T<unit -> unit>
                    "invalidate" => T<unit -> unit>
                    "invalidateAllRows" => T<unit -> unit>
                    "invalidateRow" => T<int -> unit>
                    "invalidateRows" => T<int[] -> unit>
                    "navigateDown" => T<unit -> unit>
                    "navigateLeft" => T<unit -> unit>
                    "navigateNext" => T<unit -> unit>
                    "navigatePrev" => T<unit -> unit>
                    "navigateRight" => T<unit -> unit>
                    "navigateUp" => T<unit -> unit>
                    "registerPlugin" => T<obj -> unit>
                    "removeCellCssStyles" => T<string -> unit>
                    "render" => T<unit -> unit>
                    "resetActiveCell" => T<unit -> unit>
                    "resizeCanvas" => T<unit -> unit>
                    "scrollRowIntoView" => T<int> * T<bool> ^-> T<unit>
                    "setActiveCell" => T<int> * T<int> ^-> T<unit>
                    "setCellCssStyles" => T<string> * T<obj> ^-> T<unit>
                    "setColumns" => Type.ArrayOf Column.[t] ^-> T<unit>
                    "setData" => Type.ArrayOf t * !?T<bool> ^-> T<unit>
                    "setOptions" => Options.[t] ^-> T<unit>
                    "setSelectedRows" => T<int[] -> unit>
                    "setSelectionModel" => SelectionModel.[t] ^-> T<unit>
                    "setSortColumn" => T<string> * T<bool> ^-> T<unit>
                    "setSortColumns" => Type.ArrayOf SortColumn ^-> T<unit>
                    "showHeaderRowColumns" => T<unit -> unit>
                    "showTopPanel" => T<unit -> unit>
                    "unregisterPlugin" => T<obj -> unit>
                    "updateCell" => T<int> * T<int> ^-> T<unit>
                    "updateColumnHeader" => T<string> * T<string> * T<string> * T<unit>
                    "updateRow" => T<int -> unit>
                    "updateRowCount" => T<unit -> unit>
                ]

    module Controls =

        //let columnmenu = .....to-do
        
        //let gridmenu = .......to-do

        let ColumnPickerOptions =
            Generic - fun (t: CodeModel.TypeParameter) ->
                Pattern.Config "ColumnPickerOption" {  //was Slick.Controls.Co..'s
                    Required = []
                    Optional =
                        [
                            "columnTitle", T<string>  //added
                            "forceFitTitle", T<string>  //added
                            "fadeSpeed", T<int>
                            "hideForceFitButton", T<bool>  //added
                            "hideSyncResizeButton", T<bool>  //added
                            "maxHeight", T<int>  //added
                            "minHeight", T<int>  //added
                            "syncResizeTitle", T<string>  //added
                            "headerColumnValueExtractor", Column * Options ^-> T<string>  //added
                        ]
                }
            |> ImportFrom "models/columnPicker.interface.ts"

        let ColumnPicker =
            Generic - fun (t: CodeModel.TypeParameter) ->
                Class "SlickColumnPicker"  //was Slick.Controls.ColumnPicker
                
                |+> Static [
                    Constructor (Type.ArrayOf Column.[t] * Grid_t.[t] * ColumnPickerOptions.[t])
                    Constructor (Type.ArrayOf Column.[t] * Grid_t.[t] * Options.[t])
                ]
                |+> Instance [
                        "handleHeaderContextMenu" => T<Event> * T<obj> ^-> T<unit>
                        "init" => Grid_t ^-> T<unit>   //was unit->unit
                        "updateColumn" => T<Event> ^-> T<unit>  //protected??
                        "destroy" => T<unit -> unit>
                        "setColumnVisibiliy" => T<int * bool -> unit >  //added; spelling as per typescript file!
                        "getAllColumns" => T<unit> ^-> Type.ArrayOf Column  //added
                        "getColumnbyId" => T<string> ^-> Column  //added
                        "getColumnIndexbyId" => T<string-> int>  //added
                        "getVisibleColumns" => T<unit> ^-> Type.ArrayOf Column  //added

                    ]
            |> ImportFrom "controls/slick.columnpicker.ts"

        let NavState =
            Class "Slick.Controls.NavState"  //protected getNavState from controls/slick.pager.ts??
            |+> Instance [
                    "canGotoFirst" => T<bool>
                    "canGotoLast" => T<bool>
                    "canGotoNext" => T<bool>
                    "canGotoPrev" => T<bool>
                    "pagingInfo" => Data.PagingInfo
                ]

        let Pager =
            Generic - fun (t: CodeModel.TypeParameter) ->
                Class "SlickGridPager"   //was Slick.Controls.Pager
                |> ImportFrom "controls/slick.pager.ts"
                |+> Static [Constructor (Data.DataView.[t] * Grid.[t] * T<Element>)]   //was * T<JQuery>; + other params ??
                |+> Instance [
                        //these are all 'protected' ..
                        "getNavState" => T<unit> ^-> NavState
                        "gotoFirst" => T<unit -> unit>
                        "gotoLast" => T<unit -> unit>
                        "gotoNext" => T<unit -> unit>
                        "gotoPrev" => T<unit -> unit>
                        "init" => T<unit -> unit>
                        "destroy" => T<unit -> unit>
                        "setPageSize" => T<int -> unit>
                    ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.SlickGrid.Resources.Lib" [
            //**** need to replace with sortable.js:
                //Res.JQueryEventDrag
                //Res.JQueryEventDrop
            ]
            Namespace "WebSharper.SlickGrid.Resources.Plugins" [
                Res.Plugins.Autotooltips
                Res.Plugins.Cellcopymanager
                Res.Plugins.Cellexternalcopymanager
                Res.Plugins.Cellmenu
                Res.Plugins.Cellrangedecorator
                Res.Plugins.Cellrangeselector
                Res.Plugins.Cellselectionmodel
                Res.Plugins.Checkboxselectcolumn
                Res.Plugins.Contextmenu
                Res.Plugins.Crossgridrowmovemanager
                Res.Plugins.Customtooltip
                Res.Plugins.Draggablegrouping
                Res.Plugins.HeaderMenuCss
                Res.Plugins.HeaderMenu
                Res.Plugins.HeaderButtonsCss
                Res.Plugins.HeaderButtons
                Res.Plugins.Resizer
                Res.Plugins.Rowdetailview
                Res.Plugins.Rowmovemanager
                Res.Plugins.Rowselectionmodel
                Res.Plugins.State
            ]
            Namespace "WebSharper.SlickGrid.Resources.Controls" [
                Res.Controls.Columnmenu
                Res.Controls.ColumnpickerCss
                Res.Controls.Columnpicker
                Res.Controls.Gridmenu
                Res.Controls.PagerCss
                Res.Controls.Pager
            ]
            Namespace "WebSharper.SlickGrid.Resources" [
                Res.CompositeEditor
                Res.Core
                Res.Css
                Res.DefaultTheme
                Res.Js
                Res.Dataview
                Res.Editors
                Res.Formatters
                Res.GroupItemMetadataProvider
                Res.Interactions
                Res.RemoteModel
            ]
            Namespace "WebSharper.SlickGrid.Slick" [
                Box
                AbsBox
                AutoTooltipOption
                AutoTooltips
                |> Requires [Res.Plugins.Autotooltips]
                EditorConstructor
                EditorValidationResult
                Column
                |> Requires [Res.Plugins.HeaderMenu; Res.Plugins.HeaderButtons]
                OnBeforeEditCellEventArgs
                BeforeMoveRowsArgs
                OnCellChangeEventArgs
                CellCoords
                Event
                RangesArgs
                CellCopyManager
                |> Requires [Res.Plugins.Cellcopymanager]
                CellCssEventArgs
                Range
                CellRangeDecoratorOption
                CellRangeDecorator
                |> Requires [Res.Plugins.Cellrangedecorator]
                CellRangeSelectorOption
                CellRangeSelector
                |> Requires [Res.Plugins.Cellrangeselector]
                SelectionModel
                CellSelectionModelOption
                CellSelectionModel
                |> Requires [Res.Plugins.Cellselectionmodel]
                Change
                CheckboxSelectorOption
                CheckboxSelectColumn
                |> Requires [Res.Plugins.Checkboxselectcolumn]
                EditController
                Position
                EditorArgs
                Editor
                EditorEventArgs
                EditorLock
                Editors
                |> Requires [Res.Editors]
                Formatters
                |> Requires [Res.Formatters]
                FromToRangesArgs
                NonDataItem
                GroupTotals
                Group
                HeaderEventArgs
                IEditorFactory
                IFormatterFactory
                NewRowEventArgs
                RowMoveManager
                |> Requires [Res.Plugins.Rowmovemanager]
                RowSelectionModelOptions
                RowSelectionModel
                |> Requires [Res.Plugins.Rowselectionmodel]
                RowsEventArgs
                ScrollEventArgs
                SortColumn
                SortEventArgs
                Total
                ValidationError
                VerticalRange
                Options
                Grid
            ]
            Namespace "WebSharper.SlickGrid.Slick.Data" [
                Data.Aggregator
                Data.ColumnMetadata
                Data.Metadata
                Data.GroupItemMetadataProviderOptions
                Data.GroupItemMetadataProvider
                |> Requires [Res.GroupItemMetadataProvider]
                Data.PagingInfo
                Data.PagingOptions
                Data.RefreshHints
                Data.DataViewOptions
                Data.GroupingOptions
                Data.DataView
                |> Requires [Res.Dataview]
            ]
            Namespace "WebSharper.SlickGrid.Slick.Data.Aggregators" [
                Data.Aggregators.AvgAggregator
                Data.Aggregators.MaxAggregator
                Data.Aggregators.MinAggregator
                Data.Aggregators.SumAggregator
            ]
            Namespace "WebSharper.SlickGrid.Slick.Controls" [
                
                Controls.ColumnPickerOptions
                Controls.ColumnPicker
                |> Requires [Res.Controls.Columnpicker]
                Controls.NavState
                Controls.Pager
                |> Requires [Res.Controls.Pager]
                //Controls.Columnmenu
                //Controls.Gridmenu
            ]
        ]


[<Sealed>]
type SlickExtension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<SlickExtension>)>]
do ()
