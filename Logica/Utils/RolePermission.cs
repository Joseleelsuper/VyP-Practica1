using System.Collections.Generic;

namespace Logica.Utils
{
    public class RolePermission
    {
        private static readonly Dictionary<Role, List<Permission>> permissionsByRole = new Dictionary<Role, List<Permission>>
        {
            { Role.USER, new List<Permission>
                {
                    Permission.READ_ACTIVITIES
                }
            },
            { Role.ADMIN, new List<Permission>
                {
                    Permission.READ_ACTIVITIES,
                    Permission.CREATE_ACTIVITIES,
                    Permission.DELETE_ACTIVITIES
                }
            }
        };

        public static bool HasPermission(Role role, Permission permission)
        {
            return permissionsByRole.ContainsKey(role) && permissionsByRole[role].Contains(permission);
        }
    }
}
