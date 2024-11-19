mergeInto(LibraryManager.library, {
   storeButtonClickData: function(argument, robot, iteration) {
       var argString = UTF8ToString(argument);  // Convert the Unity argument to a string
       var robotType = UTF8ToString(robot);     // Convert the robot argument to string (if needed)
       console.log("JavaScript function called with argument:", argString);
       console.log("Robot Type:", robotType);
       console.log("Iteration:", iteration);

       if (window.storeButtonClickData) {
           window.storeButtonClickData(argString, robotType, iteration);  // Call the global Firebase function
       } else {
           console.error("storeButtonClickData is not defined.");
       }
   }
});


mergeInto(LibraryManager.library, {
    storeEmailData: function(email) {
        // Convert the pointer to a string (the email)
        var emailString = UTF8ToString(email);
        
        console.log("JavaScript function called with id:", emailString);

        // Call your Firebase function to store the email in the database
        if (window.storeEmailData) {
            window.storeEmailData(emailString);  // Call the function defined in firebase.js
        } else {
            console.error("storeEmailData is not defined.");
        }
    }
});

mergeInto(LibraryManager.library, {
    CopyToClipboard: function (userId) {
        // Use UTF8ToString instead of Pointer_stringify
        var text = UTF8ToString(userId);  // Convert the pointer to a string
        var textArea = document.createElement("textarea");
        textArea.value = text;
        document.body.appendChild(textArea);
        textArea.select();
        navigator.clipboard.writeText(textArea.value).then(function() {
            console.log("Copied to clipboard: " + textArea.value);
        }).catch(function(error) {
            console.error("Unable to copy to clipboard: ", error);
        });
        document.body.removeChild(textArea);
    }
});

