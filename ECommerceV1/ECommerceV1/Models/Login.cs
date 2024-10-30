using System.ComponentModel.DataAnnotations;

namespace ECommerceV1.Models
{
    public class Login
    {
        public int Id { get; set; }
        // Propriété pour stocker l'adresse e-mail de l'utilisateur
        public string Email { get; set; }

        // Propriété pour stocker le mot de passe de l'utilisateur
        public string Password { get; set; }




        public bool Checkout(Login log)
        {
            // In production, you would hash the input password and compare the hashed values
            // Here, we are directly comparing the email and password for simplicity
            if (this.Email == log.Email && this.Password == log.Password)
            {
                return true; // Login is successful
            }
            return false; // Login failed
        }
    }
}
