namespace WebSharper.SlickGrid

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =
    
    // --- Basic Placeholders ---
    let Column = T<obj>
    let GridOption = T<obj>
    let DataView = T<obj>
    let SelectionModel = T<obj>
    let Editor = T<obj>
    let EditorConstructor = T<obj> 
    let Formatter = T<obj>
    let ItemMetadata = T<obj>
    let SlickPlugin = T<obj>
    let SlickEditorLock = T<obj>
    let EditController = T<obj>
    let PagingInfo = T<obj>
    let SlickRange = T<obj>
    let BasePubSub = T<obj>
    let CellPosition = T<obj>      
    
    // --- Event Argument Placeholders (To be implemented later) ---
    let OnActiveCellChangedEventArgs = T<obj>
    let OnActiveCellPositionChangedEventArgs = T<obj>
    let OnAddNewRowEventArgs = T<obj>
    let OnActivateChangedOptionsEventArgs = T<obj>
    let OnAfterSetColumnsEventArgs = T<obj>
    let OnAutosizeColumnsEventArgs = T<obj>
    let OnBeforeAppendCellEventArgs = T<obj>
    let OnBeforeCellEditorDestroyEventArgs = T<obj>
    let OnBeforeColumnsResizeEventArgs = T<obj>
    let OnBeforeDestroyEventArgs = T<obj>
    let OnBeforeEditCellEventArgs = T<obj>
    let OnBeforeFooterRowCellDestroyEventArgs = T<obj>
    let OnBeforeHeaderCellDestroyEventArgs = T<obj>
    let OnBeforeHeaderRowCellDestroyEventArgs = T<obj>
    let OnBeforeRemoveCachedRowEventArgs = T<obj>
    let OnBeforeSetColumnsEventArgs = T<obj>
    let OnBeforeSortEventArgs = T<obj>
    let OnBeforeUpdateColumnsEventArgs = T<obj>
    let OnCellChangeEventArgs = T<obj>
    let OnCellCssStylesChangedEventArgs = T<obj>
    let OnClickEventArgs = T<obj>
    let OnColumnsReorderedEventArgs = T<obj>
    let OnColumnsDragEventArgs = T<obj>
    let OnColumnsResizedEventArgs = T<obj>
    let OnColumnsResizeDblClickEventArgs = T<obj>
    let OnCompositeEditorChangeEventArgs = T<obj>
    let MenuCommandItemCallbackArgs = T<obj>
    let DragRowMove = T<obj>
    let OnDblClickEventArgs = T<obj>
    let OnDragReplaceCellsEventArgs = T<obj>
    let OnFooterClickEventArgs = T<obj>
    let OnFooterContextMenuEventArgs = T<obj>
    let OnFooterRowCellRenderedEventArgs = T<obj>
    let OnHeaderCellRenderedEventArgs = T<obj>
    let OnHeaderClickEventArgs = T<obj>
    let OnHeaderContextMenuEventArgs = T<obj>
    let OnHeaderMouseEventArgs = T<obj>
    let OnHeaderRowCellRenderedEventArgs = T<obj>
    let OnPreHeaderContextMenuEventArgs = T<obj>
    let OnPreHeaderClickEventArgs = T<obj>
    let OnKeyDownEventArgs = T<obj>
    let OnRenderedEventArgs = T<obj>
    let OnScrollEventArgs = T<obj>
    let OnSelectedRowsChangedEventArgs = T<obj>
    let OnSetOptionsEventArgs = T<obj>
    let OnSortEventArgs = T<obj>
    let OnValidationErrorEventArgs = T<obj>
    let OnViewportChangedEventArgs = T<obj>

    // --- DOM Types ---
    let HTMLElement = T<WebSharper.JavaScript.Dom.Element>
    let HTMLDivElement = T<WebSharper.JavaScript.Dom.Element>
    let DOMEvent = T<WebSharper.JavaScript.Dom.Event>

    let SlickEvent = 
        Generic - fun tArgs ->
            Class "Slick.Event" // Updated to correct JS name usually Slick.Event
            |+> Instance [
                "subscribe" => (DOMEvent * tArgs ^-> T<unit>)?handler ^-> T<unit>
                "unsubscribe" => (DOMEvent * tArgs ^-> T<unit>)?handler ^-> T<unit>
                "notify" => tArgs?args * !? T<obj>?eventScope * !? T<obj>?scope ^-> T<unit>
            ]

    
    let Grid =
        Generic - fun tData ->
            Class "Slick.Grid" // Updated to correct JS name usually Slick.Grid
            |+> Static [
                
                Constructor (
                    (T<string> + HTMLElement)?container * (DataView + !| tData)?data * (!| Column)?columns * !? GridOption?options *
                    !? BasePubSub?externalPubSub
                )
            ]
            |+> Instance [
                
                "init" => T<unit> ^-> T<unit>
                "destroy" => !?T<bool>?shouldDestroyAllElements ^-> T<unit>
                "getUID" => T<unit> ^-> T<string>
                "slickGridVersion" =? T<string>
                "cid" =@ T<string>
                
                "getData" => T<unit> ^-> (DataView + !| tData)
                "getDataLength" => T<unit> ^-> T<int>
                "getDataItem" => T<int>?i ^-> tData
                "setData" => (DataView + !| tData)?newData * !?T<bool>?scrollToTop ^-> T<unit>
                "hasDataView" => T<unit> ^-> T<bool>
                "getItemMetadaWhenExists" => T<int>?row ^-> ItemMetadata

                
                "getOptions" => T<unit> ^-> GridOption
                "setOptions" => GridOption?newOptions * !?T<bool>?suppressRender * !?T<bool>?suppressColumnSet * !?T<bool>?suppressSetOverflow ^-> T<unit>
                "activateChangedOptions" => !?T<bool>?suppressRender * !?T<bool>?suppressColumnSet * !?T<bool>?suppressSetOverflow ^-> T<unit>
                "validateAndEnforceOptions" => T<unit> ^-> T<unit>

                
                "getColumns" => T<unit> ^-> !| Column
                "setColumns" => (!| Column)?columnDefinitions ^-> T<unit>
                "getVisibleColumns" => T<unit> ^-> !| Column
                "getColumnIndex" => (T<string> + T<int>)?id ^-> T<int>
                "updateColumnHeader" => (T<string> + T<int>)?columnId * !?(T<string> + HTMLElement)?title * !?T<string>?toolTip ^-> T<unit>
                "autosizeColumn" => (T<string> + T<int>)?columnOrIndexOrId * !?T<bool>?isInit ^-> T<unit>
                "autosizeColumns" => !?T<string>?autosizeMode * !?T<bool>?isInit ^-> T<unit>
                "reRenderColumns" => !?T<bool>?reRender ^-> T<unit>
                "updateColumns" => T<unit> ^-> T<unit>
                "setSortColumn" => (T<string> + T<int>)?columnId * T<bool>?ascending ^-> T<unit>
                "setSortColumns" => (!| T<obj>)?cols ^-> T<unit> 
                "getSortColumns" => T<unit> ^-> !| T<obj>
                "getColumnByIndex" => T<int>?id ^-> HTMLElement
                "getAbsoluteColumnMinWidth" => T<unit> ^-> T<int>
                "getHeaderColumnWidthDiff" => T<unit> ^-> T<int>

                
                "getHeader" => Column?columnDef ^-> (HTMLDivElement + !| HTMLDivElement)
                "getHeaderColumn" => (T<string> + T<int>)?columnIdOrIdx ^-> HTMLDivElement
                "getHeaderRow" => T<unit> ^-> (HTMLDivElement + !| HTMLDivElement)
                "getHeaderRowColumn" => (T<string> + T<int>)?columnIdOrIdx ^-> HTMLDivElement
                "getFooterRow" => T<unit> ^-> (HTMLDivElement + !| HTMLDivElement)
                "getFooterRowColumn" => (T<string> + T<int>)?columnIdOrIdx ^-> HTMLDivElement
                "getTopPanel" => T<unit> ^-> HTMLDivElement
                "getTopPanels" => T<unit> ^-> !| HTMLDivElement
                "getPreHeaderPanel" => T<unit> ^-> HTMLDivElement
                "getPreHeaderPanelLeft" => T<unit> ^-> HTMLDivElement
                "getPreHeaderPanelRight" => T<unit> ^-> HTMLDivElement
                "getTopHeaderPanel" => T<unit> ^-> HTMLDivElement
                "getContainerNode" => T<unit> ^-> HTMLElement

                
                "setTopPanelVisibility" => !?T<bool>?visible * !?T<bool>?animate ^-> T<unit>
                "setHeaderRowVisibility" => !?T<bool>?visible * !?T<bool>?animate ^-> T<unit>
                "setColumnHeaderVisibility" => !?T<bool>?visible * !?T<bool>?animate ^-> T<unit>
                "setFooterRowVisibility" => !?T<bool>?visible * !?T<bool>?animate ^-> T<unit>
                "setPreHeaderPanelVisibility" => !?T<bool>?visible * !?T<bool>?animate ^-> T<unit>
                "setTopHeaderPanelVisibility" => !?T<bool>?visible ^-> T<unit>

                
                "getSelectionModel" => T<unit> ^-> SelectionModel
                "setSelectionModel" => SelectionModel?model ^-> T<unit>
                "getSelectedRows" => T<unit> ^-> !| T<int>
                "setSelectedRows" => (!| T<int>)?rows * !?T<string>?caller ^-> T<unit>
                
                
                "getActiveCell" => T<unit> ^-> T<obj> 
                "setActiveCell" => T<int>?row * T<int>?cell * !?T<bool>?opt_editMode * !?T<bool>?preClickModeOn * !?T<bool>?suppressActiveCellChangedEvent ^-> T<unit>
                "resetActiveCell" => T<unit> ^-> T<unit>
                "unsetActiveCell" => T<unit> ^-> T<unit>
                "getActiveCellNode" => T<unit> ^-> HTMLDivElement
                "getActiveCellPosition" => T<unit> ^-> T<obj> 
                "canCellBeActive" => T<int>?row * T<int>?cell ^-> T<bool>
                "canCellBeSelected" => T<int>?row * T<int>?cell ^-> T<bool>
                "focus" => T<unit> ^-> T<unit>
                "gotoCell" => T<int>?row * T<int>?cell * !?T<bool>?forceEdit * !?DOMEvent?e ^-> T<unit>
                "setActiveRow" => T<int>?row * !?T<int>?cell * !?T<bool>?suppressScrollIntoView ^-> T<unit>

                
                "navigatePageDown" => T<unit> ^-> T<unit>
                "navigatePageUp" => T<unit> ^-> T<unit>
                "navigateTop" => T<unit> ^-> T<unit>
                "navigateBottom" => T<unit> ^-> T<unit>
                "navigateToRow" => T<int>?row ^-> T<bool>
                "navigateRight" => T<unit> ^-> T<bool>
                "navigateLeft" => T<unit> ^-> T<bool>
                "navigateDown" => T<unit> ^-> T<bool>
                "navigateUp" => T<unit> ^-> T<bool>
                "navigateNext" => T<unit> ^-> T<bool>
                "navigatePrev" => T<unit> ^-> T<bool>
                "navigateRowStart" => T<unit> ^-> T<bool>
                "navigateRowEnd" => T<unit> ^-> T<bool>
                "navigateTopStart" => T<unit> ^-> T<bool>
                "navigateBottomEnd" => T<unit> ^-> T<bool>

                
                "editActiveCell" => !?EditorConstructor?editor * !?T<bool>?preClickModeOn * !?DOMEvent?e ^-> T<unit>
                "getCellEditor" => T<unit> ^-> Editor
                "getEditorLock" => T<unit> ^-> SlickEditorLock
                "getEditController" => T<unit> ^-> EditController
                "commitCurrentEdit" => T<unit> ^-> T<bool>
                "cancelCurrentEdit" => T<unit> ^-> T<bool>

                
                "render" => T<unit> ^-> T<unit>
                "invalidate" => T<unit> ^-> T<unit>
                "invalidateRow" => T<int>?row ^-> T<unit>
                "invalidateRows" => (!| T<int>)?rows ^-> T<unit>
                "invalidateAllRows" => T<unit> ^-> T<unit>
                "resizeCanvas" => T<unit> ^-> T<unit>
                "updateRowCount" => T<unit> ^-> T<unit>
                "updateRow" => T<int>?row ^-> T<unit>
                "updateCell" => T<int>?row * T<int>?cell ^-> T<unit>
                
                "getViewport" => !?T<int>?viewportTop * !?T<int>?viewportLeft ^-> T<obj> 
                "getVisibleRange" => !?T<int>?viewportTop * !?T<int>?viewportLeft ^-> T<obj>
                "getRenderedRange" => !?T<int>?viewportTop * !?T<int>?viewportLeft ^-> T<obj>
                "getViewportRowCount" => T<unit> ^-> T<int>
                "getViewportHeight" => T<unit> ^-> T<int>
                "getViewportWidth" => T<unit> ^-> T<int>
                "getCanvasNode" => !?(T<string> + T<int>)?columnIdOrIdx * !?T<int>?rowIndex ^-> HTMLDivElement
                "getActiveCanvasNode" => !?DOMEvent?e ^-> HTMLDivElement
                "getCanvases" => T<unit> ^-> !| HTMLDivElement
                "getViewportNode" => !?(T<string> + T<int>)?columnIdOrIdx * !?T<int>?rowIndex ^-> HTMLElement
                "getViewports" => T<unit> ^-> !| HTMLDivElement
                "getActiveViewportNode" => !?DOMEvent?e ^-> HTMLDivElement
                "setActiveViewportNode" => !?DOMEvent?e ^-> HTMLDivElement
                "getCanvasWidth" => T<unit> ^-> T<int>
                "getHeadersWidth" => T<unit> ^-> T<int>
                "getScrollbarDimensions" => T<unit> ^-> T<obj> 
                "getDisplayedScrollbarDimensions" => T<unit> ^-> T<obj>
                "getGridPosition" => T<unit> ^-> T<obj> 

                
                "scrollTo" => T<int>?y ^-> T<unit>
                "scrollToX" => T<int>?x ^-> T<unit>
                "scrollRowIntoView" => T<int>?row * !?T<bool>?doPaging ^-> T<unit>
                "scrollRowToTop" => T<int>?row ^-> T<unit>
                "scrollCellIntoView" => T<int>?row * T<int>?cell * !?T<bool>?doPaging ^-> T<unit>
                "scrollColumnIntoView" => T<int>?cell ^-> T<unit>
                "getFrozenRowOffset" => T<int>?row ^-> T<int>
                "getFrozenColumnId" => T<unit> ^-> (T<string> + T<int>)

                
                "addCellCssStyles" => T<string>?key * T<obj>?hash ^-> T<unit>
                "removeCellCssStyles" => T<string>?key ^-> T<unit>
                "setCellCssStyles" => T<string>?key * T<obj>?hash ^-> T<unit>
                "getCellCssStyles" => T<string>?key ^-> T<obj>
                "flashCell" => T<int>?row * T<int>?cell * !?T<int>?speed ^-> T<unit>
                "highlightRow" => T<int>?row * !?T<int>?duration ^-> T<unit>
                "sanitizeHtmlString" => T<string>?dirtyHtml * !?T<bool>?suppressLogging ^-> T<string>
                "applyFormatResultToCellNode" => (T<obj> + T<string> + HTMLElement)?formatterResult * HTMLDivElement?cellNode * !?T<bool>?suppressRemove ^-> T<unit>

                
                "registerPlugin" => SlickPlugin?plugin ^-> T<unit>
                "unregisterPlugin" => SlickPlugin?plugin ^-> T<unit>
                "getPluginByName" => T<string>?name ^-> SlickPlugin
                "getPubSubService" => T<unit> ^-> BasePubSub
                
                
                "getCellFromEvent" => DOMEvent?evt ^-> T<obj> 
                "getCellFromPoint" => T<int>?x * T<int>?y ^-> T<obj> 
                "getCellNode" => T<int>?row * T<int>?cell ^-> HTMLDivElement
                "getCellNodeBox" => T<int>?row * T<int>?cell ^-> T<obj> 
                "getCellHeight" => T<int>?row * T<int>?rowspan ^-> T<int>
                "arrayEquals" => (!| T<obj>)?arr1 * (!| T<obj>)?arr2 ^-> T<bool>
                "updatePagingStatusFromView" => PagingInfo?pagingInfo ^-> T<unit>
                
                
                "getRowSpanColumnIntersects" => T<int>?row ^-> !| T<int>
                "getRowSpanIntersect" => T<int>?row ^-> T<int> 
                "getParentRowSpanByCell" => T<int>?row * T<int>?cell * !?T<bool>?excludeParentRow ^-> T<obj>
                "remapAllColumnsRowSpan" => T<unit> ^-> T<unit>
                "findSpanStartingCell" => T<int>?row * T<int>?cell ^-> T<obj> 

                
                "validateColumnFreezeWidth" => !?T<int>?frozenColumn * !?T<bool>?forceAlert ^-> T<bool>
                "validateSetColumnFreeze" => (!| Column)?newColumns * !?T<bool>?forceAlert ^-> T<bool>
                "calculateFrozenColumnIndexById" => (!| Column)?newColumns * !?(T<string> + T<int>)?columnId * !?T<bool>?applyIndexChange ^-> T<int>
                
                // --- Events with Strict Types ---
                "onActiveCellChanged" =? SlickEvent.[OnActiveCellChangedEventArgs]
                "onActiveCellPositionChanged" =? SlickEvent.[OnActiveCellPositionChangedEventArgs]
                "onAddNewRow" =? SlickEvent.[OnAddNewRowEventArgs]
                "onActivateChangedOptions" =? SlickEvent.[OnActivateChangedOptionsEventArgs]
                "onAfterSetColumns" =? SlickEvent.[OnAfterSetColumnsEventArgs]
                "onAutosizeColumns" =? SlickEvent.[OnAutosizeColumnsEventArgs]
                "onBeforeAppendCell" =? SlickEvent.[OnBeforeAppendCellEventArgs]
                "onBeforeCellEditorDestroy" =? SlickEvent.[OnBeforeCellEditorDestroyEventArgs]
                "onBeforeColumnsResize" =? SlickEvent.[OnBeforeColumnsResizeEventArgs]
                "onBeforeDestroy" =? SlickEvent.[OnBeforeDestroyEventArgs]
                "onBeforeEditCell" =? SlickEvent.[OnBeforeEditCellEventArgs]
                "onBeforeFooterRowCellDestroy" =? SlickEvent.[OnBeforeFooterRowCellDestroyEventArgs]
                "onBeforeHeaderCellDestroy" =? SlickEvent.[OnBeforeHeaderCellDestroyEventArgs]
                "onBeforeHeaderRowCellDestroy" =? SlickEvent.[OnBeforeHeaderRowCellDestroyEventArgs]
                "onBeforeRemoveCachedRow" =? SlickEvent.[OnBeforeRemoveCachedRowEventArgs]
                "onBeforeSetColumns" =? SlickEvent.[OnBeforeSetColumnsEventArgs]
                "onBeforeSort" =? SlickEvent.[OnBeforeSortEventArgs]
                "onBeforeUpdateColumns" =? SlickEvent.[OnBeforeUpdateColumnsEventArgs]
                "onCellChange" =? SlickEvent.[OnCellChangeEventArgs]
                "onCellCssStylesChanged" =? SlickEvent.[OnCellCssStylesChangedEventArgs]
                "onClick" =? SlickEvent.[OnClickEventArgs]
                "onColumnsReordered" =? SlickEvent.[OnColumnsReorderedEventArgs]
                "onColumnsDrag" =? SlickEvent.[OnColumnsDragEventArgs]
                "onColumnsResized" =? SlickEvent.[OnColumnsResizedEventArgs]
                "onColumnsResizeDblClick" =? SlickEvent.[OnColumnsResizeDblClickEventArgs]
                "onCompositeEditorChange" =? SlickEvent.[OnCompositeEditorChangeEventArgs]
                "onContextMenu" =? SlickEvent.[MenuCommandItemCallbackArgs]
                "onDrag" =? SlickEvent.[DragRowMove]
                "onDblClick" =? SlickEvent.[OnDblClickEventArgs]
                "onDragInit" =? SlickEvent.[DragRowMove]
                "onDragStart" =? SlickEvent.[DragRowMove]
                "onDragEnd" =? SlickEvent.[DragRowMove]
                "onDragReplaceCells" =? SlickEvent.[OnDragReplaceCellsEventArgs]
                "onFooterClick" =? SlickEvent.[OnFooterClickEventArgs]
                "onFooterContextMenu" =? SlickEvent.[OnFooterContextMenuEventArgs]
                "onFooterRowCellRendered" =? SlickEvent.[OnFooterRowCellRenderedEventArgs]
                "onHeaderCellRendered" =? SlickEvent.[OnHeaderCellRenderedEventArgs]
                "onHeaderClick" =? SlickEvent.[OnHeaderClickEventArgs]
                "onHeaderContextMenu" =? SlickEvent.[OnHeaderContextMenuEventArgs]
                "onHeaderMouseEnter" =? SlickEvent.[OnHeaderMouseEventArgs]
                "onHeaderMouseLeave" =? SlickEvent.[OnHeaderMouseEventArgs]
                "onHeaderRowCellRendered" =? SlickEvent.[OnHeaderRowCellRenderedEventArgs]
                "onHeaderRowMouseEnter" =? SlickEvent.[OnHeaderMouseEventArgs]
                "onHeaderRowMouseLeave" =? SlickEvent.[OnHeaderMouseEventArgs]
                "onPreHeaderContextMenu" =? SlickEvent.[OnPreHeaderContextMenuEventArgs]
                "onPreHeaderClick" =? SlickEvent.[OnPreHeaderClickEventArgs]
                "onKeyDown" =? SlickEvent.[OnKeyDownEventArgs]
                "onMouseEnter" =? SlickEvent.[OnHeaderMouseEventArgs]
                "onMouseLeave" =? SlickEvent.[OnHeaderMouseEventArgs]
                "onRendered" =? SlickEvent.[OnRenderedEventArgs]
                "onScroll" =? SlickEvent.[OnScrollEventArgs]
                "onSelectedRowsChanged" =? SlickEvent.[OnSelectedRowsChangedEventArgs]
                "onSetOptions" =? SlickEvent.[OnSetOptionsEventArgs]
                "onSort" =? SlickEvent.[OnSortEventArgs]
                "onValidationError" =? SlickEvent.[OnValidationErrorEventArgs]
                "onViewportChanged" =? SlickEvent.[OnViewportChangedEventArgs]
            ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.SlickGrid" [
                SlickEvent
                Grid
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()