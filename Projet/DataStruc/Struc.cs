using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Projet.DataStruc
{
    class Struc
    {
        [LoadColumn(0)]
        public float a { get; set; }
        [LoadColumn(1)]
        public float b { get; set; }
        [LoadColumn(2)]
        public float phi { get; set; }
        [LoadColumn(3)]
        public float X0 { get; set; }
        [LoadColumn(4)]
        public float Y0 { get; set; }
        [LoadColumn(5)]
        public float X0_in { get; set; }
        [LoadColumn(6)]
        public float Y0_in { get; set; }
        [LoadColumn(7)]
        public float short_axis { get; set; }
        [LoadColumn(8)]
        public float long_axis { get; set; }
        [LoadColumn(9)]
        public bool Label { get; set; }
    }
}
