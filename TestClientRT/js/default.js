// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. You might use the
        // WinJS.Application.sessionState object, which is automatically
        // saved and restored across suspension. If you need to complete an
        // asynchronous operation before your application is suspended, call
        // args.setPromise().
    };

    app.start();
})();

function getMessagesAsync() {
    try {
        buttonGetMessages.disabled = "disabled";

        var pop3Client = new Pop3.Pop3Client();

        consoleLog("Connecting to POP3 server '" + textboxServer.value + "'...");
        pop3Client.connectAsync(textboxServer.value, textboxUserName.value, textboxPassword.value, checkboxUseSsl.value)
                .then(function () {
                    consoleLog("List and Retrieve Messages...");
                    return pop3Client.listAndRetrieveAsync();
                })
                .then(function (messages) {
                    for (var i = 0, len = messages.size; i < len; i++) {
                        var message = messages[i];

                        consoleLog("- Number: " + message.number);
                        consoleLog("\t* MessageId: " + message.messageId);
                        consoleLog("\t* Date: " + message.date);
                        consoleLog("\t* From: " + message.from);
                        consoleLog("\t* To: " + message.to);
                        consoleLog("\t* Subject: " + message.subject);
                        consoleLog("\t* Body Lenght: " + message.body.length);
                        consoleLog("");
                    }

                    consoleLog("Disconnecting...");
                    return pop3Client.disconnectAsync();
                })
                .done(function () {
                    consoleLog("Communication closed...");
                });
    }
    catch (e) {
        consoleLog(e.message);
    }
    finally {
        buttonGetMessages.disabled = "";
    }
}

function consoleLog(data) {
    textareaOutput.value += "\r\n" + data;
}