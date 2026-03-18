using System;
using System.Collections.Generic;
using System.Text;

namespace HamzaYasinAssessmentProject.Shared.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public bool Active { get; set; }
        public required List<string> Roles { get; set; }
    }
}
