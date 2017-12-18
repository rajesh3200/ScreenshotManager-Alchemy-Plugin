!(function () {
    try {
        // Get the JQuery object.
        $JQ = Alchemy.library("jQuery");
        var httpProtocol = window.location.protocol;
        var hostName = window.location.hostname;

        // function to enable the screenshot checkbox onload		
        function CheckStatus() {
            try {
                var page = $display.getItem();
                var pgid = page.getId();
                var url = httpProtocol + "//" + hostName + "/Alchemy/Plugins/ScreenShotManager/api/Service/getscreenshots";
                $JQ.ajax({
                    type: "GET",
                    url: url,
                    cache: false,
                    success: function (data) {
                        $JQ(data).each(function (i, val) {
                            if (val['pageid'] == pgid && val['enabled'] === true) {
                                $JQ("#chkPageTemplateEditCheck").prop('checked', true);
                            }
                        });
                    },
                    error: function () {
                        console.log("error!");
                    }
                });
            }
            catch (error) {
                $messages.registerError("Alchemy Plugin Error | EditTemplatesFromPage: ", error);
            }
        }

        // Event handler function when view is loaded.
        function onViewLoaded() {
            try {

                // Remove the event handler once the function is excuted.
                $evt.removeEventHandler($display, "start", onViewLoaded);
                // Get the page object
                var page = $display.getItem();

                // Insert the Edit button  besides the page template dropdown
                // $JQ("<div id='AlchemyPluginsEditTemplatesFromPagePageEditButton' class='editTemplateFromPagePluginButton editButton pageTemplate' title='Edit'><span class='text'>&nbsp;</span></div>").insertAfter('#PageTemplate')

                $JQ("<div class='field'><label></label><div id='PageTemplateEditCheck'><input type='checkbox' id='chkPageTemplateEditCheck' name='checkeditTemaplateFromPagePlugin'><label for='checkbox'>&nbsp;Make Screenshot Backup while publishing</label></div></div>").appendTo('#StandardFields > fieldset');

                CheckStatus();

                // Function to be execte on the Edit Button click
                $JQ("#chkPageTemplateEditCheck").click(function () {
                    // Execute the open command
                    var pageTemplateId = page.getPageTemplateId();
                    var s = new Tridion.Cme.Selection();
                    s.addItem(pageTemplateId);
                    debugger;
                    var pageId = page.getId();
                    var enableCheckbox = $JQ(this).is(":checked");
                    if (enableCheckbox === true) {
                        //url: "http://tridion.sdldemo.com/Alchemy/Plugins/ScreenShotManager/api/Service/enablescreenshot"
                        var url = httpProtocol + "//" + hostName + "/Alchemy/Plugins/ScreenShotManager/api/Service/enablescreenshot"
                        $JQ.ajax({
                            type: "POST",
                            url: url,
                            data: {
                                "pageid": pageId,
                                "enabled": enableCheckbox,
                            },
                            cache: false,
                            success: function (response) {
                                console.log("Success!");
                            },
                            error: function (data) {
                                console.log('Error occurs!');
                            }
                        });
                    }
                    else
                    {
                        var clearUrl = httpProtocol + "//" + hostName + "/Alchemy/Plugins/ScreenShotManager/api/Service/clearscreenshot"
                        $JQ.ajax({
                            type: "POST",
                            url: clearUrl,
                            data: "="+pageId,
							dataType: 'json',
                            contentType: 'application/x-www-form-urlencoded',
                            cache: false,
                            success: function (response) {
                                console.log("Success!");
                            },
                            error: function (data) {
                                console.log('Error occurs!');
                            }
                        });
                    }
                });

            }
            catch (error) {
                $messages.registerError("Alchemy Plugin Error | EditTemplatesFromPage: ", error);
            }
        }

        // Add a event handler when Dashboard display is started.
        $evt.addEventHandler($display, "start", onViewLoaded);
    }
    catch (error) {
        $messages.registerError("Alchemy Plugin Error | EditTemplatesFromPage: ", error);
    }

})();