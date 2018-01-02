$(document).ready(function () {
    $("form").bind("invalid-form.validate", function (form) {
        $('.bmvc-3-validation-summary').has('li').addClass('alert alert-danger').find('.close').show();
    });

    if ($.validator && $('form').length > 0) {
        $('form').each(function () {
            $(this).validate().settings.highlight = function (element) {
                $(element).closest('.control-group').addClass('error');
                $(element).closest('.form-group').addClass('has-error');
            };
        });
        $('form').each(function () {
            $(this).validate().settings.unhighlight = function (element) {
                $(element).closest('.control-group').removeClass('error');
                $(element).closest('.form-group').removeClass('has-error');
            };
        });
    }

    $('[rel=tooltip]').tooltip({ container: 'body' });
    $('[rel=popover]').popover();

    $('.nav-tabs li.disabled a').each(function (index, elem) {
        $(this).off('click').on('click', function (e) {
            e.preventDefault();
            return false;
        });
    });

    $('.bmvc-btn-toggle').button('toggle');
    $('.btn').button();

    $(function () {
        if ($('[data-bmvc-confirm]').length > 0) {
            if (typeof BootstrapDialog != 'undefined') {
                var confirmModal;
                BootstrapDialog.bmvcConfirm = function (message, title, callback, onshow, onshown, onhide, onhidden) {
                    confirmModal = new BootstrapDialog({
                        title: title,
                        message: message,
                        onshow: function (dialogRef) {
                            if (onshow)
                                window[onshow](dialogRef);
                        },
                        onshown: function (dialogRef) {
                            if (onshown)
                                window[onshown](dialogRef);
                        },
                        onhide: function (dialogRef) {
                            if (onhide)
                                window[onhide](dialogRef);
                        },
                        onhidden: function (dialogRef) {
                            if (onhidden)
                                window[onhidden](dialogRef);
                        },
                        closable: false,
                        data: {
                            'callback': callback
                        },
                        buttons: [{
                            label: 'Cancel',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }, {
                            label: 'OK',
                            cssClass: 'btn-primary',
                            action: function (dialog) {
                                typeof dialog.getData('callback') === 'function' && dialog.getData('callback')(true);
                            }
                        }]
                    }).open();
                };

                $(document).on('click', 'a[data-bmvc-confirm]', function (e) {
                    e.preventDefault();
                    var self = $(this);
                    var title = self.attr('data-bmvc-confirm-title');
                    if (title === undefined) {
                        title = 'Confirmation';
                    }
                    var text = self.attr('data-bmvc-confirm-text');
                    var href = self.attr('href');

                    var onshow = self.attr('data-onshow');
                    var onshown = self.attr('data-onshown');
                    var onhide = self.attr('data-onhide');
                    var onhidden = self.attr('data-onhidden');

                    BootstrapDialog.bmvcConfirm(text, title, function () {
                        window.location = href;
                    }, onshow, onshown, onhide, onhidden);
                });

                function confirmButtonHandler(e) {
                    e.preventDefault();
                    e.stopImmediatePropagation();

                    var self = $(this);
                    var title = self.attr('data-bmvc-confirm-title');
                    if (title === undefined) {
                        title = 'Confirmation';
                    }
                    var text = self.attr('data-bmvc-confirm-text');

                    var onshow = self.attr('data-onshow');
                    var onshown = self.attr('data-onshown');
                    var onhide = self.attr('data-onhide');
                    var onhidden = self.attr('data-onhidden');

                    BootstrapDialog.bmvcConfirm(text, title, function () {
                        self.unbind('click', confirmButtonHandler).click().bind('click', confirmButtonHandler);
                        confirmModal.close();
                    }, onshow, onshown, onhide, onhidden);
                };

                function confirmFormHandler(e) {
                    e.preventDefault();
                    e.stopImmediatePropagation();

                    var self = $(this);
                    var title = self.attr('data-bmvc-confirm-title');
                    if (title === undefined) {
                        title = 'Confirmation';
                    }
                    var text = self.attr('data-bmvc-confirm-text');

                    var onshow = self.attr('data-onshow');
                    var onshown = self.attr('data-onshown');
                    var onhide = self.attr('data-onhide');
                    var onhidden = self.attr('data-onhidden');

                    BootstrapDialog.bmvcConfirm(text, title, function () {
                        self.unbind('submit', confirmFormHandler).submit().bind('submit', confirmFormHandler);
                        confirmModal.close();
                    }, onshow, onshown, onhide, onhidden);
                };

                $('form[data-bmvc-confirm]').bind('submit', confirmFormHandler);
                $('button[data-bmvc-confirm], input[type=button][data-bmvc-confirm]').bind('click', confirmButtonHandler);
            } else {
                alert('You need http://nakupanda.github.io/bootstrap3-dialog/ for the confirm to work');
            }
        }

        $('[data-provide=typeahead]').each(function () {
            var self = $(this);

            var items = self.attr('data-items');
            var minLength = self.attr('data-minlength');
            var matcher = self.attr('data-matcher');
            var sorter = self.attr('data-sorter');
            var updater = self.attr('data-updater');
            var highlighter = self.attr('data-highlighter');

            var typeaheadOptions = {
                source: function (term, process) {
                    var url = self.data('url');

                    return $.getJSON(url, { term: term }, function (data) {
                        return process(data);
                    });
                }
            };

            if (items) typeaheadOptions['items'] = items;
            if (minLength) typeaheadOptions['minLength'] = minLength;
            if (matcher) typeaheadOptions['matcher'] = function (item) { window[matcher](item); };
            if (sorter) typeaheadOptions['sorter'] = function (items) { window[sorter](items); };
            if (updater) typeaheadOptions['updater'] = function (item) { window[updater](item); };
            if (highlighter) typeaheadOptions['highlighter'] = function (item) { window[highlighter](item); };

            self.typeahead(typeaheadOptions);
        });

        $('[data-disabled-depends-on]').each(function () {
            var self = $(this);
            var name = self.data('disabled-depends-on');
            var val = self.data('disabled-depends-val');
            var selector = '[name="' + name + '"]';

            $(document).on('change', selector + ':checkbox', function () {
                self.prop('disabled', $(this).prop('checked') == val);
            });

            $(document).on('change', selector + ':not(:checkbox):not(:hidden)', function () {
                self.prop('disabled', $(this).val().toString() == val.toString());
            });
            if ($(selector).is(':radio')) {
                $(selector + ':checked').change();
            } else {
                $(selector).change();
            }
        });

        var isFirstRun = true;
        $('[data-visible-depends-on]').each(function () {
            var self = $(this);
            var name = self.data('visible-depends-on');
            var val = self.data('visible-depends-val');
            var speed = self.data('visible-depends-speed');
            var selector = '[name="' + name + '"]';
            var selfName = '';
            var toHide;
            var isDiv = false;

            if (self.is('div')) {
                isDiv = true;
                toHide = self;
                selfName = self.find('input').attr('name');
            } else {
                selfName = self.attr('name');
            }

            var formGroup = self.closest('.form-group, .controls');
            if (formGroup.length > 0 && formGroup.find('input:not([type=hidden]):not([name="' + selfName + '"]),select:not([name="' + selfName + '"]),textarea:not([name="' + selfName + '"])').length === 0) {
                toHide = formGroup;
            } else if (!isDiv) {
                toHide = $('[name="' + selfName + '"],label[for="' + selfName + '"]');
            }

            $(document).on('change', selector + ':checkbox', function () {
                if ($(selector + ':not([type=hidden])').length > 1) {
                    // handle multi-checkboxes
                    var vals = [];
                    $(selector + ':checked').each(function () {
                        vals.push(this.value);
                    });
                    if ($.inArray(val.toString(), vals) >= 0) {
                        showElem();
                    } else {
                        hideElem();
                    }
                } else {
                    // handle single checkboxes
                    if ($(this).prop('checked') == val) {
                        showElem();
                    } else {
                        hideElem();
                    }
                }
            });

            $(document).on('change', selector + ':not(:checkbox):not([type=hidden])', function () {
                var inputVal = $(this).val().toString();
                if (val.toString().indexOf('|') > -1) {
                    if ($.inArray(inputVal, val.split('|')) >= 0) {
                        showElem();
                    } else {
                        hideElem();
                    }
                }
                else if (inputVal == val.toString()) {
                    showElem();
                } else {
                    hideElem();
                }
            });

            if ($(selector).is(':radio')) {
                var el = $(selector + ':checked');
                if (el.length > 0)
                    el.change();
                else
                    hideElem();
            } else {
                $(selector).change();
            }
            isFirstRun = false;

            function hideElem() {
                toHide.hide(isFirstRun ? undefined : speed);
            }

            function showElem() {
                toHide.show(isFirstRun ? undefined : speed);
            }
        });
    });
});