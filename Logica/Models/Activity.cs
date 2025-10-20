using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    /*
     * Almacena actividades físicas realizadas por los usuarios.
     */
    public class Activity
    {
        private static int _nextId = 1;

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public int UserId { get; set; } // ID del usuario que creó la actividad
        public double Distance { get; set; } // (M)
        public DateTime CreatedAt { get; private set; }

        public Activity(string name, string description, DateTime date, TimeSpan duration, int userId, double distance)
        {
            Id = _nextId++;

            Name = name;
            Description = description;
            Date = date;
            Duration = duration;
            UserId = userId;
            Distance = distance;

            CreatedAt = DateTime.Now;
        }

        // Método para testing - resetea el contador de IDs
        public static void ResetIdCounter()
        {
            _nextId = 1;
        }

        public override string ToString()
        {
            return $"Activity(Id={Id}, Name={Name}, UserId={UserId}, Date={Date}, Duration={Duration}, Distance={Distance}m, CreatedAt={CreatedAt})";
        }
    }
}
