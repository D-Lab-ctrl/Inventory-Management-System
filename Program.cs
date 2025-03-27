using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MyProgram
{
    class MyProgram
    {
        static void Main(string[] args)
        {
            
            User enrico = new User("10LR4");
            enrico.Username = "enrico994";
            enrico.Role = "engineer";

            Console.WriteLine($"The User ID {enrico.UserID} | Username: {enrico.Username} | Role: {enrico.Role}");
            enrico.SetPassword("enrico94");
            Console.WriteLine(enrico.GetHashedPassword);

        }
    }

    class User
    {
        private string _userId;              // The userID can be accessed  directly only within the class User
        private string _username = "";
        private string _role = "";
        private string _passwordHash = "";
        private string _salt = "";


        public User(string userID)
        {   
            if (string.IsNullOrEmpty(userID))           //Validate the User ID is not empty or null
            {
                throw new ArgumentException("User ID cannot be empty or null.");        //If userID is empty or null the constructor raises an error and stops the code from creating the object
            }
            
            _userId = userID;
        }

        public void SetPassword(string password)
        {
            _salt = GenerateSalt();

            _passwordHash = HashPassword(password, _salt);
        }

        //Create a salt string 
        public string GenerateSalt()
        {
            var salt = new byte[4];                         //Create a buffer array (4 bytes in this case) to store the salt
            var rng = RandomNumberGenerator.Create();       //Create an instance of a cryptographic number generator
            rng.GetBytes(salt);                             //Fill the buffer with cryptographycally secure random bytes
            return Convert.ToBase64String(salt);            //Converts a byte array into a readable string and return it
        }
        public string HashPassword(string password, string salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 256/ 8
            ));
        }

        
        public string UserID                // I create a getter to access the user ID from outside the class
        {
            get {return _userId;}
        }
        public string Username
        {
            get {return _username;}          //getters and setters are used to access and set properties such as username and role
            set {_username = value;}
        }
        public string Role
        {
            get {return _role;}
            set {_role = value;}
        }

        public string GetHashedPassword
        {
            get {return _passwordHash;}
        }
    }
}