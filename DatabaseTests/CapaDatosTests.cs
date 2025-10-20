using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Logica.Models;
using Logica.Utils;

[assembly: DoNotParallelize] // https://learn.microsoft.com/es-es/dotnet/core/testing/mstest-analyzers/mstest0001

namespace Database.Tests
{
    [TestClass()]
    public class CapaDatosTests
    {
        private CapaDatos capaDatos;

        [TestInitialize]
        public void Setup()
        {
            User.ResetIdCounter();
            Activity.ResetIdCounter();
            capaDatos = new CapaDatos();
        }

        [TestMethod()]
        public void GuardaActividadTest()
        {
            // Test case 1: Guardar actividad con usuario existente
            Activity activity = new Activity("Test Activity", "Test Description", DateTime.Now, TimeSpan.FromMinutes(30), 1, 5000);
            bool result = capaDatos.GuardaActividad(activity);
            Assert.IsTrue(result);

            // Test case 2: Guardar actividad con usuario inexistente
            Activity activityInvalidUser = new Activity("Test Activity 2", "Test Description 2", DateTime.Now, TimeSpan.FromMinutes(45), 999, 3000);
            bool resultInvalid = capaDatos.GuardaActividad(activityInvalidUser);
            Assert.IsFalse(resultInvalid);
        }

        [TestMethod()]
        public void GuardaUsuarioTest()
        {
            // Guardar usuario
            User user = new User("36609929R", "Test", "User", "test@test.com", "password123", 33, "644992334", Gender.HELICOPTERO_DE_COMBATE, 88f);

            bool result = capaDatos.GuardaUsuario(user);
            Assert.IsTrue(result);

            // Verificar que el usuario se guardó correctamente
            var savedUser = capaDatos.LeeUsuario("test@test.com");
            Assert.IsNotNull(savedUser);
            Assert.AreEqual("Test", savedUser.Name);
        }

        [TestMethod()]
        public void LeeActividadTest()
        {
            // Test case 1: Leer actividad existente
            var activity = capaDatos.LeeActividad(2);
            Assert.IsNotNull(activity);
            Assert.AreEqual("Ciclismo", activity.Name);

            // Test case 2: Leer actividad inexistente
            var nonExistentActivity = capaDatos.LeeActividad(999);
            Assert.IsNull(nonExistentActivity);
        }

        [TestMethod()]
        public void LeeUsuarioTest()
        {
            // Test case 1: Leer usuario existente
            var user = capaDatos.LeeUsuario("juanp1025@alu.ubu.es");
            Assert.IsNotNull(user);
            Assert.AreEqual("Juan", user.Name);

            // Test case 2: Leer usuario inexistente
            var nonExistentUser = capaDatos.LeeUsuario("noexiste@test.com");
            Assert.IsNull(nonExistentUser);
        }

        [TestMethod()]
        public void NumActividadesTest()
        {
            // Test case 1: Contar actividades de usuario con actividades
            int count = capaDatos.NumActividades(1);
            Assert.AreEqual(1, count);

            // Test case 2: Contar actividades de usuario sin actividades
            int countEmpty = capaDatos.NumActividades(2);
            Assert.AreEqual(0, countEmpty);

            // Test case 3: Contar actividades de usuario inexistente
            int countNonExistent = capaDatos.NumActividades(999);
            Assert.AreEqual(0, countNonExistent);
        }

        [TestMethod()]
        public void NumUsuariosTest()
        {
            // Test case: Contar usuarios (debería haber 3 usuarios iniciales)
            int count = capaDatos.NumUsuarios();
            Assert.AreEqual(3, count);

            // Agregar un usuario y verificar incremento
            capaDatos.GuardaUsuario(new User("70491648Y", "New", "User", "new@test.com", "password", 33, "+34644842590", Gender.HELICOPTERO_DE_COMBATE, 88f));
            int newCount = capaDatos.NumUsuarios();
            Assert.AreEqual(4, newCount);
        }

        [TestMethod()]
        public void NumUsuariosActivosTest()
        {
            // Test case: Contar usuarios activos
            int activeCount = capaDatos.NumUsuariosActivos();
            Assert.AreEqual(3, activeCount);

            // Inactivar un usuario y verificar disminución
            capaDatos.InactivaUsuario("juanp1025@alu.ubu.es");
            int newActiveCount = capaDatos.NumUsuariosActivos();
            Assert.AreEqual(2, newActiveCount);
        }

        [TestMethod()]
        public void ValidaUsuarioTest()
        {
            // Test case 1: Validar usuario con credenciales correctas
            bool validUser = capaDatos.ValidaUsuario("juanp1025@alu.ubu.es", "HolaQ_123");
            Assert.IsTrue(validUser);

            // Test case 2: Validar usuario con contraseña incorrecta
            bool invalidPassword = capaDatos.ValidaUsuario("juanp1025@alu.ubu.es", "wrongpassword");
            Assert.IsFalse(invalidPassword);

            // Test case 3: Validar usuario inexistente
            bool nonExistentUser = capaDatos.ValidaUsuario("noexiste@test.com", "password");
            Assert.IsFalse(nonExistentUser);
        }

        [TestMethod()]
        public void EliminaUsuarioTest()
        {
            // Test case 1: Eliminar usuario existente with actividades
            bool result = capaDatos.EliminaUsuario("juanp1025@alu.ubu.es");
            Assert.IsTrue(result);

            // Verificar que el usuario fue eliminado
            var deletedUser = capaDatos.LeeUsuario("juanp1025@alu.ubu.es");
            Assert.IsNull(deletedUser);

            // Test case 2: Eliminar usuario inexistente
            bool resultNonExistent = capaDatos.EliminaUsuario("noexiste@test.com");
            Assert.IsFalse(resultNonExistent);
        }

        [TestMethod()]
        public void EliminaActividadTest()
        {
            // Test case 1: Eliminar actividad existente (ID 5 es la actividad de "Ciclismo")
            bool result = capaDatos.EliminaActividad(2);
            Assert.IsTrue(result);

            // Verificar que la actividad fue eliminada
            var deletedActivity = capaDatos.LeeActividad(2);
            Assert.IsNull(deletedActivity);
        }

        [TestMethod()]
        public void InactivaUsuarioTest()
        {
            // Test case 1: Inactivar usuario existente
            bool result = capaDatos.InactivaUsuario("juanp1025@alu.ubu.es");
            Assert.IsTrue(result);

            // Verificar que el usuario está inactivo
            var user = capaDatos.LeeUsuario("juanp1025@alu.ubu.es");
            Assert.AreEqual(Status.INACTIVE, user.Status);

            // Test case 2: Inactivar usuario inexistente
            bool resultNonExistent = capaDatos.InactivaUsuario("noexiste@test.com");
            Assert.IsFalse(resultNonExistent);
        }

        [TestMethod()]
        public void ReactivaUsuarioTest()
        {
            // Primero inactivar un usuario
            capaDatos.InactivaUsuario("juanp1025@alu.ubu.es");

            // Test case 1: Reactivar usuario existente
            bool result = capaDatos.ReactivaUsuario("juanp1025@alu.ubu.es");
            Assert.IsTrue(result);

            // Verificar que el usuario está activo
            var user = capaDatos.LeeUsuario("juanp1025@alu.ubu.es");
            Assert.AreEqual(Status.ACTIVE, user.Status);

            // Test case 2: Reactivar usuario inexistente
            bool resultNonExistent = capaDatos.ReactivaUsuario("noexiste@test.com");
            Assert.IsFalse(resultNonExistent);
        }

        [TestMethod()]
        public void PromueveUsuarioTest()
        {
            // Test case 1: Promover usuario normal a admin
            bool result = capaDatos.PromueveUsuario("juanp1025@alu.ubu.es");
            Assert.IsTrue(result);

            // Verificar que el usuario es admin
            var user = capaDatos.LeeUsuario("juanp1025@alu.ubu.es");
            Assert.AreEqual(Role.ADMIN, user.Role);

            // Test case 2: Intentar promover usuario que ya es admin
            bool resultAlreadyAdmin = capaDatos.PromueveUsuario("admin@admin.net");
            Assert.IsFalse(resultAlreadyAdmin);

            // Test case 3: Promover usuario inexistente
            bool resultNonExistent = capaDatos.PromueveUsuario("noexiste@test.com");
            Assert.IsFalse(resultNonExistent);
        }

        [TestMethod()]
        public void DegradaUsuarioTest()
        {
            // Test case 1: Degradar usuario admin a normal
            bool result = capaDatos.DegradaUsuario("admin@admin.net");
            Assert.IsTrue(result);

            // Verificar que el usuario es normal
            var user = capaDatos.LeeUsuario("admin@admin.net");
            Assert.AreEqual(Role.USER, user.Role);

            // Test case 2: Intentar degradar usuario que ya es normal
            bool resultAlreadyUser = capaDatos.DegradaUsuario("juanp1025@alu.ubu.es");
            Assert.IsFalse(resultAlreadyUser);

            // Test case 3: Degrada usuario inexistente
            bool resultNonExistent = capaDatos.DegradaUsuario("noexiste@test.com");
            Assert.IsFalse(resultNonExistent);
        }

        [TestMethod()]
        public void CambiarUsuarioActividadTest()
        {
            // Test case 1: Cambiar actividad a usuario existente
            bool result = capaDatos.CambiarUsuarioActividad(1, 2);
            Assert.IsTrue(result);

            // Verificar que la actividad cambió de usuario
            var activity = capaDatos.LeeActividad(1);
            Assert.AreEqual(2, activity.UserId);

            // Test case 2: Cambiar actividad inexistente
            bool resultNonExistentActivity = capaDatos.CambiarUsuarioActividad(999, 1);
            Assert.IsFalse(resultNonExistentActivity);

            // Test case 3: Cambiar actividad a usuario inexistente
            bool resultNonExistentUser = capaDatos.CambiarUsuarioActividad(2, 999);
            Assert.IsFalse(resultNonExistentUser);

            // Test case 4: Actividad inexistente y usuario inexistente
            bool resultBothNonExistent = capaDatos.CambiarUsuarioActividad(999, 999);
            Assert.IsFalse(resultBothNonExistent);
        }
    }
}