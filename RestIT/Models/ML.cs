using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var reader = new TextLoader(env,
                new TextLoader.Arguments()
                {
                    Separator = ",",
                    HasHeader = true,
                    Column = new[]
                    {
                            new TextLoader.Column("City", DataKind.R4, 0),
                            new TextLoader.Column("Sex", DataKind.R4, 1),
                            new TextLoader.Column("Age", DataKind.R4, 2),
                            new TextLoader.Column("Label", DataKind.Text, 3)
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
   }

