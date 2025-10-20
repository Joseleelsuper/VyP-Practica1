using System;
using Logica.Utils;

namespace Logica.Models
{
    public class User
    {
        
        private static int _nextId = 1;

        public int Id { get; private set; }
        public string NIF { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Age { get; set; }
        public string Telf { get; set; }
        public Gender Gender { get; set; }
        public float Weight { get; set; } // (Kg)
        public Role Role { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; private set; }

        public User(string nif, string name, string surname, string email, string pwd, int age, string telf, Gender gender, float weight)
        {
            Id = _nextId++;

            NIF = Validate.NIF(nif) ? nif : throw new ArgumentException("NIF inválido.", nameof(nif));
            Name = name;
            Surname = surname;
            Email = Validate.Email(email) ? email : throw new ArgumentException("Email inválido.", nameof(email));
            PasswordHash = PasswordHasher.HashPassword(pwd);
            Age = age;
            Telf = Validate.Telf(telf) ? telf : throw new ArgumentException("Número de teléfono inválido.", nameof(telf));
            Gender = gender;
            Weight = weight;

            Status = Status.ACTIVE;
            Role = Role.USER;
            CreatedAt = DateTime.Now;
        }

        // Método para testing - resetea el contador de IDs
        public static void ResetIdCounter()
        {
            _nextId = 1;
        }

        public override string ToString()
        {
            return $"User(Id={Id}, Name={Name}, Surname={Surname}, Email={Email}, Age={Age}, Telf={Telf}, Gender={Gender}, Weight={Weight}kg, Role={Role}, Status={Status}, CreatedAt={CreatedAt})";
        }
    }
}