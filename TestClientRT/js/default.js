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

var resultAsync;
function getMessagesAsync() {
    buttonGetMessages.disabled = "disabled";
    buttonGetMessagesCancel.disabled = "";

    var pop3Client = new Pop3.Pop3Client();

    consoleLog("Connecting to POP3 server...");

    resultAsync = pop3Client.connectAsync("X", "Y", "Z", false)
            .then(function () {
                document.getElementById('output').innerHTML += "<br/>" + "List and Retrieve Messages...";
                return pop3Client.listAndRetrieveAsync();
            })
            .then(function (messages) {
                for (message in messages) {
                    consoleLog("- Number: " + message.Number);
                    consoleLog("     * MessageId: " + message.MessageId);
                    consoleLog("     * Date: " + message.Date);
                    consoleLog("     * From: " + message.From);
                    consoleLog("     * To: " + message.To);
                    consoleLog("     * Subject: " + message.Subject);
                    consoleLog("");
                }

                consoleLog("Disconnecting...");
                return pop3Client.disconnectAsync();
            })
            .done(function () {
                btnCancel.disabled = "disabled";
                btnAsync.disabled = "";
            });
}

function getMessagesAsyncCancel() {
    resultAsync.cancel();
}

function consoleLog(data) {
    document.getElementById('output').innerHTML += "<br/>" + data;
}