using System;
using System.IO; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Models;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;



using Microsoft.ML; 
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Runtime.Learners;

namespace RestIT.Models
{
    public class ML
    {
        public string Run(TrainData customer1)
        {
            // STEP 2: Create an evironment  and load your data
            var env = new LocalEnvironment();
            // If working in Visual Studio, make sure the 'Copy to Output Directory'
            // property of iris-data.txt is set to 'Copy always'
            string dataPath = "train-data.txt";
            var reader = new Microsoft.ML.Runtime.Data.TextLoader(env,
                new Microsoft.ML.Runtime.Data.TextLoader.Arguments()
                {
                    Separator = ",",
                    HasHeader = true,
                    Column = new[]
                    {
                            new Microsoft.ML.Runtime.Data.TextLoader.Column("City", Microsoft.ML.Runtime.Data.DataKind.R4, 0),
                            new Microsoft.ML.Runtime.Data.TextLoader.Column("Sex", Microsoft.ML.Runtime.Data.DataKind.R4, 1),
                            new Microsoft.ML.Runtime.Data.TextLoader.Column("Age", Microsoft.ML.Runtime.Data.DataKind.R4, 2),
                            new Microsoft.ML.Runtime.Data.TextLoader.Column("Label", Microsoft.ML.Runtime.Data.DataKind.Text, 3)
                    }
                });

            IDataView trainingDataView = reader.Read(new MultiFileSource(dataPath));
            
            var pipeline = new TermEstimator(env, "Label", "Label")
                   .Append(new ConcatEstimator(env, "Features", "City", "Sex", "Age"))
                   .Append(new SdcaMultiClassTrainer(env, new SdcaMultiClassTrainer.Arguments()))
                   .Append(new KeyToValueEstimator(env, "PredictedLabel"));

            var model = pipeline.Fit(trainingDataView);
            var prediction = model.MakePredictionFunction<TrainData, TypePrediction>(env).Predict(
                new TrainData()
                {
                    City = customer1.City,
                    Sex = customer1.Sex,
                    Age = customer1.Age,

                });

            return prediction.PredictedLabels;
        }


    }


    public class Hang
    {
        string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "hang.data");
        string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "hangModel.zip");
        public string Run(HangType customer1)
        {
            PredictionModel<HangType, ClusterPrediction> model = Train();
            ClusterPrediction predication =  model.Predict(
                new HangType()
                {
                    City = customer1.City,
                    Sex = customer1.Sex,
                    Age = customer1.Age,
                });

            switch (predication.PredictedClusterId)
            {
                case 1:
                    return "Date";
                 
                case 2:
                   return "Friends meeting";
                  
                case 3:
                    return "Family Event";
                    
                default:
                    return "";

            }
             
        }
        private  PredictionModel<HangType, ClusterPrediction> Train()
        {
            var pipeline = new LearningPipeline();
            pipeline.Add(new Microsoft.ML.Legacy.Data.TextLoader(_dataPath).CreateFrom<HangType>(separator: ','));
            pipeline.Add(new ColumnConcatenator(
                        "Features",
                        "City",
                        "Sex",
                        "Age"));
            pipeline.Add(new KMeansPlusPlusClusterer() { K = 3 });
            var model = pipeline.Train<HangType, ClusterPrediction>();
            return model; 



        }
    }


}

