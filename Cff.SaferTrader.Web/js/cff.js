
function expandCollapseCustomerInfomation() {
    $("#ageing > dt").unbind();
    $(".customerInformationPanel > dt").unbind();
    $(".clientInformationPanel > dt").unbind();
    $("#ageing > dt").click(function() {
        expandCollapse($(this).parent());
    });
    $(".customerInformationPanel > dt").click(function() {
        expandCollapse($(this).parent());
    });
    $(".clientInformationPanel > dt").click(function() {
        expandCollapse($(this).parent());
    });
}

function toggleReportNavigation()
{
    $("#rhToggle").unbind();
    $("#rhToggle").click(function() {$("body").toggleClass("rightSideHidden");});
    $("#lhToggle").unbind();
    $("#lhToggle").click(function() { $("body").toggleClass("leftSideHidden"); });
}

function trimWords(value)
{
    if (value === null)
        return [''];

    if (value ==='')
        return [''];

    if (!value) 
        return [''];
    
    var words = value.toString().split(", ");
    var result = [];
    $.each(words, function(i, value) {
        if ($.trim(value)) {
            result[i] = $.trim(value);
        }
    });
    return result;
}

function attachClientAutoComplete()
{
    //StartWith Function
    try {
        String.prototype.startsWith = function (str) {
            return (this.match("^" + str) === str);
        };
    } catch (ErrorX) { }

    var clientAutocompleteOptions = {
        autoFill: true,
        cacheLength: 500,
        max: 1000,
        scrollHeight: 400,
        clientAutoFillFormat: true,
        minLength: 1,
        delay: 30,
        
        formatItem: function (item) {
            var displayValue = eval("(" + item + ")");
            return displayValue.label;
        },

        formatResult: function (item) {
            if (item.toString().startsWith("<"))
            {
                window.location = relativePathToRoot + "LogOn.aspx";
            }
            else
            {
                var displayValue = eval("(" + item + ")");
                if (displayValue.label.search(/All Clients/) > 0) {
                    return "-1";
                }
                else {
                    return displayValue.label;
                }
            }
        }
    };

    var testValues2;
    if (!($("#ClientSearch").hasClass("ac_input")))
    {
        try {
          $.ajax({
            type: "POST",
            async: true,
            url: relativePathToRoot + "ClientSearch.ashx",
            data: "{}",
            dataType: "json",
            contentType: "application/json;  charset=utf-8",
            cache: false,
            context: document.body,

            success: function (data) {
                    var mx = JSON.stringify(data).replace("[", "").replace("]", "").toString();
                    testValues2 = data;
                    //var mx = JSON.stringify(data).replace("[", "").replace("]", "").toString();
                    //testValues2 = JSON.parse("[" + response.responseText + "]");
                },
             error: function (XMLHttpRequest, textStatus, errorThrown) {
                 //alert('error @ClientSearch.ashx -' + textStatus + '::' + errorThrown + ":: " + rText);
                 if (XMLHttpRequest.responseText!==null || XMLHttpRequest.responseText!==undefined) {
                     var rText = XMLHttpRequest.responseText.toString();
                     testValues2 = JSON.parse(rText);
                 }
             }
          });
        }
        catch (Error1) {
            //alert('ERROR @attaching clientseach autocomplete:: ' + Error1);
        }

        var tx = 0;
        var t1 = window.setTimeout(function () {
            while (tx<100)
                tx++;
        }, 70);

        try {
            //setup autocomplete function pulling from json[] array
            var t = window.setTimeout(function () {
                $("#ClientSearch").autocomplete({
                    source: testValues2,
                    option: clientAutocompleteOptions,

                    focus: function (event, ui) {
                        event.preventDefault();
                        $("#ClientSearch").val(ui.item.label);
                        $("#CustomerSearch").val("");
                    },

                    select: (function (event, ui) {
                        event.preventDefault();
                        $("#ClientSearch").val(ui.item.label);
                        selectClientFromAutoCompleteDropDown(event, ui.item);

                        var criteriaSelection = "";
                        if ($("#SearchCriteria").val() !== null) {
                            criteriaSelection = trimWords($("#SearchCriteria").val()).toString();
                        }
                       attachCustomerAutoComplete(criteriaSelection, null);
                    }),

                    open: function () {
                        $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                    },
                    close: function () {
                        $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                    }
                });
            }, 90);
        }
        catch (Error) {
            //alert('Error @setTimeoutCall:' + testValues2 + "::" + Error.toString());
        }
    }
}

////function customerClearInput() {    //dbb function to clear input field in customer search
    ////var attr = $("#CustomerSearch").attr('value');
    ////if (attr.length !== 0) {
    ////    $("#CustomerSearch").val("");
    ////    var clientSearchInput = trimWords($("#ClientSearch").val()).toString();
    ////    loadClientNameAndNumber(clientSearchInput);
    ////    updateClientQueryString(clientSearchInput);
    ////    generateCustomerQueryString(clientSearchInput);
    ////}
////}


function arrayUnique(array1) {
    var a = array1.concat();
    for (var i = 0; i < a.length; ++i) {
        for (var j = i + 1; j < a.length; ++j) {
            if (a[i].label.toString().toLowerCase() === a[j].label.toString().toLowerCase()) {
                a.splice(j--, 1);
            }
        }
    }
    return a;
};
  
function retrieveCriteriaSelection(criteriaSelection, qFilterString)
{
    var testVal3;
    try {
      $.ajax({
        type: "POST",
        async: false,
        url: relativePathToRoot + "CustomerSearch.ashx" + generateCustomerQueryString(criteriaSelection, qFilterString),
        data: "{}",
        dataType: "json",
        contentType: "application/json;  charset=utf-8",
        cache: false,
        context: document.body,

        success: function (data) {
            //alert('@retrievecriteriaselection-customersearch::' + data);
            testVal3 = data
            //testVal3.forEach(function (t) {
            //        console.log(t.label + " - " + t.value);
            //});
            //console.log(testVal3[1].label)
            //Object.keys(data).foreach(function (key) {
            //    console.log(data[key]);
            //});
            if (data !== null) {
                var mx = JSON.stringify(data).replace("[", "").replace("]", "").toString(); //to delay
            }
        },

        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert("ERROR @customerautocomplete ::" + textStatus + "::" + errorThrown + ":: " + XMLHttpRequest.responseText);
            testVal3 = JSON.parse(XMLHttpRequest.responseText);
        }
      });
    } catch (Error1) {
        testVal3 = testVal3 + "::" + Error1;
        //alert("ERROR@CALL TO CUSTOMERSEARCH:: " + Error1);
    }

    return testVal3;
}

function attachCustomerAutoComplete(criteriaSelection, qFilterString) {
    var customerAutocompleteOptions = {
            autoFill: true,
            cacheLength: 100,
            formatItem: function(item) {
                var displayValue = eval('(' + item + ')');
                return displayValue.label; //customerName
            },

            formatResult: function(item) {
                if (item.toString().startsWith("<")) {
                    window.location = relativePathToRoot + "LogOn.aspx";
                } else {
                    var displayValue = eval("(" + item + ")");
                    return displayValue.label;
                }
            },

            minLength: 2,
            delay: 120,
            customerAutoFillFormat: true,
            max: 1000,
            maxCacheLength: 1000,
            scrollHeight: 500
        };
    
    //finding a way to make this faster, if client have 2K+ consumer it slows down
    var testVal3;
    var testVal4;
    if ($("#customerArrayData").val() == "" || qFilterString != "" || qFilterString != null
                || qFilterString != undefined || criteriaSelection != $('#OldSearchCriteria').val())
    {
        testVal3 = retrieveCriteriaSelection(criteriaSelection, qFilterString);
        var tx = 0;
        var t1 = window.setTimeout(function () {
            while (tx < 100)
                tx++;
        }, 60); //needed to put timeout here to fix race conditions

        if (criteriaSelection != $('#OldSearchCriteria').val()) {
            $("#customerArrayData").val(""); //init
            $('#OldSearchCriteria').val(criteriaSelection);
        }
    }

    try {
        if ($("#customerArrayData").val() != "") {
            //push retrieved data here from the hidden array and compare for duplicates
            testVal4 = arrayUnique(JSON.parse($('#customerArrayData').val()).concat(testVal3));
            var mx2 = JSON.stringify(testVal4);
            $("#customerArrayData").val(mx2);
        }
        else {
            var mx1 = JSON.stringify(testVal3);
            $("#customerArrayData").val(mx1);
            testVal4 = testVal3;
        }

        var r = window.setTimeout(function () {
            if ($("#CustomerSearch").hasClass("ui-autocomplete-input")) {
                $("#CustomerSearch").autocomplete("destroy");
                $("#CustomerSearch").removeData("autocomplete");
            }
        }, 60) //needed to put timeout here to fix race conditions
        var t = window.setTimeout(function () {
            $("#CustomerSearch").autocomplete({
                source: function(req, response) {
                    var re = $.ui.autocomplete.escapeRegex(req.term);
                    //MSarza: Added to fix/enable StartWith option when typing values on the CustomerSearch textbox
                    var sc = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];   // Search criterias have different searching behavior on retrieved json values

                    //MSarza: wild card added on id field because Reports page prefixes an additional ct100 label to the Starts with input checkbox; a logical 'or' could
                    //          have been an option as well, i.e., ctl00_StartsWith OR ctl00_ctl00_StartsWith
                    if ($("input[id*='ctl00_StartsWith']").prop('checked') == true)  {
                        if (sc.indexOf(parseInt(criteriaSelection)) == -1) {
                            var matcher = new RegExp("^" + re, "i");     // original line
                            //console.log("A: " + matcher);
                            //alert("A");
                        }
                        else {
                            var ret = '(?=\\(' + re + ').[^\\(]*$';
                            var matcher = new RegExp('(?=\\(' + re + ').[^\\(]*$', 'i');
                            //console.log("B: " + matcher);
                            //alert("B");
                        }
                    }
                    else {
                        if (sc.indexOf(parseInt(criteriaSelection)) == -1) {
                            var matcher = new RegExp(re, "i");
                            //console.log("C: " + matcher);
                            //alert("C");
                        }
                        else {
                            var ret = re + "[^\\(]*$";
                            var matcher = new RegExp(ret, "i");
                            //console.log("D: " + matcher);
                            //alert("D");
                        }
                    }
                    //----
                    //alert(sc.indexOf(parseInt(criteriaSelection)) + " :|: " + sc.indexOf(criteriaSelection) + " :|: " + criteriaSelection)
                    if (re == "\\*") // add wildcard search
                        { response($.grep(testVal4, function (item) { return item.label; })); }
                    else
                        { response($.grep(testVal4, function (item) { return matcher.test(item.label); }));}


                },
                option: customerAutocompleteOptions,

                blur: function (event, ui) {
                    event.preventDefault();
                    if (ui.item != null) {
                        $("#CustomerSearch").val(ui.item.label);
                        $("#customerSelectedID").val(ui.item.value);
                    }
                },

                focus: function (event, ui) {
                    if (ui.item != null && event!=null) {
                        if (event.keyCode == 40 || event.keyCode == 38 
                                    || event.keyCode == 33 || event.keyCode == 34)
                        {
                            event.preventDefault();
                            $("#CustomerSearch").val(ui.item.label);
                            $("#customerSelectedID").val(ui.item.value);
                        }
                    }
                },

                select: (function (event, ui) {
                    //alert("Here! Val: " + ui.item.value + "Label: " + ui.item.label);
                    event.preventDefault();
                    $("#CustomerSearch").val(ui.item.label);
                    $("#customerSelectedID").val(ui.item.value);
                    selectCustomerFromAutoCompleteDropDown(event, ui.item);
                }),

                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },

                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            }); //.result(selectCustomerFromAutoCompleteDropDown);

        }, 90);  //needed to put timeout here to fix race conditions

    }
    catch (Error) {
        //alert('ERROR@attach CUSTOMERAUTOCOMPLE::' + Error);
    }

    var clientSearchInput = trimWords($("#ClientSearch").val()).toString();
    try {
        if (qFilterString != null && qFilterString != "%") {
            $("#customerSelectedID").val(qFilterString);
            $("#CustomerSearch").val(qFilterString);
            //$("#customerSelectedID").val(testVal3[0].value);
            //$("#CustomerSearch").val(testVal3[0].label);
            var t1 = window.setTimeout(function () {
                $("#CustomerSearch").autocomplete("search", qFilterString);
            }, 102);
        } else if (qFilterString == "%") {
            var t2 = window.setTimeout(function () {
                 //clear the customer selection so we go to client scope
                $("#customerSelection").val("");
                $("#CustomerSearch").val("");
                loadClientNameAndNumber(clientSearchInput);
                updateClientQueryString(clientSearchInput);
            }, 120);
        } else if (qFilterString == null && clientSearchInput != "All Clients") {
            qFilterString = "%";
            var s = generateClientQueryString(clientSearchInput);
        }
    } catch (Error2) { }
}

function attachAutocomplete() {
    if ($("#SearchCriteria").val() != null) {
        attachCustomerAutoComplete($("#SearchCriteria").val(), null);
    }
}


function loadCustomerNameAndNumber(custId) {
    try {
        var customerIdSelection = custId;
        if (customerIdSelection == null || customerIdSelection == undefined) {
            if ($.query.GET("Customer") == null || $.query.GET("Customer") == undefined)
                customerIdSelection = -1;
            else
                customerIdSelection = $.query.GET("Customer");
        }

        var clientSearchInput = trimWords($("#ClientSearch").val()).toString();
        var customerSelection = trimWords($("#customerSelection").val()).toString();
        var criteriaSelection = trimWords($("#SearchCriteria").val()).toString() + "::" + customerIdSelection;

        if ($("#CustomerSearch").val() != null) {
            var customerSearchInput = trimWords($("#CustomerSearch").val()).toString();
            customerSearchInput = customerSearchInput.replace(/\'/g, "\\'");
            var customerNameAndNumber = "{'customerNameAndNumber': '" + customerSearchInput + "'}";

            if ((customerSearchInput).toString().length == 0) { //===
                $("div#customerNoMatch").text("");
                $("div#customerNoMatch").addClass("customerSearchError_Hide");
                $("div#customerNoMatch").removeClass("customerSearchError_Show");
            }

            if ((customerSearchInput).toString().length == 0 && customerSelection != "") { //==. !==
                CallServiceMethod(relativePathToRoot + "CustomerAutoCompleteHelper.asmx/LoadCustomerNameAndNumber" + generateCustomerQueryString(criteriaSelection), customerNameAndNumber, SearchButton_Click());
                $("#customerSelection").val("");
            }
            else if ((customerSearchInput).toString().length != 0) {
                CallServiceMethod(relativePathToRoot + "CustomerAutoCompleteHelper.asmx/LoadCustomerNameAndNumber" + generateCustomerQueryString(criteriaSelection), customerNameAndNumber, displayCustomerNameAndNumber);
            }
        }

    } catch (Error) {
        //alert('ERROR@loadCustomerNameAndNumber::' + Error);
    }
}

function loadClientNameAndNumber(clientSearchInput) {
    var viewID = ($.query==null)?"":$.query.GET("ViewID");
    var clientNameAndNumber = "{'clientNameAndNumber': '" + clientSearchInput + '_' + viewID + "'}";
    CallServiceMethod(relativePathToRoot + "ClientAutoCompleteHelper.asmx/LoadClientNameAndNumber", clientNameAndNumber, displayClientNameAndNumber);
}

/*
update customer id in query string
*/
function updateCustomerQueryString(customerId) {
    if (customerId == "") {
        //if no customer, remove Customer
        $.query.REMOVE("Customer");
        location.href = $.query.toString();        
    }
    else
    {
        //else, set client and customer
        var clientSearchInput = $("#ClientSearch").val().toString(); //$("input[name='clientId']").val();
        var clientId = "";
        try {
            if (clientSearchInput != null) {
                xTest = clientSearchInput.toString().split("(");
                var re = new RegExp("\\(\\d*\\)");
                if (clientSearchInput.match(re)) {
                    clientId= re.exec(clientSearchInput);
                    var len = xTest.length;
                    if (len > 0) { len -= 1; }
                    if (len > 1) {
                        clientId = xTest[len].replace("(", "").replace(")", "");
                    } else {
                        clientId = clientId[0].replace("(", "").replace(")", "");
                    }
                } else {
                    clientId = "-1";
                }
            }
        } catch (Error) { }

        var query;
        if ($.query.GET("Client") != null) {
            $.query.REMOVE("Client"); //clear client field
        }
        else
        {
            $.query.SET("Client", clientId);
            query = $.query.toString();
        }


        if ($.query.GET("Criteria") != null) {
            $.query.SET("Criteria", $("#SearchCriteria").val());
            query = $.query.toString();
        }
        else {
            query = $.query.toString() + "&Criteria=" + $("#SearchCriteria").val();
        }
         
        if ($.query.GET("Customer") == null) {
            query = query + "&Client=" + ((clientId == null) ? "-1" : clientId.toString()) + "&Customer=" + customerId;
        }
        else {
            $.query.REMOVE("Customer");
            query = $.query.toString() + "&Client=" + ((clientId == null) ? "-1" : clientId.toString()) + "&Customer=" + customerId;
        }

        location.href = query;
    }
}

/*
udpate client id in query string
*/
function updateClientQueryString(clientSearchInput) {
    if (clientSearchInput!=null)
        location.href = generateClientQueryString(clientSearchInput);
}

function generateClientQueryString(clientSearchInput) {
    var xTest = new Array();
    try {
        if (clientSearchInput != null) {
            xTest = clientSearchInput.toString().split("(");
            var len = xTest.length;
            var re = new RegExp("\\(\\d*\\)");
            if (clientSearchInput.match(re)) {
                var m = re.exec(clientSearchInput);
                var customerId = ($.query==null)?0:$.query.GET("Customer");
                if (customerId != null) {
                    $.query.REMOVE("Customer");
                }
                if (len > 0) { len -= 1; }

                if (len > 1) {
                    m = xTest[len].replace("(", "").replace(")", "");
                    $.query.SET("Client", m);
                } else {
                    $.query.SET("Client", m[0].replace("(", "").replace(")", ""));
                }
            } else {
                $.query.SET("Client", "-1");
            }

            if ($.query.GET("Criteria")!=null) {
                $.query.SET("Criteria", $("#SearchCriteria").val());
            }

            return $.query.toString();
        }
        //alert('@generateClientQueryString::' + $.query.toString());
    } catch (Error) {
        //alert('ERROR @@generateClientQueryString::' + Error);
    }
}

function getQueryStrings() {
    var assoc = {};
    var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = location.search.substring(1);
    var keyValues = queryString.split('&');
    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }
    return assoc;
}

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?|&])" + key + "=.*?(&|$)", "i");
    var separator = uri.toString().indexOf('?') !== -1 ? "&" : "?";
    if (uri.toString().match(re)) {
        return uri.toString().replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

function generateCustomerQueryString(criteriaSelection, qFilterString)
{
    try {
        var customerId = $("#customerSelectedID").val();
        if (customerId = null || customerId == undefined)
            customerId = ("#CustomerSearch").val();

        var criteriaIndex = $("#SearchCriteria").val();
        var clientSearchInput = trimWords($("#ClientSearch").val());

        var queryUrl = $.query.toString();
        if (queryUrl.match('clienthit')) {
            clientSearchInput = queryUrl.substring(queryUrl.indexOf("Client")+7);
            var eidx = clientSearchInput.indexOf("&");
            if (eidx > 0)
                clientSearchInput = 'm(' + clientSearchInput.substring(0, eidx) + ')';
        }
        
        var xTest = new Array();
        xTest = (criteriaSelection == null || criteriaSelection == undefined) ? $("#SearchCriteria").val() : criteriaSelection.toString().split("::");
        if (xTest.length > 1) {
            customerId = xTest[1];
        }
        criteriaSelection = criteriaIndex;
            
        var re = new RegExp("\\(\\d*\\)");
        customerId = re.exec(customerId.toString());

        if (criteriaSelection == null || criteriaSelection == undefined) criteriaSelection = 0;

        var m = "";
        var xTest2 = new Array();
        try {
            if (clientSearchInput != null) {
                xTest2 = clientSearchInput.toString().split("(");
                var len = xTest2.length;
                if (len > 0) { len -= 1; }
                if (len > 1) { //m = re.exec(xTest2[len]);
                    m = xTest2[len].replace(")", "");
                } else {
                    m = xTest2[1].replace(")", "");
                }
            }
            else if (clientSearchInput == "All Clients") {
                m = "-1";
            }
        } catch (Error) {
            //alert('ERROR@generatecustoquerystring::' + Error);
        }

        var mlen = (m != null || m!=undefined)?m.toString().length:0;
        if (mlen > 0) {
            $.query.SET("Client", m);
        } else {
            $.query.SET("Client", "-1");
        }
        //alert('@generateCustomerQueryString::' + m + '::' +  xTest2.length.toString() +  '::' + clientSearchInput + ':: [' + $.query.toString() + ']');

        var queryString = $.query.toString();
       if (customerId != null || customerId != undefined || customerId > 0) {
            if ($.query.GET("Customer") == null && customerId !== null) {
                    if (customerId.toString().length > 0) {
                        //TODO:: check if customerid exists in sessionwrapper - do not overwrite custid with custnum
                        queryString += "&Customer=" + customerId.toString();
                    }
            }
            else if ($.query.GET("Customer") != null && customerId !== null) {
                    if (customerId.toString().length > 0) {
                        ////TODO:: check if customerid exists in sessionwrapper - do not overwrite custid with custnum
                        $.query.SET("Customer", customerId.toString());
                        queryString = $.query.toString();
                    }
            }

            if (criteriaIndex != null) {
               if ($.query.get("Criteria") == null)
                        queryString += '&Criteria=' + criteriaIndex.toString();
               else {
                    $.query.SET("Criteria", criteriaIndex.toString());
                    queryString = $.query.toString();
               }
           }
       }

       try
       {
           $.query.REMOVE("q"); //just clear the qfilterstring
           if (qFilterString != null || qFilterString != undefined) {
                queryString = queryString + '&q=' + qFilterString;
           } else {
                queryString = $.query.toString();
           }
       }
       catch (Error) {
           //alert("ERROR@generateCustomerQueryString :: " + Error + "::" + $.query);
       }

       //alert('queryString ' + queryString);

       return queryString;
    } catch (Error) {
        //alert("ERROR@generateCustomerQueryString :: " + Error.message);
    }
}

function selectCustomerFromAutoCompleteDropDown(event, item) {
    //alert("selectCustomerFromAutoCompleteDropDown! \nItem Val:  " + item.value + "; \nItem Label:\n" + item.label + "; \nEvent:\n" + event.type.toString());
    try {
        var clientSearchInput;
        var criteriaSelection;
        var displayValue;
        var customerIdJson;
        $("div#clientNoMatch").text = "";
      
        if (event == null)
        {
            criteriaSelection = $("#SearchCriteria").val();
            displayValue = trimWords(item.toString());            
        }
        else {
            //alert('@selectCustomerFromAutoCompleteDropDown::' + item.label.toString());
            criteriaSelection = $("#SearchCriteria").val();
            displayValue = item.value.toString();
        }

        customerIdJson = "{\'customerId\': \'" + displayValue + "\'}";
        criteriaSelection = criteriaSelection + "::" + displayValue;
        $("#customerSelectedID").val(displayValue);  //customerId
        //alert('selectCustomerFromAuto3:: ' + customerIdJson);

        if ($("input#customerPanelHidden").val() == undefined) {
            $("body").removeClass("rightSideHidden rhToggleHidden");
        } else {
            $("body").addClass("rightSideHidden rhToggleHidden");
        }

       
        $.ajax({
            type: "POST",
            async: true,
            url: relativePathToRoot + "CustomerAutoCompleteHelper.asmx/GetClientIdByCustomerId",
            data: customerIdJson,
            dataType: "json",
            contentType: "application/json;  charset=utf-8",
            cache: false,
            context: document.body,

            success: function (data) {
                var mx = JSON.stringify(data).replace("[", "").replace("]", "").toString();
                SettingClientId(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert('error @CustomerAutoCompleteHelper.asmx -' + textStatus + '::' + errorThrown + ":: " + XMLHttpRequest.responseText);
                SettingClientId(JSON.parse(XMLHttpRequest.responseText));
            }
        });

    
        $.ajax({
            type: "POST",
            async: true,
            url: relativePathToRoot + "CustomerAutoCompleteHelper.asmx/SelectCustomerFromAutoCompleteDropDown" + generateCustomerQueryString(criteriaSelection),
            data: customerIdJson,
            dataType: "json",
            contentType: "application/json;  charset=utf-8",
            cache: false,
            context: document.body,

            success: function (data) {
                var mx = JSON.stringify(data).replace("[", "").replace("]", "").toString();
                FillClientDetails(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                FillClientDetails(JSON.parse(XMLHttpRequest.responseText));
            }
        });

        //CallServiceMethodSynchronous(relativePathToRoot + "CustomerAutoCompleteHelper.asmx/GetClientIdByCustomerId", customerIdJson, SettingClientId);
        //CallServiceMethod(relativePathToRoot + "CustomerAutoCompleteHelper.asmx/SelectCustomerFromAutoCompleteDropDown" + generateCustomerQueryString(criteriaSelection), customerIdJson, FillClientDetails(message));
        loadCustomerNameAndNumber(item.value);
        if ($("#SearchCriteria").val() != null) {
            if (trimWords(displayValue) != null) {
                updateCustomerQueryString(displayValue);
            }
        }

    
    } catch (Error) {
        //alert('ERROR@selectCustomerFromAutoCompleteDropDown Error:: ' + Error);    
    }
}


function selectClientFromAutoCompleteDropDown(event, item) {
    try {
        var isAttached = false;
        var viewID = ($.query==null)?"":$.query.GET("ViewID");
    
        $("#customerSelection").val("");
        $("#CustomerSearch").removeAttr("value");

        var displayValue = item.value.toString();
        var clientId = "{'clientId': '" + displayValue + '_' + viewID + "'}"; //displayValue.clientId 

        if (displayValue.clientId == "-1") {
            $("body").addClass("rightSideHidden rhToggleHidden");
        } else {
            if ($("input#customerPanelHidden").val() == undefined) {
                $("body").removeClass("rightSideHidden rhToggleHidden");
            }
        }

        CallServiceMethod(relativePathToRoot + "ClientAutoCompleteHelper.asmx/SelectClientFromAutoCompleteDropDown", clientId, SearchButton_Click());

        //add some delay to let the client ui catch up
        var t = window.setTimeout(function () {
            var clientSearchInput = trimWords($("#ClientSearch").val()).toString();
            //alert('here @clientSearchInput' + clientSearchInput.toString());
            //update client details and query string
            //clear the customer selection so we go to client scope
            $("#customerSelection").val("");
            $("#CustomerSearch").val("");

            loadClientNameAndNumber(clientSearchInput);
            updateClientQueryString(clientSearchInput);
        }, 120);

    } catch (Error) {
        //alert('ERROR@selectClientFromAutoCompleteDropDown :: ' + Error);
    }
}


function addingToolTip() {
    $("tr.dxgvFooter").ready(function() {
        $("tr.dxgvFooter").attr("title", "Grand Total");
    });
}

/*set clientid into hidden variable*/
function SettingClientId(msg) {
    var clientNameId = eval('(' + msg.d + ')');
    $("input[name='clientId']").val('');
    $("input[name='clientId']").val(clientNameId);
    //alert('@SettingClientId' + $("input[name='clientId']").val());
}

function SearchButton_Click() {
    $("#SearchButton").focus();
    $("#SearchButton").click();
}

function expandCollapse(control) {
    if (control.hasClass('collapsed')) {
        control.removeClass('collapsed').addClass('expanded');
        control.children('dd').fadeIn(900);
    }
    else {
        control.children('dd').fadeOut(450, function() {
            control.removeClass('expanded').addClass('collapsed');
        });
    }
}

function selectParentTab(node) {
    var parentTab = $(node);
    while (!parentTab.parent().hasClass("sf-menu")) {
        parentTab = parentTab.parent();
    }
    parentTab.addClass("current");
    parentTab.siblings().removeClass("current");
}

function attachTabClickEvents() {
    $(".sf-menu li a").click(function() {
        if ($(this).attr('href') != '') {
            selectParentTab($(this));
        }
    });
}

function getFileName(path) {
    if (path != null) {
        var end = (path.indexOf("#") == -1) ? path.length : path.indexOf("#"); //===
        end = (path.indexOf("?") == -1) ? end : path.indexOf("?"); //===
        return path.substring(path.lastIndexOf("/") + 1, end);
    }
    return null;
}

function s4() {
    return Math.floor((1 + Math.random()) * 0x10000)
             .toString(16)
             .substring(1);
}

function randomKey() {
    return s4() + s4() + s4() +  s4();
}


function selectMenuItem() {
    var fileName = getFileName(document.location.href);
    var node = $(".sf-menu li a[href*='" + fileName + "']");

    if (fileName != '' && node.attr('href')) { //!==
        selectParentTab(node);
    }
    else if (fileName == '') { //===
        selectParentTab($(".sf-menu li:first a"));
    }
}

function SetAllClientToSearch() {
    if ($("#ClientSearch").attr("value") == "") { //===
        $("#ClientSearch").val("All Clients");
    }
    if ($("#ClientSearch").attr("value") == "All Clients") { //===
        $("div#clientNoMatch").text("");
        $("div#clientNoMatch").addClass("clientSearchError_Hide");
        $("div#clientNoMatch").removeClass("clientSearchError_Show");
    }
}

function toggleContactDetails() {
    $("div#contactDetails").slideToggle(SetContactDisplayPreference);
}

function SetContactDisplayPreference() {
    var isContactDetailsShown = "";
    //alert($("div#contactDetails").css("display"));

    if ($("div#contactDetails").css("display") == "block") { //===
        $("a#showContactDetails").html("Hide Contact Details");
        isContactDetailsShown = "{'isContactDetailsShown': '" + true + "'}";
        try {
            var t = window.setTimeout(function () {
                CallServiceMethod(relativePathToRoot + "UserPreferenceSetter.asmx/SetContactDetailsPreference", isContactDetailsShown);
            }, 90);
        } catch (Error) {}
    } else {
        $("a#showContactDetails").html("Show Contact Details");
        isContactDetailsShown = "{'isContactDetailsShown': '" + false + "'}";
        try {
            var t = window.setTimeout(function () {
                CallServiceMethod(relativePathToRoot + "UserPreferenceSetter.asmx/SetContactDetailsPreference", isContactDetailsShown);
            }, 90);
        } catch (Error) { }
    }
}

function CallServiceMethod(url, data, successFunction, failureFunction) {
    try {
        var t = window.setTimeout(function () {
            $.ajax({
                type: "POST",
                async: true,
                url: url,
                data: data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: successFunction,
                error: failureFunction
            });
        }, 60);
    }
    catch (Error) {
        alert('@@CallServiceMetthod:: ' + Error);
    }
}


function CallServiceMethodSynchronous(url, data, successFunction) {
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunction,
        error: failureFunction,
        async: false
    });
}

function failureFunction(XMLHttpRequest) {
    if (XMLHttpRequest.status == 401) {
        window.location = relativePathToRoot + "LogOn.aspx";
    }
}

function FillClientDetails(message)
{
    if (message.d == undefined || message.d != null) {
        if (message.d.toString() == '') {
            $("div#customerNoMatch").text("Invalid customer");
            $("div#customerNoMatch").removeClass("customerSearchError_Hide");
            $("div#customerNoMatch").addClass("customerSearchError_Show");
        }
        else {
            var clientNameId = eval('(' + message.d + ')');
            var clientName = $("#ClientSearch").val();
            if (clientName !== clientNameId) {
                $("#ClientSearch").val(clientNameId['name'] + "(" + clientNameId['id'] + ")");
            }

            $("#customerSelection").val("CustomerSelected");

            $("div#clientNoMatch").addClass("clientSearchError_Hide");
            $("div#clientNoMatch").removeClass("clientSearchError_Show");
            $(".SearchButton").click();
        }
    }
}

function displayClientNameAndNumber(message) {
    //alert("@displayClientNameAndNumber:: " + message);
    if (message.d == "") {
        $("#ClientSearch").val("");
    } else {
        var client = eval('(' + message.d + ')');
        if (client['nameAndNumber'] !== "") {
            $("#ClientSearch").val(client['nameAndNumber']);
            $("#customerSelection").val("");
        }
    }
    $("div#clientNoMatch").addClass("clientSearchError_Hide");
    $("div#clientNoMatch").removeClass("clientSearchError_Show");
}

function displayCustomerNameAndNumber(message) {
    //alert("@displayCustomerNameAndNumber:: " + message);

    if (message.d == "") {
        $("#CustomerSearch").val("");
    } else {
        var customer = eval('(' + message.d + ' )');
        if (customer['nameAndNumber'] != "") {
            $("#CustomerSearch").val(customer['nameAndNumber']);
        }
        
    }
    $("div#customerNoMatch").addClass("customerSearchError_Hide");
    $("div#customerNoMatch").removeClass("customerSearchError_Show");
}

function callValidationService(method, data, successFunction) {
    $.ajax({
        type: "POST",
        url: relativePathToRoot + "ValidationService.asmx/" + method,
        data: data,
        processData: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunction,
        error: failureFunction
    });
}

function callPageMethod(page, method, data) {
    $.ajax({
        type: "POST",
        url: relativePathToRoot + page + "/" + method,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: failureFunction
    });
}

function toggleGrid(toggleButtonId, gridId) {
    var toggleButton = $("#" + toggleButtonId);
    var command = toggleButton.children().attr('alt');

    if (command == 'collapse') {
        //$("#" + gridId).fadeOut(450);
        $("#" + gridId).slideToggle(gridId);  // dbb
        toggleButton.children().attr('alt', 'expand').attr('src', 'images/expand.png');
    }
    else {
        //$("#" + gridId).fadeIn(450);
        $("#" + gridId).slideToggle(gridId);  // dbb
        toggleButton.children().attr('alt', 'collapse').attr('src', 'images/collapse.png');
    }
}

function toggleHelp(toggleButton) {
    if ($(".description").css('display') == 'none') {
        $(".description").fadeIn(450);
    }
    else {
        $(".description").fadeOut(450);
    }
}

function startAnimate() {
    $(".updateButton").attr("src", relativePathToRoot + "images/btn_update_loading.gif");
    setTimeout("stopAnimate();", 450);
}

function stopAnimate() {
    $(".updateButton").attr("src", relativePathToRoot + "images/btn_update.gif");
}

function startSearchButtonAnimate() {
    $(".searchButton").attr("src", relativePathToRoot + "images/btn_search_loading.gif");
    setTimeout("stopSearchButtonAnimate();", 450);
}

function stopSearchButtonAnimate() {
    $(".searchButton").attr("src", relativePathToRoot + "images/btn_search_blue.png");
}


function attachDatePickers() {
    var options = {
        dateFormat: 'dd/mm/yy',
        showOn: 'both',
        buttonImage: relativePathToRoot + 'images/calendar.png',
        buttonImageOnly: true,
        buttonText: "Click to select a date",
        goToCurrent: true,
        clickInput: true,
        yearRange: "-30:+0",
        prevText: "",
        nextText: "",
        changeMonth: true,
        changeYear: true,
        showMonthAfterYear: true
    };

    $(".dateRange").datepicker(options);
    
    var fromdateRangeOptions = {
        dateFormat: 'dd/mm/yy',
        showOn: 'both',
        buttonImage: relativePathToRoot + 'images/calendar.png',
        buttonImageOnly: true,
        buttonText: "Click to select a date",
        goToCurrent: true,
        clickInput: true,
        yearRange: "-30:+0",
        maxDate: "+0M +0D",
        prevText: "",
        nextText: "",
        changeMonth: true,
        changeYear: true,
        showMonthAfterYear: true
    };

    var todateRangeOptions = {
        dateFormat: 'dd/mm/yy',
        showOn: 'both',
        buttonImage: relativePathToRoot + 'images/calendar.png',
        buttonImageOnly: true,
        buttonText: "Click to select a date",
        goToCurrent: true,
        clickInput: true,
        yearRange: "-30:+0",
        maxDate: "+0M +0D",
        prevText: "",
        nextText: "",
        changeMonth: true,
        changeYear: true,
        showMonthAfterYear: true,
        hideIfNoPrevNext: true
    };

    var smsDateRangeOptions = {
        dateFormat: 'dd/M/yy',
        showOn: 'both',
        buttonImage: relativePathToRoot + 'images/calendar.png',
        buttonImageOnly: true,
        buttonText: "Click to select a date",
        goToCurrent: true,
        clickInput: true,
        maxDate: "+12m",
        changeMonth: true,
        changeYear: true,
        showMonthAfterYear: true,
        selectOtherMonths: true,
        showAnim: "slideDown",
        hideIfNoPrevNext: false
    };

    $(".fromDateRange").datepicker(fromdateRangeOptions);
    $(".toDateRange").datepicker(todateRangeOptions);
    $(".smsToDateRange").datepicker(smsDateRangeOptions);
}

function enableDisableControl(control, enable) {
    if (enable)
        control.removeAttr("disabled");
    else
        control.attr("disabled", "disabled");
}

function attachCountDown() {
    var countdown = {
        init: function() {
            countdown.remaining = countdown.max - $(countdown.obj).val().toString().length;
            if (countdown.remaining < 0) {
                $(countdown.obj).val($(countdown.obj).val().substring(0, countdown.max));
                countdown.remaining = 0;
            }
            else if (countdown.remaining > countdown.max) {
                $(countdown.obj).val($(countdown.obj).val().substring(0, countdown.max));
                alert('Characters type is more than the maximum required');
            }
            if (countdown.remaining !== countdown.max) {
                $(".remaining").html(countdown.remaining + " characters remaining");
            }
        },
        //max: null,
        max: 0,
        //remaining: null,
        remaining: 0,
        //obj: null
        obj: 0
    };

    $(".countdown").each(
        function() {
           $(this).focus(
             function() {
                 var c = $(this).attr("class");
                 if ((parseInt(c.match(/limit_[0-9]{1,3}/)[0].match(/[0-9]{1,3}/)[0])) == undefined && (parseInt(c.match(/limit_[0-9]{1,3}/)[0].match(/[0-9]{1,3}/)[0])) == null) {
                     countdown.max = 0;
                 } else {
                     countdown.max = parseInt(c.match(/limit_[0-9]{1,3}/)[0].match(/[0-9]{1,3}/)[0]); // updated by dbb 31/05/2016
                 }
                 //countdown.max = parseInt(c.match(/limit_[0-9]{1,3}/)[0].match(/[0-9]{1,3}/)[0]);  // disabled dbb 17/06/2016
                 countdown.obj = this;
                 iCount = setInterval(countdown.init, 200);
              }
            ).blur(
                function() {
                    countdown.init();
                    clearInterval(iCount);
                }
             );
        } 
    );
}

function attachSmsCountDown() {         // dbb
    var countdown = {
        init: function () {
            countdown.remaining = countdown.max - $(countdown.obj).val().toString().length;
            if (countdown.remaining < 0) {
                $(countdown.obj).val($(countdown.obj).val().substring(0, countdown.max));
                countdown.remaining = 0;
            }
            else if (countdown.remaining > countdown.max) {
                $(countdown.obj).val($(countdown.obj).val().substring(0, countdown.max));
            }
            if (countdown.remaining !== countdown.max) {
                $(".remaining").html(countdown.remaining + " characters remaining");
            }
        },
        max: null,
        remaining: null,
        obj: null
    };

    $(".countdown").each(
        function () {
            $(this).focus(
              function () {
                  var c = $(this).attr("class");
                  var mx = 0;
                  mx = parseInt(c.match(/limit_[0-9]{1,3}/)[0].match(/[0-9]{1,3}/)[0]) == null ? 0 : parseInt(c.match(/limit_[0-9]{1,3}/)[0].match(/[0-9]{1,3}/)[0]); // updated by dbb 31/05/2016
                  countdown.max = mx;  //parseInt(c.match(/limit_[0-9]{1,3}/)[0].match(/[0-9]{1,3}/)[0]); updated by dbb 31/05/2016
                  countdown.obj = this;
                  iCount = setInterval(countdown.init, 200);
              }
             ).blur(
                 function () {
                     countdown.init();
                     clearInterval(iCount);
                 }
              );
        }
    );
}



function toggleBatchGridView() {
    var toggleButton = $("#batchGridViewToggle");
    var command = toggleButton.children().attr('alt');

    if (command == 'collapse') {
        hideBatchGridView();
    }
    else {
        showBatchGridView();
    }
}

function EmptyAddNotesFeedbackLabel() {
    $("FeedbackLabel").text("");
}

function expandBlockUI() {
    //$('.blockUI').css('height', $(window).height()); 
    //set this to screen's resolution instead of window as window may be minimized

    screenH = 480;
    if (parseInt(navigator.appVersion) > 3) {
        screenH = screen.height;
    }
    else if (navigator.appName == "Netscape" && parseInt(navigator.appVersion) == 3 && navigator.javaEnabled()) {
        var jToolkit = java.awt.Toolkit.getDefaultToolkit();
        if (jToolkit != null) {
            var jScreenSize = jToolkit.getScreenSize();
            screenH = jScreenSize.height;
        }
    } 
    
    $('.blockUI').css('height', screenH);
}

function onCellClick(grid, visibleIndex) {
    grid.PerformCallback(visibleIndex);
}

function attachEvents() {
    //$("a#showContactDetails").click(toggleContactDetails);
    selectMenuItem();
    attachDatePickers();
    attachCountDown();
    expandCollapseCustomerInfomation();
    //expandCollapseCustomerInfomation();
    toggleReportNavigation();
    attachTabClickEvents();
    addingToolTip();
    clientInputReady();
    customerInputReady();
}

function setSessionViewID(message) {
    if (message != null) {
        var msgArray = message.d.toString().split('_');
        if (msgArray[0] == 'null')
        { //session exists
            var viewID = randomKey();
            window.name = getFileName(document.location.href) + '_' + viewID;
        
            //call the win service helper to update view id
            var t = window.setTimeout(function() {
                var clientSearchInput = trimWords($("#ClientSearch").val()).toString();
                var clientNameAndNumber = "{'clientNameAndNumber': '" + clientSearchInput + '_' + viewID + "'}";
                CallServiceMethod(relativePathToRoot + "ClientAutoCompleteHelper.asmx/LoadClientNameAndNumber", clientNameAndNumber, displayClientNameAndNumber);
                $.query.SET("ViewID", viewID);

                if (msgArray.length > 1) {
                    if (msgArray[1] != 'null') {
                        $.query.SET("Client", msgArray[1]);
                    }
                }

                if (msgArray.length > 2) {
                    if (msgArray[2] != 'null') {
                        $.query.SET("Customer", msgArray[2]);
                    }
                }
                location.href = $.query;
            }, 90);
        }    
    }
}

function setHref(message) {
    if (message != null) {
        //alert('@setHref1::' + message.d.toString());
        var msgArray = message.d.toString().split('_');
        if (msgArray[0] == 'null') {
            var viewID = randomKey();
            window.name = getFileName(document.location.href) + '_' + viewID;
            $.query.SET("ViewID", viewID);

            if (msgArray.length > 1) {
                if (msgArray[1] != 'null') {
                    $.query.SET("Client", msgArray[1].toString());
                }
            }

            if (msgArray.length > 2) {
                if (msgArray[2] != 'null') {
                    $.query.SET("Customer", msgArray[2].toString());
                }
            }

            //alert('@setHref2::' + $.query.toString());
            location.href = $.query;
        }
    }
}


//Sys.Application.add_load(attachEvents);
function pageLoad() {
    attachEvents();
   
    if (window.name == null) { window.name = ""; }
    if (window.name.length == 0) {
        var custID = $.query.GET("Customer");
        var viewIDString =$.query.GET("ViewID").toString();
        var windowUrl = this.window.location.href.toString().substring(this.window.location.href.toString().indexOf("?"));
        if (windowUrl.match("Customer"))
        {
            custID = windowUrl.substring(windowUrl.indexOf('Customer'));
            var sidx = custID.indexOf('=');
            custID = custID.substring(sidx+1, custID.length - sidx);
            var eidx = custID.indexOf('&');
            if (eidx > 0) {
                custID = custID.substring(0, eidx);
            }
        }

        if (windowUrl.match('ViewID')) {
            viewIDString = windowUrl.substring(windowUrl.indexOf('ViewID'));
            var sidx = custID.indexOf('=');
            custID = custID.substring(sidx + 1, custID.length - sidx);
            var evidx = viewIDString.indexOf('&');
            if (evidx > 0) {
                viewIDString = viewIDString.substring(0, evidx);
            }
        }

        var uuid = "{'viewId': '" + (($.query == null) ? "" : viewIDString)
                        + "', 'clientId': '" + $.query.GET("Client") 
                            + "', 'customerId': '" + custID + "'}";

        if ($("#ClientSearch").attr("disabled") == false) {
            CallServiceMethod(relativePathToRoot + "/ClientAutoCompleteHelper.asmx?/CountHitSession", uuid.toString(), setSessionViewID);
        }
        else {
            //alert('here2::' + uuid + '::' + windowUrl);
            CallServiceMethod(relativePathToRoot + "/CustomerAutoCompleteHelper.asmx/CountHitSession", uuid, setHref,
                       function (XMLHttpRequest, textStatus, errorThrown) {
                           if (XMLHttpRequest.responseText != undefined || XMLHttpRequest.responseText != null)
                                var rText = XMLHttpRequest.responseText.toString();
                            //alert('error @CallServiceMethod -' + textStatus + '::' + errorThrown + " :: " + rText);
                       }
            );
        }
    }
}


function customerInputReady() {

    // dbb 17062015
    var clickRef = 0;
    $("#CustomerSearch").click(function () {
        clickRef = 1;
    }); // dbb

     $("#CustomerSearch").ready(function () {
        attachAutocomplete();
      
        // added to monitor the "Enter" key and submit it after
        $("#CustomerSearch").keypress(function (e) {
             var code = (e.keyCode ? e.keyCode : e.which);
             // dbb 17062015
            if (clickRef == 1) {
                $("#CustomerSearch").val("");
                clickRef = 0;
            } // dbb

            switch (code) {
                case 13: // 13 is return key
                    if ($("#CustomerSearch").val() != null) {
                        e.preventDefault();
                        if ($("#CustomerSearch").val() != "") {
                            if (trimWords($("#CustomerSearch").val()).toString().indexOf("(") >= 0) {
                                var xValue = $(e.target).val();
                                var xValue2 = $("#customerSelectedID").val();
                                if (xValue2 != null) {
                                    selectCustomerFromAutoCompleteDropDown(null, xValue2.toString());
                                }
                                else
                                    selectCustomerFromAutoCompleteDropDown(null, xValue); //this is the customer number!
                            }
                            else {
                                if ($("#SearchCriteria").val() != null) {
                                    var criteriaSelection = trimWords($("#SearchCriteria").val()).toString();
                                    attachCustomerAutoComplete(criteriaSelection, $("#CustomerSearch").val());

                                    var xs = $("#CustomerSearch").val();
                                    if ($("#SearchCriteria").val().toString() == "0")
                                        xs = xs.toUpperCase() + " ";
                                    else
                                        xs = " " + xs.toUpperCase() + " ";
                                    var t1 = window.setTimeout(function () {
                                        $("#CustomerSearch").autocomplete("search", xs);
                                    }, 102);
                                    $("#CustomerSearch").focus();
                                }
                            }
                        }
                        else {
                            //reattach customer autocomplete here
                            if ($("#SearchCriteria").val() != null) {
                                var criteriaSelection = trimWords($("#SearchCriteria").val()).toString();
                                attachCustomerAutoComplete(criteriaSelection, "%");
                            }
                        }
                    }
                    break;

                default: //if letter etc
                    if ($("#CustomerSearch").val() != null) {
                        if ($("#CustomerSearch").val() != "") {
                            var xVal = $("#CustomerSearch").val();
                            if (xVal.length > 2) {
                                if ($("#SearchCriteria").val() != null) {
                                    if ($("#CustomerSearch").hasClass("ui-autocomplete-input")) {
                                        $("#CustomerSearch").autocomplete("destroy");
                                        var t1 = window.setTimeout(function () {
                                            $("#CustomerSearch").removeData("autocomplete");
                                        }, 102);
                                    }
                                    var criteriaSelection = trimWords($("#SearchCriteria").val()).toString();
                                    attachCustomerAutoComplete(criteriaSelection, xVal);                            
                                }
                            }
                        }
                    }
                    break;
            }
        });
    });
}

function clientInputReady() {
    $("#ClientSearch").ready(function () {
        try {
            var r1 = window.setTimeout(function () {
                attachClientAutoComplete();
            }, 30);
        } catch (exc) { }

        //added to monitor the "Enter" key and submit it after
        $("#ClientSearch").keypress(function (e) {
            code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                var searchString = ($("#ClientSearch").val() == null) ? "%" : (trimWords($("#ClientSearch").val().toString()).length == 0)
                                    ? "%" : trimWords(($("#ClientSearch").val()).toString());
                //alert(searchString);
                $("#ClientSelection").val(searchString);
                if (trimWords($("#ClientSearch").val()).toString().indexOf("(") >= 0) {
                    SearchButton_Click();
                } else {
                    if (searchString != null) {
                        if ($("#ClientSearch").hasClass("ui-autocomplete-input")) {
                            $("#ClientSearch").autocomplete("destroy");
                            $("#ClientSearch").removeData("autocomplete");
                            var r2 = window.setTimeout(function () {
                                attachClientAutoComplete();
                            }, 20);  //needed to put timeout here to fix race conditions
                            $("#ClientSelection").autocomplete("search", searchString);
                            $("#ClientSelection").focus();
                        }
                    }
                }
            }
            else
            {              
                var searchStringX = ($("#ClientSearch").val() == null) ? "" : $("#ClientSearch").val();
                if (searchStringX.toString().length < 6) {
                    if ($("#ClientSearch").hasClass("ui-autocomplete-input")) {
                        var r3 = window.setTimeout(function () {
                            $("#ClientSearch").autocomplete("search", searchStringX);
                        }, 20);  //needed to put timeout here to fix race conditions
                        $("#ClientSearch").focus();
                    }
                } else {
                    if ($("#ClientSearch").hasClass("ui-autocomplete-input")) {
                        var r4 = window.setTimeout(function () {
                            $("#ClientSearch").autocomplete("destroy");
                            $("#ClientSearch").removeData("autocomplete");
                        }, 20);  //needed to put timeout here to fix race conditions
                        var r5 = window.setTimeout(function () {
                            attachClientAutoComplete();
                        }, 60);  //needed to put timeout here to fix race conditions
                        $("#ClientSearch").autocomplete("search", searchStringX);
                        $("#ClientSearch").focus();
                    }
                }
            }
        });
    });
 }

//called inside SaferTrader.Master
function setAutoComplete(criteria) {
    attachCustomerAutoComplete(criteria, null);
}


//MSarza
function isEmailValid(s) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(s)) {
        return true;
    }
    else {
        return false;
    }
}

//attachCountDown();
//addingToolTip(); //let the tooltip works on firefox when refreshing the page

// Search by phone
//function selectClientByPhoneFromAutoCompleteDropDown(event, item) {
//    
//}
//$('input[@id$=CustomerSearchByPhone]').ready(function() {
//    attachAutocompleteByPhone();
//});