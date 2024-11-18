mergeInto(LibraryManager.library, {
   storeButtonClickData: function(argument) {
       var argString = UTF8ToString(argument);
       console.log("JavaScript function called with argument:", argString);
       
       if (window.storeButtonClickData) {
           window.storeButtonClickData(argString);  // Call the global Firebase function
       } else {
           console.error("storeButtonClickData is not defined.");
       }
   }
});


mergeInto(LibraryManager.library, {
    storeEmailData: function(email) {
        // Convert the pointer to a string (the email)
        var emailString = UTF8ToString(email);
        
        console.log("JavaScript function called with email:", emailString);

        // Call your Firebase function to store the email in the database
        if (window.storeEmailData) {
            window.storeEmailData(emailString);  // Call the function defined in firebase.js
        } else {
            console.error("storeEmailData is not defined.");
        }
    }
});
