using System;
using LM.Model.Common;

namespace LM.Model.Model
{
    public class ExpertModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImgSrc { get; set; }

        public ExpertRange Range { get; set; }

        public string RangeName { get; set; }

        public string Description { get; set; }

        /// <summary>
        ///  专著：输入格式 《a》,《b》
        /// </summary>
        public string Books { get; set; }

        public DateTime SaveAt { get; set; }
    }
}
