using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
namespace Projet.DataStruc
{
    public class BalanceSample
    {
        internal static readonly List<Struc> balanceDataList = new List<Struc>()
        {
            new Struc()
            {
                a = 0.041497f,
                b =  0.071864f,
                phi =  -0.468570f,
                X0 = 0.712496f,
                Y0 =  -0.070570f,
                X0_in =  0.667570f,
                Y0_in =  0.258807f,
                short_axis =  0.082994f,
                long_axis =  0.143729f,

            },
            new Struc()
            {
                 a = 0.009672f,
                b =   0.031260f,
                phi =   0.009399f,
                X0 =  0.650597f,
                Y0 =   0.297305f,
                X0_in =   0.653363f,
                Y0_in =   0.291177f,
                short_axis =   0.019343f,
                long_axis =   0.062519f,
            },
            
        };

    }
}
