using System;

namespace LM.Model.Entity
{
    public class OperationLog
    {
        public int Id { get; set; }

        public string User { get; set; }

        public string Operation { get; set; }

        public string Ip { get; set; }

        public DateTime SaveAt { get; set; }

    }
}
