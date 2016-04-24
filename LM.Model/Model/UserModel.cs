
using System;

namespace LM.Model.Model
{
    public class UserModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Pwd { get; set; }

        public DateTime SaveAt { get; set; }

        public DateTime LastLoginAt { get; set; }
    }
}
