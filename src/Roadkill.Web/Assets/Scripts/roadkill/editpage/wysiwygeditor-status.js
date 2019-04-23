/// <reference path="../typescript-ref/references.ts" />
var Roadkill;
(function (Roadkill) {
    var Web;
    (function (Web) {
        /**
        Event bindings and handlers for the edit text area toolbar.
        */
        var WysiwygEditor = /** @class */ (function () {
            function WysiwygEditor() {
            }
            WysiwygEditor.prototype.bindEvents = function () {
                var _this = this;
                // Bind the toolbar button clicks.
                $(".wysiwyg-status-bold").click(function (e) {
                    _this.insertStyle(ROADKILL_EDIT_BOLD_TOKEN);
                    return false;
                });
                $(".wysiwyg-status-italic").click(function (e) {
                    _this.insertStyle(ROADKILL_EDIT_ITALIC_TOKEN);
                    return false;
                });
                $(".wysiwyg-status-underline").click(function (e) {
                    _this.insertStyle(ROADKILL_EDIT_UNDERLINE_TOKEN);
                    return false;
                });
                $(".wysiwyg-status-h2").click(function (e) {
                    _this.insertStyle(_this.repeat(ROADKILL_EDIT_HEADING_TOKEN, 2));
                    return false;
                });
                $(".wysiwyg-status-h3").click(function (e) {
                    _this.insertStyle(_this.repeat(ROADKILL_EDIT_HEADING_TOKEN, 3));
                    return false;
                });
                $(".wysiwyg-status-h4").click(function (e) {
                    _this.insertStyle(_this.repeat(ROADKILL_EDIT_HEADING_TOKEN, 4));
                    return false;
                });
                $(".wysiwyg-status-h5").click(function (e) {
                    _this.insertStyle(_this.repeat(ROADKILL_EDIT_HEADING_TOKEN, 5));
                    return false;
                });
                $(".wysiwyg-status-bullets").click(function (e) {
                    _this.insertListItem(ROADKILL_EDIT_BULLETLIST_TOKEN);
                    return false;
                });
                $(".wysiwyg-numbers").click(function (e) {
                    // Obselete
                    _this.insertListItem(ROADKILL_EDIT_NUMBERLIST_TOKEN);
                    return false;
                });
                $(".wysiwyg-status-picture").click(function (e) {
                    Web.Dialogs.openImageChooserModal("<iframe src='" + ROADKILL_FILESELECTURL + "' id='filechooser-iframe'></iframe>");
                    return false;
                });
                $(".wysiwyg-status-link").click(function (e) {
                    _this.insertLink();
                    return false;
                });
                $(".wysiwyg-status-help").click(function (e) {
                    Web.Dialogs.openMarkupHelpModal("<iframe src='" + ROADKILL_WIKIMARKUPHELP + "' id='help-iframe'></iframe>");
                    return false;
                });
            };
            /**
            Adds bold,italic and underline at the current selection point, e.g. **|**
            */
            WysiwygEditor.prototype.insertStyle = function (styleCode) {
                var range = $("#UpdateSearchTxt").getSelection();
                var length = styleCode.length;
                if (range !== null) {
                    var editorText = $("#UpdateSearchTxt").val();
                    // Put the caret in the middle of the style, if the current selection isn't the style being added
                    if (editorText.substr(range.start - length, length) !== styleCode && range.text.substr(0, length) !== styleCode) {
                        $("#UpdateSearchTxt").replaceSelection(styleCode + range.text + styleCode);
                        $("#UpdateSearchTxt").setSelection(range.end + length, range.end + length);
                    }
                    else {
                        $("#UpdateSearchTxt").setSelection(range.end, range.end);
                    }
                }
            };
            /**
            Adds a hyperlink tag to the current caret location.
            */
            WysiwygEditor.prototype.insertLink = function () {
                var range = $("#UpdateSearchTxt").getSelection();
                if (range !== null) {
                    var text = range.text;
                    if (range.text === "")
                        text = ROADKILL_EDIT_LINK_TEXTPLACEHOLDER;
                    var prefix = ROADKILL_EDIT_LINK_STARTTOKEN.toString();
                    prefix = prefix.replace("%URL%", ROADKILL_EDIT_LINK_URLPLACEHOLDER);
                    prefix = prefix.replace("%LINKTEXT%", text);
                    var suffix = ROADKILL_EDIT_LINK_ENDTOKEN.toString();
                    suffix = suffix.replace("%URL%", ROADKILL_EDIT_LINK_URLPLACEHOLDER);
                    suffix = suffix.replace("%LINKTEXT%", text);
                    $("#UpdateSearchTxt").replaceSelection(prefix + suffix);
                    $("#UpdateSearchTxt").setSelection(range.start + prefix.length, range.start + prefix.length);
                }
            };
            /**
            Adds a bullet or numbered list item onto the next line after the current caret location.
            */
            WysiwygEditor.prototype.insertListItem = function (styleCode) {
                var range = $("#UpdateSearchTxt").getSelection();
                if (range !== null) {
                    var val = $("#UpdateSearchTxt").val();
                    var start = range.start;
                    if (start > 0)
                        start -= 1;
                    var lastChar = val.substr(start, 1);
                    var nextChar = val.substr(range.start, 1);
                    if (nextChar === styleCode) {
                        $("#UpdateSearchTxt").setSelection(range.end + 2, range.end + 2);
                        return;
                    }
                    if (lastChar == "\n" || lastChar == "") {
                        $("#UpdateSearchTxt").replaceSelection(range.text + styleCode + " ");
                        $("#UpdateSearchTxt").setSelection(range.end + 2, range.end + 2);
                    }
                    else {
                        $("#UpdateSearchTxt").replaceSelection(range.text + "\n" + styleCode + " ");
                        $("#UpdateSearchTxt").setSelection(range.end + 3, range.end + 3);
                    }
                }
            };
            WysiwygEditor.prototype.repeat = function (text, count) {
                return new Array(count + 1).join(text);
            };
            /**
            Adds an image tag to the current caret location.
            */
            WysiwygEditor.addImage = function (image) {
                var range = $("#UpdateSearchTxt").getSelection();
                if (range !== null) {
                    var text = range.text;
                    if (range.text === "")
                        text = ROADKILL_EDIT_IMAGE_TITLE;
                    var prefix = ROADKILL_EDIT_IMAGE_STARTTOKEN.toString();
                    prefix = prefix.replace("%ALT%", text);
                    prefix = prefix.replace("%FILENAME%", image);
                    var suffix = ROADKILL_EDIT_IMAGE_ENDTOKEN.toString();
                    suffix = suffix.replace("%ALT%", text);
                    suffix = suffix.replace("%FILENAME%", image);
                    $("#UpdateSearchTxt").replaceSelection(prefix + suffix);
                    $("#UpdateSearchTxt").setSelection(range.start + prefix.length, range.start + prefix.length);
                    Web.Dialogs.closeImageChooserModal();
                    Web.EditPage.updatePreviewPane();
                }
            };
            return WysiwygEditor;
        }());
        Web.WysiwygEditor = WysiwygEditor;
    })(Web = Roadkill.Web || (Roadkill.Web = {}));
})(Roadkill || (Roadkill = {}));
//# sourceMappingURL=wysiwygeditor.js.map