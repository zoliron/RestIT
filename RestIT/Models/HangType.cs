
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Runtime.Learners;


namespace RestIT.Models
{

    

    public class HangType
    {
        [Column("0")]
        [ColumnName("City")]
        public float City;

        [Column("1")]
        [ColumnName("Sex")]
        public float Sex;

        [Column("2")]
        [ColumnName("Age")]
        public float Age;

    }

    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

       [ColumnName("Score")]
       public float[] Distances;
    }
    
    
}
