// Import the necessary Firebase modules
import { initializeApp } from "https://www.gstatic.com/firebasejs/9.0.0/firebase-app.js";
import { getDatabase, ref, push, runTransaction, get, set} from "https://www.gstatic.com/firebasejs/9.0.0/firebase-database.js";
import { getAuth, signInAnonymously, onAuthStateChanged } from "https://www.gstatic.com/firebasejs/9.0.0/firebase-auth.js";

// Import the functions you need from the SDKs you need
// import { initializeApp } from "firebase/app";
// import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyDZ-Dtx4ak9AyP73_nfejGVHkSheVwu6lk",
  authDomain: "cs-7633-study.firebaseapp.com",
  projectId: "cs-7633-study",
  storageBucket: "cs-7633-study.firebasestorage.app",
  messagingSenderId: "544838145446",
  appId: "1:544838145446:web:8d5a31b26f4ac31d89ab67",
  measurementId: "G-3MR36PDV04"
};

// Initialize Firebase

const app = initializeApp(firebaseConfig);
const database = getDatabase(app);
const auth = getAuth(app);
var currentUserEmail = "";
// const emailRef = ref(database, 'user clicks');

// // Initialize global variable for userId




window.storeButtonClickData = function(buttonName) {
    // if (!currentUserEmail) {
    //     console.error("No email set. Ensure storeEmailData is called first.");
    //     return;
    // }

    // Construct the reference to the specific user's button click data
    const buttonRef = ref(database, `user clicks/${currentUserEmail}/${buttonName}`);

    // Use a transaction to safely increment the count
    runTransaction(buttonRef, (currentValue) => {
        return (currentValue || 0) + 1; // Increment the current value or start from 1 if it doesn't exist
    })
    .then(() => {
        console.log(`${buttonName} count incremented successfully.`);
    })
    .catch((error) => {
        console.error(`Error incrementing ${buttonName} count:`, error);
    });
};



window.storeEmailData = function(email) {
    // Sanitize email to create a valid Firebase key
    const cleaned_email = email.replace('.', ',');
    console.log("email logged:", cleaned_email);
   // currentUserEmail = sanitizedEmail; // Store globally for button clicks
    currentUserEmail = cleaned_email;

    const emailRef = ref(database, 'user clicks/' + cleaned_email);

    // Initialize user data if not already present
    get(emailRef).then(snapshot => {
        if (!snapshot.exists()) {
            set(emailRef, {
                ThumbsUp: 0,
                ThumbsDown: 0
            }).then(() => {
                console.log("User data initialized for:", email);
            }).catch(error => {
                console.error("Error initializing user data:", error);
            });
        } else {
            console.log("User data already exists for:", email);
        }
    }).catch(error => {
        console.error("Error checking user data:", error);
    });
};


// JavaScript - WebGL Interop
// window.storeEmailData = function(email) {
//     const emailRef = ref(database, 'userEmails');

//     const emailRef = ref('emails/' + email.replace('.', ',')); // Replace '.' with ',' to make the email valid as a Firebase key
//     emailRef.set({
//         email: email,
//         timestamp: Date.now()
//     }).then(() => {
//         console.log("Email stored successfully:", email);
//     }).catch((error) => {
//         console.error("Error storing email:", error);
//     });
// }

