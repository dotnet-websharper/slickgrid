namespace WebSharper.SlickGrid

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

// SlickGrid for Version 5.18.0
module Definition =

    let importFromSlickGrid (name: string) (c: CodeModel.Class) = 
        c
        |> Import name "slickgrid"

    // --- DOM Types ---
    let HTMLElement = T<Dom.Element>
    let HTMLDivElement = T<Dom.Element>
    let DocumentFragment = T<Dom.DocumentFragment>
    let DOMEvent = T<Dom.Event>

    let BasePubSub =
        let name = "BasePubSub"

        Pattern.Config name {
            Required = [
                "publish", (T<string> + T<obj>)?eventName * !?T<obj>?data ^-> T<obj>
                "subscribe", (T<string> + T<obj>)?eventName * (T<obj> ^-> T<unit>)?callback ^-> T<obj>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let DataViewHints =
        let name = "DataViewHints"

        Pattern.Config name {
            Required = []
            Optional = [
                "isFilterNarrowing", T<bool>
                "isFilterExpanding", T<bool>
                "isFilterUnchanged", T<bool>
                "ignoreDiffsBefore", T<int>
                "ignoreDiffsAfter", T<int>
            ]
        }
        |> importFromSlickGrid name

    let ItemMetadataProvider =
        let name = "ItemMetadataProvider"

        Pattern.Config name {
            Required = [
                "getRowMetadata", T<obj>?item * !?T<int>?row ^-> T<obj>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let FormatterResultsArgs = [
            "addClasses", T<string>
            "removeClasses", T<string>
            "toolTip", T<string>
            "insertElementAfterTarget", HTMLElement
        ]

    let FormatterResultObject =
        let name = "FormatterResultObject"

        Pattern.Config name {
            Required = []
            Optional = FormatterResultsArgs
        }
        |> importFromSlickGrid name

    let FormatterResultWithText =
        let name = "FormatterResultWithText"

        Pattern.Config name {
            Required = [
                "text", T<string>
            ]
            Optional = FormatterResultsArgs
        }
        |> importFromSlickGrid name

    let FormatterResultWithHtml =
        let name = "FormatterResultWithHtml"

        Pattern.Config name {
            Required = [
                "html", HTMLElement
            ]
            Optional = FormatterResultsArgs
        }
        |> importFromSlickGrid name

    // Test
    let Formatter (t: Type.IType) colType =
        let ReturnType = 
            T<string> + HTMLElement + DocumentFragment + FormatterResultWithHtml + FormatterResultWithText

        (T<int>?row * T<int>?cell * T<obj>?value * colType?columnDef * t.Type?dataContext * T<obj>?grid) ^-> ReturnType

    let CellMenuOption =
        let name = "CellMenuOption"

        Class name
        |> importFromSlickGrid name
        |+> Pattern.RequiredFields []

    let Column = 
        Generic - fun t ->
            let name = "Column"

            Class name
            |> importFromSlickGrid name
            |+> Pattern.OptionalFields [
                "asyncPostRender", HTMLElement * T<int> * t * TSelf.[T<obj>] * !?T<bool> ^-> T<unit>
                "formatter", Formatter t TSelf.[t]
                "cellMenu", CellMenuOption.Type
                "asyncPostRenderCleanup", HTMLElement * T<int> * TSelf.[T<obj>] ^-> T<unit>
            ]

    let GroupItemMetadataProviderOption =
        let name = "GroupItemMetadataProviderOption"

        Pattern.Config name {
            Required = []
            Optional = [
                "checkboxSelect", T<bool>
                "checkboxSelectCssClass", T<string>
                "checkboxSelectPlugin", T<obj>
                "groupCssClass", T<string>
                "groupTitleCssClass", T<string>
                "indentation", T<int>
                "totalsCssClass", T<string>
                "groupFocusable", T<bool>
                "totalsFocusable", T<bool>
                "toggleCssClass", T<string>
                "toggleExpandedCssClass", T<string>
                "toggleCollapsedCssClass", T<string>
                "enableExpandCollapse", T<bool>
                "groupFormatter", Formatter T<obj> Column.[T<obj>]
                "totalsFormatter", Formatter T<obj> Column.[T<obj>]
                "includeHeaderTotals", T<bool>
            ]
        }
        |> importFromSlickGrid name

    let SlickGroupItemMetadataProvider =
        let name = "SlickGroupItemMetadataProvider"
        
        Class name
        |> importFromSlickGrid name
        |+> Static [
            Constructor (!?GroupItemMetadataProviderOption?inputOptions)
        ]

    let DataViewOption =
        let name = "DataViewOption"

        Pattern.Config name {
            Required = [
                "inlineFilters", T<bool>
                "useCSPSafeFilter", T<bool>
            ]
            Optional = [
                "globalItemMetadataProvider", ItemMetadataProvider.Type
                "groupItemMetadataProvider", SlickGroupItemMetadataProvider.Type
            ]
        }
        |> importFromSlickGrid name

    let SlickDataView =
        Generic - fun tData ->
            let name = "SlickDataView"

            Class name
            |> importFromSlickGrid name
            |+> Static [
                Constructor (!?DataViewOption?options * !?BasePubSub?externalPubSub)
            ]
            |+> Instance [
                "getItems" => T<unit> ^-> !| tData
                "setItems" => (!| tData)?data * !?T<string>?objectIdProperty ^-> T<unit>
                "sort" => (tData * tData ^-> T<int>)?comparer * !?T<bool>?ascending ^-> T<unit>
                "getFilteredItems" => T<unit> ^-> !| tData
                "getFilter" => T<unit> ^-> (tData * T<obj> ^-> T<bool>)
                "setFilter" => (tData * T<obj> ^-> T<bool>)?filterFn ^-> T<unit>
                "getItemByIdx" => T<int>?i ^-> tData
                "getRowByItem" => tData?item ^-> T<int>
                "getItemById" => (T<string> + T<int>)?id ^-> tData
                "mapItemsToRows" => (!| tData)?itemArray ^-> !| T<int>
                "updateSingleItem" => (T<string> + T<int>)?id * tData?item ^-> T<unit>
                "updateItem" => (T<string> + T<int>)?id * tData?item ^-> T<unit>
                "updateItems" => (!| (T<string> + T<int>))?ids * (!| tData)?newItems ^-> T<unit>
                "insertItem" => T<int>?insertBefore * tData?item ^-> T<unit>
                "insertItems" => T<int>?insertBefore * (!| tData)?newItems ^-> T<unit>
                "addItem" => tData?item ^-> T<unit>
                "addItems" => (!| tData)?newItems ^-> T<unit>
                "deleteItem" => (T<string> + T<int>)?id ^-> T<unit>
                "deleteItems" => (!| (T<string> + T<int>))?ids ^-> T<unit>
    
                "sortedAddItem" => tData?item ^-> T<unit>
                "sortedUpdateItem" => (T<string> + T<int>)?id * tData?item ^-> T<unit>
    
                "getItemCount" => T<unit> ^-> T<int>
                "getLength" => T<unit> ^-> T<int>
                "getItem" => T<int>?i ^-> tData
                "getAllSelectedItems" => T<unit> ^-> !| tData
                "getAllSelectedFilteredItems" => T<unit> ^-> !| tData
            ]

    let CellViewportRange =
        let name = "CellViewportRange"

        Pattern.Config name {
            Required = [
                "bottom", T<int>
                "top", T<int>
                "leftPx", T<int>
                "rightPx", T<int>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let CssStyleHash = T<obj> 

    let ColumnPickerOption =
        let name = "ColumnPickerOption"

        Class name
        |> importFromSlickGrid name
        |+> Pattern.RequiredFields []

    let OnColumnsChangedArgs =
        let name = "OnColumnsChangedArgs"

        Class name 
        |> importFromSlickGrid name

    let ContextMenuOption =
        let name = "ContextMenuOption"

        Class name 
        |> importFromSlickGrid name
        |+> Pattern.RequiredFields []            

    let ElementPosition =
        let name = "ElementPosition"

        Pattern.Config name {
            Required = [
                "top", T<int>
                "left", T<int>
                "bottom", T<int>
                "right", T<int>
                "height", T<int>
                "width", T<int>
                "visible", T<bool>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let PositionMethod = T<unit> ^-> ElementPosition

    let EditorArguments =
        let name = "EditorArguments"

        Class name
        |> importFromSlickGrid name
        |+> Pattern.RequiredFields [
            "column", Column.[T<obj>]
            "container", HTMLDivElement
            "event", DOMEvent
            "gridPosition", ElementPosition.Type
            "item", T<obj>
            "position", PositionMethod + ElementPosition.Type
                    
            "cancelChanges", T<unit> ^-> T<unit>
            "commitChanges", T<unit> ^-> T<unit>
        ]
        |+> Pattern.OptionalFields [
            "columnMetaData", T<obj>
            "compositeEditorOptions", T<obj>
            "dataView", SlickDataView.[T<obj>]
        ]

    let EditorValidationResult =
        let name = "EditorValidationResult"

        Pattern.Config name {
            Required = [
                "valid", T<bool>
            ]
            Optional = [
                "msg", T<string>
                "errors", !| T<obj>
            ]
        }
        |> importFromSlickGrid name
        
    let Editor =
        let name = "Editor"

        Class name
        |> importFromSlickGrid name
        |+> Static [
            Constructor (!?EditorArguments)
        ]
        |+> Pattern.RequiredFields [
            "init", !?EditorArguments ^-> T<unit>
            "destroy", T<unit> ^-> T<unit>
            "focus", T<unit> ^-> T<unit>
            "applyValue", T<obj>?item * T<obj>?state ^-> T<unit>
            "loadValue", T<obj>?item ^-> T<unit>
            "serializeValue", T<unit> ^-> T<obj>
            "isValueChanged", T<unit> ^-> T<bool>
            "validate", !?HTMLElement?targetElm * !?T<obj>?options ^-> EditorValidationResult.Type
        ]
        |+> Pattern.OptionalFields [
            "suppressClearOnEdit", T<bool>
            "dataContext", T<obj>
            "disabled", T<bool>
            "keyCaptureList", !| T<int>
                
            "save", T<unit> ^-> T<unit>
            "cancel", T<unit> ^-> T<unit>
            "hide", T<unit> ^-> T<unit>
            "show", T<unit> ^-> T<unit>
            "position", T<obj>?position ^-> T<unit>
            "preClick", T<unit> ^-> T<unit>
                
            "setValue", T<obj>?value * !?T<bool>?isApplyingValue * !?T<bool>?triggerOnCompositeEditorChange ^-> T<unit>
        ]

    let EditCommand =
        let name = "EditCommand"

        Pattern.Config name {
            Required = [
                "row", T<int>
                "cell", T<int>
                "editor", Editor + T<obj>
                "serializedValue", T<obj>
                "prevSerializedValue", T<obj>
                "execute", T<unit> ^-> T<unit>
                "undo", T<unit> ^-> T<unit>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let FormatterFactory =
        Generic - fun t ->
            let name = "FormatterFactory"

            Pattern.Config name { 
                Required = [
                    "getFormatter", Column.[t] ^-> Formatter t Column.[t]
                ]
                Optional = [] 
            }
            |> importFromSlickGrid name

    let SlickRange =
        let name = "SlickRange"

        Class name
        |> importFromSlickGrid name
        |+> Static [
            Constructor (T<int>?fromRow * T<int>?fromCell * !?T<int>?toRow * !?T<int>?toCell)
        ]
        |+> Instance [
            "fromRow" =@ T<int>
            "fromCell" =@ T<int>
            "toRow" =@ T<int>
            "toCell" =@ T<int>

            "isSingleRow" => T<unit> ^-> T<bool>
            "isSingleCell" => T<unit> ^-> T<bool>
            "rowCount" => T<unit> ^-> T<int>
            "cellCount" => T<unit> ^-> T<int>
            "contains" => T<int>?row * T<int>?cell ^-> T<bool>
            "toString" => T<unit> ^-> T<string>
        ]

    let SlickEventData =
        Generic - fun t ->
            let name = "SlickEventData"

            Class name
            |> importFromSlickGrid name
            |+> Static [
                Constructor (!?DOMEvent?event * !?t?args)
            ]
            |+> Instance [
                // Readonly Properties pulled from DOMEvent
                "altKey" =? T<bool>
                "ctrlKey" =? T<bool>
                "metaKey" =? T<bool>
                "shiftKey" =? T<bool>
                "key" =? T<string>
                "keyCode" =? T<int>
                "clientX" =? T<int>
                "clientY" =? T<int>
                "offsetX" =? T<int>
                "offsetY" =? T<int>
                "pageX" =? T<int>
                "pageY" =? T<int>
                "bubbles" =? T<bool>
                "target" =? HTMLElement
                "type" =? T<string>
                "which" =? T<int>
                "x" =? T<int>
                "y" =? T<int>
                "defaultPrevented" =? T<bool>

                // Methods
                "stopPropagation" => T<unit> ^-> T<unit>
                "isPropagationStopped" => T<unit> ^-> T<bool>
                "stopImmediatePropagation" => T<unit> ^-> T<unit>
                "isImmediatePropagationStopped" => T<unit> ^-> T<bool>
                
                "getNativeEvent" => T<unit> ^-> DOMEvent
                "preventDefault" => T<unit> ^-> T<unit>
                "isDefaultPrevented" => T<unit> ^-> T<bool>
                
                "addReturnValue" => T<obj>?value ^-> T<unit>
                "getReturnValue" => T<unit> ^-> T<obj>
                
                "getArguments" => T<unit> ^-> t
            ]

    let ExcelCopyBufferOption =
        Generic - fun t ->
            let CopyArgs = 
                Pattern.Config "ExcelCopyArgs" { 
                    Required = [ "ranges", !| SlickRange ]
                    Optional = [] 
                }

            let ExtractorReturn = T<string> + FormatterResultWithHtml + FormatterResultWithText

            let name = "ExcelCopyBufferOption"

            Pattern.Config name {
                Required = []
                Optional = [
                    "clearCopySelectionDelay", T<int>
                    "clipboardPasteDelay", T<int>
                    "copiedCellStyle", T<string>
                    "copiedCellStyleLayerKey", T<string>

                    "dataItemColumnValueExtractor", t * Column.[t] ^-> ExtractorReturn
                    "dataItemColumnValueSetter", t * Column.[t] * T<obj> ^-> ExtractorReturn

                    "clipboardCommandHandler", T<obj> ^-> T<unit>

                    "includeHeaderWhenCopying", T<bool>
                    "bodyElement", HTMLElement

                    "onCopyInit", T<unit> ^-> T<unit>
                    "onCopySuccess", T<int> ^-> T<unit>
                    "newRowCreator", T<int> ^-> T<unit>
                    "readOnlyMode", T<bool>

                    "headerColumnValueExtractor", Column.[t] ^-> T<string> + HTMLElement + DocumentFragment

                    // Events
                    "onCopyCells", SlickEventData.[T<obj>] * CopyArgs ^-> T<unit>
                    "onCopyCancelled", SlickEventData.[T<obj>] * CopyArgs ^-> T<unit>
                    "onPasteCells", SlickEventData.[T<obj>] * CopyArgs ^-> T<unit>
                ]
            }
            |> importFromSlickGrid name
            |=> Nested [CopyArgs]

    let GridMenuOption =
        let name = "GridMenuOption"

        Class name
        |+> Pattern.RequiredFields []
        |> importFromSlickGrid name

    let EditorFactory =
        Generic - fun t -> 
            let name = "EditorFactory"

            Pattern.Config name { 
                Required = [
                    "getEditor", t ^-> Editor
                ]
                Optional = [] 
            }
            |> importFromSlickGrid name

    let EditController =
        let name = "EditController"

        Pattern.Config name {
            Required = [
                "commitCurrentEdit", T<unit> ^-> T<bool>
                "cancelCurrentEdit", T<unit> ^-> T<bool>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let SlickEditorLock =
        let name = "SlickEditorLock"

        Class name
        |> importFromSlickGrid name
        |+> Instance [
            "activeEditController" =@ EditController
        
            "isActive" => !?EditController?editController ^-> T<bool>
            "activate" => EditController?editController ^-> T<unit>
            "deactivate" => EditController?editController ^-> T<unit>
            "commitCurrentEdit" => T<unit> ^-> T<bool>
            "cancelCurrentEdit" => T<unit> ^-> T<bool>
        ]        

    let GridOption =
        Generic - fun t ->
            let name = "GridOption"

            Class name
            |> importFromSlickGrid name
            |+> Pattern.RequiredFields []
            |+> Pattern.OptionalFields [
                "addNewRowCssClass", T<string>
                "alwaysAllowHorizontalScroll", T<bool>
                "alwaysShowVerticalScroll", T<bool>
                "asyncEditorLoadDelay", T<int>
                "asyncEditorLoading", T<bool>
                "asyncPostRenderDelay", T<int>
                "asyncPostRenderCleanupDelay", T<int>
                "auto", T<bool>
                "nonce", T<string>
                "autoCommitEdit", T<bool>
                "autoEdit", T<bool>
                "autoEditByKeypress", T<bool>
                "autoEditNewRow", T<bool>
                "autoHeight", T<bool>
                "autosizeColsMode", T<string>
                "autosizeColPaddingPx", T<int>
                "autosizeTextAvgToMWidthRatio", T<float>
                "cellFlashingCssClass", T<string>
                "cellMenu", CellMenuOption.Type
                "columnPicker", ColumnPickerOption.Type
                "contextMenu", ContextMenuOption.Type
                "createFooterRow", T<bool>
                "createPreHeaderPanel", T<bool>
                "createTopHeaderPanel", T<bool>
                    
                "dataItemColumnValueExtractor", T<obj> * t ^-> T<obj>
                    
                "defaultColumnWidth", T<int>
                "defaultFormatter", Formatter t Column.[t]
                    
                "devMode", T<bool> + T<obj> 
                "doPaging", T<bool>
                "editable", T<bool>
                    
                "editCommandHandler", T<obj> * t * EditCommand ^-> T<unit>
                    
                "editorFactory", EditorFactory.[t]
                    
                "editorLock", SlickEditorLock.Type
                "emulatePagingWhenScrolling", T<bool>
                "enableAddRow", T<bool>
                "enableAsyncPostRender", T<bool>
                "enableAsyncPostRenderCleanup", T<bool>
                "enableAutoTooltip", T<bool>
                "enableAutoSizeColumns", T<bool>
                "enableCellNavigation", T<bool>
                "editorCellNavOnLRKeys", T<bool>
                "enableCellRowSpan", T<bool>
                    
                "unorderableColumnCssClass", T<string>
                "enableHtmlRendering", T<bool>
                "enableMouseWheelScrollHandler", T<bool>
                "enableTextSelectionOnCells", T<bool>
                "excelCopyBufferOptions", ExcelCopyBufferOption.[T<obj>]
                "explicitInitialization", T<bool>
                "ffMaxSupportedCssHeight", T<int>
                "footerRowHeight", T<int>
                "forceFitColumns", T<bool>
                "forceSyncScrolling", T<bool>
                    
                "formatterFactory", FormatterFactory.[T<obj>]
                    
                "frozenBottom", T<bool>
                "frozenColumn", T<int>
                "frozenRow", T<int>
                "frozenRightViewportMinWidth", T<int>
                "fullWidthRows", T<bool>
                "gridMenu", GridMenuOption.Type
                "headerRowHeight", T<int>
                "leaveSpaceForNewRows", T<bool>
                "logSanitizedHtml", T<bool>
                "maxPartialRowSpanRemap", T<int>
                "maxSupportedCssHeight", T<int>
                "minRowBuffer", T<int>
                "maxRowBuffer", T<int>
                "mixinDefaults", T<bool>
                "multiColumnSort", T<bool>
                "multiSelect", T<bool>
                "numberedMultiColumnSort", T<bool>
                "preHeaderPanelHeight", T<int>
                "preHeaderPanelWidth", T<int> + T<string>
                "topHeaderPanelHeight", T<int>
                "topHeaderPanelWidth", T<int> + T<string>
                "preserveCopiedSelectionOnPaste", T<bool>
                "preventDragFromKeys", !| T<string>
                "rowHeight", T<int>
                "rowHighlightCssClass", T<string>
                "rowHighlightDuration", T<int>
                "rowTopOffsetRenderType", T<string>
                "sanitizer", T<string> ^-> T<string>
                "scrollRenderThrottling", T<int>
                "selectedCellCssClass", T<string>
                "showColumnHeader", T<bool>
                "showCellSelection", T<bool>
                "showFooterRow", T<bool>
                "showHeaderRow", T<bool>
                "showPreHeaderPanel", T<bool>
                "showTopHeaderPanel", T<bool>
                "showTopPanel", T<bool>
                "sortColNumberInSeparateSpan", T<bool>
                "shadowRoot", T<Dom.ShadowRoot>
                "suppressActiveCellChangeOnEdit", T<bool>
                "suppressCssChangesOnHiddenInit", T<bool>
                "syncColumnCellResize", T<bool>
                "skipFreezeColumnValidation", T<bool>
                "throwWhenFrozenNotAllViewable", T<bool>
                "invalidColumnFreezePickerMessage", T<string>
                "invalidColumnFreezePickerCallback", T<string> ^-> T<unit>
                "invalidColumnFreezeWidthMessage", T<string>
                "invalidColumnFreezeWidthCallback", T<string> ^-> T<unit>
                "topPanelHeight", T<int>
                "tristateMultiColumnSort", T<bool>
                "viewportClass", T<string>
                "viewportSwitchToScrollModeWidthPercent", T<int>
                "viewportMinWidthPx", T<int>
                "viewportMaxWidthPx", T<int>
                "columnPickerTitle", T<string>
                "forceFitTitle", T<string>
                "syncResizeTitle", T<string>
            ]

    let CustomDataView =
        Generic - fun t ->
            let name = "CustomDataView"

            Class name
            |> importFromSlickGrid name
            |+> Pattern.RequiredFields [
                "getItem", T<int> ^-> t
                    
                "getLength", T<unit> ^-> T<int>
            ]

    let Grid =
        Generic - fun tData ->
            let name = "SlickGrid"

            Class name
            |> importFromSlickGrid name
            |+> Static [
                Constructor (
                    (T<string> + HTMLElement)?container * (CustomDataView.[tData] + !| tData)?data * (!| Column.[tData])?columns * !? GridOption.[Column.[tData]]?options *
                    !? BasePubSub?externalPubSub
                )
            ]
            |+> Instance [
                "getData" => T<unit> ^-> (CustomDataView.[tData] + !| CustomDataView.[tData])
                "getDataLength" => T<unit> ^-> T<int>
                "getDataItem" => T<int>?i ^-> tData
                "setData" => (CustomDataView.[tData] + !| tData)?newData * !?T<bool>?scrollToTop ^-> T<unit>
                "getColumns" => T<unit> ^-> !| Column.[tData]
                "setColumns" => (!| Column.[tData])?columnDefinitions ^-> T<unit>
                "getVisibleColumns" => T<unit> ^-> !| Column.[tData]
                "getHeader" => Column.[tData]?columnDef ^-> (HTMLDivElement + !| HTMLDivElement)
                "validateSetColumnFreeze" => (!| Column.[tData])?newColumns * !?T<bool>?forceAlert ^-> T<bool>
                "calculateFrozenColumnIndexById" => (!| Column.[tData])?newColumns * !?(T<string> + T<int>)?columnId * !?T<bool>?applyIndexChange ^-> T<int>
            ]    

    let GroupTotalsFormatter = T<obj> * Column.[T<obj>] * Grid.[T<obj>] ^-> T<string>

    let ItemMetadata =
        let name = "ItemMetadata"  
            
        Class name 
        |> importFromSlickGrid name
        |+> Pattern.RequiredFields []   
        |+> Pattern.OptionalFields [
            "cssClasses", T<string>
            "editor", Editor.Type
            "focusable", T<bool>
            "formatter", GroupTotalsFormatter + Formatter (T<obj>) Column.[T<obj>]
            "selectable", T<bool>
            "columns", T<obj>
        ]            

    let AutoSize =
        Generic - fun t -> 
            let name = "AutoSize"

            Pattern.Config name {
                Required = []
                Optional = [
                    "allowAddlPercent", T<float>
                    "autosizeMode", T<string>
                    "colDataTypeOf", T<obj>
                    "colValueArray", !| T<obj>
                    "contentSizePx", T<int>
                    "formatterOverride", Formatter t Column.[t]
                    "headerWidthPx", T<int>
                    "ignoreHeaderText", T<bool>
                    "rowSelectionModeOnInit", T<bool>
                    "rowSelectionMode", T<string>
                    "rowSelectionCount", T<int>
                    "sizeToRemaining", T<bool>
                    "valueFilterMode", T<string>
                    "widthEvalMode", T<string>
                    "widthPx", T<int>
                ]
            }
            |> importFromSlickGrid name

    let RowCellFields = [ "row", T<int>; "cell", T<int> ]

    let MenuCallbackFields (t: CodeModel.TypeParameter) = 
        [
            "grid", Grid.[t]
            "column", Column.[t]
            "dataContext", T<obj>
        ] @ RowCellFields

    let MenuCallbackArgs = 
        Generic - fun t ->
            let name = "MenuCallbackArgs"

            Pattern.Config name {
                Required = []
                Optional = MenuCallbackFields t
            }
            |> importFromSlickGrid name

    let GridMenuCallbackArgs = 
        let name = "GridMenuCallbackArgs"

        Pattern.Config name {
            Required = [
                "grid", Grid.[T<obj>]
                "menu", T<obj>
                "columns", !| Column.[T<obj>]
                "visibleColumns", !| Column.[T<obj>]
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let MenuItem = 
        Generic - fun t ->
            let name = "MenuItem"

            Pattern.Config name {
                Required = []
                Optional = [
                    "cssClass", T<string>
                    "disabled", T<bool>
                    "divider", T<bool> + T<string>
                    "hidden", T<bool>
                    "iconCssClass", T<string>
                    "iconImage", T<string>
                    "positionOrder", T<int>
                    "subMenuTitle", T<string>
                    "subMenuTitleCssClass", T<string>
                    "textCssClass", T<string>
                    "title", T<string>
                    "tooltip", T<string>
            
                    "itemVisibilityOverride", t ^-> T<bool>
                    "itemUsabilityOverride", t ^-> T<bool>
                ]
            }
            |> importFromSlickGrid name

    let MenuCommandItemCallbackArgs = 
        Generic - fun t ->
            let name = "MenuCommandItemCallbackArgs"

            Pattern.Config name {
                Required = [
                    "command", T<string>
                    "item", T<obj>
                ]
                Optional = [
                    "value", T<obj>
                ] @ MenuCallbackFields t
            }
            |> importFromSlickGrid name

    let GridMenuCommandItemCallbackArgs = 
        let name = "GridMenuCommandItemCallbackArgs"
        
        Class name
        |> importFromSlickGrid name
        |+> Pattern.RequiredFields [
            "command", T<string>
            "item", T<obj>
            "grid", Grid.[T<obj>]
            "allColumns", !| Column.[T<obj>]
            "visibleColumns", !| Column.[T<obj>]
        ]  

    // Check inheritance after MenuItem definition
    let MenuCommandItem = 
        let name = "MenuCommandItem"
        
        Class name  
        |> importFromSlickGrid name
        |=> Inherits MenuItem.[MenuCallbackArgs.[T<obj>]]          

    let GridMenuItem = 
        let name = "GridMenuItem"

        Class name
        |> importFromSlickGrid name
        |=> Inherits MenuCommandItem
        |+> Pattern.RequiredFields []

    let MenuOptionItem = 
        let name = "MenuOptionItem"

        Class name
        |> importFromSlickGrid name
        |+> Pattern.RequiredFields []

    // Check inheritance after MenuOptionItem definition
    let MenuOptionItemCallbackArgs = 
        let name = "MenuOptionItemCallbackArgs"

        Class name
        |> importFromSlickGrid name
        |=> Inherits MenuCallbackArgs.[T<obj>]
        |+> Pattern.RequiredFields [
            "option", T<string>
            "item", MenuOptionItem.Type
        ]
        |+> Pattern.OptionalFields [
            "value", T<obj>
        ]

    let StandardFields = [ "grid", Grid.[T<obj>] ]

    let ColumnSortFields = [
            "sortCol", Column.[T<obj>]
            "columnId", T<string> + T<int>
            "sortAsc", T<bool>
        ]

    let ColumnSort =
        let name = "ColumnSort"

        Pattern.Config name {
            Required = []
            Optional = ColumnSortFields
        }
        |> importFromSlickGrid name

    let SingleColumnSort =
        let name = "SingleColumnSort"

        Pattern.Config name {
            Required = []
            Optional = ColumnSortFields @ [
                "grid", Grid.[T<obj>]
                "multiColumnSort", T<bool>
                "previousSortColumns", !| ColumnSort
            ]
        }
        |> importFromSlickGrid name

    let MultiColumnSort =
        let name = "MultiColumnSort"

        Pattern.Config name {
            Required = [
                "grid", Grid.[T<obj>]
                "multiColumnSort", T<bool>
                "sortCols", !| ColumnSort
            ]
            Optional = [
                "previousSortColumns", !| ColumnSort
            ]
        }
        |> importFromSlickGrid name

    let OnSortEventArgs = SingleColumnSort + MultiColumnSort

    let OnViewportChangedEventArgs = 
        let name = "OnViewportChangedEventArgs"

        Pattern.Config name {
            Required = StandardFields
            Optional = []
        }
        |> importFromSlickGrid name

    let CellResult = 
        let name = "CellResult"

        Pattern.Config name {
            Required = RowCellFields
            Optional = []
        }
        |> importFromSlickGrid name

    let SlickGridArg = 
        let name = "SlickGridArg"

        Pattern.Config name {
            Required = StandardFields
            Optional = []
        }
        |> importFromSlickGrid name

    let OnActiveCellChangedEventArgs = 
        let name = "OnActiveCellChangedEventArgs"

        Pattern.Config name {
            Required = StandardFields @ RowCellFields
            Optional = []
        }
        |> importFromSlickGrid name

    let OnActiveCellPositionChangedEventArgs =
        let name = "OnActiveCellPositionChangedEventArgs"

        Pattern.Config name {
            Required = StandardFields
            Optional = []
        }
        |> importFromSlickGrid name

    let OnAddNewRowEventArgs = 
        let name = "OnAddNewRowEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "item", T<obj>
                "column", Column.[T<obj>] 
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnAfterSetColumnsEventArgs = 
        let name = "OnAfterSetColumnsEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "newColumns", !| Column.[T<obj>] ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnAutosizeColumnsEventArgs = 
        let name = "OnAutosizeColumnsEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "columns", !| Column.[T<obj>] ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnBeforeUpdateColumnsEventArgs = 
        let name = "OnBeforeUpdateColumnsEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "columns", !| Column.[T<obj>] ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnBeforeAppendCellEventArgs = 
        let name = "OnBeforeAppendCellEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "row", T<int>
                "cell", T<int>
                "value", T<obj>
                "dataContext", T<obj> 
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnBeforeCellEditorDestroyEventArgs = 
        let name = "OnBeforeCellEditorDestroyEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "editor", Editor.Type ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnBeforeColumnsResizeEventArgs = 
        let name = "OnBeforeColumnsResizeEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "triggeredByColumn", T<string> ]
            Optional = []
        }
        |> importFromSlickGrid name

    let CompositeEditorModalType =
        let name = "CompositeEditorModalType"

        Pattern.EnumStrings "CompositeEditorModalType" [
            "create"
            "edit"
            "clone"
            "auto-mass"
            "mass-update"
            "mass-selection"
        ]
        |> importFromSlickGrid name

    let CompositeEditorOption =
        let name = "CompositeEditorOption"

        Pattern.Config name {
            Required = [
                "modalType", CompositeEditorModalType.Type
                "editors", T<obj>
                "formValues", T<obj>
            ]
            Optional = [
                "validationFailedMsg", T<string>
                "validationMsgPrefix", T<string>
                "show", T<unit> ^-> T<unit>
                "hide", T<unit> ^-> T<unit>
                "position", T<obj> ^-> T<unit>
                "destroy", T<unit> ^-> T<unit>
            ]
        }
        |> importFromSlickGrid name

    let OnBeforeEditCellEventArgs = 
        let name = "OnBeforeEditCellEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "item", T<obj>
                "column", Column.[T<obj>]
            ]
            Optional = [
                "row", T<int>
                "cell", T<int>
                "target", T<string> // 'grid' | 'composite'
                "compositeEditorOptions", CompositeEditorOption.Type
            ]
        }
        |> importFromSlickGrid name

    let NodeColmnArgs = [ "node", HTMLElement; "column", Column.[T<obj>] ]

    let OnBeforeHeaderCellDestroyEventArgs = 
        let name = "OnBeforeHeaderCellDestroyEventArgs"

        Pattern.Config name {
            Required = StandardFields @ NodeColmnArgs
            Optional = []
        }
        |> importFromSlickGrid name

    let OnBeforeHeaderRowCellDestroyEventArgs = 
        let name = "OnBeforeHeaderRowCellDestroyEventArgs"

        Pattern.Config name {
            Required = StandardFields @ NodeColmnArgs
            Optional = []
        }
        |> importFromSlickGrid name

    let OnBeforeFooterRowCellDestroyEventArgs = 
        let name = "OnBeforeFooterRowCellDestroyEventArgs"

        Pattern.Config name {
            Required = StandardFields @ NodeColmnArgs
            Optional = []
        }
        |> importFromSlickGrid name

    let OnBeforeSetColumnsEventArgs = 
        let name = "OnBeforeSetColumnsEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "previousColumns", !| Column.[T<obj>] 
                "newColumns", !| Column.[T<obj>]
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnCellChangeEventArgs = 
        let name = "OnCellChangeEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "row", T<int>
                "cell", T<int> 
                "item", T<obj>
                "column", Column.[T<obj>] 
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnCellCssStylesChangedEventArgs = 
        let name = "OnCellCssStylesChangedEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "key", T<string>; "hash", T<obj> ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnColumnsDragEventArgs = 
        let name = "OnColumnsDragEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "triggeredByColumn", T<string>; "resizeHandle", HTMLDivElement ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnColumnsReorderedEventArgs = 
        let name = "OnColumnsReorderedEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "impactedColumns", !| Column.[T<obj>]
                "previousColumnOrder", !| (T<string> + T<int>) 
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnColumnsResizedEventArgs = 
        let name = "OnColumnsResizedEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "triggeredByColumn", T<string> ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnColumnsResizeDblClickEventArgs = 
        let name = "OnColumnsResizeDblClickEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "triggeredByColumn", T<string> ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnCompositeEditorChangeEventArgs = 
        let name = "OnCompositeEditorChangeEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "item", T<obj>
                "column", Column.[T<obj>]
                "formValues", T<obj>
                "editors", T<obj> // { [columnId: string]: Editor }
            ]
            Optional = [
                "triggeredBy", T<string> // 'user' | 'system'
            ] @ RowCellFields
        }
        |> importFromSlickGrid name

    let OnClickEventArgs = 
        let name = "OnClickEventArgs"

        Pattern.Config name {
            Required = StandardFields @ RowCellFields
            Optional = []
        }
        |> importFromSlickGrid name

    let OnDblClickEventArgs = 
        let name = "OnDblClickEventArgs"

        Pattern.Config name {
            Required = StandardFields @ RowCellFields
            Optional = []
        }
        |> importFromSlickGrid name

    let OnFooterContextMenuEventArgs = 
        let name = "OnFooterContextMenuEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "column", Column.[T<obj>] ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnFooterRowCellRenderedEventArgs = 
        let name = "OnFooterRowCellRenderedEventArgs"

        Pattern.Config name {
            Required = StandardFields @ NodeColmnArgs
            Optional = []
        }
        |> importFromSlickGrid name

    let OnHeaderCellRenderedEventArgs = 
        let name = "OnHeaderCellRenderedEventArgs"

        Pattern.Config name {
            Required = StandardFields @ NodeColmnArgs
            Optional = []
        }
        |> importFromSlickGrid name

    let OnFooterClickEventArgs = 
        let name = "OnFooterClickEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "column", Column.[T<obj>] ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnHeaderClickEventArgs = 
        let name = "OnHeaderClickEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "column", Column.[T<obj>] ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnHeaderContextMenuEventArgs = 
        let name = "OnHeaderContextMenuEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "column", Column.[T<obj>] ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnHeaderMouseEventArgs = 
        let name = "OnHeaderMouseEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "column", Column.[T<obj>] ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnHeaderRowCellRenderedEventArgs = 
        let name = "OnHeaderRowCellRenderedEventArgs"

        Pattern.Config name {
            Required = StandardFields @ NodeColmnArgs
            Optional = []
        }
        |> importFromSlickGrid name

    let OnPreHeaderClickEventArgs = 
        let name = "OnPreHeaderClickEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "node", HTMLElement ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnPreHeaderContextMenuEventArgs = 
        let name = "OnPreHeaderContextMenuEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "node", HTMLElement ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnKeyDownEventArgs = 
        let name = "OnKeyDownEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "row", T<int>; "cell", T<int> ]
            Optional = []
        }
        |> importFromSlickGrid name

    let EditorValidator =
            (T<obj>?value * !? EditorArguments.[T<obj>]?args) ^-> EditorValidationResult

    let OnValidationErrorEventArgs = 
        let name = "OnValidationErrorEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "row", T<int>
                "cell", T<int>
                "validationResults", EditorValidationResult.Type
                "column", Column.[T<obj>]
                "editor", Editor.Type
                "cellNode", HTMLDivElement 
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnRenderedEventArgs = 
        let name = "OnRenderedEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "startRow", T<int> 
                "endRow", T<int> 
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnSelectedRowsChangedEventArgs = 
        let name = "OnSelectedRowsChangedEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "rows", !| T<int>
                "previousSelectedRows", !| T<int>
                "changedSelectedRows", !| T<int>
                "changedUnselectedRows", !| T<int>
                "caller", T<string>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnSetOptionsEventArgs = 
        let name = "OnSetOptionsEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "optionsBefore", GridOption.[Column.[T<obj>]] 
                "optionsAfter", GridOption.[Column.[T<obj>]] 
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnActivateChangedOptionsEventArgs = 
        let name = "OnActivateChangedOptionsEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "options", GridOption.[Column.[T<obj>]]  ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnScrollEventArgs = 
        let name = "OnScrollEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ 
                "scrollLeft", T<int> 
                "scrollTop", T<int>
                "cell", T<int>
                "row", T<int> 
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let DragRowMove = 
        let name = "DragRowMove"

        Pattern.Config name {
            Required = StandardFields @ [
                "count", T<int>
                "deltaX", T<int>; "deltaY", T<int>
                "offsetX", T<int>; "offsetY", T<int>
                "originalX", T<int>; "originalY", T<int>
                "available", HTMLDivElement + !| HTMLDivElement
                "drag", HTMLDivElement
                "drop", HTMLDivElement + !| HTMLDivElement
                "helper", HTMLDivElement
                "proxy", HTMLDivElement
                "target", HTMLDivElement
                "mode", T<string>
                "row", T<int>
                "rows", !| T<int>
                "startX", T<int>; "startY", T<int>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let SlickCopyRange =
        let name = "SlickCopyRange"

        Class name
        |> importFromSlickGrid name
        |+> Static [
            Constructor (T<int>?fromRow * T<int>?fromCell * T<int>?rowCount * T<int>?cellCount)
        ]
        |+> Instance [
            "fromRow" =@ T<int>
            "fromCell" =@ T<int>
            "rowCount" =@ T<int>
            "cellCount" =@ T<int>
        ]

    let OnDragReplaceCellsEventArgs = 
        let name = "OnDragReplaceCellsEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [
                "prevSelectedRange", SlickRange.Type
                "selectedRange", SlickRange.Type
                "copyToRange", SlickCopyRange.Type
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnBeforeRemoveCachedRowEventArgs = 
        let name = "OnBeforeRemoveCachedRowEventArgs"

        Pattern.Config name {
            Required = StandardFields @ [ "row", T<int> ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnBeforeDestroyEventArgs = 
        let name = "OnBeforeDestroyEventArgs"

        Pattern.Config name { 
            Required = StandardFields
            Optional = [] 
        }
        |> importFromSlickGrid name

    let OnBeforeSortEventArgs = SingleColumnSort + MultiColumnSort

    let ApplyHtmlCodeOption = 
        let name = "ApplyHtmlCodeOption"

        Pattern.Config name {
            Required = []
            Optional = [
                "emptyTarget", T<bool>
                "skipEmptyReassignment", T<bool>
            ]
        }
        |> importFromSlickGrid name

    let RangePx = 
        Pattern.Config "RangePx" {
            Required = [
                "top", T<int>
                "bottom", T<int>
                "leftPx", T<int>
                "rightPx", T<int>
            ]
            Optional = []
        }

    let CellFields = [
            "top", T<int>
            "bottom", T<int>
            "left", T<int>
            "right", T<int>
        ]

    let Cell = 
        Pattern.Config "Cell" {
            Required = CellFields
            Optional = []
        }

    let ParentRowSpanResult = 
        Pattern.Config "ParentRowSpanResult" {
            Required = [
                "start", T<int>
                "end", T<int>
                "range", T<string>
            ]
            Optional = []
        }

    let DimensionsFields = [
            "height", T<int>
            "width", T<int>
        ]

    let Dimensions = 
        Pattern.Config "Dimensions" {
            Required = DimensionsFields
            Optional = []
        }

    let Position = 
        Pattern.Config "Position" {
            Required = CellFields @ DimensionsFields @ [
                "visible", T<bool>
            ]
            Optional = []
        }

    let SlickEvent =
        Generic - fun t ->
            let handlerType = (SlickEventData.[t] + DOMEvent) * t ^-> T<obj>
            let name = "SlickEvent"

            Class name
            |> importFromSlickGrid name
            |+> Static [
                Constructor (!?T<string>?eventName * !?BasePubSub?pubSub)
            ]
            |+> Instance [
                "subscriberCount" =? T<int>

                "subscribe" => handlerType?fn ^-> T<unit>                
                "unsubscribe" => !?handlerType?fn ^-> T<unit>                
                "notify" => t?args * !?(SlickEventData.[t] + DOMEvent)?evt * !?T<obj>?scope ^-> SlickEventData.[t]                
                "setPubSubService" => BasePubSub?pubSub ^-> T<unit>
            ]

    let ObservableFields = [
        "subscribe", (T<obj> ^-> T<unit>) * !? (T<obj> ^-> T<unit>) * !? (T<unit> ^-> T<unit>) ^-> T<obj>
        "pipe", !+ T<obj> ^-> T<obj>
    ]

    let Observable =
        Generic - fun t ->
            let name = "Observable"

            Pattern.Config name {
                Required = ObservableFields
                Optional = []
            }
            |> importFromSlickGrid name

    let Subject =
        Generic - fun t ->
            let name = "Subject"

            Pattern.Config name {
                Required = ObservableFields @ [
                    "next", t ^-> T<unit>
                    "complete", T<unit> ^-> T<obj>
                    "unsubscribe", T<unit> ^-> T<unit>
                ]
                Optional = []
            }
            |> importFromSlickGrid name

    let AsyncProcess (t: CodeModel.TypeParameter) =
        let ReturnType = 
            T<Promise<_>>.[t] + 
            Observable.[t] + 
            Subject.[t]

        (T<int>?row * T<int>?cell * T<obj>?value * Column.[t]?columnDef * t?dataContext * !?Grid?grid) ^-> ReturnType

    let UsabilityOverrideArgs =
        Generic - fun t ->
            Pattern.Config "UsabilityOverrideArgs" {
                Required = [
                    "cell", T<int>
                    "row", T<int>
                    "column", Column.[t]
                    "dataContext", T<obj>
                    "type", T<string>
                    "grid", Grid.[T<obj>]   
                ]
                Optional = []
            }

    let CustomTooltipOption =
        Generic - fun t ->
            let name = "CustomTooltipOption"

            Pattern.Config name {
                Required = []
                Optional = [
                    "asyncParamsPropName", T<string>
                    "asyncProcess", AsyncProcess t
                
                    "asyncPostFormatter", Formatter t Column.[t]
                    "headerFormatter", Formatter t Column.[t]
                    "headerRowFormatter", Formatter t Column.[t]
                    "formatter", Formatter t Column.[t]

                    "hideArrow", T<bool>
                    "className", T<string>
                
                    "maxHeight", T<int>
                    "maxWidth", T<int>
                    "offsetLeft", T<int>
                    "offsetRight", T<int>
                    "offsetTopBottom", T<int>
                    "position", T<string>

                    "useRegularTooltip", T<bool>
                    "useRegularTooltipFromFormatterOnly", T<bool>
                    "renderRegularTooltipAsHtml", T<bool>
                
                    "tooltipTextMaxLength", T<int>
                    "regularTooltipWhiteSpace", T<string>
                    "whiteSpace", T<string>

                    "usabilityOverride", UsabilityOverrideArgs.[t]?args ^-> T<bool>
                ]
            }
            |> importFromSlickGrid name

    let GroupingGetterFunction t = t ^-> T<obj>

    let SortDirectionNumber =
        let name = "SortDirectionNumber"
        Pattern.EnumInlines name [
            "asc", "1"
            "desc", "-1"
            "neutral", "0"
        ]
        |> importFromSlickGrid name

    let Aggregator =
        let name = "Aggregator"

        Pattern.Config name {
            Required = [
                "field", T<int> + T<string>
                "type", T<string>
                "init", T<unit> ^-> T<unit>
                "storeResult", T<obj> + T<unit> ^-> T<unit>
            ]
            Optional = [
                "accumulate", T<obj> ^-> T<unit>
            ]
        }
        |> importFromSlickGrid name

    let GroupingComparerItem =
        let name = "GroupingComparerItem"

        Pattern.Config name {
            Required = [
                "count", T<int>
                "value", T<obj>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let GroupingFormatterItem =
        let name = "GroupingFormatterItem"

        Pattern.Config name {
            Required = [
                "count", T<int>
                "level", T<int>
                "groupingKey", T<string>
                "value", T<obj>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let Grouping =
        Generic - fun t ->
            let name = "Grouping"

            Pattern.Config name {
                Required = []
                Optional = [
                    "aggregators", !| Aggregator
                    "aggregateChildGroups", T<bool>
                    "aggregateCollapsed", T<bool>
                    "aggregateEmpty", T<bool>
                    "collapsed", T<bool>
                    "comparer", GroupingComparerItem * GroupingComparerItem ^-> SortDirectionNumber
                    "displayTotalsRow", T<bool>
                    "formatter", GroupingFormatterItem ^-> T<string>
                    "getter", T<string> + GroupingGetterFunction t
                    "lazyTotalsCalculation", T<bool>                    
                    "predefinedValues", !| T<obj>
                    "sortAsc", T<bool>
                ]
            }
            |> importFromSlickGrid name

    let HeaderButtonActionArgs =
        let name = "HeaderButtonActionArgs"

        Pattern.Config name {
            Required = [
                "command", T<string>
                "button", T<obj>
                "column", Column.[T<obj>]
                "grid", Grid.[T<obj>]
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let HeaderButtonOverrideArgs =
        let name = "HeaderButtonOverrideArgs"

        Pattern.Config name {
            Required = [
                "node", T<obj>
                "column", Column.[T<obj>]
                "grid", Grid.[T<obj>]
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let HeaderButtonItem =
        let name = "HeaderButtonItem"

        Pattern.Config name {
            Required = []
            Optional = [
                "command", T<string>
                "cssClass", T<string>
                "disabled", T<bool>
                "handler", DOMEvent ^-> T<unit>
                "image", T<string>
                "showOnHover", T<bool>
                "tooltip", T<string>
                
                "action", DOMEvent * HeaderButtonActionArgs ^-> T<unit>
                "itemVisibilityOverride", HeaderButtonOverrideArgs ^-> T<bool>
                "itemUsabilityOverride", HeaderButtonOverrideArgs ^-> T<bool>
            ]
        }
        |> importFromSlickGrid name

    let HeaderMenuCommandItem =
        Generic - fun t ->
            let name = "HeaderMenuCommandItem"

            Pattern.Config name {
                Required = []
                Optional = [
                    "command", T<string>
                    "title", T<string>
                    "disabled", T<bool>
                    "tooltip", T<string>
                    "cssClass", T<string>
                    "iconCssClass", T<string>
                    "iconImage", T<string>
                    "divider", T<bool> + T<string>
                    "hidden", T<bool>
                    "commandItems", !| (TSelf + T<string>)
                
                    // Deprecated 'items'
                    "items", !| (TSelf + T<string>) 
                ]
            }
            |> importFromSlickGrid name

    let HeaderMenuCommandItemCallbackArgs =
        Generic - fun t ->
            let name = "HeaderMenuCommandItemCallbackArgs"

            Pattern.Config name {
                Required = [
                    "column", Column.[t]
                    "grid", Grid.[t]
                    "menu", !| (HeaderMenuCommandItem.[t] + T<string>)
                ]
                Optional = []
            }
            |> importFromSlickGrid name

    let HeaderMenuItems =
        let name = "HeaderMenuItems"

        Pattern.Config name {
            Required = []
            Optional = [
                "commandItems", !| (HeaderMenuCommandItem + T<string>)
                
                "items", !| (HeaderMenuCommandItem + T<string>)
            ]
        }
        |> importFromSlickGrid name

    let HeaderButtonsOrMenu =
        let name = "HeaderButtonsOrMenu"

        Pattern.Config name {
            Required = []
            Optional = [
                "buttons", !| HeaderButtonItem
                "menu", HeaderMenuItems.Type
            ]
        }
        |> importFromSlickGrid name

    let SlickPluginFields = [
            "pluginName", T<string>
            "init", Grid.[T<obj>] ^-> T<unit>
            "destroy", T<unit> ^-> T<unit>
        ]

    let SlickPlugin = 
        let name = "SlickPlugin"

        Pattern.Config name {
            Required = SlickPluginFields
            Optional = []
        }
        |> importFromSlickGrid name

    let SelectionModel =
        let name = "SelectionModel"

        Pattern.Config name {
            Required = SlickPluginFields @ [
                "refreshSelections", T<unit> ^-> T<unit>
                "onSelectedRangesChanged", SlickEvent.[!| SlickRange]
                "getOptions", T<unit> ^-> T<obj>
                "getSelectedRanges", T<unit> ^-> !| SlickRange
                "setSelectedRanges", (!| SlickRange)?ranges * !?T<string>?caller * !?T<string>?selectionMode ^-> T<unit>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let ColumnMetadata =
        Generic - fun t ->
            let name = "ColumnMetadata"

            Pattern.Config name {
                Required = []
                Optional = [
                    "colspan", T<int> + T<string>
                    "rowspan", T<int>
                    "cssClass", T<string>
                    "editor", Editor.Type
                    "focusable", T<bool>
                    "formatter", Formatter t Column.[t]
                    "selectable", T<bool>
                ]
            }
            |> importFromSlickGrid name

    let PagingInfo =
        let name = "PagingInfo"

        Pattern.Config name {
            Required = []
            Optional = [
                "pageSize", T<int>
                "pageNum", T<int>
                "totalRows", T<int>
                "totalPages", T<int>
                "dataView", T<obj>
            ]
        }
        |> importFromSlickGrid name

    let FormatterOverrideCallback =
        (T<int>?row * T<int>?cell * T<obj>?value * Column.[T<obj>]?columnDef * T<obj>?item * Grid.[T<obj>]?grid) ^-> (T<string> + FormatterResultWithHtml + FormatterResultWithText)

    let OnGroupExpandedEventArgs =
        let name = "OnGroupExpandedEventArgs"

        Pattern.Config name {
            Required = [
                "level", T<int>
                "groupingKey", T<string> + T<int>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnGroupCollapsedEventArgs =
        let name = "OnGroupCollapsedEventArgs"

        Pattern.Config name {
            Required = [
                "level", T<int>
                "groupingKey", T<string> + T<int>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnRowCountChangedEventArgs =
        let name = "OnRowCountChangedEventArgs"

        Pattern.Config name {
            Required = [
                "previous", T<int>
                "current", T<int>
                "itemCount", T<int>
                "dataView", SlickDataView.[T<obj>]
                "callingOnRowsChanged", T<bool>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnRowsChangedEventArgs =
        let name = "OnRowsChangedEventArgs"

        Pattern.Config name {
            Required = [
                "rows", !| T<int>
                "itemCount", T<int>
                "dataView", SlickDataView.[T<obj>]
                "calledOnRowCountChanged", T<bool>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnRowsOrCountChangedEventArgs =
        let name = "OnRowsOrCountChangedEventArgs"

        Pattern.Config name {
            Required = [
                "rowsDiff", !| T<int>
                "previousRowCount", T<int>
                "currentRowCount", T<int>
                "itemCount", T<int>
                "rowCountChanged", T<bool>
                "rowsChanged", T<bool>
                "dataView", SlickDataView.[T<obj>]
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let OnSelectedRowIdsChangedEventArgs =
        let name = "OnSelectedRowIdsChangedEventArgs"

        Pattern.Config name {
            Required = [
                "filteredIds", !| (T<string> + T<int>)
                "selectedRowIds", !| (T<string> + T<int>)
                "ids", !| (T<string> + T<int>)
                "rows", !| T<int>
                "dataView", SlickDataView.[T<obj>]
            ]
            Optional = [
                "grid", Grid.[T<obj>]
                "added", T<bool>
            ]
        }
        |> importFromSlickGrid name

    let OnSetItemsCalledEventArgs =
        let name = "OnSetItemsCalledEventArgs"

        Pattern.Config name {
            Required = [
                "idProperty", T<string>
                "itemCount", T<int>
            ]
            Optional = []
        }
        |> importFromSlickGrid name

    let SlickNonDataItem =
        let name = "SlickNonDataItem"

        Class name
        |> importFromSlickGrid name
        |+> Instance [
            "_nonDataRow" =? T<bool>
        ]

    let SlickGroup =
        let name = "SlickGroup"

        Class name
        |> importFromSlickGrid name
    
    let SlickGroupTotals =
        let name = "SlickGroupTotals"

        Class name
        |> importFromSlickGrid name
        |=> Inherits SlickNonDataItem
        |+> Static [
            Constructor T<unit>
        ]
        |+> Instance [
            "_groupTotals" =? T<bool>
            "group" =? SlickGroup
            "initialized" =? T<bool>
        ]

    let Formatters =
        let name = "Formatters"
        Class name
        |> importFromSlickGrid name
        |+> Static [
            "PercentComplete" =? Formatter T<obj> Column.[T<obj>]
            "PercentCompleteBar" =? Formatter T<obj> Column.[T<obj>]
            "YesNo" =? Formatter T<obj> Column.[T<obj>]
            "Checkbox" =? Formatter T<obj> Column.[T<obj>]
            "Checkmark" =? Formatter T<obj> Column.[T<obj>]
        ]

    let Styles = 
        Class "Styles"
        |+> Static [
            "alpineTheme" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick-alpine-theme.css"

            "defaultTheme" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick-default-theme.css"

            "icons" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick-icons.css"

            "cellMenu" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.cellmenu.css"

            "columnPicker" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.columnpicker.css"

            "contextMenu" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.contextmenu.css"

            "customTooltip" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.customtooltip.css"

            "draggableGrouping" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.draggablegrouping.css"

            "grid" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.grid.css"

            "gridMenu" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.gridmenu.css"

            "headerButtons" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.headerbuttons.css"

            "headerMenu" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.headermenu.css"

            "pager" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.pager.css"

            "rowDetailView" => T<unit> ^-> T<unit>
            |> ImportFile "slickgrid/dist/styles/css/slick.rowdetailview.css"
        ]

    // --- Extend Classes ---

    SlickGroup
        |=> Inherits SlickNonDataItem
        |+> Static [
            Constructor T<unit>
        ]
        |+> Instance [
            "_group" =? T<bool>
            "level" =? T<int>
            "count" =? T<int>
            "value" =? T<obj>
            "title" =? T<string>
            "collapsed" =? T<bool> + T<int>
            "selectChecked" =? T<bool>
            "totals" =? SlickGroupTotals
            "rows" =? !| T<int>
            "groups" =? !| T<obj>
            "groupingKey" =? T<obj>

            "equals" => TSelf ^-> T<bool>
        ]
        |> ignore

    SlickGroupItemMetadataProvider
        |+> Instance [
            "pluginName" =? T<string>

            "init" => Grid.[T<obj>]?grid ^-> T<unit>
            "destroy" => T<unit> ^-> T<unit>
        
            "getOptions" => T<unit> ^-> GroupItemMetadataProviderOption
            "setOptions" => GroupItemMetadataProviderOption?inputOptions ^-> T<unit>
        
            "getGroupRowMetadata" => T<obj>?item * !?T<int>?row * !?T<int>?cell ^-> ItemMetadata
            "getTotalsRowMetadata" => T<obj>?item * !?T<int>?row * !?T<int>?cell ^-> ItemMetadata
        ]
        |> ignore

    SlickDataView
        |+> Instance [
            // Properties
            "onBeforePagingInfoChanged" =? SlickEvent.[PagingInfo]
            "onGroupExpanded" =? SlickEvent.[OnGroupExpandedEventArgs]
            "onGroupCollapsed" =? SlickEvent.[OnGroupCollapsedEventArgs]
            "onPagingInfoChanged" =? SlickEvent.[PagingInfo]
            "onRowCountChanged" =? SlickEvent.[OnRowCountChangedEventArgs]
            "onRowsChanged" =? SlickEvent.[OnRowsChangedEventArgs]
            "onRowsOrCountChanged" =? SlickEvent.[OnRowsOrCountChangedEventArgs]
            "onSelectedRowIdsChanged" =? SlickEvent.[OnSelectedRowIdsChangedEventArgs]
            "onSetItemsCalled" =? SlickEvent.[OnSetItemsCalledEventArgs]

            // Methods
            "beginUpdate" => !?T<bool>?bulkUpdate ^-> T<unit>
            "endUpdate" => T<unit> ^-> T<unit>
            "destroy" => T<unit> ^-> T<unit>
            "setRefreshHints" => DataViewHints?hints ^-> T<unit>
            "getFilterArgs" => T<unit> ^-> T<obj>
            "setFilterArgs" => T<obj>?args ^-> T<unit>
            
            "getIdPropertyName" => T<unit> ^-> T<string>
    
            "setPagingOptions" => PagingInfo?args ^-> T<unit>
            "getPagingInfo" => T<unit> ^-> PagingInfo

            "fastSort" => (T<string> + (T<unit> ^-> T<string>))?field * !?T<bool>?ascending ^-> T<unit>
            "reSort" => T<unit> ^-> T<unit>   
            
            "getFilteredItemCount" => T<unit> ^-> T<int>    
            "getGrouping" => T<unit> ^-> !| Grouping.[T<obj>]
            "setGrouping" => (Grouping.[T<obj>] + !| Grouping.[T<obj>])?groupingInfo ^-> T<unit>    
            
            "getIdxById" => (T<string> + T<int>)?id ^-> T<int>            
            "getRowById" => (T<string> + T<int>)?id ^-> T<int>
            
            "mapIdsToRows" => (!| (T<string> + T<int>))?idArray ^-> !| T<int>
            "mapRowsToIds" => (!| T<int>)?rowArray ^-> !| (T<string> + T<int>)

            "getItemMetadata" => T<int>?row ^-> ItemMetadata
    
            "collapseAllGroups" => !?T<int>?level ^-> T<unit>
            "expandAllGroups" => !?T<int>?level ^-> T<unit>
            "expandCollapseGroup" => T<int>?level * T<string>?groupingKey * !?T<bool>?collapse ^-> T<unit>
            "collapseGroup" => !+ T<obj> ^-> T<unit>
            "expandGroup" => !+ T<obj> ^-> T<unit>
            "getGroups" => T<unit> ^-> !| SlickGroup
    
            "refresh" => T<unit> ^-> T<unit>
            "syncGridSelection" => Grid.[T<obj>]?grid * T<bool>?preserveHidden * !?T<bool>?preserveHiddenOnSelectionChange ^-> SlickEvent.[OnSelectedRowIdsChangedEventArgs]
            "getAllSelectedIds" => T<unit> ^-> !| (T<string> + T<int>)
            "getAllSelectedFilteredIds" => T<unit> ^-> !| T<obj>
            "setSelectedIds" => (!| (T<string> + T<int>))?selectedIds * !?T<obj>?options ^-> T<unit>
            
            "syncGridCellCssStyles" => Grid.[T<obj>]?grid * T<string>?key ^-> T<unit>
        ]
        |> ignore

    CustomDataView
        |+> Pattern.OptionalFields [
            "getItemMetadata", T<int> * !?(T<bool> + T<int>) ^-> ItemMetadata
        ]
        |> ignore

    EditorArguments
        |+> Pattern.RequiredFields [
            "grid", Grid.[T<obj>]
        ]
        |> ignore

    Column 
        |+> Pattern.RequiredFields [
            "id", T<string> + T<int>
            "field", T<string>
        ]
        |+> Pattern.OptionalFields [
            "alwaysRenderColumn", T<bool>            
            "autoSize", AutoSize.[T<obj>]
            "behavior", T<string>
            "cellAttrs", T<obj>
            "headerCellAttrs", T<obj>
            "cannotTriggerInsert", T<bool>
            "columnGroup", T<string>
            "colspan", T<int> + T<string>
            "cssClass", T<string>
            "customTooltip", CustomTooltipOption.[T<obj>]
            "defaultSortAsc", T<bool>
            "denyPaste", T<bool>
            "disableTooltip", T<bool>
            "editor", Editor.Type
            "editorFixedDecimalPlaces", T<int>
            "excludeFromColumnPicker", T<bool>
            "excludeFromExport", T<bool>
            "excludeFromGridMenu", T<bool>
            "excludeFromQuery", T<bool>
            "excludeFromHeaderMenu", T<bool>
            "focusable", T<bool>
            "formatterOverride", T<obj> + FormatterOverrideCallback
            "grouping", Grouping.[T<obj>]
            "groupTotalsFormatter", GroupTotalsFormatter
            "header", HeaderButtonsOrMenu.Type
            "headerCssClass", T<string>
            "hidden", T<bool>
            "maxWidth", T<int>
            "minWidth", T<int>
            "name", T<string> + HTMLElement + DocumentFragment
            "offsetWidth", T<int>
            "params", T<obj>
            "previousWidth", T<int>
            "reorderable", T<bool>
            "rerenderOnResize", T<bool>
            "resizable", T<bool>
            "rowspan", T<int>
            "selectable", T<bool>
            "sortable", T<bool>
            "toolTip", T<string>
            "unselectable", T<bool>
            "validator", EditorValidator
            "width", T<int>
            "widthRequest", T<int>
        ]
        |> ignore

    MenuCommandItem 
        |+> Pattern.RequiredFields [
            "command", T<string>
        ]
        |+> Pattern.OptionalFields [
            "commandItems", !| (TSelf + T<string>)
            "action", (SlickEventData.[T<obj>] + DOMEvent) * (MenuCommandItemCallbackArgs + GridMenuCommandItemCallbackArgs) ^-> T<unit>
        ]
        |> ignore

    Grid 
        |+> Instance [
            "init" => T<unit> ^-> T<unit>
            "cacheCssForHiddenInit" => T<unit> ^-> T<unit>
            "restoreCssFromHiddenInit" => T<unit> ^-> T<unit>

            "destroy" => !?T<bool>?shouldDestroyAllElements ^-> T<unit>
            "getUID" => T<unit> ^-> T<string>
            "slickGridVersion" =? T<string>
            "cid" =@ T<string>
               
            "hasDataView" => T<unit> ^-> T<bool>
            "getItemMetadaWhenExists" => T<int>?row ^-> ItemMetadata

            "getOptions" => T<unit> ^-> GridOption.[Column.[T<obj>]]
            "setOptions" => GridOption.[Column.[T<obj>]]?newOptions * !?T<bool>?suppressRender * !?T<bool>?suppressColumnSet * !?T<bool>?suppressSetOverflow ^-> T<unit>
            "activateChangedOptions" => !?T<bool>?suppressRender * !?T<bool>?suppressColumnSet * !?T<bool>?suppressSetOverflow ^-> T<unit>
            "validateAndEnforceOptions" => T<unit> ^-> T<unit>

            "getColumnIndex" => (T<string> + T<int>)?id ^-> T<int>
            "updateColumnHeader" => (T<string> + T<int>)?columnId * !?(T<string> + HTMLElement + DocumentFragment)?title * !?T<string>?toolTip ^-> T<unit>
            "autosizeColumn" => (T<string> + T<int>)?columnOrIndexOrId * !?T<bool>?isInit ^-> T<unit>
            "autosizeColumns" => !?T<string>?autosizeMode * !?T<bool>?isInit ^-> T<unit>
            "reRenderColumns" => !?T<bool>?reRender ^-> T<unit>
            "updateColumns" => T<unit> ^-> T<unit>
            "setSortColumn" => (T<string> + T<int>)?columnId * T<bool>?ascending ^-> T<unit>
            "setSortColumns" => (!| ColumnSort)?cols ^-> T<unit> 
            "getSortColumns" => T<unit> ^-> !| ColumnSort
            "getColumnByIndex" => T<int>?id ^-> HTMLElement
            "getAbsoluteColumnMinWidth" => T<unit> ^-> T<int>
            "getHeaderColumnWidthDiff" => T<unit> ^-> T<int>

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
                
            "getActiveCell" => T<unit> ^-> CellResult
            "setActiveCell" => T<int>?row * T<int>?cell * !?T<bool>?opt_editMode * !?T<bool>?preClickModeOn * !?T<bool>?suppressActiveCellChangedEvent ^-> T<unit>
            "resetActiveCell" => T<unit> ^-> T<unit>
            "unsetActiveCell" => T<unit> ^-> T<unit>
            "getActiveCellNode" => T<unit> ^-> HTMLDivElement
            "getActiveCellPosition" => T<unit> ^-> Position 
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

            "editActiveCell" => !?Editor?editor * !?T<bool>?preClickModeOn * !?DOMEvent?e ^-> T<unit>
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

            "applyHtmlCode" => HTMLElement?target * (HTMLElement + T<string> + DocumentFragment)?value * !?ApplyHtmlCodeOption?options ^-> T<unit>
                
            "getViewport" => !?T<int>?viewportTop * !?T<int>?viewportLeft ^-> RangePx
            "getVisibleRange" => !?T<int>?viewportTop * !?T<int>?viewportLeft ^-> RangePx
            "getRenderedRange" => !?T<int>?viewportTop * !?T<int>?viewportLeft ^-> RangePx
            "getViewportRowCount" => T<unit> ^-> T<int>
            "getViewportHeight" => T<unit> ^-> T<int>
            "getViewportWidth" => T<unit> ^-> T<int>
            "getCanvasNode" => !?(T<string> + T<int>)?columnIdOrIdx * !?T<int>?rowIndex ^-> HTMLDivElement
            "getActiveCanvasNode" => !?(DOMEvent + SlickEventData.[T<obj>])?e ^-> HTMLDivElement
            "getCanvases" => T<unit> ^-> !| HTMLDivElement
            "getViewportNode" => !?(T<string> + T<int>)?columnIdOrIdx * !?T<int>?rowIndex ^-> HTMLElement
            "getViewports" => T<unit> ^-> !| HTMLDivElement
            "getActiveViewportNode" => !?(DOMEvent + SlickEventData.[T<obj>])?e ^-> HTMLDivElement
            "setActiveViewportNode" => !?(DOMEvent + SlickEventData.[T<obj>])?e ^-> HTMLDivElement
            "getCanvasWidth" => T<unit> ^-> T<int>
            "getHeadersWidth" => T<unit> ^-> T<int>
            "getScrollbarDimensions" => T<unit> ^-> Dimensions
            "getDisplayedScrollbarDimensions" => T<unit> ^-> Dimensions
            "getGridPosition" => T<unit> ^-> Position
            "getRowCache" => T<unit> ^-> T<obj>

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
            "applyFormatResultToCellNode" => (FormatterResultWithHtml + FormatterResultWithText + T<string> + HTMLElement + DocumentFragment)?formatterResult * HTMLDivElement?cellNode * !?T<bool>?suppressRemove ^-> T<unit>

            "registerPlugin" => SlickPlugin?plugin ^-> T<unit>
            "unregisterPlugin" => SlickPlugin?plugin ^-> T<unit>
            "getPluginByName" => T<string>?name ^-> SlickPlugin
            "getPubSubService" => T<unit> ^-> BasePubSub
                
            "getCellFromEvent" => (DOMEvent + SlickEventData.[T<obj>])?evt ^-> CellResult
            "getCellFromPoint" => T<int>?x * T<int>?y ^-> CellResult
            "getCellNode" => T<int>?row * T<int>?cell ^-> HTMLDivElement
            "getCellNodeBox" => T<int>?row * T<int>?cell ^-> Cell
            "getCellHeight" => T<int>?row * T<int>?rowspan ^-> T<int>
            "arrayEquals" => (!| T<obj>)?arr1 * (!| T<obj>)?arr2 ^-> T<bool>
            "updatePagingStatusFromView" => PagingInfo?pagingInfo ^-> T<unit>
                
            "getRowSpanColumnIntersects" => T<int>?row ^-> !| T<int>
            "getRowSpanIntersect" => T<int>?row ^-> T<int> 
            "getParentRowSpanByCell" => T<int>?row * T<int>?cell * !?T<bool>?excludeParentRow ^-> ParentRowSpanResult
            "remapAllColumnsRowSpan" => T<unit> ^-> T<unit>
            "findSpanStartingCell" => T<int>?row * T<int>?cell ^-> CellResult

            "validateColumnFreezeWidth" => !?T<int>?frozenColumn * !?T<bool>?forceAlert ^-> T<bool>
                
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
            "onContextMenu" =? SlickEvent.[MenuCommandItemCallbackArgs.[T<obj>]]
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
        |> ignore

    GridMenuItem 
        |+> Pattern.OptionalFields [
            // Deprecated: customItems
            "customItems", !| (GridMenuItem + T<string>)     
            
            "itemVisibilityOverride", GridMenuCallbackArgs ^-> T<bool>        
            "itemUsabilityOverride", GridMenuCallbackArgs ^-> T<bool>
            "action", (SlickEventData.[T<obj>] + DOMEvent) * GridMenuCommandItemCallbackArgs ^-> T<unit>
        ]
        |> ignore

    // Check inheritance
    MenuOptionItem 
        |=> Inherits MenuItem.[MenuCallbackArgs[T<obj>]]
        |+> Pattern.OptionalFields [
            "action", DOMEvent * MenuOptionItemCallbackArgs ^-> T<unit>
            "option", T<obj>
            "optionItems", !| (TSelf + T<string>)
        ]
        |> ignore

    CellMenuOption 
       |+> Pattern.OptionalFields [
            "autoAdjustDrop", T<bool>
            "autoAdjustDropOffset", T<int>
            "autoAlignSide", T<bool>
            "autoAlignSideOffset", T<int>             
            "commandTitle", T<string>
            "dropDirection", T<string>
            "dropSide", T<string>                
            "hideCloseButton", T<bool>
            "hideCommandSection", T<bool>
            "hideMenuOnScroll", T<bool>
            "hideOptionSection", T<bool>
            "maxHeight", T<int> + T<string>
            "maxWidth", T<int> + T<string>
            "width", T<int> + T<string>                         
            "optionTitle", T<string>
            "subItemChevronClass", T<string>
            "subMenuOpenByEvent", T<string>
            "menuUsabilityOverride", MenuCallbackArgs.[T<obj>] ^-> T<bool>
            "commandItems", !| (MenuCommandItem + T<string>)   
            "optionItems", !| (MenuOptionItem + T<string>)  
        ]   
        |> ignore

    ColumnPickerOption 
        |+> Pattern.OptionalFields [
            "columnTitle", T<string>
            "forceFitTitle", T<string>
            "fadeSpeed", T<int>
            "hideForceFitButton", T<bool>
            "hideSyncResizeButton", T<bool>
            "maxHeight", T<int> + T<string>
            "minHeight", T<int> + T<string>
            "syncResizeTitle", T<string>
            "headerColumnValueExtractor", Column.[T<obj>] * !?GridOption.[Column.[T<obj>]] ^-> T<string> + HTMLElement + DocumentFragment
        ]
        |> ignore

    OnColumnsChangedArgs 
        |+> Pattern.RequiredFields [
            "columnId", T<int> + T<string>
            "showing", T<bool>
            "grid", Grid.[T<obj>]
            "allColumns", !| Column.[T<obj>]
            "columns", !| Column.[T<obj>]
            "visibleColumns", !| Column.[T<obj>]
        ]
        |> ignore

    ContextMenuOption 
        |+> Pattern.OptionalFields [
            "autoAdjustDrop", T<bool>
            "autoAdjustDropOffset", T<int>
            "autoAlignSide", T<bool>
            "autoAlignSideOffset", T<int>
            "commandItems", !| (MenuCommandItem + T<string>)
            "commandShownOverColumnIds", !| T<string>
            "commandTitle", T<string>
            "dropDirection", T<string>
            "dropSide", T<string>
            "hideCloseButton", T<bool>
            "hideCommandSection", T<bool>
            "hideMenuOnScroll", T<bool>
            "hideOptionSection", T<bool>
            "maxHeight", T<int> + T<string>
            "maxWidth", T<int> + T<string>
            "width", T<int> + T<string>
            "optionItems", !| (MenuOptionItem + T<string>)
            "optionShownOverColumnIds", !| T<string>
            "optionTitle", T<string>
            "subItemChevronClass", T<string>
            "subMenuOpenByEvent", T<string>
            "menuUsabilityOverride", MenuCallbackArgs.[T<obj>] ^-> T<bool>
        ]
        |> ignore

    let ColumnReorderFunction =
        let t = Column.[T<obj>]
        (
            Grid.[t] *
            !| HTMLElement *
            T<int> *
            ((!| t) ^-> T<unit>) *
            (T<unit> ^-> T<unit>) *
            !| t*
            (T<string> ^-> T<int>) * 
            T<string> *
            (SlickEvent.[T<obj>] * !?T<obj> ^-> T<unit>)
        ) ^-> T<unit>

    GridOption         
        |+> Pattern.OptionalFields [
            "customTooltip", CustomTooltipOption.[T<obj>]
            "enableColumnReorder", T<bool> + ColumnReorderFunction
        ]
        |> ignore
    
    GridMenuOption 
        |+> Pattern.OptionalFields [
                "commandTitle", T<string>
                "commandItems", !| (MenuCommandItem + T<string>)
                "customTitle", T<string>
                "customItems", !| (GridMenuItem + T<string>)
                "contentMinWidth", T<int>
                "columnTitle", T<string>
                "forceFitTitle", T<string>
                "height", T<int> + T<string>
                "hideForceFitButton", T<bool>
                "hideSyncResizeButton", T<bool>
                "iconButtonContainer", T<string>
                "iconImage", T<string>
                "iconCssClass", T<string>
                "leaveOpen", T<bool>
                "marginBottom", T<int>
                "maxHeight", T<int> + T<string>
                "maxWidth", T<int> + T<string>
                "menuWidth", T<int>
                "resizeOnShowHeaderRow", T<bool>
                "showButton", T<bool>
                "subItemChevronClass", T<string>
                "subMenuOpenByEvent", T<string>
                "syncResizeTitle", T<string>
                "useClickToRepositionMenu", T<bool>
                "width", T<int> + T<string>
                    
                "headerColumnValueExtractor", Column.[T<obj>] * !?GridOption.[T<obj>] ^-> T<string> + HTMLElement + DocumentFragment
                "menuUsabilityOverride", MenuCallbackArgs.[T<obj>] ^-> T<bool>
            ]
            |> ignore

    let Assembly =
        Assembly [
            Namespace "WebSharper.SlickGrid" [
                Grid
                SlickDataView
                Column
                SlickEvent
                Observable
                Subject
                AutoSize
                ApplyHtmlCodeOption
                CellMenuOption
                CustomTooltipOption
                FormatterResultObject
                FormatterResultWithText
                FormatterResultWithHtml
                RangePx
                Cell
                Dimensions
                Position
                CellResult
                ParentRowSpanResult
                MenuItem
                MenuCommandItem
                MenuOptionItem
                GridMenuItem
                MenuCallbackArgs
                GridMenuCallbackArgs
                MenuCommandItemCallbackArgs
                GridMenuCommandItemCallbackArgs
                MenuOptionItemCallbackArgs
                OnActiveCellChangedEventArgs
                OnActiveCellPositionChangedEventArgs
                OnAddNewRowEventArgs
                OnActivateChangedOptionsEventArgs
                OnAfterSetColumnsEventArgs
                OnAutosizeColumnsEventArgs
                OnBeforeAppendCellEventArgs
                OnBeforeCellEditorDestroyEventArgs
                OnBeforeColumnsResizeEventArgs
                OnBeforeDestroyEventArgs
                OnBeforeEditCellEventArgs
                OnBeforeFooterRowCellDestroyEventArgs
                OnBeforeHeaderCellDestroyEventArgs
                OnBeforeHeaderRowCellDestroyEventArgs
                OnBeforeRemoveCachedRowEventArgs
                OnBeforeSetColumnsEventArgs
                OnBeforeUpdateColumnsEventArgs
                OnCellChangeEventArgs
                OnCellCssStylesChangedEventArgs
                OnClickEventArgs
                OnColumnsDragEventArgs
                OnColumnsReorderedEventArgs
                OnColumnsResizedEventArgs
                OnColumnsResizeDblClickEventArgs
                OnCompositeEditorChangeEventArgs
                OnDblClickEventArgs
                OnDragReplaceCellsEventArgs
                OnFooterClickEventArgs
                OnFooterContextMenuEventArgs
                OnFooterRowCellRenderedEventArgs
                OnHeaderCellRenderedEventArgs
                OnHeaderClickEventArgs
                OnHeaderContextMenuEventArgs
                OnHeaderMouseEventArgs
                OnHeaderRowCellRenderedEventArgs
                OnPreHeaderClickEventArgs
                OnPreHeaderContextMenuEventArgs
                OnKeyDownEventArgs
                OnRenderedEventArgs
                OnScrollEventArgs
                OnSelectedRowsChangedEventArgs
                OnSetOptionsEventArgs
                OnValidationErrorEventArgs
                OnViewportChangedEventArgs
                DragRowMove  
                UsabilityOverrideArgs
                EditorValidationResult
                ElementPosition
                Grouping
                GroupingFormatterItem
                GroupingComparerItem
                SortDirectionNumber
                Aggregator
                HeaderButtonsOrMenu
                HeaderMenuItems
                HeaderMenuCommandItem
                HeaderMenuCommandItemCallbackArgs
                HeaderButtonItem
                HeaderButtonOverrideArgs
                HeaderButtonActionArgs
                OnColumnsChangedArgs
                ColumnPickerOption
                ContextMenuOption
                GridOption
                GridMenuOption
                ExcelCopyBufferOption
                FormatterFactory
                EditCommand
                EditorFactory
                ColumnSort
                SlickPlugin
                SelectionModel
                Editor
                EditorArguments
                ColumnMetadata
                ItemMetadata
                SlickEditorLock
                EditController
                PagingInfo
                SlickRange
                SlickCopyRange
                BasePubSub
                CompositeEditorOption
                CompositeEditorModalType
                SingleColumnSort
                MultiColumnSort
                SlickEventData
                CustomDataView
                OnSetItemsCalledEventArgs
                OnSelectedRowIdsChangedEventArgs
                OnRowsOrCountChangedEventArgs
                OnRowsChangedEventArgs
                OnRowCountChangedEventArgs
                DataViewHints
                OnGroupCollapsedEventArgs
                OnGroupExpandedEventArgs
                ItemMetadataProvider
                DataViewOption
                SlickGroupItemMetadataProvider
                GroupItemMetadataProviderOption
                SlickGroup
                SlickNonDataItem
                SlickGroupTotals
                Formatters
                Styles
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()