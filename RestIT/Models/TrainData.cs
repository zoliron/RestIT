
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Runtime.Learners;


namespace RestIT.Models
{
    public enum Sex
    {
        Male = 0,
        Female = 1,
    };

    public enum City 
    {
        TelAviv = 0,
        Jerusalem = 1,
        RamatGan = 2,
        Raanana = 3,
        Ashdod = 4,
        Ashkelon = 5,


    };
    public class TrainData
    {
        [Column("0")]
        public float City;

        [Column("1")]
        public float Sex;

        [Column("2")]
        public float Age;

        [Column("3")]
        [ColumnName("Label")]
        public string Label;
        

   
    }
    public class TypePrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabels;
    }

   
   


}
