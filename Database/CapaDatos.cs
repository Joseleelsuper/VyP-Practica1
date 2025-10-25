using Logica.Models;
using Logica.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Database
{
    public class CapaDatos : ICapaDatos
    {

        private List<User> tblUsuarios = new List<User>();
        private List<Activity> tblActividades = new List<Activity>();

        public CapaDatos()
        {
            tblUsuarios.Add(new User("71478806G", "Juan", "Pérez", "juanp1025@alu.ubu.es", "bWW51KpD?6`@Q0l32b\\x~*rB85;f5@ct", 25, "644842590", Gender.MALE, 75.23f));
            tblUsuarios.Add(new User("46541439L", "María", "Gómez", "mariag1002@gmail.com", "*diy9RzFFeHTlHflJ.Ov&I=DC_6'7tWt", 18, "654655368", Gender.FEMALE, 57.1f));
            tblUsuarios.Add(new User("67177373P", "Admin", "Admin", "admin@admin.net", "H+DHwwRcp`D'?X`#pNQy3nrQG0LP1aVa", 99, "644992132", Gender.HELICOPTERO_DE_COMBATE, 999999f) { Role = Role.ADMIN });
            tblActividades.Add(new Activity("Correr", "Carrera matutina en el parque", DateTime.Now, TimeSpan.FromMinutes(30), 1, 5000));
            tblActividades.Add(new Activity("Ciclismo", "Paseo en bicicleta por la ciudad", DateTime.Now, TimeSpan.FromMinutes(45), 3, 15000));
        }

        public bool GuardaActividad(Activity e)
        {
            if (!tblUsuarios.Exists(u => u.Id == e.UserId))
            {
                return false;
            }

            tblActividades.Add(e);

            return true;
        }

        public bool GuardaUsuario(User u)
        {
            if (tblUsuarios.Exists(existingUser => existingUser.Email == u.Email) ||
                tblUsuarios.Exists(existingUser => existingUser.NIF == u.NIF))
            {
                return false;
            }

            tblUsuarios.Add(u);
            return true;
        }

        public Activity LeeActividad(int idElemento)
        {
            return tblActividades.Find(a => a.Id == idElemento);
        }

        public User LeeUsuario(string email)
        {
            return tblUsuarios.Find(u => u.Email == email);
        }

        public int NumActividades(int idUsuario)
        {
            return tblActividades.FindAll(a => a.UserId == idUsuario).Count;
        }

        public int NumUsuarios()
        {
            return tblUsuarios.Count;
        }

        public int NumUsuariosActivos()
        {
            return tblUsuarios.FindAll(u => u.Status == Status.ACTIVE).Count;
        }

        public bool ValidaUsuario(string email, string password)
        {
            return tblUsuarios.Exists(u => u.Email == email && PasswordHasher.VerifyPassword(password, u.PasswordHash));
        }

        public bool EliminaUsuario(string email)
        {
            User user = LeeUsuario(email);
            if (user != null)
            {
                var actividadesAEliminar = tblActividades.Where(a => a.UserId == user.Id).Select(a => a.Id).ToList();

                foreach (var actividadId in actividadesAEliminar)
                {
                    EliminaActividad(actividadId);
                }

                tblUsuarios.Remove(user);
                return true;
            }

            return false;
        }

        public bool EliminaActividad(int idElemento)
        {
            Activity activity = LeeActividad(idElemento);

            if (activity != null)
            {
                tblActividades.Remove(activity);
                return true;
            }

            return false;
        }

        public bool InactivaUsuario(string email)
        {
            User user = LeeUsuario(email);
            if (user != null)
            {
                user.Status = Status.INACTIVE;
                return true;
            }

            return false;
        }

        public bool ReactivaUsuario(string email)
        {
            User user = LeeUsuario(email);
            if (user != null)
            {
                user.Status = Status.ACTIVE;
                return true;
            }

            return false;
        }

        public bool PromueveUsuario(string email)
        {
            User user = LeeUsuario(email);
            if (user != null && user.Role == Role.USER)
            {
                user.Role = Role.ADMIN;
                return true;
            }

            return false;
        }

        public bool DegradaUsuario(string email)
        {
            User user = LeeUsuario(email);
            if (user != null && user.Role == Role.ADMIN)
            {
                user.Role = Role.USER;
                return true;
            }

            return false;
        }

        public bool CambiarUsuarioActividad(int idActividad, int nuevoIdUsuario)
        {
            Activity activity = LeeActividad(idActividad);
            if (activity != null && tblUsuarios.Exists(u => u.Id == nuevoIdUsuario))
            {
                activity.UserId = nuevoIdUsuario;
                return true;
            }

            return false;
        }
    }
}
