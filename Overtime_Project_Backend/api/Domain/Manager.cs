using System;
using System.Collections.Generic;

namespace api.Domain
{
    public class Manager
    {
        public int ManagerId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Relación 1:N -> Un manager tiene muchos usuarios
        public ICollection<User>? Users { get; set; }
    }
}
